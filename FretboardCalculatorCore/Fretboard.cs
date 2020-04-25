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
                    PositionValue = Notes.ToRoman(positionCount),
                    PositionName = (pattern.Positions != null && pattern.Positions[0] != null) ? pattern.Positions[0] : Notes.ToRoman(positionCount)
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
                        PositionValue = Notes.ToRoman(positionCount),
                        PositionName = (pattern.Positions != null && pattern.Positions[positionCount - 1] != null) ? pattern.Positions[positionCount - 1] : Notes.ToRoman(positionCount)
                    });
                    lastNoteValue = calcNoteValue;
                    positionCount++;
                }
                UsedNotes = usedNoteList.ToArray();

                if (PositionHighlight != "")
                    calculatePositionHighlights(configuration.FretCount);
            }
        }

        private void calculatePositionHighlights(int fretCount)
        {
            var startNote = (from u in UsedNotes where u.PositionValue == PositionHighlight select u.NoteValue).FirstOrDefault();

            foreach (var s in Strings)
            {
                //Skip short strings, assume drone like banjo
                if (s.Frets.Length < fretCount)
                {
                    //Start with first fret, ignore open string
                }
            }
        }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name;
        [JsonProperty(PropertyName = "strings", Required = Required.Always)]
        public InstrumentString[] Strings;
        [JsonProperty(PropertyName = "usedNotes", Required = Required.Default)]
        public UsedNote[] UsedNotes;
        [JsonProperty(PropertyName = "positionHighlight", Required = Required.Default)]
        public string PositionHighlight { get; set; } = "";

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
