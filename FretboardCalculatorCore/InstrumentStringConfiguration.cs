using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class InstrumentStringConfiguration
    {
        [JsonProperty(PropertyName = "index", Required = Required.Always)]
        public int Index;
        [JsonProperty(PropertyName = "fretCount", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FretCount;
        [JsonProperty(PropertyName = "startAtFret", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? StartAtFret;
        [JsonProperty(PropertyName = "tuneTo", Required = Required.Always)]
        public decimal TuneTo;
        [JsonProperty(PropertyName = "octave", Required = Required.Always)]
        public int Octave;
        [JsonProperty(PropertyName = "fretModifiers", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public FretModifier[] FretModifiers;
    }
}
