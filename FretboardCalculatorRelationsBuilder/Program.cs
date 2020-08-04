using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FretboardCalculatorCore;
using System.Collections;
using System.Collections.Generic;

namespace FretboardCalculatorRelationsBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var intervalFile = "";
                var compareIntervalFile = "";
                var outputFile = "";
                var intervalType = "";
                var compareIntervalType = "";

                if (args == null || args.Length == 0 || args[0].Contains("-help"))
                {
                    Console.WriteLine("\r\n*Help Information*");
                    Console.WriteLine("-----------------------------\r\n");
                    Console.WriteLine("-p path_to_interval_file");
                    Console.WriteLine("-t Type of pattern in interval file, use CHORD or SCALE");
                    Console.WriteLine("-cp path_to_comparison_interval_file");
                    Console.WriteLine("-ct Type of pattern in comparison file, use CHORD or SCALE");
                    Console.WriteLine("-o path_to_output_file");
                    Console.WriteLine("FretboardCalculatorRelationsBuilder -p c:\\json_data\\scales_file.json -t SCALE -cp c:\\json_data\\chords_file.json -ct CHORD -o c:\\json_data\\scale_relationship_file.json");
                    return;
                }
                else if (args.Length != 10)
                {
                    Console.WriteLine("Invalid Parameters, see --help");
                    return;
                }

                //Get parameters from args
                getInputParameters(args, out intervalFile, out compareIntervalFile, out outputFile, out intervalType, out compareIntervalType);

                //Open the interval file
                var intervalJsonString = File.ReadAllText(intervalFile);
                var compareIntervalJsonString = File.ReadAllText(compareIntervalFile);

                var intervalJson = JsonConvert.DeserializeObject<IntervalPattern[]>(intervalJsonString);
                var compareIntervalJson = JsonConvert.DeserializeObject<IntervalPattern[]>(compareIntervalJsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:" + ex.Message);
            }
        }

        private static void getInputParameters(string[] args, out string intervalFile, out string compareIntervalFile, out string outputFile, out string intervalType, out string compareIntervalType)
        {
            intervalFile = "";
            compareIntervalFile = "";
            outputFile = "";
            intervalType = "";
            compareIntervalType = "";

            for (var c = 0; c < args.Length; c++)
            {
                switch (args[c]) {
                    case "-p":
                        intervalFile = args[c + 1];
                        break;
                    case "-t":
                        intervalType = args[c + 1].ToUpper();
                        break;
                    case "-cp":
                        compareIntervalFile = args[c + 1];
                        break;
                    case "-ct":
                        compareIntervalType = args[c + 1].ToUpper();
                        break;
                    case "-o":
                        outputFile = args[c + 1];
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
