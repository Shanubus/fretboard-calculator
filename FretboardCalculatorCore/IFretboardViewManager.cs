using System.Collections.Generic;

namespace FretboardCalculatorCore
{
    public interface IFretboardViewManager
    {
        List<string> GetStoredConfigurationList();
        List<string> GetStoredChordsList();
        List<string> GetStoredScalesList();
        FretboardConfiguration GetConfigurationByName(string name);
        Chord GetChordByName(string name);
        Scale GetScaleByName(string name);
        Fretboard GetFretboard(FretboardConfiguration configuration);
        Fretboard GetFretboard(FretboardConfiguration configuration, IntervalPattern pattern);
    }
}