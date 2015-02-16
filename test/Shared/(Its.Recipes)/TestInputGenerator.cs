// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// THIS FILE IS NOT INTENDED TO BE EDITED. 
// 
// It has been imported using NuGet from the Its.Recipes project (http://codebox/ItsRecipes). 
// 
// This file can be updated in-place using the Package Manager Console. To check for updates, run the following command:
// 
// PM> Get-Package -Updates

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Microsoft.Its.Recipes
{
    /// <summary>
    ///     Generates values for test cases.
    /// </summary>
    /// <remarks>
    ///     This class was inspired and refactored from the Any class presented in the Microsoft TDD class.
    ///     <see href="http://mylearning/coursedetails.aspx?COURSENO=COUR2006051616181090700057" />
    ///     Additional materials for the class can be found here:
    ///     <see href="http://www.netobjectives.com/downloads/TDD_Course_Materials.html" />
    /// </remarks>
#if !RecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    internal static partial class Any
    {
        public delegate void Trace(string method, object value);

        public static Trace TraceGeneratedValues = (method, value) => { };

        private const string CharsFrom0ToA = @":;<=>?@";
        private const string CharsFromZToa = @"[\]^`_";
        private const string CharsFrom0Toz = CharsFrom0ToA + CharsFromZToa;
        private const int LargestChar = 250;
        private const int SmallestChar = 32;

        /// <remarks>
        /// Since this is a static class and Random is not thread-safe, we use a ThreadLocal.
        /// </remarks>
        private static readonly ThreadLocal<Random> s_random = new ThreadLocal<Random>(() => new Random((int)DateTime.Now.Ticks));

        /// <summary>
        ///     Generates an alphanumeric char.
        /// </summary>
        /// <returns>A char.</returns>
        public static char AlphanumericChar()
        {
            char value;
            do
            {
                value = Char('0', 'z');
            } while (CharsFrom0Toz.IndexOf(value) != -1);

            TraceGeneratedValues("AlphanumericChar", value);

            return value;
        }

        /// <summary>
        ///     Generates a string.
        /// </summary>
        /// <param name="minLength">The minimum desired length.</param>
        /// <param name="maxLength">The maximum desired length.</param>
        public static string AlphanumericString(int minLength = 0, int? maxLength = null)
        {
            return String(minLength, maxLength, Characters.LatinLettersAndNumbers());
        }

        /// <summary>
        ///     Generate an ASCII char.
        /// </summary>
        public static char AsciiChar()
        {
            var value = (char)Int(SmallestChar, LargestChar);
            TraceGeneratedValues("AsciiChar", value);
            return value;
        }

        /// <summary>
        ///     Generate Bool Value
        /// </summary>
        public static bool Bool()
        {
            var value = Int(0, 1) == 1;
            TraceGeneratedValues("Bool", value);
            return value;
        }

        /// <summary>
        ///     Generate Byte Value
        /// </summary>
        public static byte Byte()
        {
            var value = (byte)Int(byte.MinValue, byte.MaxValue);
            TraceGeneratedValues("Byte", value);
            return value;
        }

        /// <summary>
        ///     Generate Char Value limited from below and from above
        /// </summary>
        /// <param name="first">Lower limit</param>
        /// <param name="last">Upper Limit</param>
        public static char Char(char first, char last)
        {
            var value = (char)Int(first, last);
            TraceGeneratedValues("Char", value);
            return value;
        }

        /// <summary>
        /// Generate a random <see cref="DateTimeOffset" />.
        /// </summary>
        public static DateTimeOffset DateTimeOffset(
            string min = "1970-01-01 00:00:00 +00:00",
            string max = "2050-01-01 00:00:00 +00:00",
            TimeSpan? offset = null)
        {
            var ticks = Long(System.DateTimeOffset.Parse(min).Ticks, System.DateTimeOffset.Parse(max).Ticks);

            var value = new DateTimeOffset(ticks,
                                           offset ?? TimeSpan(0, Int(-12, 12), 0, 0, 0));

            TraceGeneratedValues("DateTimeOffset", value.ToString());
            return value;
        }

        /// <summary>
        /// Generate a random <see cref="TimeSpan" />.
        /// </summary>
        public static TimeSpan TimeSpan(
            int? days = null,
            int? hours = null,
            int? minutes = null,
            int? seconds = null,
            int? milliseconds = null)
        {
            var value = new TimeSpan(days ?? Int(0, 1000),
                                     hours ?? Int(0, 23),
                                     minutes ?? Int(0, 59),
                                     seconds ?? Int(0, 59),
                                     milliseconds ?? Int(0, 999));
            TraceGeneratedValues("TimeSpan", value.ToString());
            return value;
        }

        /// <summary>
        ///     Generate a decimal.
        /// </summary>
        /// <param name="min">The minimum value to be generated.</param>
        /// <param name="max">The maximum value to be generated.</param>
        /// <returns>A decimal between the specified min and max values.</returns>
        public static decimal Decimal(decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
        {
            var double0To1 = s_random.Value.NextDouble();
            var randomDouble = (double0To1 * (double)max) + ((1 - double0To1) * (double)min);
            var value = (decimal)randomDouble;
            TraceGeneratedValues("Decimal", value);
            return value;
        }

        /// <summary>
        ///     Generate a double.
        /// </summary>
        /// <param name="min">The minimum value to be generated.</param>
        /// <param name="max">The maximum value to be generated.</param>
        /// <returns>A double</returns>
        public static double Double(double min = double.MinValue, double max = double.MaxValue)
        {
            var double0To1 = s_random.Value.NextDouble();
            var randomDouble = (double0To1 * max) + ((1 - double0To1) * min);
            TraceGeneratedValues("Double", randomDouble);
            return randomDouble;
        }

        /// <summary>
        ///     Generates an email.
        /// </summary>
        /// <param name="topLevelDomain">The top level domain for the email.</param>
        /// <returns>An email</returns>
        public static string Email(string topLevelDomain = null)
        {
            var email = string.Format("{0}@{1}",
                                      Regex.Replace(FullName(),
                                                    @"(""[^""""]+"")|([ .,]+)",
                                                    @""),
                                      DomainName(topLevelDomain));
            TraceGeneratedValues("Email", email);
            return email;
        }

        /// <summary>
        /// Generates a domain name
        /// </summary>
        /// <param name="topLevelDomain">The top level domain.</param>
        public static string DomainName(string topLevelDomain = null)
        {
            if (string.IsNullOrWhiteSpace(topLevelDomain))
            {
                topLevelDomain = TopLevelDomain();
            }

            return CamelCaseName(Int(1, 3)) + topLevelDomain;
        }

        /// <summary>
        /// Generates a company name
        /// </summary>
        public static string CompanyName()
        {
            string companyName = null;

            switch (Int(1, 5))
            {
                case 1:
                case 2:
                    companyName = string.Format(Names.CompanyNameWordFormats.RandomElement(),
                                                Words(1, true).Single());
                    break;
                case 3:
                case 4:
                    companyName = string.Format(Names.CompanyNameFormats.RandomElement(),
                                                string.Join(" ",
                                                            Enumerable.Range(1, Int(1, 2))
                                                                      .SelectMany(_ => Words(1, true))));
                    break;
                case 5:
                    companyName = string.Format(
                        "{0} & {1}", string.Join(", ",
                                                 Enumerable.Range(1, Int(1, 3)).Select(_ => LastName())),
                        LastName());
                    break;
                default:
                    companyName = DomainName();
                    break;
            }

            if (Int(1, 3) == 1)
            {
                companyName = string.Format(Names.CompanyNameFormats.RandomElement(), companyName);
            }

            return companyName;
        }

        /// <summary>
        /// Returns a random top level domain.
        /// </summary>
        public static string TopLevelDomain()
        {
            return Names.TopLevelDomains.RandomElement();
        }

        /// <summary>
        ///     Generates a float
        /// </summary>
        /// <param name="min">The minimum value to be generated.</param>
        /// <param name="max">The maximum value to be generated.</param>
        public static float Float(float min = float.MinValue, float max = float.MaxValue)
        {
            var value = (float)Double(min, max);
            TraceGeneratedValues("Float", value);
            return value;
        }

        /// <summary>
        ///     Generates a guid
        /// </summary>
        public static Guid Guid()
        {
            var guid = System.Guid.NewGuid();
            TraceGeneratedValues("Guid", guid);
            return guid;
        }

        /// <summary>
        ///     Generate Integer Value limited from below and from above
        /// </summary>
        /// <param name="min">The minimum value to be generated.</param>
        /// <param name="max">The maximum value to be generated.</param>
        public static int Int(int min = int.MinValue, int max = int.MaxValue)
        {
            var val = s_random.Value.Next(min, max >= int.MaxValue ? int.MaxValue : max + 1);
            TraceGeneratedValues("Int", val);
            return val;
        }

        /// <summary>
        ///     Generates an IntPtr
        /// </summary>
        public static IntPtr IntPtr()
        {
            var value = (IntPtr)Int();
            TraceGeneratedValues("IntPtr", value);
            return value;
        }

        /// <summary>
        ///     Generate Long Value
        /// </summary>
        public static long Long(long min = long.MinValue, long max = long.MaxValue)
        {
            var nextDouble = s_random.Value.NextDouble();
            double range = max - min;

            var value = (long)(nextDouble * range) + min;
            TraceGeneratedValues("Long", value);
            return value;
        }

        /// <summary>
        ///     Generate Positive Integer Value limited from above
        /// </summary>
        /// <param name="max">Upper Limit</param>
        public static int PositiveInt(int max = int.MaxValue)
        {
            return Int(1, max);
        }

        /// <summary>
        ///     Returns a random value from the values of the given enumeration type.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to select a value from.</typeparam>
        /// <returns>A random value from the values of the given enumeration type.</returns>
        /// <remarks>
        /// These is a way in IL to represent an enumeration constraint, but it's too complicated for just this usage
        /// see: http://msmvps.com/blogs/jon_skeet/archive/2009/09/10/generic-constraints-for-enums-and-delegates.aspx
        /// </remarks>
        public static TEnum EnumValue<TEnum>()
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException(string.Format("TEnum must be an enum type ({0} is not).", typeof(TEnum).FullName));
            }

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray().RandomElement();
        }

        /// <summary>
        ///     Selects a random element from the specified collection.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <returns>A random element from the specified collection.</returns>
        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            return source.RandomSequence(1).Single();
        }

        /// <summary>
        ///     Selects the specified number of elements randomly from the given sequence.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="numberOfElements">The number of elements to return.</param>
        /// <returns>The specified number of elements, selected randomly from the specified sequence.</returns>
        /// <remarks>
        /// Note that the returned elements are not distinct (the same element may be returned multiple times).
        /// </remarks>
        public static IEnumerable<T> RandomSequence<T>(this IEnumerable<T> source, int numberOfElements)
        {
            var sourceAsList = source.AsList();
            var lastItemIndex = sourceAsList.Count - 1;
            return Enumerable.Range(1, numberOfElements)
                             .Select(_ => sourceAsList[Int(min: 0, max: lastItemIndex)]);
        }

        /// <summary>
        ///     Selects a specified number of elements randomly from a sequence.
        /// </summary>
        [Obsolete("This method has an ambiguous name. Please Use RandomSequence or RandomSubset.", false)]
        public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int numberOfItems)
        {
            return source.RandomSequence(numberOfItems);
        }

        /// <summary>
        ///     Sorts the elements of a sequence in a random order.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>A sequence whose elements are sorted in a random order.</returns>
        public static IEnumerable<T> OrderByRandom<T>(this IEnumerable<T> source)
        {
            var sourceAsList = source.AsList();
            var indices = Enumerable.Range(0, sourceAsList.Count).ToArray();
            indices.Shuffle();
            return indices.Select(index => sourceAsList[index]);
        }


        /// <summary>
        ///     Selects the specified number of distinct elements randomly from the given sequence.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="numberOfElements">The number of elements to return.</param>
        /// <returns>The specified number of elements, selected randomly from the specified sequence.</returns>
        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int numberOfElements)
        {
            return source.OrderByRandom().Take(numberOfElements);
        }

        /// <summary>
        ///     Returns a sequence using the provided function to generate each item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="next">A function called to create each item in the sequence.</param>
        /// <param name="count">The number of items in the sequence.</param>
        public static IEnumerable<T> Sequence<T>(Func<int, T> next, int count = 5)
        {
            return Enumerable.Range(1, count).Select(next);
        }

        /// <summary>
        /// Generate a random common English words
        /// </summary>
        public static string Word()
        {
            return Words(1).Single();
        }

        /// <summary>
        /// Generate a sequence of common English words
        /// </summary>
        /// <param name="wordCount">Number of words to return</param>
        /// <param name="capitalize">True to capitalize the words</param>
        public static IEnumerable<string> Words(int wordCount = 4, bool capitalize = false)
        {
            var words = Recipes.Words.Common.RandomSequence(wordCount);
            if (capitalize)
            {
                words = words.Select(w => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(w));
            }
            return words;
        }

        /// <summary>
        /// Generate a CamelCaseName using <see cref="Words"/>
        /// </summary>
        /// <param name="wordCount">Number of words to compose together as the name</param>
        public static string CamelCaseName(int wordCount = 4)
        {
            return string.Join("", Words(wordCount, true));
        }

        /// <summary>
        /// Returns a random first name.
        /// </summary>
        public static string FirstName()
        {
            var firstName = Names.FirstNames.RandomElement();
            TraceGeneratedValues("FirstName", firstName);
            return firstName;
        }

        /// <summary>
        /// Returns a random full name, including possible titles and suffixes, because they're funny.
        /// </summary>
        public static string FullName()
        {
            var firstName = FirstName();
            string middle = "";

            switch (Int(1, 10))
            {
                case 1:
                case 2:
                case 3:
                    // middle initial
                    middle = Characters.LatinUppercase.RandomElement() + ". ";
                    break;
                case 4:
                case 5:
                    // middle name option 1
                    middle = FirstName();
                    break;
                case 6:
                case 7:
                case 8:
                    // middle name option 2
                    middle = Words(1, true).Single();
                    break;
                case 9:
                case 10:
                    // "nickname"
                    middle = "\"" + ((Int(1, 2) == 1 ? "The " : "")) + Words(1, true).Single() + "\"";

                    // sometimes nicknames come before the first name
                    if (Int(1, 4) == 1)
                    {
                        var temp = middle;
                        middle = firstName;
                        firstName = temp;
                    }
                    break;
            }

            var title = (Int(1, 10) == 1 ? Names.Titles.RandomElement() + " " : "");
            var suffix = (Int(1, 10) == 1 ? ", " + Names.Suffixes.RandomElement() : "");

            var fullName = string.Format("{0}{1} {2} {3}{4}", title, firstName, middle, LastName(), suffix).Replace("  ", " ");
            TraceGeneratedValues("FullName", fullName);
            return fullName;
        }

        /// <summary>
        /// Returns a random last name.
        /// </summary>
        public static string LastName()
        {
            var lastName = "";
            var prefixOrSuffixFreqPer10 = 1;

            switch (Int(1, 8))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    // choose a name from Names.LastName
                    lastName = Names.LastNames.RandomElement();
                    prefixOrSuffixFreqPer10 = 2;
                    break;
                case 5:
                case 6:
                    // pick a word
                    lastName = Words(1, true).Single();
                    prefixOrSuffixFreqPer10 = 6;
                    break;
                case 7:
                case 8:
                    // make one up out of words!
                    lastName = Words(1, true).Single() + (Bool() ? Words(1).Single() : "");
                    prefixOrSuffixFreqPer10 = 0;
                    break;
            }

            if (Int(1, 10) <= prefixOrSuffixFreqPer10)
            {
                // some people have a prefix
                var prefix = Names.LastNamePrefixes.RandomElement();
                lastName = prefix + lastName;
            }

            if (Int(1, 10) <= prefixOrSuffixFreqPer10)
            {
                // some people have a suffix
                var suffix = Names.LastNameSuffixes.RandomElement();
                lastName = lastName + suffix;
            }

            TraceGeneratedValues("LastName", lastName);

            return lastName;
        }

        /// <summary>
        /// Generate a bunch of words space-delimited using <see cref="Words"/>
        /// </summary>
        /// <param name="wordCount">Number of words to compose together as the name</param>
        public static string Paragraph(int wordCount = 50)
        {
            return string.Join(" ", Words(wordCount));
        }

        /// <summary>
        ///     Generate Short Value
        /// </summary>
        public static short Short()
        {
            var value = (short)Int(short.MinValue, short.MaxValue);
            TraceGeneratedValues("Short", value);
            return value;
        }

        /// <summary>
        ///     Generates a random string.
        /// </summary>
        /// <param name="minLength">The minimum desired length.</param>
        /// <param name="maxLength">The maximum desired length. If null, a string of length 1 - 100 is returned.</param>
        /// <param name="characterSet">The character set from which the string is drawn. If null, <see cref="Characters.Unicode" /> is used.</param>
        /// <returns>
        ///     The generated string.
        /// </returns>
        public static string String(int minLength = 0, int? maxLength = null, IEnumerable<char> characterSet = null)
        {
            maxLength = maxLength ?? (Int(1, 100) + minLength);
            characterSet = characterSet ?? Characters.Unicode();

            var s = new string(Enumerable
                                   .Range(0, Int(minLength, maxLength.Value))
                                   .Select(i => characterSet.RandomElement()).ToArray());

            TraceGeneratedValues("String", s);

            return s;
        }

        /// <summary>
        /// Generates a random <see cref="Uri" />.
        /// </summary>
        /// <param name="scheme">The URI scheme.</param>
        /// <param name="tld">The URI's top-level domain.</param>
        /// <param name="allowQuerystring">If true, allow a random querystring to be part of the URI.</param>
        public static Uri Uri(
            string scheme = "http",
            string tld = null,
            bool allowQuerystring = true)
        {
            var builder = new UriBuilder(scheme)
            {
                Host = DomainName(tld),
                Path = UriPath(1)
            };

            if (allowQuerystring && Bool())
            {
                builder.Query = string.Join("&", Enumerable.Range(1, 10).Select(_ => string.Format("{0}={1}", Paragraph(1), Paragraph(1))));
            }

            return builder.Uri;
        }

        /// <summary>
        /// Generates a random <see cref="Uri" /> Path.
        /// </summary>
        /// <param name="minSegments">Minimum number of path segments.</param>
        /// <param name="maxSegments">Maximum number of path segments.</param>
        public static string UriPath(int minSegments = 0, int maxSegments = 5)
        {
            return string.Join("/", Words(Int(minSegments, maxSegments)));
        }
    }

    /// <summary>
    ///     Provides access to several common character sets as enumrable sequences.
    /// </summary>
#if !RecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    internal static partial class Characters
    {
        private static readonly Lazy<char[]> s_unicode = new Lazy<char[]>(
            () => Enumerable.Range(0, 0xD7FF).Select(Convert.ToChar).ToArray());

        private static readonly Lazy<char[]> s_latinLetters = new Lazy<char[]>(
            () => LatinUppercase.Concat(LatinLowercase).ToArray());

        private static readonly Lazy<char[]> s_latinLettersAndNumbers = new Lazy<char[]>(
            () => LatinUppercase
                      .Concat(LatinLowercase)
                      .Concat(Digits)
                      .ToArray());

        private static readonly Lazy<char[]> s_latinLettersAndNumbersAndUnderscore = new Lazy<char[]>(
            () => LatinLettersAndUnderscore()
                      .Concat(Digits)
                      .ToArray());

        private static readonly Lazy<char[]> s_latinLettersAndUnderscore = new Lazy<char[]>(
            () => LatinUppercase
                      .Concat(LatinLowercase)
                      .Concat(new[] { '_' })
                      .ToArray());

        private static readonly Lazy<char[]> s_latinLettersNumbersAndWhiteSpace = new Lazy<char[]>(
            () => LatinUppercase
                      .Concat(LatinLowercase)
                      .Concat(Digits)
                      .Concat(WhiteSpace)
                      .ToArray());

        private static readonly Lazy<char[]> s_latinLettersNumbersPunctuationAndWhiteSpace = new Lazy<char[]>(
            () => LatinUppercase.Concat(LatinLowercase)
                                .Concat(Digits)
                                .Concat(Punctuation)
                                .Concat(WhiteSpace)
                                .ToArray());

        private static readonly Lazy<char[]> s_latinLettersAndWhiteSpace = new Lazy<char[]>(
            () => LatinUppercase.Concat(LatinLowercase)
                                .Concat(WhiteSpace)
                                .ToArray());

        /// <summary>
        ///     Digits from 0 to 9.
        /// </summary>
        public static IReadOnlyList<char> Digits = GetCharacters('0', '9').ToArray();

        /// <summary>
        ///     Lowercase Latin letters
        /// </summary>
        public static IReadOnlyList<char> LatinLowercase = GetCharacters('a', 'z').ToArray();

        /// <summary>
        ///     Uppercase Latin letters
        /// </summary>
        public static IReadOnlyList<char> LatinUppercase = GetCharacters('A', 'Z').ToArray();

        /// <summary>
        ///     Common punctuation characters.
        /// </summary>
        /// <returns>Character Set</returns>
        public static IReadOnlyList<char> Punctuation =
            GetCharacters('!', '/')
                .Concat(GetCharacters(':', '@'))
                .Concat(GetCharacters('[', '`'))
                .Concat(GetCharacters('{', '~'))
                .ToArray();

        /// <summary>
        ///     The set of whitespace characters, excluding '\v' and '\f' for XML compatibility.
        /// </summary>
        public static readonly IReadOnlyList<char> WhiteSpace = Unicode()
            .Where(Char.IsWhiteSpace)
            .Except(new[] { '\v', '\f' })
            .ToArray();

        /// <summary>
        ///     ASCII characters from 0 - 127.
        /// </summary>
        /// <returns>Character Set</returns>
        public static readonly IReadOnlyList<char> Ascii = Enumerable.Range(0, 128)
                                                                   .Select(Convert.ToChar)
                                                                   .ToArray();

        /// <summary>
        ///     ASCII characters from 0 - 255.
        /// </summary>
        /// <returns>Character Set</returns>
        public static IReadOnlyList<char> AsciiExtended = Enumerable.Range(0, 256)
                                                                  .Select(Convert.ToChar)
                                                                  .ToArray();

        /// <summary>
        ///     Generates a character set bounded by the two provided characters, inclusive.
        /// </summary>
        /// <param name="from">Start Char</param>
        /// <param name="to">End Char</param>
        /// <returns>Result Character Set</returns>
        public static IEnumerable<char> GetCharacters(char from, char to)
        {
            var startIndex = Convert.ToInt32(@from);
            var endIndex = Convert.ToInt32(to);

            for (var i = startIndex; i <= endIndex; i++)
            {
                yield return Convert.ToChar(i);
            }
        }

        /// <summary>
        ///     Gets all Latin letters.
        /// </summary>
        /// <returns>Character Set</returns>
        public static IReadOnlyList<char> LatinLetters()
        {
            return s_latinLetters.Value;
        }

        /// <summary>
        ///     Gets all Latin letters and numbers.
        /// </summary>
        /// <returns>Character Set</returns>
        public static IReadOnlyList<char> LatinLettersAndNumbers()
        {
            return s_latinLettersAndNumbers.Value;
        }

        /// <summary>
        ///     Gets all Latin letters and numbers.
        /// </summary>
        /// <returns>Character Set</returns>
        public static IReadOnlyList<char> LatinLettersAndUnderscore()
        {
            return s_latinLettersAndUnderscore.Value;
        }

        /// <summary>
        ///     Gets all Latin letters and numbers.
        /// </summary>
        /// <returns>Character Set</returns>
        public static IReadOnlyList<char> LatinLettersAndNumbersAndUnderscore()
        {
            return s_latinLettersAndNumbersAndUnderscore.Value;
        }

        /// <summary>
        ///     Gets Latin letters, numbers, and whitespace characters.
        /// </summary>
        public static IReadOnlyList<char> LatinLettersNumbersAndWhiteSpace()
        {
            return s_latinLettersNumbersAndWhiteSpace.Value;
        }

        /// <summary>
        ///     Gets Latin letters, numbers, punctuation, and whitespace characters.
        /// </summary>
        public static IReadOnlyList<char> LatinLettersNumbersPunctuationAndWhiteSpace()
        {
            return s_latinLettersNumbersPunctuationAndWhiteSpace.Value;
        }

        /// <summary>
        ///     Gets all Latin letters and whitespace characters.
        /// </summary>
        /// <returns>Character Set</returns>
        public static IReadOnlyList<char> LatinLettersAndWhiteSpace()
        {
            return s_latinLettersAndWhiteSpace.Value;
        }

        /// <summary>
        ///     Gets all Unicode characters
        /// </summary>
        public static IReadOnlyList<char> Unicode()
        {
            return s_unicode.Value;
        }
    }

    internal static partial class Names
    {
        #region FirstNames

        internal static string[] FirstNames = new[]
        {
            "Abe",
            "Ada",
            "Adonis",
            "Ahab",
            "Alan",
            "Albert",
            "Alec",
            "Alex",
            "Angela",
            "Angus",
            "Aragorn",
            "Arnold",
            "Arthur",
            "Ashmina",
            "Autumn",
            "Axl",
            "Babs",
            "Barack",
            "Barbara",
            "Barry",
            "Bernadette",
            "Bernie",
            "Bertha",
            "Bilbo",
            "Billy",
            "Blanche",
            "Bo",
            "Bonnie",
            "Boris",
            "Brad",
            "Branford",
            "Brenda",
            "Brendan",
            "Brett",
            "Brienne",
            "Brock",
            "Bruce",
            "Brunhilde",
            "Burt",
            "Cadwallader",
            "Caesar",
            "Cate",
            "Chalmers",
            "Charles",
            "Charlie",
            "Chet",
            "Chip",
            "Chuck",
            "Claude",
            "Clifford",
            "Clive",
            "Corgi",
            "Cormac",
            "Curly",
            "Daniel",
            "Django",
            "Dmitry",
            "Dolph",
            "Donald",
            "Doris",
            "Dusky",
            "Dwayne",
            "Edward",
            "Eleanor",
            "Elvis",
            "Enoch",
            "Flavor",
            "Frodo",
            "Gandalf",
            "Garth",
            "Gary",
            "George",
            "Gilligan",
            "Glinda",
            "Gracia",
            "Grant",
            "Greedo",
            "Greta",
            "Grover",
            "Gustav",
            "Gwenda",
            "Hank",
            "Harrison",
            "Haskell",
            "Henrietta",
            "Henry",
            "Herbert",
            "Hirendra",
            "Homer",
            "Horatio",
            "Horst",
            "Hugh",
            "Iggy",
            "Italo",
            "Jake",
            "James",
            "Jane",
            "Jason",
            "Jean-Claude",
            "Jean-Luc",
            "Jean-Pierre",
            "Jedediah",
            "Jeff",
            "Jerry",
            "Jesco",
            "Jimbo",
            "Joey",
            "John",
            "Johnny",
            "Jonathan",
            "Jose",
            "Josephine",
            "Jo\u00E3o",
            "Judd",
            "Julie",
            "Julius",
            "Junebug",
            "J\u00FCrgen",
            "Justin",
            "Keith",
            "Kevin",
            "Kurt",
            "Laird",
            "Lando",
            "Larry",
            "Larry",
            "Lars",
            "Laverna",
            "Leia",
            "Leo",
            "Leslie",
            "Liam",
            "Linus",
            "Livar",
            "Lou",
            "Lourdes",
            "Lucilla",
            "Lucy",
            "Lukasz",
            "Lyle",
            "Mack",
            "Macy",
            "Madonna",
            "Mahadev",
            "Marge",
            "Marshall",
            "Marshawn",
            "Martin",
            "Marvin",
            "Mathleen",
            "Max",
            "Medea",
            "Mehmet",
            "Mercury",
            "Michael",
            "Mick",
            "Miles",
            "Miley",
            "Milton",
            "Moe",
            "Nathan",
            "Neil",
            "Nicholas",
            "Nina",
            "Norbert",
            "Obi-Wan",
            "Oleg",
            "Optimus",
            "Ori",
            "Orlando",
            "Owen",
            "Ozzy",
            "Pat",
            "Patrick",
            "Paul",
            "Pen\u00E9lope",
            "Percy",
            "Pernilla",
            "Phillip",
            "Pierre",
            "Piotr",
            "Pratima",
            "Prithvi",
            "Qbert",
            "Queequeg",
            "Ralph",
            "Ram",
            "Randy",
            "Rhett",
            "Richard",
            "Ringo",
            "Roald",
            "Robert",
            "Roddy",
            "Roger",
            "Rohit",
            "Roland",
            "Rosario",
            "Rudy",
            "Sammy",
            "Scarlett",
            "Seamus",
            "Sebastian",
            "Sheb",
            "Sherlock",
            "Sigmund",
            "Silas",
            "Skeletor",
            "Snake",
            "Stan",
            "Stanislas",
            "Starbuck",
            "Tarzan",
            "Terrence",
            "Tilbert",
            "Tim",
            "Tom",
            "Tommy",
            "Topper",
            "Travis",
            "Trina",
            "Trygve",
            "Ulna",
            "Ulysses",
            "Valerie",
            "Victoria",
            "Viggo",
            "Vince",
            "Vint",
            "Vlad",
            "Walter",
            "Whoopi",
            "Willard",
            "Woody",
            "Yimou",
            "Yuanfei",
            "Ziv",
        };

        #endregion

        #region LastNames

        internal static string[] LastNames = new string[]
        {
            "Affleck",
            "Alabama",
            "Alaska",
            "Amerigo",
            "Babbage",
            "Bacon",
            "Baggins",
            "Bailey",
            "Bakshi",
            "Ballmer",
            "Banner",
            "Berners-Lee",
            "Bieber",
            "Blanchette",
            "Bon Jovi",
            "Boole",
            "Brokaw",
            "Bronson",
            "Budweiser",
            "Buffett",
            "Burns",
            "Burton",
            "Byrd",
            "Caesar",
            "Calrissian",
            "Calvino",
            "Cerf",
            "Chang",
            "Cheddars",
            "Chimay",
            "Clapton",
            "Clinton",
            "Coltrane",
            "Crawford",
            "Cruise",
            "Cruz",
            "Curry",
            "da Vinci",
            "Dahl",
            "Danders",
            "Davis",
            "Diamond",
            "Dixit",
            "Dorito",
            "Durango",
            "Dynamite",
            "Eich",
            "Einstein",
            "Femur",
            "Fermi",
            "Ferrigno",
            "Fine",
            "Flav",
            "Fonzarelli",
            "Fowler",
            "Frakes",
            "Freud",
            "Gates",
            "Giraffe",
            "Goldberg",
            "Gorey",
            "Gosling",
            "Grabowski",
            "Greyskull",
            "Grimes",
            "Groves",
            "Gruber",
            "Guinness",
            "Guthrie",
            "Harvin",
            "Hazzard",
            "Headon",
            "Hilton",
            "Hopper",
            "Hoskins",
            "Howard",
            "Idaho",
            "Jagger",
            "Jenkins",
            "Jobs",
            "Johnson",
            "Jones",
            "Kelley",
            "Kennedy",
            "Kenobi",
            "Khan",
            "Kirk",
            "Knuth",
            "Koenig",
            "Lawrence",
            "Lee",
            "Lemming",
            "Lemur",
            "Lennon",
            "Liskov",
            "Lovelace",
            "Lucas",
            "Lundgren",
            "Lyon",
            "Markov",
            "Marmalados",
            "Marmoset",
            "Marsalis",
            "Martin",
            "Mathers",
            "McCarthy",
            "McCartney",
            "Michelangelo",
            "Miller",
            "Minksy",
            "Mix-A-Lot",
            "Mortensen",
            "Nevada",
            "Nichols",
            "Nixon",
            "Nustria",
            "O'Possum",
            "O'Toole",
            "Obama",
            "Odysseus",
            "Oppenheimer",
            "Osbourne",
            "Pabst",
            "Parker",
            "Patella",
            "Picard",
            "Pinkerton",
            "Pliskin",
            "Poncherello",
            "Pop",
            "Potter",
            "Presley",
            "Prime",
            "Proudfoot",
            "Puerco",
            "Ramirez",
            "Ramone",
            "Reenskaug",
            "Rodman",
            "Rollins",
            "Rose",
            "Rotten",
            "Rucker",
            "Sasquatch",
            "Satyanarayanan",
            "Schneier",
            "Shah",
            "Shannon",
            "Shatner",
            "Simone",
            "Simonon",
            "Simpson",
            "Skywalker",
            "Smith",
            "Solo",
            "Spock",
            "Stallman",
            "Stallone",
            "Stark",
            "Starr",
            "Steele",
            "Sterling",
            "Stewart",
            "Strangelove",
            "Strummer",
            "Szilard",
            "Takei",
            "Targaryen",
            "Teller",
            "Tennessee",
            "Texas",
            "Thunders",
            "Timberlake",
            "Torvalds",
            "Travis",
            "Trump",
            "Turing",
            "Vader",
            "Van Damme",
            "Vermeer",
            "Vicoda",
            "von Braun",
            "von Neumann",
            "Wang",
            "Weiss",
            "Wiener",
            "Williams",
            "Young",
            "Zhang",
        };

        #endregion

        #region LastNamePrexies

        internal static string[] LastNamePrefixes = new[]
        {
            "al ",
            "bin ",
            "Bon ",
            "D'",
            "De ",
            "De la ",
            "Di ",
            "L'",
            "Le",
            "Mac",
            "Mc",
            "O'",
            "van ",
            "Vander",
            "von ",
        };

        #endregion

        #region LastNameSuffixes

        internal static string[] LastNameSuffixes = new[]
        {
            "arello",
            "arito",
            "baum",
            "berg",
            "bottom",
            "ersons",
            "field",
            "garten",
            "head",
            "hoffer",
            "ito",
            "kins",
            "ly",
            "man",
            "mann",
            "ner",
            "s",
            "sen",
            "smith",
            "son",
            "sons",
            "stein",
            "ton",
            "worth",
        };

        #endregion

        #region Titles

        internal static string[] Titles = new[]
        {
            "Dr.",
            "Duke",
            "M.",
            "Ms.",
            "Mlle.",
            "Mme.",
            "Mr.",
            "Mrs.",
            "Prof.",
            "Rev.",
            "Sir",
            "Sgt.",
            "Pfc.",
            "Capt.",
        };

        #endregion

        #region Suffixes

        internal static string[] Suffixes = new[]
        {
            "D.D.S.",
            "II",
            "III",
            "IV",
            "Jr.",
            "Sr.",
            "Esq.",
            "PhD.",
            "Esq.",
        };

        #endregion

        #region

        internal static string[] CompanyNameFormats = new[]
        {
            "General {0}",
            "The {0} Company",
            "The {0} Corporation",
            "The {0} Group",
            "United {0}",
            "{0} & Co., Inc.",
            "{0} & Co., LLC",
            "{0} & Sons",
            "{0} Biotech",
            "{0} Bros.",
            "{0} Chemical",
            "{0} City",
            "{0} Communications",
            "{0} Corporation",
            "{0} Data",
            "{0} Dynamics",
            "{0} Energy",
            "{0} Engineering",
            "{0} Entertainment",
            "{0} Financial",
            "{0} Foods",
            "{0} Holdings",
            "{0} Inc.",
            "{0} Insurance",
            "{0} Intl.",
            "{0} Networks",
            "{0} Partners",
            "{0} Resorts",
            "{0} Sciences",
            "{0} Solutions",
            "{0} Technologies",
            "{0}, GmbH.",
            "{0}, LLC",
        };

        #endregion

        #region

        internal static string[] CompanyNameWordFormats = new[]
        {
            "Auto{0}",
            "Chem-{0}",
            "Con-{0}",
            "e{0}",
            "i{0}",
            "Net{0}",
            "Next{0}",
            "Omni{0}",
            "Sun{0}",
            "Total{0}",
            "Trans{0}",
            "Value-{0}",
            "You{0}",
            "{0}!",
            "{0}aroo",
            "{0}-o-rama",
            "{0}Co",
            "{0}com",
            "{0}era",
            "{0}ex",
            "{0}ify",
            "{0}igent",
            "{0}iprise",
            "{0}is",
            "{0}istics",
            "{0}ix",
            "{0}ola",
            "{0}oleum",
            "{0}opia",
            "{0}opic",
            "{0}Pets",
            "{0}r",
            "{0}Smart",
            "{0}soft",
            "{0}Star",
            "{0}ster",
            "{0}tastic",
            "{0}topia",
            "{0}tron",
            "{0}Tube",
            "{0}Zoom",
        };

        #endregion

        #region

        public static readonly string[] TopLevelDomains = new[]
        {
            ".com",
            ".biz",
            ".gov",
            ".io",
            ".it",
            ".ly",
            ".org",
            ".tv"
        };
        #endregion
    }

    internal static partial class Words
    {
        #region Common

        public static readonly string[] Common = new[]
        {
            "ability",
            "able",
            "about",
            "above",
            "accept",
            "according",
            "across",
            "act",
            "action",
            "activity",
            "actually",
            "add",
            "address",
            "administration",
            "adult",
            "affect",
            "after",
            "again",
            "against",
            "age",
            "agency",
            "agent",
            "ago",
            "agree",
            "ahead",
            "air",
            "all",
            "allow",
            "almost",
            "along",
            "already",
            "also",
            "although",
            "always",
            "amaze",
            "among",
            "amount",
            "analysis",
            "anger",
            "animal",
            "another",
            "answer",
            "any",
            "anyone",
            "anything",
            "appear",
            "apply",
            "approach",
            "area",
            "argue",
            "arm",
            "around",
            "arrive",
            "art",
            "article",
            "artist",
            "as",
            "ask",
            "asparagus",
            "attack",
            "attention",
            "author",
            "authority",
            "available",
            "avocado",
            "avoid",
            "away",
            "baby",
            "back",
            "bad",
            "bag",
            "ball",
            "bank",
            "bar",
            "base",
            "bean",
            "beast",
            "beat",
            "beautiful",
            "because",
            "become",
            "bed",
            "before",
            "before",
            "begin",
            "behavior",
            "behind",
            "believe",
            "benefit",
            "best",
            "better",
            "between",
            "beyond",
            "big",
            "bill",
            "billion",
            "bird",
            "bit",
            "black",
            "blood",
            "blue",
            "board",
            "body",
            "bond",
            "book",
            "both",
            "box",
            "boy",
            "break",
            "bring",
            "broccoli",
            "broth",
            "brother",
            "build",
            "building",
            "business",
            "butcher",
            "buy",
            "call",
            "camera",
            "campaign",
            "cancer",
            "candidate",
            "car",
            "card",
            "care",
            "care",
            "career",
            "carry",
            "case",
            "catch",
            "cause",
            "cell",
            "center",
            "central",
            "century",
            "certain",
            "certainly",
            "chair",
            "challenge",
            "chance",
            "change",
            "character",
            "charge",
            "check",
            "cheese",
            "chicken",
            "child",
            "choice",
            "choose",
            "church",
            "chutney",
            "city",
            "claim",
            "clam",
            "class",
            "clear",
            "clearly",
            "close",
            "coach",
            "coffee",
            "cold",
            "collection",
            "college",
            "color",
            "come",
            "commercial",
            "common",
            "community",
            "company",
            "compare",
            "computer",
            "concern",
            "concussion",
            "condition",
            "conference",
            "Congress",
            "consider",
            "contain",
            "continue",
            "contraption",
            "control",
            "cost",
            "could",
            "counter",
            "couple",
            "courage",
            "course",
            "court",
            "cousin",
            "cover",
            "crab",
            "create",
            "crime",
            "croissant",
            "cultural",
            "culture",
            "cup",
            "current",
            "cut",
            "dark",
            "data",
            "daughter",
            "day",
            "dead",
            "deal",
            "deal",
            "death",
            "decade",
            "decide",
            "decision",
            "deep",
            "defense",
            "degree",
            "Democrat",
            "democratic",
            "describe",
            "design",
            "design",
            "despite",
            "detail",
            "determine",
            "develop",
            "development",
            "die",
            "difference",
            "different",
            "difficult",
            "direction",
            "director",
            "discover",
            "discuss",
            "discussion",
            "disease",
            "doctor",
            "dog",
            "door",
            "down",
            "down",
            "draw",
            "dream",
            "drive",
            "drop",
            "drum",
            "duck",
            "during",
            "each",
            "each",
            "early",
            "early",
            "east",
            "easy",
            "eat",
            "economic",
            "economy",
            "edge",
            "education",
            "effect",
            "effort",
            "eight",
            "either",
            "election",
            "else",
            "employee",
            "end",
            "energy",
            "enjoy",
            "enough",
            "enter",
            "entire",
            "environment",
            "especially",
            "establish",
            "even",
            "evening",
            "event",
            "ever",
            "every",
            "everybody",
            "everyone",
            "everything",
            "evidence",
            "exactly",
            "example",
            "executive",
            "exist",
            "expect",
            "experience",
            "expert",
            "explain",
            "eye",
            "face",
            "face",
            "fact",
            "factor",
            "fail",
            "fall",
            "fallacy",
            "family",
            "far",
            "farm",
            "father",
            "fear",
            "federal",
            "feel",
            "feeling",
            "few",
            "field",
            "fight",
            "figure",
            "fill",
            "film",
            "final",
            "finally",
            "financial",
            "find",
            "fine",
            "finger",
            "finish",
            "fire",
            "firm",
            "first",
            "fish",
            "five",
            "floor",
            "fly",
            "focus",
            "follow",
            "food",
            "foot",
            "force",
            "foreign",
            "forget",
            "form",
            "form",
            "former",
            "forward",
            "four",
            "free",
            "friend",
            "full",
            "fund",
            "future",
            "game",
            "garden",
            "gas",
            "general",
            "generation",
            "girl",
            "give",
            "glass",
            "gluttony",
            "goal",
            "good",
            "government",
            "grammar",
            "great",
            "green",
            "ground",
            "group",
            "grow",
            "growth",
            "guess",
            "guitar",
            "gun",
            "guy",
            "hair",
            "half",
            "hammer",
            "hand",
            "hang",
            "happen",
            "happy",
            "hard",
            "hard",
            "harm",
            "head",
            "health",
            "hear",
            "heart",
            "heat",
            "heavy",
            "help",
            "her",
            "here",
            "herself",
            "high",
            "him",
            "himself",
            "history",
            "hit",
            "hold",
            "home",
            "hope",
            "horse",
            "hospital",
            "hot",
            "hotel",
            "hour",
            "house",
            "how",
            "however",
            "huge",
            "human",
            "hundred",
            "husband",
            "idea",
            "identify",
            "image",
            "imagine",
            "impact",
            "important",
            "improve",
            "in",
            "include",
            "including",
            "increase",
            "indeed",
            "indicate",
            "individual",
            "indivisible",
            "industry",
            "information",
            "inside",
            "instead",
            "institution",
            "interest",
            "international",
            "interview",
            "into",
            "investment",
            "involve",
            "issue",
            "item",
            "its",
            "itself",
            "jam",
            "jelly",
            "job",
            "join",
            "junk",
            "junket",
            "jury",
            "just",
            "justice",
            "kale",
            "keep",
            "kelp",
            "ketchup",
            "kid",
            "kill",
            "kind",
            "knowledge",
            "land",
            "language",
            "large",
            "last",
            "late",
            "later",
            "laugh",
            "law",
            "lawyer",
            "lay",
            "lead",
            "leader",
            "learn",
            "least",
            "leave",
            "left",
            "leg",
            "legal",
            "less",
            "less",
            "let",
            "letter",
            "lettuce",
            "level",
            "lie",
            "life",
            "light",
            "like",
            "like",
            "likely",
            "line",
            "list",
            "listen",
            "litter",
            "little",
            "live",
            "lizard",
            "lobster",
            "local",
            "long",
            "look",
            "loop",
            "lose",
            "loss",
            "lot",
            "love",
            "low",
            "lozenge",
            "luck",
            "lunch",
            "machine",
            "macho",
            "magazine",
            "main",
            "maintain",
            "major",
            "man",
            "manage",
            "management",
            "manager",
            "many",
            "market",
            "marriage",
            "material",
            "matter",
            "may",
            "maybe",
            "me",
            "mean",
            "measure",
            "media",
            "medical",
            "meet",
            "meeting",
            "member",
            "memory",
            "mention",
            "message",
            "method",
            "middle",
            "might",
            "military",
            "million",
            "mind",
            "minute",
            "miss",
            "mittens",
            "model",
            "modern",
            "moment",
            "money",
            "month",
            "more",
            "morning",
            "most",
            "mother",
            "move",
            "movement",
            "movie",
            "much",
            "muck",
            "muffin",
            "music",
            "must",
            "mustard",
            "myself",
            "name",
            "name",
            "nation",
            "national",
            "natural",
            "nature",
            "near",
            "nearly",
            "necessary",
            "need",
            "need",
            "network",
            "never",
            "new",
            "news",
            "next",
            "nice",
            "night",
            "no",
            "none",
            "nope",
            "north",
            "note",
            "note",
            "nothing",
            "notice",
            "now",
            "number",
            "occur",
            "off",
            "offend",
            "offer",
            "office",
            "officer",
            "official",
            "often",
            "oh",
            "oil",
            "ok",
            "old",
            "on",
            "once",
            "one",
            "only",
            "onto",
            "open",
            "open",
            "operation",
            "opportunity",
            "order",
            "organization",
            "other",
            "otter",
            "our",
            "out",
            "out",
            "outside",
            "over",
            "over",
            "own",
            "owner",
            "page",
            "pain",
            "paper",
            "parent",
            "parsley",
            "part",
            "particular",
            "party",
            "pass",
            "past",
            "paste",
            "pastime",
            "patient",
            "pattern",
            "paws",
            "pay",
            "peace",
            "people",
            "per",
            "perform",
            "performance",
            "perhaps",
            "period",
            "person",
            "personal",
            "phone",
            "physical",
            "pick",
            "pickle",
            "picture",
            "piece",
            "placard",
            "place",
            "plan",
            "planet",
            "plant",
            "play",
            "play",
            "player",
            "pluck",
            "PM",
            "point",
            "poker",
            "police",
            "policy",
            "polite",
            "politeness",
            "poor",
            "popular",
            "population",
            "pork",
            "position",
            "possible",
            "power",
            "practice",
            "pram",
            "prepare",
            "present",
            "president",
            "pressure",
            "pretty",
            "price",
            "pride",
            "private",
            "probably",
            "problem",
            "process",
            "produce",
            "product",
            "production",
            "professor",
            "program",
            "project",
            "property",
            "protect",
            "prove",
            "provide",
            "public",
            "publicity",
            "puffin",
            "pull",
            "purpose",
            "push",
            "put",
            "quahog",
            "quality",
            "question",
            "quickly",
            "quite",
            "racy",
            "radio",
            "radish",
            "raise",
            "range",
            "ratchet",
            "rate",
            "rather",
            "reach",
            "read",
            "ready",
            "real",
            "reality",
            "realize",
            "really",
            "reason",
            "receive",
            "recent",
            "recently",
            "recognize",
            "record",
            "red",
            "reduce",
            "reflect",
            "region",
            "relationship",
            "religious",
            "relish",
            "remain",
            "remember",
            "remove",
            "report",
            "represent",
            "Republican",
            "require",
            "research",
            "resource",
            "respond",
            "response",
            "rest",
            "result",
            "return",
            "reveal",
            "right",
            "rise",
            "risk",
            "road",
            "robot",
            "rock",
            "role",
            "room",
            "rule",
            "run",
            "salsa",
            "same",
            "sand",
            "save",
            "scallop",
            "scene",
            "school",
            "science",
            "scone",
            "sea",
            "season",
            "seat",
            "second",
            "section",
            "secular",
            "security",
            "see",
            "seek",
            "seem",
            "sell",
            "send",
            "sense",
            "series",
            "serious",
            "serve",
            "service",
            "set",
            "set",
            "seven",
            "several",
            "shake",
            "shampoo",
            "share",
            "shark",
            "sheep",
            "shoot",
            "short",
            "shortage",
            "should",
            "shoulder",
            "shovel",
            "show",
            "shrimp",
            "side",
            "sign",
            "significant",
            "similar",
            "simple",
            "simply",
            "since",
            "since",
            "sing",
            "single",
            "sister",
            "sit",
            "site",
            "situation",
            "six",
            "size",
            "skill",
            "skin",
            "small",
            "snowflake",
            "so",
            "soap",
            "social",
            "society",
            "soldier",
            "some",
            "somebody",
            "someone",
            "something",
            "sometimes",
            "son",
            "song",
            "soon",
            "sort",
            "sound",
            "source",
            "south",
            "space",
            "speak",
            "special",
            "specific",
            "spend",
            "sponge",
            "sport",
            "spring",
            "staff",
            "stage",
            "stand",
            "standard",
            "star",
            "start",
            "state",
            "state",
            "statement",
            "station",
            "stay",
            "steam",
            "step",
            "still",
            "stock",
            "stop",
            "store",
            "stork",
            "storm",
            "story",
            "strategy",
            "street",
            "strong",
            "structure",
            "student",
            "study",
            "study",
            "stuff",
            "style",
            "subject",
            "success",
            "such",
            "suction",
            "suddenly",
            "suggest",
            "summer",
            "supper",
            "support",
            "sure",
            "surf",
            "surface",
            "system",
            "table",
            "take",
            "talk",
            "tank",
            "task",
            "tax",
            "teach",
            "teacher",
            "team",
            "technology",
            "television",
            "tell",
            "ten",
            "tend",
            "tenderloin",
            "term",
            "test",
            "test",
            "than",
            "thank",
            "that",
            "them",
            "themselves",
            "then",
            "theory",
            "there",
            "these",
            "thing",
            "think",
            "third",
            "those",
            "though",
            "thought",
            "thousand",
            "three",
            "through",
            "throughout",
            "throw",
            "thus",
            "time",
            "today",
            "together",
            "tonight",
            "too",
            "top",
            "topiary",
            "total",
            "tough",
            "toward",
            "town",
            "trade",
            "traditional",
            "training",
            "trap",
            "treat",
            "treatment",
            "tree",
            "trial",
            "trip",
            "trouble",
            "true",
            "truth",
            "try",
            "turn",
            "TV",
            "two",
            "type",
            "under",
            "understand",
            "unit",
            "until",
            "up",
            "upon",
            "us",
            "use",
            "usually",
            "value",
            "vampire",
            "various",
            "very",
            "view",
            "violence",
            "visit",
            "voice",
            "waffle",
            "wait",
            "walk",
            "wall",
            "wallaby",
            "want",
            "war",
            "watch",
            "water",
            "way",
            "weapon",
            "wear",
            "week",
            "weight",
            "well",
            "well",
            "west",
            "whale",
            "whatever",
            "when",
            "when",
            "where",
            "whether",
            "which",
            "while",
            "white",
            "whole",
            "whom",
            "whose",
            "why",
            "wide",
            "wife",
            "win",
            "window",
            "within",
            "without",
            "wolf",
            "woman",
            "wonder",
            "word",
            "work",
            "worker",
            "world",
            "worry",
            "wrench",
            "write",
            "writer",
            "wrong",
            "yard",
            "yeah",
            "year",
            "yes",
            "yet",
            "yeti",
            "young",
            "your",
            "yourself",
            "zebra",
            "zombie",
        };
        #endregion
    }

    /// <remarks>
    /// Consider using this as the basis for a new Its.Enumerable
    /// </remarks>
    internal static class AnyEnumerableExtensions
    {
        private static readonly ThreadLocal<Random> s_random = new ThreadLocal<Random>(() => new Random());

        /// <summary>
        /// Return the given enumerable as a list if it already is one. If it is not, returns a new list containing all the elements in it.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The given enumerable as a list if it already is one. If it is not, returns a new list containing all the elements in it.</returns>
        public static IList<T> AsList<T>(this IEnumerable<T> source)
        {
            var sourceAsList = source as IList<T>;
            if (sourceAsList != null)
            {
                return sourceAsList;
            }

            return source.ToList();
        }

        public static void Shuffle<T>(this IList<T> source)
        {
            /*
             * From Wikipedia:
             * http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
             * To shuffle an array a of n elements (indices 0..n-1):
             *   for i from n − 1 downto 1 do
             *        j ← random integer with 0 ≤ j ≤ i
             *        exchange a[j] and a[i]
             */

            for (int i = source.Count - 1; i >= 1; i--)
            {
                int j = s_random.Value.Next(i + 1);

                T temp = source[i];
                source[i] = source[j];
                source[j] = temp;
            }
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            IEnumerable<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            return first.Zip(second).Zip(third, (firstAndSecondItem, thirdItem) => resultSelector(firstAndSecondItem.Item1, firstAndSecondItem.Item2, thirdItem));
        }

        public static IEnumerable<Tuple<TFirst, TSecond>> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        {
            return first.Zip(second, (firstElement, secondElement) => Tuple.Create(firstElement, secondElement));
        }
    }
}