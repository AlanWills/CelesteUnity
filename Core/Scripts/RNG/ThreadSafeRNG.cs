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
using Celeste.Core.Interfaces;

namespace Celeste.Core
{
    /// <summary>
    /// xor-shift based random generator.
    /// </summary>
    /// <remarks>
    /// This class is thread-safed random number generator by xor-shift calculation.
    /// http://en.wikipedia.org/wiki/Xorshift
    /// If not require thread-safe, use Random class.
    /// </remarks>
    public sealed class ThreadSafeRNG : System.Random, IRNG
    {
        private InternalXorRandom random_;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// Seed value is internal tick count.
        /// </remarks>
        public ThreadSafeRNG()
        {
            random_ = new InternalXorRandom(Seeder.GetSeed());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="seed">Random seed value.</param>
        public ThreadSafeRNG(int seed)
        {
            random_ = new InternalXorRandom(Seeder.Normalize(seed));
        }

        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <returns>32bit random value.</returns>
        public override int Next()
        {
            lock (this)
            {
                return (int)(random_.Next() & 0x7fffffff);
            }
        }

        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>32bit random value.</returns>
        public override int Next(int maxValue)
        {
            lock (this)
            {
                return (int)random_.Next(maxValue);
            }
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

            lock (this)
            {
                random_.NextBytes(buffer);
            }
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

            lock (this)
            {
                random_.NextValues(buffer);
            }
        }

        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <returns>Floating point value.</returns>
        protected override double Sample()
        {
            lock (this)
            {
                return ((double)((int)random_.Next())) * 4.6566128752457969E-10;
            }
        }

        int IRNG.FromRangeInclusive(int inclusiveMin, int inclusiveMax)
        {
            return Next(inclusiveMin, inclusiveMax + 1);
        }
    }
}