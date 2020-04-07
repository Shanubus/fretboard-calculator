using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using FretboardCalculatorCore;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace FretboardCalculatorApi
{

    public class Function
    {
        private static readonly FretboardViewManager fvm = new FretboardViewManager(configurationsPath, scalesPath, chordsPath);
        private static readonly HttpClient client = new HttpClient();
        private const string configurationsPath = "https://fretboard-calculator-json.s3.amazonaws.com/fretboard-configurations.json";
        private const string scalesPath = "https://fretboard-calculator-json.s3.amazonaws.com/scales.json";
        private const string chordsPath = "https://fretboard-calculator-json.s3.amazonaws.com/chords.json";
        
        public async Task<APIGatewayProxyResponse> GetStoredConfigurationsList(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var fretboardConfigurations = await Task.Run(() => fvm.GetStoredConfigurationList());
            return getResponse(fretboardConfigurations);
        }

        public async Task<APIGatewayProxyResponse> GetStoredConfigurationByName(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["name"]);
            var fretboardConfiguration = await Task.Run(() => fvm.GetConfigurationByName(qName));
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredChordsList(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var fretboardConfiguration = await Task.Run(() => fvm.GetStoredChordsList());
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredChordsByName(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["name"]);
            var fretboardConfiguration = await Task.Run(() => fvm.GetChordByName(qName));
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredScalesList(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var fretboardConfiguration = await Task.Run(() => fvm.GetStoredScalesList());
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredScalesByName(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["name"]);
            var fretboardConfiguration = await Task.Run(() => fvm.GetScaleByName(qName));
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetFretboard(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["configurationName"]);
            var fretboardConfiguration = await Task.Run(() => fvm.GetConfigurationByName(qName));
            var fretboard = await Task.Run(() => fvm.GetFretboard(fretboardConfiguration));
            return getResponse(fretboard);
        }

        public async Task<APIGatewayProxyResponse> GetFretboardWithIntervals(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["configurationName"]);
            var iType = WebUtility.UrlDecode(apiProxyEvent.PathParameters["intervalType"]);
            var iName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["intervalName"]);
            var iKey = getNoteValue(apiProxyEvent.PathParameters["keyOf"]);
            var fretboardConfiguration = await Task.Run(() => fvm.GetConfigurationByName(qName));
            var intervalType = iType;
            IntervalPattern intervals = null;

            if (intervalType == "scale")
            {
                intervals = await Task.Run(() => fvm.GetScaleByName(iName));
            } else if (intervalType == "chord")
            {
                intervals = await Task.Run(() => fvm.GetChordByName(iName));
            }
            intervals.StartNote = iKey;

            var fretboard = await Task.Run(() => fvm.GetFretboard(fretboardConfiguration, intervals));
            return getResponse(fretboard);
        }

        private static decimal getNoteValue(string note)
        {
            switch (note.ToUpper())
            {
                case "C":
                    return Notes.C;
                case "CSHARP":
                case "DFLAT":
                    return Notes.Csharp;
                case "D":
                    return Notes.D;
                case "DSHARP":
                case "EFLAT":
                    return Notes.Dsharp;
                case "E":
                    return Notes.E;
                case "F":
                    return Notes.F;
                case "FSHARP":
                case "GFLAT":
                    return Notes.Fsharp;
                case "G":
                    return Notes.G;
                case "GSHARP":
                case "AFLAT":
                    return Notes.Gsharp;
                case "A":
                    return Notes.A;
                case "ASHARP":
                case "BFLAT":
                    return Notes.Asharp;
                case "B":
                    return Notes.B;
                default:
                    return Notes.C;
            }
        }

        private static APIGatewayProxyResponse getResponse(object payload)
        {
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(payload),
                StatusCode = 200,
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "Content-Type" },
                    { "Access-Control-Allow-Methods", "OPTIONS,POST,GET" }
                }
            };
        }
    }
}
