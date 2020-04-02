using System;
using System.Collections.Generic;
using System.Text;

namespace FretboardCalculatorCore
{
    public class FretboardConfiguration
    {
        public string Name;
        public int OctaveBase;
        public int FretCount;
        public decimal StepSpan;
        public InstrumentStringConfiguration[] Strings;
    }
}
