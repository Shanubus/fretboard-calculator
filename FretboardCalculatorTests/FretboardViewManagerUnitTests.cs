using System;
using Xunit;
using System.Collections.Generic;
using FretboardCalculatorCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FretboardCalculatorTests
{
    public class FretboardViewManagerUnitTests
    {
        private const string configurationsPath = "https://fretboard-calculator-json.s3.amazonaws.com/fretboard-configurations.json";
        private const string scalesPath = "https://fretboard-calculator-json.s3.amazonaws.com/scales.json";
        private const string chordsPath = "https://fretboard-calculator-json.s3.amazonaws.com/chords.json";

        private FretboardViewManager _fvm;
        public FretboardViewManagerUnitTests()
        {
            _fvm = getFretboardViewManager();
        }

        [Fact]
        public void GetFretboardWithoutPatternTest()
        {
            var fretboard = _fvm.GetFretboard(getTwoStringBassForTesting());

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
            var fretboard = _fvm.GetFretboard(get5StringBanjoForTesting(), getScaleWithModesForTesting());

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
                fretboard.UsedNotes[0].NoteValue == 0M &&
                fretboard.UsedNotes[1].NoteValue == 1M &&
                fretboard.UsedNotes[2].NoteValue == 2M &&
                fretboard.UsedNotes[3].NoteValue == 2.5M &&
                fretboard.UsedNotes[4].NoteValue == 3.5M,
                "Key Values Do Not Match"
                );
        }
        [Fact]
        public void GetConfigurationsListTest()
        {
            var configurationList = _fvm.GetStoredConfigurationList();
            Assert.True(
                configurationList != null &&
                configurationList[0].Length > 0,
                "Key Values Do Not Match"
                );
        }
        [Fact]
        public void GetScalesListTest()
        {
            var scalesList = _fvm.GetStoredScalesList();
            Assert.True(
                scalesList != null &&
                scalesList[0].Length > 0,
                "Key Values Do Not Match"
                );
        }
        [Fact]
        public void GetChordsListTest()
        {
            var chordsList = _fvm.GetStoredChordsList();
            Assert.True(
                chordsList != null &&
                chordsList[0].Length > 0,
                "Key Values Do Not Match"
                );
        }
        [Fact]
        public void GetConfigurationByNameTest()
        {
            var conf = _fvm.GetConfigurationByName("24 Fret 6 String Guitar");
            Assert.True(conf.Name == "24 Fret 6 String Guitar", "Failed to get configuration by name");
        }
        [Fact]
        public void GetChordByNameTest()
        {
            var chord = _fvm.GetChordByName("Major Chord");
            Assert.True(chord.Name == "Major Chord", "Failed to get chord by name");
        }
        [Fact]
        public void GetScaleByNameTest()
        {
            var scale = _fvm.GetScaleByName("Major Scale");
            Assert.True(scale.Name == "Major Scale", "Failed to get scale by name");
        }

        private FretboardViewManager getFretboardViewManager()
        {
            return new FretboardViewManager(configurationsPath, scalesPath, chordsPath);
        }

        private IntervalPattern getIntervalPatternForTesting()
        {
            return new IntervalPattern()
            {
                Name = "Whole Half, Half Whole",
                StartNote = 2.5M,
                Intervals = new decimal[] { 1, 0.5M, 0.5M, 1 }
            };
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

        private FretboardConfiguration get5StringBanjoForTesting()
        {
            string banjoJson = "{\"name\":\"22 Fret 5 String Banjo\",\"octaveBase\":2,\"fretCount\":22,\"stepSpan\":0.5,\"strings\":[{\"index\":0,\"fretCount\":17,\"startAtFret\":5,\"tuneTo\":3.5,\"octave\":2},{\"index\":1,\"tuneTo\":1.0,\"octave\":0},{\"index\":2,\"tuneTo\":3.5,\"octave\":0},{\"index\":3,\"tuneTo\":5.5,\"octave\":0},{\"index\":4,\"tuneTo\":1.0,\"octave\":1}]}";
            var conf = JsonConvert.DeserializeObject<FretboardConfiguration>(banjoJson);
            return conf;
        }

        private IntervalPattern getScaleWithModesForTesting()
        {
            string scaleJson = "{\"name\": \"Major Scale\",\"intervals\": [ 1, 1, 0.5, 1, 1, 1, 0.5 ],\"positionName\": [\"Ionian\",\"Dorian\",\"Phrygian\",\"Lydian\",\"Mixolydian\",\"Aeolian\",\"Locrian\"],\"startNote\": 0,\"isFlatContext\": false}";
            var conf = JsonConvert.DeserializeObject<IntervalPattern>(scaleJson);
            return conf;
        }
    }
}
