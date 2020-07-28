﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetZipCompressor.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Compression.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Compression.Recipes
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression.Recipes.Internal;

    /// <summary>
    /// Build in dot net implementation of <see cref="ICompressAndDecompress"/>.
    /// Implementation from: <a href="https://stackoverflow.com/questions/40909052/using-gzip-to-compress-decompress-an-array-of-bytes" />.
    /// </summary>
#if !OBeautifulCodeCompressionRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Compression.Recipes", "See package version number")]
    internal
#else
    public
#endif
    class DotNetZipCompressor : ICompressor
    {
        /// <inheritdoc />
        public CompressionKind CompressionKind => CompressionKind.DotNetZip;

        /// <summary>
        /// Compresses the provided byte array.
        /// </summary>
        /// <param name="uncompressedBytes">Byte array to compress.</param>
        /// <returns>
        /// Compressed version of the supplied byte array.
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = ObcSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndNoGoodAlternative)]
        public static byte[] CompressBytes(
            byte[] uncompressedBytes)
        {
            new { uncompressedBytes }.AsArg().Must().NotBeNull();

            byte[] result;

            using (var compressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(uncompressedBytes, 0, uncompressedBytes.Length);

                    zipStream.Close();

                    result = compressedStream.ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Decompresses the provided byte array.
        /// </summary>
        /// <param name="compressedBytes">Byte array to decompress.</param>
        /// <returns>
        /// Decompressed version of the supplied byte array.
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = ObcSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndNoGoodAlternative)]
        public static byte[] DecompressBytes(
            byte[] compressedBytes)
        {
            new { compressedBytes }.AsArg().Must().NotBeNull();

            byte[] result;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);

                        result = resultStream.ToArray();
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        byte[] ICompress.CompressBytes(
            byte[] uncompressedBytes)
        {
            new { uncompressedBytes }.AsArg().Must().NotBeNull();

            var result = CompressBytes(uncompressedBytes);

            return result;
        }

        /// <inheritdoc />
        byte[] IDecompress.DecompressBytes(
            byte[] compressedBytes)
        {
            new { compressedBytes }.AsArg().Must().NotBeNull();

            var result = DecompressBytes(compressedBytes);

            return result;
        }
    }
}