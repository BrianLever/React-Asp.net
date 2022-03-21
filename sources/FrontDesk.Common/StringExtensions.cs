using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FrontDesk.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// Return string as phone number w/o any non-digit separators
        /// </summary>
        /// <param name="phone">Formatted phone number</param>
        /// <returns>Phone number w/o non-digit charactgers</returns>
        public static string AsRawPhoneNumber(this string phone)
        {
            if (String.IsNullOrEmpty(phone))
                return phone;

            Regex regex = new Regex(@"\D", RegexOptions.IgnoreCase);
            return regex.Replace(phone, string.Empty);

        }

        /// <summary>
        /// Split 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="stringSegmentLengths"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitTo3AddressLines(this string address, params int[] stringSegmentLengths)
        {
            List<string> addressLines = new List<string>(3);
            if (stringSegmentLengths.Length == 0)
            {
                stringSegmentLengths = new int[] { 30 };
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                return new string[0];
            }

            Regex regex = new Regex(@"(?:(?<line1>P\.?O\.?\s*\w*\s*\d+)\s*(?<line2>.*))|(?:(?<line1>.+)\s+(?<line2>P\.?O\.?\s*\w*\s*\d+)+?)", RegexOptions.IgnoreCase);
            if (regex.IsMatch(address))
            {
                var match = regex.Match(address);
                if (match.Success && match.Groups.Count > 0)
                {
                    if (match.Groups["line1"].Success)
                    {
                        addressLines.Add(match.Groups["line1"].Value.Trim());
                    }
                    if (match.Groups["line2"].Success)
                    {
                        addressLines.Add(match.Groups["line2"].Value.Trim());
                    }
                }


            }
            else
            {
                addressLines.Add(address);
            }

            for (int i = 0; i < addressLines.Count; i++)
            {
                int maxLength = (stringSegmentLengths.Length >= i ? stringSegmentLengths[i] : stringSegmentLengths.Last());
                if (addressLines[i].Length > maxLength)
                {
                    //exceeded max number of characters in row, splot line and get max capacity of space separated words
                    var segements = addressLines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder strBuffer = new StringBuilder();
                    int additionalLines = 0;
                    foreach (var sg in segements)
                    {
                        maxLength = (stringSegmentLengths.Length >= i ? stringSegmentLengths[i] : stringSegmentLengths.Last());

                        if ((strBuffer.Length + sg.Length <= maxLength - 1) || strBuffer.Length == 0) //consider space as separater
                        {
                            if (strBuffer.Length > 0)
                                strBuffer.Append(" ");
                            strBuffer.Append(sg);
                        }
                        else
                        {
                            if (additionalLines == 0)
                            {
                                addressLines[i] = strBuffer.ToString(); //set line limited with max length
                            }
                            else
                            {
                                addressLines.Insert(i, strBuffer.ToString());
                            }
                            i++;
                            strBuffer.Clear();
                            strBuffer.Append(sg);

                            additionalLines++;
                        }
                    }

                    if (strBuffer.Length > 0)
                    {
                        addressLines.Insert(i, strBuffer.ToString());
                    }
                }
            }


            if (addressLines.Count > 3)
            {
                addressLines[2] = string.Join(" ", addressLines.Where((s, index) => index >= 2));
                addressLines.RemoveRange(3, addressLines.Count - 3);
            }

            return addressLines;
        }


        
    }
}
