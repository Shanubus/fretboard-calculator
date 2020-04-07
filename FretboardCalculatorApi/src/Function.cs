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
            var fretboardConfigurations = fvm.GetStoredConfigurationList();
            return getResponse(fretboardConfigurations);
        }

        public async Task<APIGatewayProxyResponse> GetStoredConfigurationByName(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["name"]);
            var fretboardConfiguration = fvm.GetConfigurationByName(qName);
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredChordsList(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var fretboardConfiguration = fvm.GetStoredChordsList();
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredChordsByName(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["name"]);
            var fretboardConfiguration = fvm.GetChordByName(qName);
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredScalesList(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var fretboardConfiguration = fvm.GetStoredScalesList();
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetStoredScalesByName(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["name"]);
            var fretboardConfiguration = fvm.GetScaleByName(qName);
            return getResponse(fretboardConfiguration);
        }

        public async Task<APIGatewayProxyResponse> GetFretboard(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["configurationName"]);
            var fretboardConfiguration = fvm.GetConfigurationByName(qName);
            var fretboard = fvm.GetFretboard(fretboardConfiguration);
            return getResponse(fretboard);
        }

        public async Task<APIGatewayProxyResponse> GetFretboardWithIntervals(APIGatewayProxyRequest apiProxyEvent, ILambdaContext context)
        {
            var qName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["configurationName"]);
            var iType = WebUtility.UrlDecode(apiProxyEvent.PathParameters["intervalType"]);
            var iName = WebUtility.UrlDecode(apiProxyEvent.PathParameters["intervalName"]);
            var fretboardConfiguration = fvm.GetConfigurationByName(qName);
            var intervalType = iType;
            IntervalPattern intervals = null;

            if (intervalType == "scale")
            {
                intervals = fvm.GetScaleByName(iName);
            } else if (intervalType == "chord")
            {
                intervals = fvm.GetChordByName(iName);
            }

            var fretboard = fvm.GetFretboard(fretboardConfiguration, intervals);
            return getResponse(fretboard);
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
