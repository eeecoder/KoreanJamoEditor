using HangulJamoEditor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using static KoreanJamoEditor.Letter;
using static KoreanJamoEditor.Letter.Vowel;


namespace KoreanJamoEditor
{
    // Map a syllable block to its decomposed positional forms.

    public enum SpanType
    {
        CV_HorizontalVowel,
        CV_VerticalVowel,
        CVV,
        CVC_HorizontalVowel,
        CVC_VerticalVowel,
        CVVC,
        CVCC_HorizontalVowel,
        CVCC_VerticalVowel,
        CVVCC,
        Latin
    }

	public static class RawInputManager
	{
        private static RichTextBox _rawInputArea;
        private static RichTextBox _inputArea;
        public static Dictionary<Char, SyllableBlock> syllables = GetSyllables();
        public static Dictionary<Char, EnhancedSyllableBlock> enhancedSyllables = GetEnhancedSyllables();
        public static Boolean EnhancedMode = true; 

        public static void Init(RichTextBox rawInputArea, RichTextBox inputArea)
        {
            _rawInputArea = rawInputArea;
            _inputArea = inputArea;
            
            /*
            Debug.WriteLine($"syllables: {syllables.Count}");
            foreach (KeyValuePair<Char, SyllableBlock> kvp in syllables)
            {
                Debug.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
         
            Debug.WriteLine($"enhanced syllables: {enhancedSyllables.Count}");
            foreach (KeyValuePair<Char, EnhancedSyllableBlock> kvp in enhancedSyllables)
            {
                Debug.WriteLine("Key = {0}, Value = {1}\n", kvp.Key, kvp.Value);
            }
            */
        }

        private static Run CreateRun(char content)
        {
            return new Run(content.ToString());

        }

        private static void AppendRunToKoreanSpan(char content, Span span)
        {
           if (content == IOManager.Symbol.EMPTY_CHAR)
            {
                return;
            }
            Run run = CreateRun(content);
            span.Inlines.Add(run);
        }

        private static SpanType GetKoreanSpanType(char initial, char medial, Letter.Vowel.Orientation medialOrientation, char final)
        {
            if (final == 0 && medialOrientation == Letter.Vowel.Orientation.Horizontal)
                return SpanType.CV_HorizontalVowel;
            if (final == 0 && medialOrientation == Letter.Vowel.Orientation.Vertical)
                return SpanType.CV_VerticalVowel;
            if (final == 1 && medialOrientation == Letter.Vowel.Orientation.Horizontal)
                return SpanType.CVC_HorizontalVowel;
            if (final == 1 && medialOrientation == Letter.Vowel.Orientation.Vertical)
                return SpanType.CVC_VerticalVowel;
            return SpanType.CVC_VerticalVowel;
        }


        private static SpanType GetEnhancedKoreanSpanType(char initial, 
            char medial0, Letter.Vowel.Orientation medialOrientation0, char medial1, Letter.Vowel.Orientation medialOrientation1,
            char final0, char final1)
        {
            int numVowels = (medial1 != 0) ? 2 : 1;
            int numTrailingConsonants = 0;
            if (final0 != IOManager.Symbol.EMPTY_CHAR) numTrailingConsonants += 1;
            if (final1 != IOManager.Symbol.EMPTY_CHAR) numTrailingConsonants += 1;

            if (numVowels == 1 && numTrailingConsonants == 0 && medialOrientation0 == Letter.Vowel.Orientation.Horizontal)
                return SpanType.CV_HorizontalVowel;
            if (numVowels == 1 && numTrailingConsonants == 0 && medialOrientation0 == Letter.Vowel.Orientation.Vertical)
                return SpanType.CV_VerticalVowel;
            if (numVowels == 2 && numTrailingConsonants == 0) return SpanType.CVV;
            if (numVowels == 1 && numTrailingConsonants == 1 && medialOrientation0 == Letter.Vowel.Orientation.Horizontal) 
                return SpanType.CVC_HorizontalVowel;
            if (numVowels == 1 && numTrailingConsonants == 1 && medialOrientation0 == Letter.Vowel.Orientation.Vertical)
                return SpanType.CVC_VerticalVowel;
            if (numVowels == 2 && numTrailingConsonants == 1) return SpanType.CVVC;
            if (numVowels == 1 && numTrailingConsonants == 2 && medialOrientation0 == Letter.Vowel.Orientation.Horizontal) 
                    return SpanType.CVCC_HorizontalVowel;
            if (numVowels == 1 && numTrailingConsonants == 2 && medialOrientation0 == Letter.Vowel.Orientation.Vertical)
                return SpanType.CVCC_VerticalVowel;
            if (numVowels == 2 && numTrailingConsonants == 2) return SpanType.CVVCC;
            return SpanType.CVC_VerticalVowel;
        }

