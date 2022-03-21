using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontDesk
{
    public static class LogExtentions
    {
        private const char MASK_SYMBOL = '*';

        /// <summary>
        /// Mask patient name for logs to make it HIPPA complient
        /// </summary>
        /// <param name="patientName">Full patient name in the format LASTNAME, FIRST MIDDLE</param>
        /// <returns>Masked patient full name , that includes firt 3 latter form the last name and only capital letter from other names.
        /// All other characters are replaced by 'start' symbol
        /// Special cases:
        /// 1. When last name has less than 3 letters, the last name is masked except the first letter
        /// 2. When last name has less than 6 letter, the last 3 letters are replaced by mask symbol
        /// </returns>
        public static string AsMaskedFullName(this string patientName)
        {
            var maskedPatientName = string.Empty;

            List<string> maskedNames = new List<string>();


            if (string.IsNullOrWhiteSpace(patientName)) return string.Empty;

            var parts = patientName.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var maskedLastName = MaskLastName(parts.First());

            foreach (var name in parts.Skip(1))
            {
                maskedNames.Add(MaskName(name));
            }

            if (parts.Length == 1)
                return maskedLastName;

            return string.Format("{0}, {1}", maskedLastName, string.Join(" ", maskedNames));
        }

        private static string MaskName(string name)
        {
            return MaskString(name, name.Length > 1 ? 1 : 0);
        }
        
        private static string MaskLastName(string lastName)
        {
            var length = lastName.Length;
            int maskStartPoint = 3; //default mask rule

            if (length <= 3) maskStartPoint = 1;
            else if (length <= 6) maskStartPoint = length - 3;

            return MaskString(lastName, maskStartPoint);
        }

        private static string MaskString(string name, int maskFromPosition = 1)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;

            char[] letters = name.ToArray();

            for (var index = maskFromPosition; index < name.Length; index++)
            {
                letters[index] = MASK_SYMBOL;
            }

            return string.Join("", letters);
        }


       
    }
}
