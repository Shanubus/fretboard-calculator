using System;
using System.Collections.Generic;
using System.Text;

namespace FretboardCalculatorCore
{
    public class InstrumentStringConfiguration
    {
        public int Index;
        public int FretCount;
        public int StartAtFret;
        public decimal TuneTo;
        public int Octave;
        public FretModifier[] FretModifiers;
    }
}
