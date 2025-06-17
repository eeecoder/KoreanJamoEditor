using System.Linq;

namespace KoreanJamoEditor
{
    public abstract class Letter
    {

        public class Consonant 
        {
            public char Letter { get; set; }
            public int TrailingUnicode;
            public Consonant(char letter, int trailingUnicode)
            {
                Letter = letter;
                TrailingUnicode = trailingUnicode;
                
            }
        }

        public class DoubleConsonant
        {
            public char Letter { get; set; }
            public int TrailingUnicode0;
            public int TrailingUnicode1;
            public DoubleConsonant(char letter, int trailingUnicode0, int trailingUnicode1)
            {
                Letter = letter;
                TrailingUnicode0 = trailingUnicode0;
                TrailingUnicode1 = trailingUnicode1;
            }
        }

        public class Vowel
        {
            public enum Orientation
            {
                Horizontal,
                Vertical,
                Both
            }
            public char Letter { get; set; }
            public int MedialUnicode;
            public Orientation MyOrientation;

            public Vowel(char letter, int medialUnicode, Orientation orientation)
            {
                Letter = letter;
                MedialUnicode = medialUnicode;
                MyOrientation = orientation;
            }
        }

        public class DoubleVowel
        {
            public char Letter { get; set; }
            public int MedialUnicode0;
            public int MedialUnicode1;

            public DoubleVowel(char letter, int medialUnicode0, int medialUnicode1)
            {
                Letter = letter;
                MedialUnicode0 = medialUnicode0;
                MedialUnicode1 = medialUnicode1;
            }
        }


        // Single consonants
        static readonly Consonant c_11A8 = new('ᆨ', 0x11A8);
        static readonly Consonant c_11AB = new('ᆫ', 0x11AB);
        static readonly Consonant c_11AE = new('ᆮ', 0x11AE);
        static readonly Consonant c_11AF = new('ᆯ', 0x11AF);
        static readonly Consonant c_11B7 = new('ᆷ', 0x11B7);
        static readonly Consonant c_11B8 = new('ᆸ', 0x11B8);
        static readonly Consonant c_11BA = new('ᆺ', 0x11BA);
        static readonly Consonant c_11BC = new('ᆼ', 0x11BC);
        static readonly Consonant c_11BD = new('ᆽ', 0x11BD);
        static readonly Consonant c_11BE = new('ᆾ', 0x11BE);
        static readonly Consonant c_11BF = new('ᆿ', 0x11BF);
        static readonly Consonant c_11C0 = new('ᇀ', 0x11C0);
        static readonly Consonant c_11C1 = new('ᇁ', 0x11C1);
        static readonly Consonant c_11C2 = new('ᇂ', 0x11C2);

        // Double consonants 
        static readonly DoubleConsonant c_11A9 = new('ᆩ', c_11A8.TrailingUnicode, c_11A8.TrailingUnicode);
        static readonly DoubleConsonant c_11AA = new('ᆪ', c_11A8.TrailingUnicode, c_11BA.TrailingUnicode);
        static readonly DoubleConsonant c_11AC = new('ᆬ', c_11AB.TrailingUnicode, c_11BD.TrailingUnicode);
        static readonly DoubleConsonant c_11AD = new('ᆭ', c_11AB.TrailingUnicode, c_11C2.TrailingUnicode);
        static readonly DoubleConsonant c_11B0 = new('ᆰ', c_11AF.TrailingUnicode, c_11A8.TrailingUnicode);
        static readonly DoubleConsonant c_11B1 = new('ᆱ', c_11AF.TrailingUnicode, c_11B7.TrailingUnicode);
        static readonly DoubleConsonant c_11B2 = new('ᆲ', c_11AF.TrailingUnicode, c_11B8.TrailingUnicode);
        static readonly DoubleConsonant c_11B3 = new('ᆳ', c_11AF.TrailingUnicode, c_11BA.TrailingUnicode);
        static readonly DoubleConsonant c_11B4 = new('ᆴ', c_11AF.TrailingUnicode, c_11C0.TrailingUnicode);
        static readonly DoubleConsonant c_11B5 = new('ᆵ', c_11AF.TrailingUnicode, c_11C1.TrailingUnicode);
        static readonly DoubleConsonant c_11B6 = new('ᆶ', c_11AF.TrailingUnicode, c_11C2.TrailingUnicode);
        static readonly DoubleConsonant c_11B9 = new('ᆹ', c_11B8.TrailingUnicode, c_11BA.TrailingUnicode);
        static readonly DoubleConsonant c_11BB = new('ᆻ', c_11BA.TrailingUnicode, c_11BA.TrailingUnicode);


        static readonly DoubleConsonant[] doubleConsonantList = new DoubleConsonant[]
        {
            c_11A9, c_11AA, 
            c_11AC, c_11AD, 
            c_11B0, c_11B1, c_11B2, c_11B3, c_11B4, c_11B5, c_11B6, 
            c_11B9, 
            c_11BB
        };

        public static readonly Dictionary<char, DoubleConsonant> DoubleConsonants = GetDoubleConsonantsDictionary();

