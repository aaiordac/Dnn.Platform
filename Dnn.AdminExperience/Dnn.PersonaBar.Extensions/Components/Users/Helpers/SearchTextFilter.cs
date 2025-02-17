﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace Dnn.PersonaBar.Users.Components.Helpers
{
    using System.Text;
    using System.Text.RegularExpressions;

    public class SearchTextFilter
    {
        public static string CleanWildcards(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || searchText.Equals("*") || searchText.Equals("%"))
            {
                return null;
            }

            var pattern = string.Empty;

            var prefixWildcard = searchText.StartsWith("%") || searchText.StartsWith("*");
            var suffixWildcard = searchText.EndsWith("%") || searchText.EndsWith("*");

            bool iN_STRING = prefixWildcard == true && suffixWildcard == true;
            bool pREFIX = prefixWildcard == true && suffixWildcard == false;
            bool sUFFIX = suffixWildcard == true && prefixWildcard == false;
            bool eXACT = suffixWildcard == false && prefixWildcard == false;

            if (eXACT)
            {
                pattern = searchText.Replace("*", string.Empty).Replace("%", string.Empty);
            }
            else
            {
                if (iN_STRING == true)
                {
                    pattern = GetInStringSearchPattern(searchText);
                }
                else if (pREFIX == true)
                {
                    pattern = GetPrefixSearchPattern(searchText);
                }
                else if (sUFFIX == true)
                {
                    pattern = GetSuffixSearchPattern(searchText);
                }
            }

            return pattern;
        }

        private static string GetInStringSearchPattern(string searchText)
        {
            var pattern = new StringBuilder();
            var regexOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            var inStringRegex = "^(\\*|%)?([\\w\\-_\\s\\*\\%\\.\\@]+)(\\*|%)$";
            var regex = new Regex(inStringRegex, regexOptions);
            var matches = regex.Matches(searchText);

            if (matches.Count > 0)
            {
                var matchText = matches[0].Groups[2].Value;
                if (matchText != null && !string.IsNullOrEmpty(matchText))
                {
                    pattern.Append("%");
                    pattern.Append(matchText.Replace("*", string.Empty).Replace("%", string.Empty));
                    pattern.Append("%");
                }
            }

            return pattern.ToString();
        }

        private static string GetPrefixSearchPattern(string searchText)
        {
            var regexOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            var pattern = new StringBuilder();
            var prefixRegex = "^(\\*|%)?([\\w\\-_\\s\\*\\%\\.\\@]+)";
            var regex = new Regex(prefixRegex, regexOptions);
            var matches = regex.Matches(searchText);

            if (matches.Count > 0)
            {
                var matchText = matches[0].Groups[2].Value;
                if (matchText != null && !string.IsNullOrEmpty(matchText))
                {
                    pattern.Append("%");
                    pattern.Append(matchText.Replace("*", string.Empty).Replace("%", string.Empty));
                }
            }

            return pattern.ToString();
        }

        private static string GetSuffixSearchPattern(string searchText)
        {
            var pattern = new StringBuilder();
            var regexOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            var suffixRegex = "([\\w\\-_\\*\\s\\%\\.\\@]+)(\\*|%)$";
            var regex = new Regex(suffixRegex, regexOptions);
            var matches = regex.Matches(searchText);

            if (matches.Count > 0)
            {
                var matchText = matches[0].Groups[1].Value;
                if (matchText != null && !string.IsNullOrEmpty(matchText))
                {
                    pattern.Append(matchText.Replace("*", string.Empty).Replace("%", string.Empty));
                    pattern.Append("%");
                }
            }

            return pattern.ToString();
        }
    }
}
