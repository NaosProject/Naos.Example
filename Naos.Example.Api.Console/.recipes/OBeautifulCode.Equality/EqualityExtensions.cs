﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualityExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Equality.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Equality.Recipes
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Extension methods that test for equality between two objects.
    /// </summary>
#if !OBeautifulCodeEqualityRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Equality.Recipes", "See package version number")]
    internal
#else
    public
#endif
    static class EqualityExtensions
    {
        /// <summary>
        /// Compares objects for equality.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="item1">The first object to compare.</param>
        /// <param name="item2">The second object to compare.</param>
        /// <param name="comparer">Optional equality comparer to use to compare the objects.  Default is to call <see cref="EqualityComparerHelper.GetEqualityComparerToUse{T}(IEqualityComparer{T})"/>.</param>
        /// <returns>
        /// - true if the two objects are equal
        /// - otherwise, false.
        /// </returns>
        public static bool IsEqualTo<T>(
            this T item1,
            T item2,
            IEqualityComparer<T> comparer = null)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var equalityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(comparer);

            var result = equalityComparerToUse.Equals(item1, item2);

            return result;
        }

        /// <summary>
        /// Compares two dictionaries for equality.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionaries.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionaries.</typeparam>
        /// <param name="item1">The first <see cref="IReadOnlyDictionary{TKey, TValue}"/> to compare.</param>
        /// <param name="item2">The second <see cref="IReadOnlyDictionary{TKey, TValue}"/> to compare.</param>
        /// <param name="valueComparer">Optional equality comparer to use to compare values.  Default is to call <see cref="EqualityComparerHelper.GetEqualityComparerToUse{T}(IEqualityComparer{T})"/>.</param>
        /// <returns>
        /// - true if the two source dictionaries are null.
        /// - false if one or the other is null.
        /// - false if the dictionaries are of different length.
        /// - true if the two dictionaries are of equal length and their values are equal for the same keys.
        /// - otherwise, false.
        /// </returns>
        public static bool IsDictionaryEqualTo<TKey, TValue>(
            this IDictionary<TKey, TValue> item1,
            IDictionary<TKey, TValue> item2,
            IEqualityComparer<TValue> valueComparer = null)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            if (item1.Keys.Count != item2.Keys.Count)
            {
                return false;
            }

            IEqualityComparer<TValue> valueEqualityComparerToUse = null;

            foreach (var key in item1.Keys)
            {
                // We rely on the IEqualityComparer<T> contained within the dictionaries to compare keys.
                // As such, two dictionaries that use a default equality comparer that just compares
                // object references, will never be equal.  For example, two dictionaries that are keyed on
                // a List<string> will never be equal, regardless of whether
                // item1.Keys.IsUnorderedEqualTo(item2.Keys) is true or false unless the keys are the same
                // object reference.
                // We took this approach because we wanted to respect the dictionary's contract for comparing
                // keys.  A dictionary that is keyed on a type that can only be compared by object reference
                // is rare and it's own smell.  In the example above, two dictionaries may well "look" like
                // they are equal, but if their List<string> keys are different references, then the dictionaries
                // cannot be substituted.  Looking-up values using the keys of one dictionary with the other
                // dictionary would fail.
                //
                // Note that two dictionaries keyed on DateTime, having the keys with the same number of Ticks
                // but with different DateTime.Kind, will be considered equal because here we are using .NET's
                // default equality comparer and not our own.  There is no way to use our own while solving for
                // the comments above (the reason why we use the comparer embedded in the dictionary in the first place).
                if (!item2.ContainsKey(key))
                {
                    return false;
                }

                var item1Value = item1[key];
                var item2Value = item2[key];

                if (valueEqualityComparerToUse == null)
                {
                    valueEqualityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(valueComparer);
                }

                if (!valueEqualityComparerToUse.Equals(item1Value, item2Value))
                {
                    return false;
                }
            }

            // As mentioned above, we rely on the IEqualityComparer<T> contained within the dictionaries
            // to compare keys.  As such, we need to check that every item1 key is contained within item2
            // AND vice-versa and hence the need for the following second loop.  To illustrate the need,
            // take two dictionaries that are keyed on string where item1 was constructed using
            // StringComparer.InvariantCulture and item1 was constructed using
            // StringComparer.InvariantCultureIgnoreCase.  If every item1 key is upper-case and every item2
            // key is lower-case, but otherwise the keys match, then the loop above would determine that
            // the dictionaries are equal whereas the loop below would return false and as such these
            // dictionaries are not substitutable as inputs to some consuming body of code.
            foreach (var key in item2.Keys)
            {
                if (!item1.ContainsKey(key))
                {
                    return false;
                }

                var item1Value = item1[key];
                var item2Value = item2[key];

                if (valueEqualityComparerToUse == null)
                {
                    valueEqualityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(valueComparer);
                }

                if (!valueEqualityComparerToUse.Equals(item1Value, item2Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two dictionaries for equality.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionaries.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionaries.</typeparam>
        /// <param name="item1">The first <see cref="IReadOnlyDictionary{TKey, TValue}"/> to compare.</param>
        /// <param name="item2">The second <see cref="IReadOnlyDictionary{TKey, TValue}"/> to compare.</param>
        /// <param name="valueComparer">Optional equality comparer to use to compare values.  Default is to call <see cref="EqualityComparerHelper.GetEqualityComparerToUse{T}(IEqualityComparer{T})"/>.</param>
        /// <returns>
        /// - true if the two source dictionaries are null.
        /// - false if one or the other is null.
        /// - false if the dictionaries are of different length.
        /// - true if the two dictionaries are of equal length and their values are equal for the same keys.
        /// - otherwise, false.
        /// </returns>
        public static bool IsReadOnlyDictionaryEqualTo<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> item1,
            IReadOnlyDictionary<TKey, TValue> item2,
            IEqualityComparer<TValue> valueComparer = null)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            if (item1.Keys.Count() != item2.Keys.Count())
            {
                return false;
            }

            IEqualityComparer<TValue> valueEqualityComparerToUse = null;

            foreach (var key in item1.Keys)
            {
                // We rely on the IEqualityComparer<T> contained within the dictionaries to compare keys.
                // As such, two dictionaries that use a default equality comparer that just compares
                // object references, will never be equal.  For example, two dictionaries that are keyed on
                // a List<string> will never be equal, regardless of whether
                // item1.Keys.IsUnorderedEqualTo(item2.Keys) is true or false unless the keys are the same
                // object reference.
                // We took this approach because we wanted to respect the dictionary's contract for comparing
                // keys.  A dictionary that is keyed on a type that can only be compared by object reference
                // is rare and it's own smell.  In the example above, two dictionaries may well "look" like
                // they are equal, but if their List<string> keys are different references, then the dictionaries
                // cannot be substituted.  Looking-up values using the keys of one dictionary with the other
                // dictionary would fail.
                //
                // Note that two dictionaries keyed on DateTime, having the keys with the same number of Ticks
                // but with different DateTime.Kind, will be considered equal because here we are using .NET's
                // default equality comparer and not our own.  There is no way to use our own while solving for
                // the comments above (the reason why we use the comparer embedded in the dictionary in the first place).
                if (!item2.ContainsKey(key))
                {
                    return false;
                }

                var item1Value = item1[key];
                var item2Value = item2[key];

                if (valueEqualityComparerToUse == null)
                {
                    valueEqualityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(valueComparer);
                }

                if (!valueEqualityComparerToUse.Equals(item1Value, item2Value))
                {
                    return false;
                }
            }

            // As mentioned above, we rely on the IEqualityComparer<T> contained within the dictionaries
            // to compare keys.  As such, we need to check that every item1 key is contained within item2
            // AND vice-versa and hence the need for the following second loop.  To illustrate the need,
            // take two dictionaries that are keyed on string where item1 was constructed using
            // StringComparer.InvariantCulture and item1 was constructed using
            // StringComparer.InvariantCultureIgnoreCase.  If every item1 key is upper-case and every item2
            // key is lower-case, but otherwise the keys match, then the loop above would determine that
            // the dictionaries are equal whereas the loop below would return false and as such these
            // dictionaries are not substitutable as inputs to some consuming body of code.
            foreach (var key in item2.Keys)
            {
                if (!item1.ContainsKey(key))
                {
                    return false;
                }

                var item1Value = item1[key];
                var item2Value = item2[key];

                if (valueEqualityComparerToUse == null)
                {
                    valueEqualityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(valueComparer);
                }

                if (!valueEqualityComparerToUse.Equals(item1Value, item2Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two dictionaries for equality.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of the input sequences.</typeparam>
        /// <param name="item1">An <see cref="IEnumerable{T}"/> to compare to <paramref name="item2"/>.</param>
        /// <param name="item2">An <see cref="IEnumerable{T}"/> to compare to the first sequence.</param>
        /// <param name="elementComparer">Optional equality comparer to use to compare the elements.  Default is to call <see cref="EqualityComparerHelper.GetEqualityComparerToUse{T}(IEqualityComparer{T})"/>.</param>
        /// <returns>
        /// - true if the two source sequences are null.
        /// - false if one or the other is null.
        /// - true if the two sequences are of equal length and their corresponding elements are equal according to <paramref name="elementComparer"/>.
        /// - otherwise, false.
        /// </returns>
        public static bool IsSequenceEqualTo<TElement>(
            this IEnumerable<TElement> item1,
            IEnumerable<TElement> item2,
            IEqualityComparer<TElement> elementComparer = null)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var equalityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(elementComparer);

            var result = item1.SequenceEqual(item2, equalityComparerToUse);

            return result;
        }

        /// <summary>
        /// Determines if two enumerables have the exact same elements in any order.
        /// Every unique element in the first set has to appear in the second set the same number of times it appears in the first.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of the input sequences.</typeparam>
        /// <param name="item1">An <see cref="IEnumerable{T}"/> to compare to <paramref name="item2"/>.</param>
        /// <param name="item2">An <see cref="IEnumerable{T}"/> to compare to the first sequence.</param>
        /// <param name="elementComparer">Optional equality comparer to use to compare the elements.  Default is to call <see cref="EqualityComparerHelper.GetEqualityComparerToUse{T}(IEqualityComparer{T})"/>.</param>
        /// <returns>
        /// - true if the two source sequences are null.
        /// - false if one or the other is null.
        /// - false if there is any symmetric difference.
        /// - true if the two sequences both contain the same number of elements for each unique element.
        /// - otherwise, false.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "This is not excessively complex.")]
        public static bool IsUnorderedEqualTo<TElement>(
            this IEnumerable<TElement> item1,
            IEnumerable<TElement> item2,
            IEqualityComparer<TElement> elementComparer = null)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var equalityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(elementComparer);

            // ReSharper disable once PossibleMultipleEnumeration
            var item1AsList = item1.ToList();

            // ReSharper disable once PossibleMultipleEnumeration
            var item2AsList = item2.ToList();

            if (item1AsList.Count != item2AsList.Count)
            {
                return false;
            }

            foreach (var item1Element in item1AsList)
            {
                var elementFound = false;

                for (var x = 0; x < item2AsList.Count; x++)
                {
                    var item2Element = item2AsList[x];

                    if (equalityComparerToUse.Equals(item1Element, item2Element))
                    {
                        item2AsList.RemoveAt(x);

                        elementFound = true;

                        break;
                    }
                }

                if (!elementFound)
                {
                    return false;
                }
            }

            return true;
        }
    }
}