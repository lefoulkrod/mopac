using System;

namespace Mopac.Library.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Concatinates a value to a string using the seperator or just returns the value if the string is null or empty.
        /// </summary>
        /// <param name="me">The string to concatinate to</param>
        /// <param name="seperator">The seperator used to concatinate the two strings</param>
        /// <param name="value">The value to be concatinated to the string</param>
        /// <returns>A new string which is either the existing string concatinated with the new value or the value.</returns>
        public static string ConcatOrBegin(this string me, string seperator, string value)
        {
            if (seperator == null) throw new ArgumentNullException(nameof(seperator));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty( me))
            {
                return value;
            }

            return string.Concat(me, seperator, value);
        }
    }
}