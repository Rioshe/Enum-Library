using System.Collections.Generic;
using System.Linq;
namespace TC.EnumLibrary {
    public static class StringExtension {
        /// <summary>
        /// Determines whether the specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="val">The string to test.</param>
        /// <returns>True if the value parameter is null or <see cref="System.String.Empty"/>, or if value consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string val) => string.IsNullOrWhiteSpace(val);

        /// <summary>
        /// Converts the input string to an alphanumeric string, optionally allowing periods.
        /// </summary>
        /// <param name="input">The input string to be converted.</param>
        /// <param name="allowPeriods">A boolean flag indicating whether periods should be allowed in the output string.</param>
        /// <returns>
        /// A new string containing only alphanumeric characters, underscores, and optionally periods.
        /// If the input string is null or empty, an empty string is returned.
        /// </returns>
        public static string ConvertToAlphanumeric(this string input, bool allowPeriods = false) {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            List<char> filteredChars = new List<char>();
            int lastValidIndex = -1;

            // Iterate over the input string, filtering and determining valid start/end indices
            foreach (char character in input
                         .Where(character => char
                                    .IsLetterOrDigit(character) || character == '_' || (allowPeriods && character == '.'))
                         .Where(character => filteredChars.Count != 0 || (!char.IsDigit(character) && character != '.'))) {

                filteredChars.Add(character);
                lastValidIndex = filteredChars.Count - 1; // Update lastValidIndex for valid characters
            }

            // Remove trailing periods
            while (lastValidIndex >= 0 && filteredChars[lastValidIndex] == '.') {
                lastValidIndex--;
            }

            // Return the filtered string
            return lastValidIndex >= 0
                ? new string(filteredChars.ToArray(), 0, lastValidIndex + 1) : string.Empty;
        }
    }
}