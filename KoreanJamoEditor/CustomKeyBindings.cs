using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static HangulJamoEditor.CustomKeyBindings;

namespace HangulJamoEditor
{
	public class CustomKeyBindings
	{
        public static readonly RoutedCommand CustomRoutedCommand = new();

        public struct KeyMapping
        {
            public Key key;
            public ModifierKeys modifier;
            public String jamo;

            public KeyMapping(Key key, String jamo)
            {
                this.key = key;
                this.modifier = ModifierKeys.None;
                this.jamo = jamo;  
            }

            public KeyMapping(ModifierKeys modifier, String jamo)
            {
                this.key = Key.None;
                this.modifier = modifier;
                this.jamo = jamo;
            }

            public KeyMapping(Key key, ModifierKeys modifier, String jamo)
            {
                this.key = key;
                this.modifier = modifier;
                this.jamo = jamo;  
            }

            public override string ToString() { 
                return $"{key} {modifier} => {jamo} ";
            }
        }

        

        public static class KeyList {
            private static KeyMapping Test_CV_HorizontalVowel = new(Key.F1, "보");
            private static KeyMapping Test_CV_VerticalVowel = new(Key.F2, "바");
            private static KeyMapping Test_CVV = new(Key.F3, "봐");
            private static KeyMapping Test_CVC_HorizontalVowel = new(Key.F4, "블");
            private static KeyMapping Test_CVC_VerticalVowel = new(Key.F5, "발");
            private static KeyMapping Test_CVVC = new(Key.F6, "될");
            private static KeyMapping Test_CVCC_HorizontalVowel = new(Key.F7, "봃");
            private static KeyMapping Test_CVCC_VerticalVowel = new(Key.F8, "밝");
            private static KeyMapping Test_CVVCC = new(Key.F9, "봟");


            public static readonly List<KeyMapping> MyList =
            [
                Test_CV_HorizontalVowel, Test_CV_VerticalVowel, // 2
                Test_CVV, Test_CVC_HorizontalVowel, Test_CVC_VerticalVowel, // 3
                Test_CVVC, Test_CVCC_HorizontalVowel, Test_CVCC_VerticalVowel, // 4
                Test_CVVCC // 5
            ];
        }

       

        public void AddKeyBindings(Window window, RichTextBox inputTextbox)
		{
            CommandBinding OpenCmdBinding = new(
                CustomRoutedCommand,
                ExecutedCustomCommand,
                CanExecuteCustomCommand);

            window.CommandBindings.Add(OpenCmdBinding);

            foreach (var keyMapping in KeyList.MyList)
            {
                Debug.WriteLine(keyMapping.ToString());
                KeyBinding b = new()
                {
                    Command = CustomRoutedCommand,
                    Key = keyMapping.key,
                    Modifiers = keyMapping.modifier,
                    CommandTarget = inputTextbox,
                    CommandParameter = keyMapping.jamo
                };
                window.InputBindings.Add(b);
            }
        }


  
        private void ExecutedCustomCommand(object sender, ExecutedRoutedEventArgs e)
        {
           
            RichTextBox richTextBox = (RichTextBox)e.Source;
       
            Paragraph paragraph = new Paragraph();
            String? insertText = e.Parameter.ToString();
            paragraph.Inlines.Add(new Run(insertText));
           
            richTextBox.Document.Blocks.Remove(richTextBox.Document.Blocks.FirstBlock);
            richTextBox.Document.Blocks.Add(paragraph);
        }

        private void CanExecuteCustomCommand(object sender,
            CanExecuteRoutedEventArgs e)
        {
            if (e.Source is Control)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

    }
}