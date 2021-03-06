// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Buffers
{
    using System;

    /// <summary>
    /// Abstract base class for <see cref="IByteBufferAllocator"/> instances
    /// </summary>
    public abstract class AbstractByteBufferAllocator : IByteBufferAllocator
    {
        const int DefaultInitialCapacity = 256;
        const int DefaultMaxComponents = 16;

        readonly IByteBuffer emptyBuffer;

        protected AbstractByteBufferAllocator()
        {
            this.emptyBuffer = new EmptyByteBuffer(this);
        }

        public IByteBuffer Buffer()
        {
            return this.Buffer(DefaultInitialCapacity, int.MaxValue);
        }

        public IByteBuffer Buffer(int initialCapacity)
        {
            return this.Buffer(initialCapacity, int.MaxValue);
        }

        public IByteBuffer Buffer(int initialCapacity, int maxCapacity)
        {
            if (initialCapacity == 0 && maxCapacity == 0)
            {
                return this.emptyBuffer;
            }

            Validate(initialCapacity, maxCapacity);

            return this.NewBuffer(initialCapacity, maxCapacity);
        }

        public CompositeByteBuffer CompositeBuffer()
        {
            return this.CompositeBuffer(DefaultMaxComponents);
        }

        public CompositeByteBuffer CompositeBuffer(int maxComponents)
        {
            return new CompositeByteBuffer(this, maxComponents);
        }

        protected abstract IByteBuffer NewBuffer(int initialCapacity, int maxCapacity);

        #region Range validation

        static void Validate(int initialCapacity, int maxCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException("initialCapacity", "initialCapacity must be greater than zero");
            }

            if (initialCapacity > maxCapacity)
            {
                throw new ArgumentOutOfRangeException("initialCapacity", string.Format("initialCapacity ({0}) must be greater than maxCapacity ({1})", initialCapacity, maxCapacity));
            }
        }

        #endregion
    }
}