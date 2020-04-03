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
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name;
        [JsonProperty(PropertyName = "startNote", Required = Required.Always)]
        public decimal StartNote;
        [JsonProperty(PropertyName = "intervals", Required = Required.Always)]
        public decimal[] Intervals;
        [JsonProperty(PropertyName = "positions", Required = Required.DisallowNull)]
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
