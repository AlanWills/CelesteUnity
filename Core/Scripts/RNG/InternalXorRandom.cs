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

namespace Celeste.Core
{
    internal struct InternalXorRandom
    {
        private const uint sv_ = 1812433253U;
        private const double uintCount_ = ((double)uint.MaxValue) + 1;

        private uint seed0_;
        private uint seed1_;
        private uint seed2_;
        private uint seed3_;

        public InternalXorRandom(uint seed)
        {
            seed0_ = sv_ * (seed ^ (seed >> 30)) + 1U;
            seed1_ = sv_ * (seed0_ ^ (seed0_ >> 30)) + 1U;
            seed2_ = sv_ * (seed1_ ^ (seed1_ >> 30)) + 1U;
            seed3_ = sv_ * (seed2_ ^ (seed2_ >> 30)) + 1U;
        }

        public uint NextRawValue()
        {
            var t = seed0_ ^ (seed0_ << 11);

            seed0_ = seed1_;
            seed1_ = seed2_;
            seed2_ = seed3_;
            seed3_ = (seed3_ ^ (seed3_ >> 19)) ^ (t ^ (t >> 8));

            return seed3_;
        }

        public int Next()
        {
            var t = seed0_ ^ (seed0_ << 11);

            seed0_ = seed1_;
            seed1_ = seed2_;
            seed2_ = seed3_;
            seed3_ = (seed3_ ^ (seed3_ >> 19)) ^ (t ^ (t >> 8));

            return (int)(seed3_ & 0x7fffffff);
        }

        public double Sample()
        {
            var t = seed0_ ^ (seed0_ << 11);

            seed0_ = seed1_;
            seed1_ = seed2_;
            seed2_ = seed3_;
            seed3_ = (seed3_ ^ (seed3_ >> 19)) ^ (t ^ (t >> 8));

            return seed3_ / uintCount_;
        }

        public int Next(int maxValue)
        {
            var dvalue = (double)this.NextRawValue();
            var m = dvalue * (maxValue + 1) / uintCount_;
            return (int)m;
        }

        public int Next(int minValue, int maxValue)
        {
            var dvalue = (double)this.NextRawValue();
            var m = dvalue * (maxValue - minValue + 1) / uintCount_;
            return ((int)m) + minValue;
        }

        public void NextValues(int[] buffer)
        {
            for (var index = 0; index < buffer.Length; index++)
            {
                buffer[index] = this.Next();
            }
        }

        public void NextBytes(byte[] buffer)
        {
            var index = 0;
            var ceil = (buffer.Length / 4) * 4;
            while (index < ceil)
            {
                var value32 = this.NextRawValue();

                buffer[index++] = (byte)value32;
                buffer[index++] = (byte)(value32 >> 8);
                buffer[index++] = (byte)(value32 >> 16);
                buffer[index++] = (byte)(value32 >> 24);
            }

            if (index < buffer.Length)
            {
                var value32 = this.NextRawValue();
                buffer[index++] = (byte)value32;

                if (index < buffer.Length)
                {
                    buffer[index++] = (byte)(value32 >> 8);

                    if (index < buffer.Length)
                    {
                        buffer[index] = (byte)(value32 >> 16);
                    }
                }
            }
        }
    }
}