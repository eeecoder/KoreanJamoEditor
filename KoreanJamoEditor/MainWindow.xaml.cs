using HangulJamoEditor;
using Microsoft.CSharp.RuntimeBinder;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KoreanJamoEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RichTextBox _rawInputArea;
        private readonly RichTextBox _inputArea;
        private readonly Canvas _outputArea;
        private readonly Button _boldSetter;
        private readonly Button _italicSetter;
        private readonly Button _underlineSetter;
        private readonly ComboBox _fontFamilySelector;
        private readonly ComboBox _fontSizeSelector;
        private const int DEFAULT_FONT_SIZE = 14; 
        public static readonly TextDecoration UnderlineDecoration = new TextDecoration();

        public MainWindow()
        {
            InitializeComponent();
            _rawInputArea = (RichTextBox)FindName("RawInputArea");
            _inputArea = (RichTextBox)FindName("InputArea");
            _outputArea = (Canvas)FindName("OutputArea");
            _boldSetter = (Button)FindName("BoldFormatter");
            _italicSetter = (Button)FindName("ItalicFormatter");
            _underlineSetter = (Button)FindName("UnderlineFormatter");
            _fontFamilySelector = (ComboBox)FindName("FontFamilySelector");
            _fontSizeSelector = (ComboBox)FindName("FontSizeSelector");
            _rawInputArea.FontSize = DEFAULT_FONT_SIZE;
            _inputArea.FontSize = DEFAULT_FONT_SIZE;
            FillFontFamilyComboBox();
            FillFontSizeComboBox();
            RawInputManager.Init(_rawInputArea, _inputArea);
            IOManager.Init(_inputArea, _outputArea);
            new CustomKeyBindings().AddKeyBindings(this, _rawInputArea);
        }

        public void FillFontFamilyComboBox()
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                _fontFamilySelector.Items.Add(fontFamily.Source);
            }
            _fontFamilySelector.SelectedIndex = 0;
        }

        public void FillFontSizeComboBox()
        {
            int[] fontSizes = new int[] { 10, 12, DEFAULT_FONT_SIZE, 16, 18, 20};
            foreach (int fontSize in fontSizes)
            {
                _fontSizeSelector.Items.Add(fontSize);
            }
            _fontSizeSelector.SelectedIndex = 2;
        }


        private void FontSizeSelector_OnSelectItem(object sender, System.EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            TextSelection selection = _inputArea.Selection;
            TextPointer start = selection.Start;
            TextPointer end = selection.End;
            TextPointer current = start;
            string fontStyle = (string)combo.Tag;
            while (current != null && current.CompareTo(end) < 0)
            {
                int runLength = current.GetTextRunLength(LogicalDirection.Forward);
                Run run = (Run)current.Parent;
                run.FontSize = (int)combo.SelectedItem;
                current = current.GetNextContextPosition(LogicalDirection.Forward);
                current = current.GetNextContextPosition(LogicalDirection.Forward);
                current = current.GetNextInsertionPosition(LogicalDirection.Forward);
            }
            IOManager.RenderOutput();
        }

        private void FontFamilySelector_OnSelectItem(object sender, System.EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            TextSelection selection = _inputArea.Selection;
            TextPointer start = selection.Start;
            TextPointer end = selection.End;
            TextPointer current = start;
            string fontStyle = (string)combo.Tag;
            while (current != null && current.CompareTo(end) < 0)
            {
                int runLength = current.GetTextRunLength(LogicalDirection.Forward);
                Run run = (Run)current.Parent;
                run.FontFamily = new FontFamily((string)combo.SelectedItem);
                current = current.GetNextContextPosition(LogicalDirection.Forward);
                current = current.GetNextContextPosition(LogicalDirection.Forward);
                current = current.GetNextInsertionPosition(LogicalDirection.Forward);
            }
            IOManager.RenderOutput();
        }

        private void FontFormatButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TextSelection selection = _inputArea.Selection;
            TextPointer start = selection.Start;
            TextPointer end = selection.End;
            TextPointer current = start;
            string fontStyle = (string)button.Tag;
            while (current != null && current.CompareTo(end) < 0)
            {
                int runLength = current.GetTextRunLength(LogicalDirection.Forward);
                Run run = (Run)current.Parent;
                switch (fontStyle)
                {
                    case "ColorPicker":
                        run.Foreground = button.Background;
                        break;
                    case "BoldFormatter":
                        run.FontWeight = (run.FontWeight == FontWeights.Bold) ? FontWeights.Regular : FontWeights.Bold;
                        break;
                    case "ItalicFormatter":
                        run.FontStyle = (run.FontStyle == FontStyles.Italic) ? FontStyles.Normal : FontStyles.Italic;
                        break;
                    case "UnderlineFormatter":
                        TextDecoration underline = new TextDecoration();
                        if (run.TextDecorations.Contains(UnderlineDecoration))
                        {
                            run.TextDecorations.Remove(UnderlineDecoration);
                        }
                        else
                        {
                            run.TextDecorations.Add(UnderlineDecoration);
                        }
                        break;
                    case "ClearFormatter":
                        run.TextDecorations.Clear();
                        run.FontWeight = FontWeights.Regular;
                        run.FontStyle = FontStyles.Normal;
                        break;
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
                current = current.GetNextContextPosition(LogicalDirection.Forward);
                current = current.GetNextInsertionPosition(LogicalDirection.Forward);
            }
            IOManager.RenderOutput();
        }

        protected virtual void InputArea_OnSelectionChanged(Object sender, EventArgs e)
        {
            TextSelection selection = _inputArea.Selection;
            Run? currentRun = null;
            foreach (Paragraph paragraph in _inputArea.Document.Blocks.Cast<Paragraph>())
            {
                foreach (Span span in paragraph.Inlines.Cast<Span>())
                {
                    foreach (Run run in span.Inlines.Cast<Run>())
                    {
                        if (selection.Contains(run.ElementStart))
                        {
                            currentRun = run;
                            break;
                        }
                    }
                }
            }
            if (currentRun == null) return;
            _boldSetter.Background = currentRun.FontWeight == FontWeights.Bold ?
                new SolidColorBrush(Colors.DarkGray) : new SolidColorBrush(Colors.LightGray);
            _italicSetter.Background = currentRun.FontStyle == FontStyles.Italic ?
                new SolidColorBrush(Colors.DarkGray) : new SolidColorBrush(Colors.LightGray);
            _underlineSetter.Background = currentRun.TextDecorations.Contains(UnderlineDecoration) ?
                new SolidColorBrush(Colors.DarkGray) : new SolidColorBrush(Colors.LightGray);
            IOManager.RenderOutput();
        }

        protected virtual void RawInputArea_TextChanged(Object sender, EventArgs e)
        {
            RawInputManager.ConvertRawInputToInput();
            IOManager.RenderOutput();
        }

    }
}