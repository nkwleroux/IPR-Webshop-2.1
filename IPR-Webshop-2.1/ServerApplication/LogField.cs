using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ServerApplication
{
    public class LogField
    {
        // The UI element in our window
        private RichTextBox log;
        // We use a single paragraph to add all the printed lines
        private Paragraph loggingTextParagraph;
        public LogField(RichTextBox log)
        {
            this.log = log;
            // Makes it readonly
            this.log.IsReadOnly = true;
            // We select the first block in our UI RichTextBox element to have an easy to edit enviroment
            this.loggingTextParagraph = (Paragraph)this.log.Document.Blocks.FirstBlock;
        }
        /*
         * This method handles the message printing on the log,
         * param "label" = the bold printed text at the start of message
         * param "nmessage" = the message printed after the label
         * Construction:
         * 
         * [TimeStamp.Now] label: message
         */
        public void PrintLine(string label, string message)
        {
            Run date = new Run("[" + DateTime.Now.ToString() + "] ");
            date.Foreground = new SolidColorBrush(Color.FromRgb(229,88,18));
            this.loggingTextParagraph.Inlines.Add(new Bold(date));
            Run labelText = new Run(label + ": ");
            labelText.Foreground = new SolidColorBrush(Colors.White);
            this.loggingTextParagraph.Inlines.Add(new Bold(labelText));
            Run messageText = new Run(message + "\n");
            messageText.Foreground = new SolidColorBrush(Colors.White);
            this.loggingTextParagraph.Inlines.Add(messageText);
            log.ScrollToEnd();
        }
    }
}
