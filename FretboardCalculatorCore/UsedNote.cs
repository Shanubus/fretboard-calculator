using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class UsedNote
    {
        [JsonProperty(PropertyName = "index", Required = Required.Always)]
        public int Index;
        [JsonProperty(PropertyName = "noteValue", Required = Required.Always)]
        public decimal NoteValue;
        [JsonProperty(PropertyName = "positionValue", Required = Required.Always)]
        public string PositionValue;
        [JsonProperty(PropertyName = "positionName", Required = Required.DisallowNull)]
        public string PositionName;
    }
}
