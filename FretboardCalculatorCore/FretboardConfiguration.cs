using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class FretboardConfiguration : IJsonOperations
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name;
        [JsonProperty(PropertyName = "octaveBase", Required = Required.Always)]
        public int OctaveBase;
        [JsonProperty(PropertyName = "fretCount", Required = Required.Always)]
        public int FretCount;
        [JsonProperty(PropertyName = "stepSpan", Required = Required.Always)]
        public decimal StepSpan;
        [JsonProperty(PropertyName = "strings", Required = Required.Always)]
        public InstrumentStringConfiguration[] Strings;

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void LoadFromJsonString(string json)
        {
            var newObj = JsonConvert.DeserializeObject<FretboardConfiguration>(json);
            this.Name = newObj.Name;
            this.OctaveBase = newObj.OctaveBase;
            this.FretCount = newObj.FretCount;
            this.StepSpan = newObj.StepSpan;
            this.Strings = newObj.Strings;
        }
    }
}
