using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    public class FretboardViewManager : IFretboardViewManager
    {
        private string _configurationsPath;
        private string _scalesPath;
        private string _chordsPath;

        public FretboardViewManager(string configurationsPath, string scalesPath, string chordsPath)
        {
            _configurationsPath = configurationsPath;
            _scalesPath = scalesPath;
            _chordsPath = chordsPath;
        }

        public Fretboard GetFretboard(FretboardConfiguration configuration)
        {
            return GetFretboard(configuration, null);
        }

        public Fretboard GetFretboard(FretboardConfiguration configuration, IntervalPattern pattern)
        {
            return new Fretboard(configuration, pattern);
        }

        public List<string> GetStoredConfigurationList()
        {
            return GetNameListFromJsonArray(_configurationsPath);
        }

        public List<string> GetStoredScalesList()
        {
            return GetNameListFromJsonArray(_scalesPath);
        }

        public List<string> GetStoredChordsList()
        {
            return GetNameListFromJsonArray(_chordsPath);
        }

        public FretboardConfiguration GetConfigurationByName(string name)
        {
            return null;
        }

        public Scale GetScaleByName(string name)
        {
            return null;
        }

        public Chord GetChordByName(string name)
        {
            return null;
        }

        private List<string> GetNameListFromJsonArray(string jsonPath)
        {
            return new List<string>();
        }
    }
}
