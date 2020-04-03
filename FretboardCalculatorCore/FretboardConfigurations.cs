using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonArray()]
    public class FretboardConfigurations : IJsonOperations
    {
        public FretboardConfiguration[] Configurations;

        public void LoadFromJsonString(string json)
        {
            var newObj = JsonConvert.DeserializeObject<FretboardConfigurations>(json);
            this.Configurations = newObj.Configurations;
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
