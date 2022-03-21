using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FrontDesk.Kiosk.Controls.TextBoxExtensions
{
	public partial class TerminalTextBox: UserControl
    {
        #region Constructor

        public TerminalTextBox()
        {
            InitializeComponent();

            IsTemplated = false;
            CursorDefaultOffset = 10;
            cursorStoryboard = this.FindResource("cursorStoryboard") as Storyboard;
        }

        #endregion

        #region Custom styles

        public string BorderStyle 
        {
            set
            {
                bBorder.SetResourceReference(StyleProperty, value);
            }
        }

        public string TextBoxStyle 
        {
            set
            {
                txt.SetResourceReference(StyleProperty, value);
            }
        }

        public string CursorStyle
        {
            set
            {
                bCursor.SetResourceReference(StyleProperty, value);
            }
        }

        #endregion

        public double CursorDefaultOffset
        {
            get;
            set;
        }

        private Storyboard cursorStoryboard;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Gets or sets the text contents of the text box.
        /// </summary>
        public string Text
        {
            get
            {
                return txt.Text;
            }
            set
            {
                txt.Text = value;
                SetCursorPosition();
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that can be manually entered
        ///  into the text box.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return txt.MaxLength;
            }
            set
            {
                txt.MaxLength = value;
            }
        }

        public bool IsTemplated { get; set; }

        /// <summary>
        /// Clear text bpx
        /// </summary>
        public void Clear()
        {
            txt.Clear();
            SetCursorPosition();
        }

        /// <summary>
        /// Add symbol to the end of text string
        /// </summary>
        public void Add(char c)
        {
            txt.Text += c.ToString();
            SetCursorPosition();
        }

        /// <summary>
        /// Remove last symbol (backspace action)
        /// </summary>
        public void Remove()
        {
            if(!String.IsNullOrEmpty(txt.Text))
            {
                txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
                SetCursorPosition();
            }
        }
      

        /// <summary>
        /// Calculate the position of cursor
        /// </summary>
        protected virtual void SetCursorPosition()
        {
            double cursorOffset = 0;
            if (!IsTemplated)
            {
                if (txt.Text.Length > 0)
                {
                    cursorOffset += txt.GetRectFromCharacterIndex(txt.Text.Length).TopRight.X;
                    if (cursorOffset >= txt.ActualWidth)
                    {
                        cursorOffset = txt.ActualWidth;
                        txt.PageRight();
                    }
                    else if(double.IsNegativeInfinity(cursorOffset))
                    {
                        cursorOffset = 0;
                    }
                }
            }

            bCursor.Margin = new Thickness(cursorOffset + CursorDefaultOffset, 0, 0, 0);

        }


        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }


        public void SetFocus()
        {
            //start custom cursor animation
            SetCursorPosition();
            cursorStoryboard.Begin();
        }

        public void ClearFocus()
        {
            //stop cursor blinking
            bCursor.Visibility = Visibility.Hidden;
            cursorStoryboard.Stop();
        }
    }
}
