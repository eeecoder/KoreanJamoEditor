using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Printing;
using System.Text;

namespace KoreanJamoEditor
{
	public static class IOManager
	{
        private static RichTextBox _inputArea;
        private static Canvas _outputCanvas;

        public static void Init(RichTextBox inputArea, Canvas outputCanvas)
        {
            _inputArea = inputArea;
            _outputCanvas = outputCanvas;
        }

        public static class Symbol
        {
            public static char DELIMITER_CHAR = '*';
            public static string DELIMITER_STRING = DELIMITER_CHAR.ToString();
            public static char EMPTY_CHAR = ' ';
            public static int EMPTY_VALUE = 0;
        }

        public static class Dimension
        {
            public static int BLOCK = 30;
        }



        private static Thickness GetMarginOfCV_Horizontal(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            if (ordinal == 1)
            {
                return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2 - 5, 0, 0);
            } else
            {
                return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2 + 5, 0, 0);
            }      
        }

        private static Thickness GetMarginOfCV_Vertical(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            if (ordinal == 1)
            {
                return new Thickness(left + Dimension.BLOCK / 2 - 5, top + Dimension.BLOCK / 2, 0, 0);
            }
            else
            {
                return new Thickness(left + Dimension.BLOCK / 2 + 5, top + Dimension.BLOCK / 2, 0, 0);
            }
        }

        private static Thickness GetMarginOfCVV(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            switch (ordinal)
            {
                case 1:
                    return new Thickness(left + Dimension.BLOCK / 2 - 3, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 2:
                    return new Thickness(left + Dimension.BLOCK / 2 - 3, top + Dimension.BLOCK / 2, 0, 0);
                case 3:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2, 0, 0);
                default:
                    return new Thickness(left, top, 0, 0);
            }
        }

        private static Thickness GetMarginOfCVC_Horizontal(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            switch (ordinal)
            {
                case 1:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 2:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2, 0, 0);
                case 3:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2 + 3, 0, 0);
                default:
                    return new Thickness(left, top, 0, 0);
            }
        }

        private static Thickness GetMarginOfCVC_Vertical(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            switch (ordinal)
            {
                case 1:
                    return new Thickness(left + Dimension.BLOCK / 2 - 3, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 2:
                    return new Thickness(left + Dimension.BLOCK / 2 + 3, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 3:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2, 0, 0);
                default:
                    return new Thickness(left, top, 0, 0);
            }
        }

        private static Thickness GetMarginOfCVVC(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            switch (ordinal)
            {
                case 1:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 2:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2, 0, 0);
                case 3:
                    return new Thickness(left + Dimension.BLOCK / 2 + 3, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 4:
                    return new Thickness(left + Dimension.BLOCK / 2 + 3, top + Dimension.BLOCK / 2 + 3, 0, 0);
                default:
                    return new Thickness(left, top, 0, 0);
            }
        }

        private static Thickness GetMarginOfCVCC_HorizontalVowel(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            switch (ordinal)
            {
                case 1:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 2:
                    return new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2, 0, 0);
                case 3:
                    return new Thickness(left + Dimension.BLOCK / 2 - 4, top + Dimension.BLOCK / 2 + 3, 0, 0);
                case 4:
                    return new Thickness(left + Dimension.BLOCK / 2 + 4, top + Dimension.BLOCK / 2 + 3, 0, 0);
                default:
                    return new Thickness(left, top, 0, 0);
            }
        }

        private static Thickness GetMarginOfCVCC_VerticalVowel(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            switch (ordinal)
            {
                case 1:
                    return new Thickness(left + Dimension.BLOCK / 2 - 3, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 2:
                    return new Thickness(left + Dimension.BLOCK / 2 + 3, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 3:
                    return new Thickness(left + Dimension.BLOCK / 2 - 5, top + Dimension.BLOCK / 2 + 3, 0, 0);
                case 4:
                    return new Thickness(left + Dimension.BLOCK / 2 + 5, top + Dimension.BLOCK / 2 + 3, 0, 0);
                default:
                    return new Thickness(left, top, 0, 0);
            }
        }

        private static Thickness GetMarginOfCVVCC(int x, int y, int ordinal)
        {
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            switch (ordinal)
            {
                case 1:
                    return new Thickness(left + Dimension.BLOCK / 2 - 3, top + Dimension.BLOCK / 2 - 3, 0, 0);
                case 2:
                    return new Thickness(left + Dimension.BLOCK / 2 - 3, top + Dimension.BLOCK / 2 - 1, 0, 0);
                case 3:
                    return new Thickness(left + Dimension.BLOCK / 2 + 3, top + Dimension.BLOCK / 2 - 2, 0, 0);
                case 4:
                    return new Thickness(left + Dimension.BLOCK / 2 - 5, top + Dimension.BLOCK / 2 + 2, 0, 0);
                case 5:
                    return new Thickness(left + Dimension.BLOCK / 2 + 5, top + Dimension.BLOCK / 2 + 2, 0, 0);
                default:
                    return new Thickness(left, top, 0, 0);
            }
        }
        

        private static int ConvertKoreanSpanToLabel(Span span, int x, int y)
        {
            int ordinal = 0;
            Console.OutputEncoding = Encoding.Unicode;
            foreach (Run run in span.Inlines.Cast<Run>())
            {
                if (run.Text == Symbol.DELIMITER_STRING || run.Text.Length == 0 || run.Text.Contains('\0'))
                {
                    continue;
                }        
                ordinal += 1;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = run.Text;
                textBlock.Foreground = run.Foreground;
                textBlock.Background = run.Background;
                textBlock.FontStyle = run.FontStyle;
                textBlock.FontWeight = run.FontWeight;
                textBlock.FontFamily = run.FontFamily;
                textBlock.FontSize = run.FontSize;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                if (run.TextDecorations.Contains(MainWindow.UnderlineDecoration))
                {
                    textBlock.TextDecorations.Add(MainWindow.UnderlineDecoration);
                }
                textBlock.Margin = SetMargin(span, x, y, ordinal);
                _outputCanvas.Children.Add(textBlock);
            }
            return x + 1;
        }

        private static Thickness SetMargin(Span span, int x, int y, int ordinal)
        {
            string? tag = span.Tag.ToString();
            if (tag == SpanType.CV_HorizontalVowel.ToString())
            {
                return GetMarginOfCV_Horizontal(x, y, ordinal);
            }
            if (tag == SpanType.CV_VerticalVowel.ToString())
            {
                return GetMarginOfCV_Vertical(x, y, ordinal);
            }
            if (tag == SpanType.CVV.ToString()) {
                return GetMarginOfCVV(x, y, ordinal);
            }
            if (tag == SpanType.CVC_HorizontalVowel.ToString())
            {
                return GetMarginOfCVC_Horizontal(x, y, ordinal);
            }
            if (tag == SpanType.CVC_VerticalVowel.ToString())
            {
                return GetMarginOfCVC_Vertical(x, y, ordinal);
            }
            if (tag == SpanType.CVVC.ToString()) {
                return GetMarginOfCVVC(x, y, ordinal);
            }
            if (tag == SpanType.CVCC_HorizontalVowel.ToString()) {
                return GetMarginOfCVCC_HorizontalVowel(x, y, ordinal);
            }
            if (tag == SpanType.CVCC_VerticalVowel.ToString())
            {
                return GetMarginOfCVCC_VerticalVowel(x, y, ordinal);
            }
            if (tag == SpanType.CVVCC.ToString())
            {
                return GetMarginOfCVVCC(x, y, ordinal);
            }
            return GetMarginOfCVC_Vertical(x, y, ordinal);
        }

        private static int ConvertLatinSpanToLabel(Span span, int x, int y)
        {
            TextBlock textBlock = new TextBlock();
            Run run = (Run)span.Inlines.FirstInline;
            textBlock.Foreground = run.Foreground;
            textBlock.Background = run.Background;
            textBlock.FontStyle = run.FontStyle;
            textBlock.FontFamily = run.FontFamily;
            textBlock.FontWeight = run.FontWeight;
            textBlock.FontSize = run.FontSize;
            int left = x * Dimension.BLOCK;
            int top = y * Dimension.BLOCK;
            textBlock.Margin = new Thickness(left + Dimension.BLOCK / 2, top + Dimension.BLOCK / 2, 0, 0);
            textBlock.Text = run.Text;            
            _outputCanvas.Children.Add(textBlock);
            return x + run.Text.Length;
        }

        public static void RenderOutput()
        {
            int x = 0;
            int y = 0;
            _outputCanvas.Children.Clear();
            foreach (Paragraph paragraph in _inputArea.Document.Blocks.Cast<Paragraph>())
            {
                foreach (Span span in paragraph.Inlines.Cast<Span>())
                {
                    if (span.Tag.ToString() != SpanType.Latin.ToString()) {
                        x = ConvertKoreanSpanToLabel(span, x, y);
                    }
                    else
                    {
                        x = ConvertLatinSpanToLabel(span, x, y);
                    }
                }
                y++;
                x = 0;
            }

        }

    }
}