using System;
using Xunit;
using System.Collections.Generic;
using FretboardCalculatorCore;

namespace FretboardCalculatorTests
{
    public class FretboardViewManagerUnitTests
    {
        [Fact]
        public void GetFretboardWithoutPatternTest()
        {
            var fvm = new FretboardViewManager("", "", "");
            var fretboard = fvm.GetFretboard(getTwoStringBassForTesting());

            Assert.True(
                fretboard.Strings.Length == 2 &&
                fretboard.Strings[0].Frets.Length == 23 &&
                fretboard.Strings[1].Frets.Length == 23 &&
                fretboard.Strings[0].OpenStringNote == 2 &&
                fretboard.Strings[1].OpenStringNote == 4.5M &&
                fretboard.Strings[0].Frets[1].Note == 2.5M &&
                fretboard.Strings[0].Frets[2].Note == 3 &&
                fretboard.Strings[1].Frets[1].Note == 5 &&
                fretboard.Strings[1].Frets[2].Note == 5.5M &&
                fretboard.UsedNotes == null,
                "Key Values Do Not Match"
                );
        }
        [Fact]
        public void GetFretboardWithPatternTest()
        {
            var fvm = new FretboardViewManager("", "", "");
            var fretboard = fvm.GetFretboard(getTwoStringBassForTesting(), new IntervalPattern() {
                Name = "Whole Half, Half Whole",
                StartNote = 2.5M,
                Intervals = new decimal[] { 1, 0.5M, 0.5M, 1 }
            });

            Assert.True(
                fretboard.UsedNotes[0].Index == 0 &&
                fretboard.UsedNotes[1].Index == 1 &&
                fretboard.UsedNotes[2].Index == 2 &&
                fretboard.UsedNotes[3].Index == 3 &&
                fretboard.UsedNotes[4].Index == 4 &&
                fretboard.UsedNotes[0].PositionValue == "I" &&
                fretboard.UsedNotes[1].PositionValue == "II" &&
                fretboard.UsedNotes[2].PositionValue == "III" &&
                fretboard.UsedNotes[3].PositionValue == "IV" &&
                fretboard.UsedNotes[4].PositionValue == "V" &&
                fretboard.UsedNotes[0].NoteValue == 2.5M &&
                fretboard.UsedNotes[1].NoteValue == 3.5M &&
                fretboard.UsedNotes[2].NoteValue == 4 &&
                fretboard.UsedNotes[3].NoteValue == 4.5M &&
                fretboard.UsedNotes[4].NoteValue == 5.5M,
                "Key Values Do Not Match"
                );
        }
        [Fact]
        public void GetConfigurationsListTest()
        {
            Assert.True(1 == 2, "Not Implemented");
        }
        [Fact]
        public void GetScalesListTest()
        {
            Assert.True(1 == 2, "Not Implemented");
        }
        [Fact]
        public void GetChordsListTest()
        {
            Assert.True(1 == 2, "Not Implemented");
        }
        [Fact]
        public void GetConfigurationByNameTest()
        {
            Assert.True(1 == 2, "Not Implemented");
        }
        [Fact]
        public void GetChordByNameTest()
        {
            Assert.True(1 == 2, "Not Implemented");
        }
        [Fact]
        public void GetScaleByNameTest()
        {
            Assert.True(1 == 2, "Not Implemented");
        }

        private FretboardConfiguration getTwoStringBassForTesting()
        {
            var stringList = new List<InstrumentStringConfiguration>();

            stringList.Add(
                new InstrumentStringConfiguration()
                {
                    FretCount = 22,
                    FretModifiers = null,
                    Octave = 0,
                    StartAtFret = 0,
                    Index = 0,
                    TuneTo = 2
                }
                );
            stringList.Add(
                new InstrumentStringConfiguration()
                {
                    FretCount = 22,
                    FretModifiers = null,
                    Octave = 0,
                    StartAtFret = 0,
                    Index = 1,
                    TuneTo = 4.5M
                }
                );

            return new FretboardConfiguration()
            {
                Name = "Test 2 String Bass Guitar",
                FretCount = 22,
                OctaveBase = 0,
                StepSpan = 0.5M,
                Strings = stringList.ToArray()
            };
        }
    }
}
