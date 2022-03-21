using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Controls;


namespace FrontDesk.Kiosk.Controls.Keyboard
{
    public class KeyboardController
    {
        /// <summary>
        /// The type of keyboard
        /// </summary>
        private KeyboardType kbType;

        /// <summary>
        /// Keyboard map array
        /// </summary>
        private char[][] keyboard = null;

        private bool showAdditionalSymbols = false;


        /// <summary>
        /// Get the array of symbol by provided kb type
        /// </summary>
        private char[][] InitializeKeyboardLayout(KeyboardType type)
        {
            char[][] keyboardLayout = null;

            switch (type)
            {
                case KeyboardType.Full:
                    keyboardLayout = new char[][] {
                        showAdditionalSymbols ?
                        new char[] {'1','2','3','4','5','6','7','8','9','0','#','/'}:
                        new char[] {'1','2','3','4','5','6','7','8','9','0'},
                        new char[] {'Q','W','E','R','T','Y','U','I','O','P','-'} ,
                        new char[] {'A','S','D','F','G','H','J','K','L','\'' },
                        new char[] {'Z','X','C','V','B','N','M',',','.'},
                        new char[] {' ', }
                    };
                    break;

                case KeyboardType.Symbol:
                    keyboardLayout = new[] {
                        new [] {'Q', 'W', 'E','R','T','Y','U','I','O','P' },
                        new [] {'A','S','D','F','G','H','J','K','L', '\'' },
                        new [] {'Z','X','C','V','B','N','M', ',', '.', '_'},
                        new [] {' ', '-' }
                    };
                    break;

                case KeyboardType.Digital:
                    keyboardLayout = new char[][] {
                        new char[] {'1', '2', '3'},
                        new char[] { '4', '5', '6' },
                        new char[] { '7', '8', '9' },
                        new char[] { ' ', '0', ' ' },
                    };
                    break;
            }

            return keyboardLayout;
        }

        /// <summary>
        /// Get kb's rows number
        /// </summary>
        public int RowsCount
        {
            get
            {
                return keyboard.Length;
            }
        }

        public static readonly char[] PreUpperCaseChars = new char[] { ' ', '-' };

        /// <summary>
        /// Get items count in row
        /// </summary>
        public int GetRowLength(int rowIndex)
        {
            int lenght = 0;
            if (rowIndex < keyboard.Length)
            {
                lenght = keyboard[rowIndex].Length;
            }
            return lenght;
        }

        /// <summary>
        /// Keyboard container
        /// </summary>
        private StackPanel container;

        public KeyboardController(StackPanel container, KeyboardType kbType, bool showAdditionalSymbols)
        {
            this.container = container;
            this.kbType = kbType;
            this.showAdditionalSymbols = showAdditionalSymbols;
            this.keyboard = InitializeKeyboardLayout(kbType);
        }


        /// <summary>
        /// Initialize keyboard button
        /// </summary>
        public void Initialize(KeyButton key)
        {
            Regex pattern = new Regex("key_\\d{2}_\\d{2}");
            if (pattern.IsMatch(key.Name))
            {
                int x = 0;
                int y = 0;

                string name = key.Name;
                int.TryParse(name.Substring(4, 2), out x);
                int.TryParse(name.Substring(7, 2), out y);

                char symbol = keyboard[y][x];
                key.Symbol = symbol;


                if (symbol == ' ') // space
                {
                    if (kbType == KeyboardType.Digital)
                    {
                        key.DepressedTemplateKey = "Grey";
                        key.DisabledTemplateKey = "Grey";
                        key.CanBePressed = false;
                        key.Text = "";
                        key.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                    else
                    {
                        key.DepressedTemplateKey = "Space";
                        key.PressedTemplateKey = "Space_Pressed";
                        key.DisabledTemplateKey = "Space_Disabled";
                        key.Text = "space";
                        key.HorizontalAlignment = HorizontalAlignment.Left;
                    }
                }
                else if (symbol == '\u2408') // backspace
                {
                    //key.DepressedTemplateKey = "BackspaceSep";
                    key.Text = "Backspace";
                }
                else if (symbol == '\u8679') //shift
                {
                    /*
                    key.Width = 205;
                    key.Text = "Shift";
                     */
                }
                else
                {
                    if (kbType == KeyboardType.Digital)
                    {
                        key.DepressedTemplateKey = "DigitalKey";
                        key.PressedTemplateKey = "DigitalKey_Pressed";
                        key.DisabledTemplateKey = "DigitalKey_Disabled";
                    }
                    else
                    {
                        key.DepressedTemplateKey = "SimpleKey";
                        key.PressedTemplateKey = "SimpleKey_Pressed";
                        key.DisabledTemplateKey = "SimpleKey_Disabled";
                    }
                    key.Text = symbol.ToString();
                }

                key.ApplyTemplate(key.DepressedTemplateKey);
            }
        }

        public StackPanel CreateRowControl()
        {
            StackPanel sp = new StackPanel()
            {
                HorizontalAlignment = kbType == KeyboardType.Digital ? HorizontalAlignment.Center : HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal
            };
            container.Children.Add(sp);
            return sp;
        }
    }

    public enum KeyboardType
    {
        Full,
        Symbol,
        Digital
    }
}
