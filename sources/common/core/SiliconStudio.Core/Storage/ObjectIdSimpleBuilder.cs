// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System.Runtime.CompilerServices;

namespace SiliconStudio.Core.Storage
{
    /// <summary>
    /// An optimized version of <see cref="ObjectIdBuilder"/> to output a <see cref="ObjectId"/> expecting data to hash be 32bits integers only. See remarks.
    /// </summary>
    /// <remarks>
    /// This implementation is suited when it can be feeded with 32bits values. The resulting value must be identical to <see cref="ObjectIdBuilder"/> if
    /// the length size of the data is a multiple of 16 bytes.
    /// </remarks>
    public unsafe struct ObjectIdSimpleBuilder
    {
        private readonly uint seed;
        const uint C1 = 0x239b961b;
        const uint C2 = 0xab0e9789;
        const uint C3 = 0x38b34ae5;
        const uint C4 = 0xa1e38b93;

        private uint H1;
        private uint H2;
        private uint H3;
        private uint H4;
        private uint length;
        
        public ObjectIdSimpleBuilder(uint seed = 0)
        {
            this.seed = seed;

            // initialize hash values to seed values
            H1 = H2 = H3 = H4 = seed;
            length = 0;
        }

        public uint Seed { get { return seed; } }

        public uint Length
        {
            get
            {
                return length;
            }
        }

        public void Reset()
        {
            // initialize hash values to seed values
            H1 = H2 = H3 = H4 = Seed;
            length = 0;
        }

        /// <summary>
        /// Gets the current calculated hash.
        /// </summary>
        /// <value>The current hash.</value>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObjectId ComputeHash()
        {
            ObjectId result;
            ComputeHash(out result);
            return result;
        }

        /// <summary>
        /// Gets the current calculated hash.
        /// </summary>
        /// <value>The current hash.</value>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ComputeHash(out ObjectId result)
        {
            var h4 = H4;
            var h3 = H3;
            var h2 = H2;
            var h1 = H1;

            var fullLength = length << 2;

            // pipelining friendly algorithm
            h1 ^= fullLength; h2 ^= fullLength; h3 ^= fullLength; h4 ^= fullLength;

            h1 += (h2 + h3 + h4);
            h2 += h1; h3 += h1; h4 += h1;

            h1 = FMix(h1);
            h2 = FMix(h2);
            h3 = FMix(h3);
            h4 = FMix(h4);

            h1 += (h2 + h3 + h4);
            h2 += h1; h3 += h1; h4 += h1;

            fixed (void* ptr = &result)
            {
                var h = (uint*)ptr;
                *h++ = h1;
                *h++ = h2;
                *h++ = h3;
                *h = h4;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int data)
        {
            Write(unchecked((uint)data));
        }

        /// <summary>
        /// Writes the specified data to this builder. Size of data must be multiple of 4 bytes.
        /// </summary>
        /// <typeparam name="T">Struct type with a size multiple of 4 bytes</typeparam>
        /// <param name="data">The data to add to this builder</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T data) where T : struct
        {
            var pData = (int*)Interop.Fixed(ref data);
            int count = Utilities.SizeOf<T>() >> 2;
            for (int i = 0; i < count; i++)
            {
                Write(*pData++);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(uint data)
        {
            switch (length & 3)
            {
                case 0:
                    // K1 - consume first integer
                    H1 ^= RotateLeft((data * C1), 15) * C2;
                    H1 = (RotateLeft(H1, 19) + H2) * 5 + 0x561ccd1b;
                    break;

                case 1:
                    // K2 - consume second integer
                    H2 ^= RotateLeft((data * C2), 16) * C3;
                    H2 = (RotateLeft(H2, 17) + H3) * 5 + 0x0bcaa747;
                    break;

                case 2:
                    // K3 - consume third integer
                    H3 ^= RotateLeft((data * C3), 17) * C4;
                    H3 = (RotateLeft(H3, 15) + H4) * 5 + 0x96cd1c35;
                    break;

                case 3:
                    // K4 - consume fourth integer
                    H4 ^= RotateLeft((data * C4), 18) * C1;
                    H4 = (RotateLeft(H4, 13) + H1) * 5 + 0x32ac3b17;
                    break;
            }
            length++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RotateLeft(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FMix(uint h)
        {
            // pipelining friendly algorithm
            h = (h ^ (h >> 16)) * 0x85ebca6b;
            h = (h ^ (h >> 13)) * 0xc2b2ae35;
            return h ^ (h >> 16);
        }
    }
}