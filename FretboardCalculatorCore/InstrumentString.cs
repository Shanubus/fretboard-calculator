﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorCore
{
    [JsonObject()]
    public class InstrumentString
    {
        public InstrumentString(decimal tuneTo, int fretCount, decimal stepSpan, FretModifier[] modifiers = null, int startAtFret = 0, bool isFlattedContext = false)
        {
            OpenStringNote = tuneTo;
            StartAtFret = startAtFret;
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
                    fret.NoteName = Notes.GetNoteName(tuneTo, isFlattedContext);
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
                    fret.NoteName = Notes.GetNoteName(lastNote, isFlattedContext);

                    //if (fret.NoteName.Contains('#') || fret.NoteName.Contains('b'))
                    //    fret.NoteName = fret.NoteName + ":" + Notes.GetNoteName(lastNote, true);
                }
            }
        }

        [JsonProperty(PropertyName = "openStringNote", Required = Required.Always)]
        public decimal OpenStringNote;
        [JsonProperty(PropertyName = "startAtFret", Required = Required.Always)]
        public int StartAtFret;
        [JsonProperty(PropertyName = "frets", Required = Required.Always)]
        public Fret[] Frets;
    }
}
