using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class Fret
    {
        [JsonProperty(PropertyName = "stepSpan", Required = Required.DisallowNull)]
        public decimal StepSpan;
        [JsonProperty(PropertyName = "note", Required = Required.Always)]
        public decimal Note;
        [JsonProperty(PropertyName = "noteName", Required = Required.Always)]
        public string NoteName;
    }
}