        private static Span CreateKoreanSpan(char initial, char medial, Letter.Vowel.Orientation medialOrientation, char final)
        {
            Span span = new();
            span.Tag = GetKoreanSpanType(initial, medial, medialOrientation, final);
            AppendRunToKoreanSpan(initial, span);
            AppendRunToKoreanSpan(medial, span);
            AppendRunToKoreanSpan(final, span);
            AppendRunToKoreanSpan(IOManager.Symbol.DELIMITER_CHAR, span);
            return span;
        }

        
        private static Span CreateEnhancedKoreanSpan(char initial, 
            char medial0, Letter.Vowel.Orientation medialOrientation0, char medial1, Letter.Vowel.Orientation medialOrientation1,
            char final0, char final1)
        {
            Span span = new();
            span.Tag = GetEnhancedKoreanSpanType(initial, medial0, medialOrientation0, medial1, medialOrientation1, final0, final1);
            AppendRunToKoreanSpan(initial, span);
            AppendRunToKoreanSpan(medial0, span);
            AppendRunToKoreanSpan(medial1, span);
            AppendRunToKoreanSpan(final0, span);
            AppendRunToKoreanSpan(final1, span);
            AppendRunToKoreanSpan(IOManager.Symbol.DELIMITER_CHAR, span);
            return span;
        }

        private static Span CreateLatinSpan(char character)
        {
            Span span = new();
            span.Tag = SpanType.Latin;
            Run run = CreateRun(character);
            span.Inlines.Add(run);
            return span;
        }

        public static void ConvertRawInputToInput()
        {
            FlowDocument myFlowDoc = new();
            _inputArea.Document = myFlowDoc;
            foreach (Paragraph paragraph in _rawInputArea.Document.Blocks.Cast<Paragraph>())
            {
                Paragraph myParagraph = new();
                myFlowDoc.Blocks.Add(myParagraph);
                foreach (Run run in paragraph.Inlines.Cast<Run>())
                {
                    foreach (char character in run.Text)
                    {
                        if (!syllables.ContainsKey(character))
                        {
                            Span mySpan = CreateLatinSpan(character);
                            myParagraph.Inlines.Add(mySpan);
                        }
                        else
                        {
                            if (EnhancedMode)
                            {
                                EnhancedSyllableBlock syllable = enhancedSyllables[character];
                                Span mySpan = CreateEnhancedKoreanSpan(syllable.InitialChar, 
                                                                        syllable.Medial0Char, syllable.MedialOrientation0,
                                                                        syllable.Medial1Char, syllable.MedialOrientation1,
                                                                        syllable.Final0Char, syllable.Final1Char);
                                myParagraph.Inlines.Add(mySpan);
                            }
                            else
                            {
                                SyllableBlock syllable = syllables[character];
                                Span mySpan = CreateKoreanSpan(syllable.InitialChar, syllable.MedialChar, syllable.MedialOrientation, syllable.FinalChar);
                                myParagraph.Inlines.Add(mySpan);
                            }
                        }
                    }
                }
            }
        }


        public static Dictionary<Char, SyllableBlock> GetSyllables()
        {
            Dictionary<char, Vowel> vowels = Letter.GetVowelsDictionary();
            Dictionary<Char, SyllableBlock> syllables = new Dictionary<char, SyllableBlock>();
            for (int initial = 0; initial <= 18; initial++)
            {
                for (int medial = 0; medial <= 20; medial++)
                {
                    for (int final = 0; final <= 27; final++)
                    {
                        int decimalValue = initial * 588 + medial * 28 + final + 44032;
                        string hexValue = $"{decimalValue:X}";
                        char charValue = (char)decimalValue;
                        Letter.Vowel.Orientation orientation = vowels[(char)(medial+HexOffset.MEDIAL)].MyOrientation;
                        SyllableBlock block = new(initial, medial, orientation, final, 
                            decimalValue, hexValue, charValue);
                        syllables.Add(charValue, block);
                    }
                }
            }
            return syllables;
        }

        public static class HexOffset
        {
            public static readonly int INITIAL = 0x1100;
            public static readonly int MEDIAL = 0x1161;
            public static readonly int FINAL = 0x11A7;
        }


        public static Dictionary<Char, EnhancedSyllableBlock> GetEnhancedSyllables()
        {
            Dictionary<Char, SyllableBlock> syllables = GetSyllables();
            Dictionary<Char, EnhancedSyllableBlock> enhancedSyllables = [];
            foreach (KeyValuePair<Char, SyllableBlock> entry in syllables)
            {
                char character = entry.Key;
                SyllableBlock originalBlock = entry.Value;
                int medial0 = originalBlock.Medial;
                Letter.Vowel.Orientation medialOrientation0 = originalBlock.MedialOrientation;
                int medial1 = IOManager.Symbol.EMPTY_VALUE;
                Letter.Vowel.Orientation medialOrientation1 = Letter.Vowel.Orientation.Vertical;
                if (Letter.DoubleVowels.TryGetValue(originalBlock.MedialChar, out Letter.DoubleVowel? doubleVowel)) {
                    medial0 = doubleVowel.MedialUnicode0;
                    medial1 = doubleVowel.MedialUnicode1;
                    medialOrientation1 = Letter.Vowels[doubleVowel.Letter].MyOrientation;
                } 

                int trailing0 = originalBlock.Final;
                int trailing1 = IOManager.Symbol.EMPTY_VALUE;
                if (Letter.DoubleConsonants.TryGetValue(originalBlock.FinalChar, out Letter.DoubleConsonant? doubleConsonant))
                {
                    trailing0 = doubleConsonant.TrailingUnicode0;
                    trailing1 = doubleConsonant.TrailingUnicode1;
                } 
                EnhancedSyllableBlock enhancedBlock = new(
                        originalBlock.Initial,
                        medial0,
                        medialOrientation0,
                        medial1,
                        medialOrientation1,
                        trailing0,
                        trailing1,
                        originalBlock.DecimalValue,
                        originalBlock.HexValue,
                        originalBlock.CharValue
                    );
                 enhancedSyllables.Add(character, enhancedBlock);
            }
            return enhancedSyllables;
        }

    }


