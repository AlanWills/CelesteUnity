////////////////////////////////////////////////////////////////////////////////////////////////////
//
// CenterCLR.XorRandomGenerator - Random number generator by xor-shift calculation
// Copyright (c) Kouji Matsui, All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice,
//   this list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Celeste.Core.Interfaces;

namespace Celeste.Core
{
    /// <summary>
    /// xor-shift based random generator.
    /// </summary>
    /// <remarks>
    /// This class is random number generator by xor-shift calculation.
    /// http://en.wikipedia.org/wiki/Xorshift
    /// If require thread-safe, use SafeRandom class.
    /// </remarks>
    public sealed class XorRNG : System.Random, IRNG
    {
        private InternalXorRandom random_;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// Seed value is internal tick count.
        /// </remarks>
        public XorRNG()
        {
            random_ = new InternalXorRandom(Seeder.GetSeed());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="seed">Random seed value.</param>
        public XorRNG(int seed)
        {
            random_ = new InternalXorRandom(Seeder.Normalize(seed));
        }

        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <returns>32bit random value.</returns>
        public override int Next()
        {
            return random_.Next();
        }

        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>32bit random value.</returns>
        public override int Next(int maxValue)
        {
            return random_.Next(maxValue);
        }

        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>32bit random value.</returns>
        public override int Next(int minValue, int maxValue)
        {
            return random_.Next(minValue, maxValue);
        }

        /// <summary>
        /// Get random values.
        /// </summary>
        /// <param name="buffer">Random value fill target.</param>
        public void NextValues(int[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            random_.NextValues(buffer);
        }

        /// <summary>
        /// Get random values.
        /// </summary>
        /// <param name="buffer">Random value fill target.</param>
        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            random_.NextBytes(buffer);
        }

        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <returns>Floating point value.</returns>
        protected override double Sample()
        {
            return random_.Sample();
        }

        internal double InternalSample()
        {
            return random_.Sample();
        }

        /// <summary>
        /// Generate random sequence.
        /// </summary>
        /// <param name="count">Number of random values.</param>
        /// <returns>Random sequence.</returns>
        public static IEnumerable<int> Sequence(int count)
        {
            var r = new XorRNG();

            for (var index = 0; index < count; index++)
            {
                yield return r.Next();
            }
        }

        /// <summary>
        /// Generate random sequence.
        /// </summary>
        /// <param name="count">Number of random arrays.</param>
        /// <param name="bytes">Bytes on array.</param>
        /// <returns>Random sequence.</returns>
        public static IEnumerable<byte[]> BytesSequence(int count, int bytes)
        {
            var r = new XorRNG();

            for (var index = 0; index < count; index++)
            {
                var buffer = new byte[bytes];
                r.NextBytes(buffer);
                yield return buffer;
            }
        }

        /// <summary>
        /// Generate random sequence.
        /// </summary>
        /// <param name="count">Number of random arrays.</param>
        /// <param name="values">Values on array.</param>
        /// <returns>Random sequence.</returns>
        public static IEnumerable<int[]> ValuesSequence(int count, int values)
        {
            var r = new XorRNG();

            for (var index = 0; index < count; index++)
            {
                var buffer = new int[values];
                r.NextValues(buffer);
                yield return buffer;
            }
        }

        int IRNG.FromRangeInclusive(int inclusiveMin, int inclusiveMax)
        {
            return Next(inclusiveMin, inclusiveMax + 1);
        }
    }
}