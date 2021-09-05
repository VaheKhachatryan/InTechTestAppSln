using System;
using System.Threading;

namespace BE.Common.ComponentModel
{
    /// <summary>
    /// Provides a convenience methodology for implementing locked access to resources. 
    /// </summary>
    /// <remarks>
    /// Intended as an infrastructure class.
    /// </remarks>
    public sealed class ReaderWriteLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;
        private readonly ReaderWriteLockType _readerWriteLockType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderWriteLockDisposable"/> class.
        /// </summary>
        /// <param name="rwLock">The readers–writer lock</param>
        /// <param name="readerWriteLockType">Lock type</param>
        public ReaderWriteLockDisposable(ReaderWriterLockSlim rwLock, ReaderWriteLockType readerWriteLockType = ReaderWriteLockType.Write)
        {
            this._rwLock = rwLock;
            this._readerWriteLockType = readerWriteLockType;

            switch (this._readerWriteLockType)
            {
                case ReaderWriteLockType.Read:
                    this._rwLock.EnterReadLock();
                    break;
                case ReaderWriteLockType.Write:
                    this._rwLock.EnterWriteLock();
                    break;
                case ReaderWriteLockType.UpgradeableRead:
                    this._rwLock.EnterUpgradeableReadLock();
                    break;
            }
        }

        public void Dispose()
        {
            switch (this._readerWriteLockType)
            {
                case ReaderWriteLockType.Read:
                    this._rwLock.ExitReadLock();
                    break;
                case ReaderWriteLockType.Write:
                    this._rwLock.ExitWriteLock();
                    break;
                case ReaderWriteLockType.UpgradeableRead:
                    this._rwLock.ExitUpgradeableReadLock();
                    break;
            }
        }
    }
}