using System;
using System.Collections.Generic;

namespace FretboardCalculatorCore
{
    public class Fretboard
    {
        public Fretboard(FretboardConfiguration configuration, IntervalPattern pattern)
        {
            Name = configuration.Name;
            var stringList = new List<InstrumentString>();
            foreach(var instrumentString in configuration.Strings)
            {
                stringList.Add(new InstrumentString(instrumentString.TuneTo, instrumentString.FretCount, configuration.StepSpan, instrumentString.FretModifiers));
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
                    PositionValue = ToRoman(positionCount),
                    PositionName = (pattern.Positions != null && pattern.Positions[0] != null) ? pattern.Positions[0] : ToRoman(positionCount)
                });
                positionCount++;

                var lastNoteValue = pattern.StartNote;
                foreach (var interval in pattern.Intervals)
                {
                    var calcNoteValue = lastNoteValue + interval;


                    if (calcNoteValue >= 6)
                        calcNoteValue = calcNoteValue - 6;

                    usedNoteList.Add(new UsedNote()
                    {
                        Index = positionCount - 1,
                        NoteValue = calcNoteValue,
                        PositionValue = ToRoman(positionCount),
                        PositionName = (pattern.Positions != null && pattern.Positions[0] != null) ? pattern.Positions[0] : ToRoman(positionCount)
                    });
                    lastNoteValue = calcNoteValue;
                    positionCount++;
                }
                UsedNotes = usedNoteList.ToArray();
            }
        }

        public string Name;
        public InstrumentString[] Strings;
        public UsedNote[] UsedNotes;

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
    }
}
