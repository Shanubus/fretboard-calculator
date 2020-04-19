using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Linq;

namespace FretboardCalculatorCore
{
    public class FretboardViewManager : IFretboardViewManager
    {
        private string _configurationsPath;
        private string _scalesPath;
        private string _chordsPath;

        private FretboardConfiguration[] _configurations;
        private Scale[] _scales;
        private Chord[] _chords;

        public FretboardViewManager(string configurationsPath, string scalesPath, string chordsPath)
        {
            _configurationsPath = configurationsPath;
            _scalesPath = scalesPath;
            _chordsPath = chordsPath;

            var configurationsJson = new WebClient().DownloadString(_configurationsPath);
            var scalesJson = new WebClient().DownloadString(_scalesPath);
            var chordsJson = new WebClient().DownloadString(_chordsPath);

            _configurations = JsonConvert.DeserializeObject<FretboardConfiguration[]>(configurationsJson);
            _scales = JsonConvert.DeserializeObject<Scale[]>(scalesJson);
            _chords = JsonConvert.DeserializeObject<Chord[]>(chordsJson);
        }

        public Fretboard GetFretboard(FretboardConfiguration configuration)
        {
            return GetFretboard(configuration, null);
        }

        public Fretboard GetFretboard(FretboardConfiguration configuration, IntervalPattern pattern)
        {
            return new Fretboard(configuration, pattern);
        }

        public Fretboard GetFretboard(FretboardConfiguration configuration, IntervalPattern pattern, string positionValue)
        {
            return new Fretboard(configuration, pattern, positionValue);
        }

        public List<string> GetStoredConfigurationList()
        {
            return (from c in _configurations orderby c.Name select c.Name).ToList<string>();
        }

        public List<string> GetStoredScalesList()
        {
            return (from s in _scales orderby s.Name select s.Name).ToList<string>();
        }

        public List<string> GetStoredChordsList()
        {
            return (from c in _chords orderby c.Name select c.Name).ToList<string>();
        }

        public FretboardConfiguration GetConfigurationByName(string name)
        {
            return (from c in _configurations where c.Name == name select c).FirstOrDefault<FretboardConfiguration>();
        }

        public Scale GetScaleByName(string name)
        {
            return (Scale)(from s in _scales where s.Name == name select s).FirstOrDefault<IntervalPattern>();
        }

        public Chord GetChordByName(string name)
        {
            return (Chord)(from c in _chords where c.Name == name select c).FirstOrDefault<IntervalPattern>();
        }
    }
}
