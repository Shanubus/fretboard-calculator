using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class IntervalPattern : IJsonOperations
    {
        private decimal _startNoteValue;
        private string _startNoteName;
        private bool _isFlatContext;

        public IntervalPattern()
        {
            _isFlatContext = false;
        }

        public IntervalPattern(decimal startNoteValue, bool isFlatted = false)
        {
            _startNoteValue = startNoteValue;
            _isFlatContext = isFlatted;
            _startNoteName = Notes.GetNoteName(startNoteValue, isFlatted);
        }

        public IntervalPattern(string startNoteName)
        {
            _startNoteName = startNoteName;

            startNoteName = startNoteName.Replace("#", "SHARP");
            startNoteName = startNoteName.Replace("b", "FLAT");

            _startNoteValue = Notes.GetNoteValue(startNoteName);
        }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name;
        [JsonProperty(PropertyName = "startNote", Required = Required.Default)]
        public decimal StartNote
        {
            get { return _startNoteValue; }
            set {
                _startNoteValue = value;
                _startNoteName = Notes.GetNoteName(_startNoteValue, false);
            }
        }
        [JsonProperty(PropertyName = "startNoteName", Required = Required.Default)]
        public string StartNoteName
        {
            get { return _startNoteName; }
            set { _startNoteName = value; _startNoteValue = Notes.GetNoteValue(_startNoteName); }
        }
        [JsonProperty(PropertyName = "isFlatContext", Required = Required.Default)]
        public bool IsFlatContext {
            get
            {
                if (_startNoteName.ToUpper().Contains("FLAT") || _startNoteName.Contains("b") || _startNoteName == "F")
                    _isFlatContext = true;
                else
                    _isFlatContext = false;

                return _isFlatContext;
            }
        }

        [JsonProperty(PropertyName = "intervals", Required = Required.Always)]
        public decimal[] Intervals;
        [JsonProperty(PropertyName = "positionName", Required = Required.Default)]
        public string[] Positions;

        public void LoadFromJsonString(string json)
        {
            var newObj = JsonConvert.DeserializeObject<IntervalPattern>(json);
            this.Name = newObj.Name;
            this.StartNote = newObj.StartNote;
            this.Intervals = newObj.Intervals;
            this.Positions = newObj.Positions;
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
