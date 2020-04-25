using System;
using System.Collections.Generic;
using System.Text;

namespace FretboardCalculatorCore
{
    public static class Notes
    {
        public const decimal C = 0;
        public const decimal Csharp = .5M;
        public const decimal D = 1;
        public const decimal Dsharp = 1.5M;
        public const decimal E = 2;
        public const decimal F = 2.5M;
        public const decimal Fsharp = 3;
        public const decimal G = 3.5M;
        public const decimal Gsharp = 4;
        public const decimal A = 4.5M;
        public const decimal Asharp = 5;
        public const decimal B = 5.5M;

        public static decimal GetNoteValue(string noteName)
        {
            switch (noteName.ToUpper())
            {
                case "C":
                    return Notes.C;
                case "CSHARP":
                case "DFLAT":
                    return Notes.Csharp;
                case "D":
                    return Notes.D;
                case "DSHARP":
                case "EFLAT":
                    return Notes.Dsharp;
                case "E":
                    return Notes.E;
                case "F":
                    return Notes.F;
                case "FSHARP":
                case "GFLAT":
                    return Notes.Fsharp;
                case "G":
                    return Notes.G;
                case "GSHARP":
                case "AFLAT":
                    return Notes.Gsharp;
                case "A":
                    return Notes.A;
                case "ASHARP":
                case "BFLAT":
                    return Notes.Asharp;
                case "B":
                    return Notes.B;
                default:
                    return Notes.C;
            }
        }

        public static string GetNoteName(decimal note, bool flatted = false)
        {
            var noteName = "";
            switch (note)
            {
                case Notes.C:
                    noteName = "C";
                    break;
                case Notes.Csharp:
                    noteName = "C#";
                    if (flatted)
                        noteName = "Db";
                    break;
                case Notes.D:
                    noteName = "D";
                    break;
                case Notes.Dsharp:
                    noteName = "D#";
                    if (flatted)
                        noteName = "Eb";
                    break;
                case Notes.E:
                    noteName = "E";
                    break;
                case Notes.F:
                    noteName = "F";
                    break;
                case Notes.Fsharp:
                    noteName = "F#";
                    if (flatted)
                        noteName = "Gb";
                    break;
                case Notes.G:
                    noteName = "G";
                    break;
                case Notes.Gsharp:
                    noteName = "G#";
                    if (flatted)
                        noteName = "Ab";
                    break;
                case Notes.A:
                    noteName = "A";
                    break;
                case Notes.Asharp:
                    noteName = "A#";
                    if (flatted)
                        noteName = "Bb";
                    break;
                case Notes.B:
                    noteName = "B";
                    break;
                default:
                    noteName = "";
                    break;
            }
            return noteName;
        }

        public static string ToRoman(int number)
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

        public static int FromRoman(string positionValue)
        {
            switch (positionValue)
            {
                case "I":
                    return 1;
                case "II":
                    return 2;
                case "III":
                    return 3;
                case "IV":
                    return 4;
                case "V":
                    return 5;
                case "VI":
                    return 6;
                case "VII":
                    return 7;
                case "VIII":
                    return 8;
                case "IX":
                    return 9;
                case "X":
                    return 10;
                case "XI":
                    return 11;
                case "XII":
                    return 12;
                case "XIII":
                    return 13;
                case "XIV":
                    return 14;
                case "XV":
                    return 15;
                default:
                    return 0;
            }
        }

    }
}
