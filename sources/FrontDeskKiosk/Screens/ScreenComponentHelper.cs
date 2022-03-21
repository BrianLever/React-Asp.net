using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;

namespace FrontDesk.Kiosk.Screens
{
    /// <summary>
    /// Implements common logic and behavior for all screen's UI componenents
    /// </summary>
    public class ScreenComponentHelper
    {
        private IVisualScreen _screenComponent;
        protected UserControl _control { get { return (UserControl)_screenComponent; } }

        public ScreenComponentHelper(IVisualScreen screenComponent)
        {
            if (screenComponent != null)
            {
                _screenComponent = screenComponent;
                _control.Visibility = Visibility.Collapsed;
            }
        }

        public void Show(bool withAnimation)
        {
            _control.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            _control.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Set formatted question text to textblock. Split text into lines and make text segments as Bold
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="formattedText"></param>
        public void FormatTextAndSetToUI(TextBlock textBlock, string formattedText)
        {
            textBlock.Text = string.Empty;

            if (string.IsNullOrEmpty(formattedText)) return;

            var inlines = FormatTextIntoInlines(formattedText);

            if (inlines.Count > 0)
            {
                textBlock.Inlines.AddRange(inlines);
            }
        }

        /// <summary>
        /// Split formatted Section Question text into TextBlock inlines
        /// </summary>
        /// <param name="formattedText"></param>
        /// <returns></returns>
        public List<Inline> FormatTextIntoInlines(string formattedText)
        {
            bool textBlockIsEmpty = true;

            //split text on lines
            string[] lines = formattedText.Split(new string[] { @"\n" }, StringSplitOptions.None);

            List<Inline> inlines = new List<Inline>();


            foreach (string line in lines)
            {
                string text = line.Trim();

                if (!string.IsNullOrEmpty(text))
                {
                    if (!textBlockIsEmpty)
                    {
                        //add new line separator
                        inlines.Add(new LineBreak());
                    }
                    ProcessTextLineString(inlines, text, 0);
                    textBlockIsEmpty = false;
                }
            }
            return inlines;
        }


        /// <summary>
        /// Recursive string that adds bold and normal text to the textblock
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="line"></param>
        /// <param name="currentPosition"></param>
        private void ProcessTextLineString(List<Inline> inlines, string line, int currentPosition)
        {
            StringBuilder lineBuffer = new StringBuilder();
            Run temp;
            int boldTextStartIndex;

            //we need to find bolded text
            //add text to buffer unless we find <b> meta tag
            boldTextStartIndex = line.IndexOf("<b>", currentPosition);
            if (boldTextStartIndex < 0)
            {
                lineBuffer.Append(line.Substring(currentPosition));

                //flush new line to buffer
                inlines.Add(new Run(lineBuffer.ToString()));
                lineBuffer.Length = 0;
            }
            else
            {
                if (currentPosition != boldTextStartIndex)
                {
                    lineBuffer.Append(line.Substring(currentPosition, boldTextStartIndex - currentPosition));

                    //flush new line to buffer - non-bold
                    inlines.Add(new Run(lineBuffer.ToString()));
                    lineBuffer.Length = 0;
                }
                //get next bold text
                currentPosition = boldTextStartIndex + "<b>".Length;

                boldTextStartIndex = line.IndexOf("</b>", currentPosition, StringComparison.Ordinal);
                lineBuffer.Append(line.Substring(currentPosition, boldTextStartIndex - currentPosition));

                temp = new Run(lineBuffer.ToString());
                temp.FontWeight = FontWeights.SemiBold;
                temp.FontStyle = FontStyles.Normal;

                inlines.Add(temp);
                lineBuffer.Length = 0;

                currentPosition = boldTextStartIndex + "</b>".Length;

                ProcessTextLineString(inlines, line, currentPosition);
            }
        }
    }
}
