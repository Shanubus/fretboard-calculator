using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class FretModifier
    {
        [JsonProperty(PropertyName ="frets", Required = Required.Always)]
        public int[] Frets;
        [JsonProperty(PropertyName = "stepSpan", Required = Required.Always)]
        public decimal StepSpan;
    }
}
