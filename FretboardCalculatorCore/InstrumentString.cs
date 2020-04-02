using System;
using System.Collections.Generic;
using System.Text;

namespace FretboardCalculatorCore
{
    public class InstrumentString
    {
        public InstrumentString(decimal tuneTo, int fretCount, decimal stepSpan, FretModifier[] modifiers = null)
        {
            OpenStringNote = tuneTo;
            Frets = new Fret[fretCount+1];
            for (var x = 0; x < Frets.Length; x++)
            {
                Frets[x] = new Fret();
                Frets[x].StepSpan = stepSpan;
            }

            if (modifiers != null)
            {
                foreach(var modifier in modifiers)
                {
                    foreach (var mod in modifier.Frets)
                    {
                        Frets[mod].StepSpan = modifier.StepSpan;
                    }
                }
            }

            decimal lastNote = -1;
            foreach(var fret in Frets)
            {
                if (lastNote == -1)
                {
                    fret.Note = tuneTo;
                    lastNote = tuneTo;
                }
                else
                {
                    lastNote = lastNote + fret.StepSpan;
                    if (lastNote >= 6)
                    {
                        lastNote = 0;
                    }
                    fret.Note = lastNote;
                }
            }
        }

        public decimal OpenStringNote;
        public Fret[] Frets;
    }
}
