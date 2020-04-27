﻿using System;
using System.Linq;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public static class IntExtensions
    {
        const int AlphabetLength = 26;
        static readonly char[] Alphabet = Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char) c).ToArray();

        /// <summary>
        /// Returns a-z for the first 26 (0-25); then za - zz for the next; zza - zzz, etc.
        /// </summary>
        /// <remarks>
        ///     Useful for merging Pdfs. Gotenberg merges them alphabetically by file name
        ///     https://thecodingmachine.github.io/gotenberg/#merge
        /// </remarks>
        /// <param name="sortNumber"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public static string ToAlphabeticallySortableName(this int sortNumber)
        {
            if(sortNumber < 0) throw new ArgumentOutOfRangeException(nameof(sortNumber));

            return $"{new string('Z', sortNumber / AlphabetLength)}{Alphabet[sortNumber % AlphabetLength]}";
        }
    }
}