    public class EnhancedSyllableBlock
    {
        public int Initial { get; set; }
        public char InitialChar { get; set; }
        public int Medial0 { get; set; }
        public int Medial1 { get; set; }
        public Letter.Vowel.Orientation MedialOrientation0 { get; set; }
        public Letter.Vowel.Orientation MedialOrientation1 { get; set; }
        public char Medial0Char { get; set; }
        public char Medial1Char { get; set; }
        public int Final0 { get; set; }
        public int Final1 { get; set; }
        public char Final0Char { get; set; }
        public char Final1Char { get; set; }
        public int DecimalValue { get; set; } // decimal representation of binary encoding for unicode mapping
        public string HexValue { get; set; } // hex representation of binary encoding for unicode mapping
        public char CharValue { get; set; } // unicode char, c# uses UTF-8 encoding
        public EnhancedSyllableBlock(int initial, 
            int medial0, Letter.Vowel.Orientation medialOrientation0, int medial1, Letter.Vowel.Orientation medialOrientation1,
            int final0, int final1, 
            int decimalValue, string hexValue, char charValue)
        {
            this.Initial = initial;
            this.InitialChar = (char)initial;
            this.Medial0 = medial0;
            this.Medial0Char = (char)medial0;
            this.MedialOrientation0 = medialOrientation0;
            this.Medial1 = medial1;
            this.Medial1Char = (char)medial1;
            this.MedialOrientation1 = medialOrientation1;
            this.Final0 = final0;
            this.Final0Char = (final0 == IOManager.Symbol.EMPTY_VALUE) ? IOManager.Symbol.EMPTY_CHAR : (char)final0;
            this.Final1 = final1;
            this.Final1Char = (final1 == IOManager.Symbol.EMPTY_VALUE) ? IOManager.Symbol.EMPTY_CHAR : (char)final1;
            this.DecimalValue = decimalValue;
            this.HexValue = hexValue;
            this.CharValue = charValue;
        }

        public override string ToString()
        {
            return 
                $"[ " +
                    $"I={Initial}|{InitialChar}, " +
                    $"M0={Medial0}|{Medial0Char}, M1={Medial1}|{Medial1Char}, " +
                    $"F0={Final0}|{Final0Char} F1={Final1}|{Final1Char} : " +
                    $"D={DecimalValue}, " +
                    $"X={HexValue}, " +
                    $"U={CharValue}" +
                $" ]";
        }
    }


    public class SyllableBlock
    {
        
        public int Initial { get; set; }
        public char InitialChar { get; set; }
        public int Medial { get; set; }
        public char MedialChar { get; set; }
        public Letter.Vowel.Orientation MedialOrientation { get; set; }
        public int Final { get; set; }
        public char FinalChar { get; set; }
        public int DecimalValue { get; set; } // decimal representation of binary encoding for unicode
        public string HexValue { get; set; } // hex representation of binary encoding for unicode
        public char CharValue { get; set; } // unicode char


        public SyllableBlock(int rawInitial, int rawMedial, Letter.Vowel.Orientation medialOrientation, int rawFinal, int decimalValue, string hexValue, char charValue)
        {
            this.Initial = rawInitial + RawInputManager.HexOffset.INITIAL;
            this.InitialChar = (char)Initial;

            this.Medial = rawMedial + RawInputManager.HexOffset.MEDIAL;
            this.MedialChar = (char)Medial;
            this.MedialOrientation = medialOrientation;

            this.Final = (rawFinal == IOManager.Symbol.EMPTY_VALUE) ? IOManager.Symbol.EMPTY_CHAR : rawFinal + RawInputManager.HexOffset.FINAL;
            this.FinalChar = (rawFinal == IOManager.Symbol.EMPTY_VALUE) ? IOManager.Symbol.EMPTY_CHAR : (char)Final;
            this.DecimalValue = decimalValue;
            this.HexValue = hexValue;
            this.CharValue = charValue;
        }
        
        public override string ToString()
        {
            return 
                $"[ " +
                    $"I={Initial}|{InitialChar}, " +
                    $"M={Medial}|{MedialChar}, " +
                    $"F={Final}|{FinalChar}) : " +
                    $"D={DecimalValue}, " +
                    $"X={HexValue}, " +
                    $"U={CharValue}" +
                $" ]";
        }
    }


}