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

            var usedNoteList = new List<UsedNote>();
            var positionCount = 1;

            usedNoteList.Add(new UsedNote()
            {
                NoteValue = pattern.StartNote,
                PositionValue = ToRoman(positionCount),
                PositionName = (pattern.Positions != null && pattern.Positions[0] != null) ? pattern.Positions[0] : ToRoman(positionCount)
            });
            positionCount = positionCount++;

            var lastNoteValue = pattern.StartNote;
            foreach (var interval in pattern.Intervals)
            {
                var calcNoteValue = lastNoteValue + interval;


                if (calcNoteValue >= 6)
                    calcNoteValue = calcNoteValue - 6;

                usedNoteList.Add(new UsedNote()
                {
                    NoteValue = calcNoteValue,
                    PositionValue = ToRoman(positionCount),
                    PositionName = (pattern.Positions != null && pattern.Positions[0] != null) ? pattern.Positions[0] : ToRoman(positionCount)
                });
                lastNoteValue = calcNoteValue;
                positionCount = positionCount++;
            }
            UsedNotes = usedNoteList.ToArray();
        }

        public string Name;
        public InstrumentString[] Strings;
        public UsedNote[] UsedNotes;

        private string ToRoman(int number)
        {
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("Out of Range for Roman");
        }
    }
}
