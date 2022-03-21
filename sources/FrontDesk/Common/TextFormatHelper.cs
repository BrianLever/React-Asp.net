using System;
using System.Text;
using System.Globalization;

namespace FrontDesk.Common
{
    public class TextFormatHelper
    {
        // Alphabet to tightly pack byte arrays to nice "license-like" strings
        private static byte bitsPerLetter = 5;   // 32 letters fit to 5 bits
       
        // use string for better obfuscation results
        private static string alphabet =
            "RWAP689EZK" +
            "XH1NC3LQ7F" +
            "DVY4MBU5GT" +
            "J2";
        
        private static string groupsDelimiter = "-";

        private static Random randomizer = null;
        private static Random Randomizer
        {
            get
            {
                if (randomizer == null)
                {
                    randomizer = new Random();
                }
                return randomizer;
            }
        }

        /// <summary>
        /// Encode byte array to the string with 32-letter alphabet,
        /// like "LLFXNQDWYTLP9M5Z8G5M56DQD2". 
        /// 
        /// Additional zero byte may be appended to the source array to provide correct encoding. Also note that additional bytes
        /// may be added during reverse decoding with UnpackString, so you must know initial length of source byte array.
        /// 
        /// Use FormatWithGroups to insert dashes for readability, like "LLFXN-QDWYT-LP9M5-Z8G5M-56DQD-201P5"
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string PackString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            int bits = bytes.Length * 8;

            int blocks = bits / bitsPerLetter;  // int div
            if (bits % bitsPerLetter > 0)
            {
                blocks++;
            };

            int paddedBytes = (blocks * bitsPerLetter) / 8;
            if ((blocks * bitsPerLetter) % 8 > 0)
            {
                paddedBytes++;
            }

            byte[] padded = new byte[paddedBytes];
            Array.Copy(bytes, padded, bytes.Length);
            for (int i = 0; i < blocks; i++)
            {
                int index = 0;
                for (int j = 0; j < bitsPerLetter; j++)
                {
                    int bitIndex = i * bitsPerLetter + j;
                    int byteIndex = bitIndex / 8;
                    int byteOffset = bitIndex % 8;

                    if ((padded[byteIndex] & (1 << byteOffset)) > 0)
                    {
                        index |= (1 << j);
                    }
                }

                sb.Append(alphabet[index]);
            }

            return sb.ToString();
        }

        public static string PackString(Int32 int32)
        {
            return PackString(new byte[]{ 
                (byte)((int32 & 0xFF000000) >> 24),  
                (byte)((int32 & 0x00FF0000) >> 16),  
                (byte)((int32 & 0x0000FF00) >> 8),  
                (byte)((int32 & 0x000000FF))
            });
        }

        public static string PackString(Int16 int16)
        {
            return PackString(new byte[]{                 
                (byte)((int16 & 0xFF00) >> 8),  
                (byte)((int16 & 0x00FF))
            });
        }

        /// <summary>
        /// Pack Int16 value into XXXX string
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static string FormatWithGroupsInt16(Int16 keyValue)
        {
            return FormatWithGroups(PackString(keyValue), 4, false, false);
        }
       


