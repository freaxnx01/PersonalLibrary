using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PersonalLibrary.WinFormsExtensions
{
    public static class RichTextBoxExtensions
    {
        public static void GoToLinePos(this RichTextBox richTextBox, int lineNumber, int pos)
        {
            GoToLine(richTextBox, lineNumber);
            richTextBox.SelectionStart += pos;
        }

        public static void GoToLine(RichTextBox richTextBox, int lineNumber)
        {
            int charIndex = 0;
            int lineCounter = 0;

            foreach (string line in richTextBox.Lines)
            {
                lineCounter++;

                if (lineCounter == lineNumber)
                {
                    break;
                }

                charIndex += line.Length + 1;
            }

            if (charIndex != richTextBox.SelectionStart)
            {
                richTextBox.SelectionStart = charIndex;
            }
        }
    }
}