        public static Dictionary<char, DoubleConsonant> GetDoubleConsonantsDictionary()
        {
            Dictionary<char, DoubleConsonant> myDict = new();
            foreach (DoubleConsonant doubleConsonant in doubleConsonantList)
            {
                myDict.Add((char)doubleConsonant.Letter, doubleConsonant);
            }
            return myDict;
        }


        // Vowels
        static readonly Vowel v_1161 = new('ᅡ', 0x1161, Vowel.Orientation.Vertical);
        static readonly Vowel v_1162 = new('ᅢ', 0x1162, Vowel.Orientation.Vertical);
        static readonly Vowel v_1163 = new('ᅣ', 0x1163, Vowel.Orientation.Vertical);
        static readonly Vowel v_1164 = new('ᅤ', 0x1164, Vowel.Orientation.Vertical);
        static readonly Vowel v_1165 = new('ᅥ', 0x1165, Vowel.Orientation.Vertical);
        static readonly Vowel v_1166 = new('ᅦ', 0x1166, Vowel.Orientation.Vertical);
        static readonly Vowel v_1167 = new('ᅧ', 0x1167, Vowel.Orientation.Vertical);
        static readonly Vowel v_1168 = new('ᅨ', 0x1168, Vowel.Orientation.Vertical);
        static readonly Vowel v_1169 = new('ᅩ', 0x1169, Vowel.Orientation.Horizontal);
        static readonly Vowel v_116D = new('ᅭ', 0x116D, Vowel.Orientation.Horizontal);
        static readonly Vowel v_116E = new('ᅮ', 0x116E, Vowel.Orientation.Horizontal);
        static readonly Vowel v_1172 = new('ᅲ', 0x1172, Vowel.Orientation.Horizontal);
        static readonly Vowel v_1173 = new('ᅳ', 0x1173, Vowel.Orientation.Horizontal);
        static readonly Vowel v_1175 = new('ᅵ', 0x1175, Vowel.Orientation.Vertical);
        static readonly Vowel v_116A = new('ᅪ', 0x116A, Vowel.Orientation.Both);
        static readonly Vowel v_116B = new('ᅫ', 0x116B, Vowel.Orientation.Both);
        static readonly Vowel v_116C = new('ᅬ', 0x116C, Vowel.Orientation.Both);
        static readonly Vowel v_116F = new('ᅯ', 0x116F, Vowel.Orientation.Both);
        static readonly Vowel v_1170 = new('ᅰ', 0x1170, Vowel.Orientation.Both);
        static readonly Vowel v_1171 = new('ᅱ', 0x1171, Vowel.Orientation.Both);
        static readonly Vowel v_1174 = new('ᅴ', 0x1174, Vowel.Orientation.Both);

        static readonly Vowel[] vowelList = new Vowel[] {
            v_1161, v_1162, v_1163, v_1164, v_1165, v_1166, v_1167, v_1168, 
            v_1169, v_116D, v_116E, v_1172, v_1173, 
            v_1175,
            v_116A, v_116B, v_116C, v_116F, v_1170, v_1171, v_1174

        };

        public static readonly Dictionary<char, Vowel> Vowels = GetVowelsDictionary();

        public static Dictionary<char, Vowel> GetVowelsDictionary()
        {
            Dictionary<char, Vowel> myDict = new();
            foreach (Vowel vowel in vowelList)
            {
                myDict.Add((char)vowel.Letter, vowel);
            }
            return myDict;
        }

        // Double Vowels
        static readonly DoubleVowel dv_116A = new('ᅪ', v_1169.MedialUnicode, v_1161.MedialUnicode);
        static readonly DoubleVowel dv_116B = new('ᅫ', v_1169.MedialUnicode, v_1162.MedialUnicode);
        static readonly DoubleVowel dv_116C = new('ᅬ', v_1169.MedialUnicode, v_1175.MedialUnicode);
        static readonly DoubleVowel dv_116F = new('ᅯ', v_116E.MedialUnicode, v_1165.MedialUnicode);
        static readonly DoubleVowel dv_1170 = new('ᅰ', v_116E.MedialUnicode, v_1166.MedialUnicode);
        static readonly DoubleVowel dv_1171 = new('ᅱ', v_116E.MedialUnicode, v_1175.MedialUnicode);
        static readonly DoubleVowel dv_1174 = new('ᅴ', v_1173.MedialUnicode, v_1175.MedialUnicode);

        static readonly DoubleVowel[] doubleVowelList =
        [
            dv_116A, dv_116B, dv_116C, 
            dv_116F, dv_1170, dv_1171, 
            dv_1174
        ];

        public static readonly Dictionary<char, DoubleVowel> DoubleVowels = GetDoubleVowelsDictionary();

        public static Dictionary<char, DoubleVowel> GetDoubleVowelsDictionary()
        {
            Dictionary<char, DoubleVowel> myDict = new();
            foreach (DoubleVowel doubleVowel in doubleVowelList)
            {
                myDict.Add((char)doubleVowel.Letter, doubleVowel);
            }
            return myDict;
        }



    }
}