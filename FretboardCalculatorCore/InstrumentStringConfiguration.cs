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
        [JsonProperty(PropertyName = "fretCount", Required = Required.Always)]
        public int FretCount;
        [JsonProperty(PropertyName = "startAtFret", Required = Required.DisallowNull)]
        public int StartAtFret;
        [JsonProperty(PropertyName = "tuneTo", Required = Required.Always)]
        public decimal TuneTo;
        [JsonProperty(PropertyName = "octave", Required = Required.Always)]
        public int Octave;
        [JsonProperty(PropertyName = "fretModifiers", Required = Required.DisallowNull)]
        public FretModifier[] FretModifiers;
    }
}
