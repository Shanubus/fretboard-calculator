using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class Fretboard : IJsonOperations
    {
        public Fretboard(FretboardConfiguration configuration, IntervalPattern pattern, string positionValue) : this(configuration, pattern)
        {
            PositionHighlight = positionValue;
        }

        public Fretboard(FretboardConfiguration configuration, IntervalPattern pattern)
        {
            Name = configuration.Name;
            var stringList = new List<InstrumentString>();
            var isFlattedContext = false;
            if (pattern != null)
            {
                isFlattedContext = pattern.IsFlatContext;
            }

            foreach(var instrumentString in configuration.Strings)
            {
                stringList.Add(new InstrumentString(instrumentString.TuneTo, instrumentString.FretCount > 0 ? instrumentString.FretCount : configuration.FretCount, configuration.StepSpan, instrumentString.FretModifiers, instrumentString.StartAtFret == null ? 0 : (int)instrumentString.StartAtFret, isFlattedContext));
            }
            Strings = stringList.ToArray();

            if (pattern != null)
            {
                var usedNoteList = new List<UsedNote>();
                var positionCount = 1;

                usedNoteList.Add(new UsedNote()
                {
                    Index = positionCount - 1,
                    NoteValue = pattern.StartNote,
                    NoteName = Notes.GetNoteName(pattern.StartNote, pattern.IsFlatContext),
                    PositionValue = ToRoman(positionCount),
                    PositionName = (pattern.Positions != null && pattern.Positions[0] != null) ? pattern.Positions[0] : ToRoman(positionCount)
                });
                positionCount++;

                var lastNoteValue = pattern.StartNote;
                decimal lastIntervalValue = 0;
                foreach (var interval in pattern.Intervals)
                {
                    if (pattern.Positions != null && (pattern.Positions.Length == positionCount - 1))
                        continue;

                    var calcNoteValue = lastNoteValue + interval;
                    lastIntervalValue = interval;

                    if (calcNoteValue >= 6)
                        calcNoteValue = calcNoteValue - 6;

                    var checkIt = (from u in usedNoteList where u.NoteValue == calcNoteValue select u).FirstOrDefault();

                    if (checkIt != null)
                        continue;

                    usedNoteList.Add(new UsedNote()
                    {
                        Index = positionCount - 1,
                        NoteValue = calcNoteValue,
                        NoteName = Notes.GetNoteName(calcNoteValue, pattern.IsFlatContext),
                        PositionValue = ToRoman(positionCount),
                        PositionName = (pattern.Positions != null && pattern.Positions[positionCount - 1] != null) ? pattern.Positions[positionCount - 1] : ToRoman(positionCount)
                    });
                    lastNoteValue = calcNoteValue;
                    positionCount++;
                }
                UsedNotes = usedNoteList.ToArray();

                if (PositionHighlight != "")
                    calculatePositionHighlights();
            }
        }

        private void calculatePositionHighlights()
        {

        }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name;
        [JsonProperty(PropertyName = "strings", Required = Required.Always)]
        public InstrumentString[] Strings;
        [JsonProperty(PropertyName = "usedNotes", Required = Required.Default)]
        public UsedNote[] UsedNotes;
        [JsonProperty(PropertyName = "positionHighlight", Required = Required.Default)]
        public string PositionHighlight { get; set; } = "";


        //private int FromRoman(string value)
        //{
        //    switch (value)
        //    {
        //        case "I":
        //            return 1;
        //        case "II":
        //            return 2;
        //        case "III":
        //            return 3;
        //        case "IV":
        //            return 4;
        //        case "V":
        //            return 5;
        //        case "VI":
        //            return 6;
        //        case "VII":
        //            return 7;
        //        case "VIII":
        //            return 8;
        //        case "IX":
        //            return 9;
        //        case "X:
        //            return 10;
        //        case "XI":
        //            return 11;
        //        case "XII":
        //            return 12;
        //        case "XIII":
        //            return 13;
        //        case "XIV":
        //            return 14;
        //        case "XV":
        //            return 15;
        //        default:
        //            return 0;
        //    }
        //}

        private string ToRoman(int number)
        {
            switch (number)
            {
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                case 5:
                    return "V";
                case 6:
                    return "VI";
                case 7:
                    return "VII";
                case 8:
                    return "VIII";
                case 9:
                    return "IX";
                case 10:
                    return "X";
                case 11:
                    return "XI";
                case 12:
                    return "XII";
                case 13:
                    return "XIII";
                case 14:
                    return "XIV";
                case 15:
                    return "XV";
                default:
                    return "";
            }
        }

        public void LoadFromJsonString(string json)
        {
            var newObj = JsonConvert.DeserializeObject<Fretboard>(json);
            this.Name = newObj.Name;
            this.Strings = newObj.Strings;
            this.UsedNotes = newObj.UsedNotes;
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