        /// <summary>
        /// Format string for readability, splitting it to separated groups.
        /// For example, "LLFXNQDWYTLP9M5Z8G5M56DQD2" formatted to 
        /// "LLFXN-QDWYT-LP9M5-Z8G5M-56DQD-201P5".
        /// 
        /// Last group may be padded with random letters to the same length. ("01P5" added in example above)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string FormatWithGroups(string source, int lettersInGroup, bool padWithRandomLetters, bool mergeLastIncompleteGroup)
        {
            int i;
             //remove all special characters
            source = source.Replace(groupsDelimiter, String.Empty);

            StringBuilder sb = new StringBuilder(source);
            int delims = (source.Length / lettersInGroup) - 1;
            bool incompleteLastGroup = (source.Length % lettersInGroup) > 0;
            if (incompleteLastGroup && !mergeLastIncompleteGroup)
            {
                delims++;
            }
            for ( i = 0; i < delims; i++)
            {
                sb.Insert(i + (i + 1) * lettersInGroup, groupsDelimiter);
            }

            if (padWithRandomLetters && (source.Length % lettersInGroup) > 0)
            {
                int numPad = lettersInGroup - (source.Length % lettersInGroup);
                //Random rnd = new Random();    // got the same rnd in fast cycle! it's time-initialized. use class member instead.
                for (i = 0; i < numPad; i++)
                {
                    //sb.Append(alphabet[rnd.Next(alphabet.Length)]);
                    sb.Append(alphabet[Randomizer.Next(alphabet.Length)]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Decode string from 32-letter alphabet to byte array.
        /// 
        /// NOTE that due to paddings in FormatWithGroups,
        /// resulting byte array may be longer than original encoded byte array.
        /// You must know length of original array and use only that bytes of decoded array.
        /// Extra bytes in decoded array will contain random data.
        /// </summary>
        /// <param name="licenseString"></param>
        /// <returns></returns>
        public static byte[] UnpackString(string licenseString)
        {
            licenseString = licenseString.Trim().Replace(groupsDelimiter, "").ToUpperInvariant();

            int bits = licenseString.Length * bitsPerLetter;
            int bytesRequired = bits / 8;
            if ((bits % 8) > 0)
            {
                bytesRequired++;
            }

            byte[] bytes = new byte[bytesRequired];

            for (int i = 0; i < licenseString.Length; i++) // each letter
            {
                for (byte j = 0; j < alphabet.Length; j++)
                {
                    if (alphabet[j] == licenseString[i])    // j == letter index
                    {
                        // j == 5-bit block
                        for (byte k = 0; k < bitsPerLetter; k++)   // append next 5 bits to byte[]
                        {
                            int bitIndex = i * bitsPerLetter + k;
                            int byteIndex = bitIndex / 8;
                            int byteOffset = bitIndex % 8;

                            if (((1 << k) & j) != 0)
                            {
                                bytes[byteIndex] |= (byte)(1 << byteOffset);
                            }
                        }

                        break;
                    }
                }
            }

            return bytes;
        }

        public static Int32 UnpackStringInt32(string licenseString)
        {
            if (licenseString.Trim().Replace(groupsDelimiter, "").Length < 7)
            {
                throw new ArgumentException(string.Format("Cannot restore Int32 from '{0}'", licenseString));
            }
            byte[] bytes = UnpackString(licenseString);
            //if (bytes.Length < 4)
            //{
            //    throw new ArgumentException(string.Format("Cannot restore Int32 from '{0}'", licenseString));
            //}

            return (Int32)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
        }
        /// <summary>
        /// Unpack string with format XXXX into int16
        /// </summary>
        /// <param name="licenseString"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">"Cannot restore Int16 from licenseString</exception>
        public static Int16 UnpackStringInt16(string licenseString)
        {
            if (licenseString.Trim().Replace(groupsDelimiter, "").Length < 4)
            {
                throw new ArgumentException(string.Format("Cannot restore Int16 from '{0}'", licenseString));
            }
            byte[] bytes = UnpackString(licenseString);

            return (Int16)(bytes[0] << 8 | bytes[1]);
        }

        /// <summary>
        /// Convert hex string to bytes, for example "12ef" will be converted to byte[] {0x12, 0xEF}.
        /// When source string length is odd, string will be padded with zero, for example, "12f" will be padded as "12f0".
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string str)
        {
            if (str.Length % 2 > 0)
            {
                str = str + "0";
            }
            int length = str.Length / 2;
            byte[] bytes = new byte[length];

            for (int i = 0; i < length; i++)
            {
                bytes[i] = byte.Parse(str.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            return bytes;
        }

        /// <summary>
        /// Convert byte array to hex string, for example, byte[] {0x12, 0xEF} will be converted to "12ef".
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
