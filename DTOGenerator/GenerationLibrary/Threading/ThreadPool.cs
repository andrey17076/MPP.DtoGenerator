using System;
using System.Collections.Generic;
using System.Threading;

namespace GenerationLibrary.Threading
{
    internal class ThreadPool : IDisposable
    {
        private readonly int _threadCount;
        private readonly Queue<WaitCallback> _queue = new Queue<WaitCallback>();
        private Thread[] _threads;
        private int _threadsWaitingCount;
        private bool _isTerminated;

        internal ThreadPool(int threadCount)
        {
            _threadCount = threadCount;
        }

        internal void QueueUserWorkItem(WaitCallback work)
        {
            EnsureStarted();

            lock (_queue)
            {
                _queue.Enqueue(work);
                if (_threadsWaitingCount > 0)
                    Monitor.Pulse(_queue);
            }
        }

        public void Dispose()
        {
            _isTerminated = true;
            lock (_queue)
            {
                Monitor.PulseAll(_queue);
            }
            foreach (var thread in _threads)
            {
                thread.Join();
            }
        }

        private void EnsureStarted()
        {
            if (_threads == null)
            {
                lock (_queue)
                {
                    if (_threads == null)
                    {
                        _threads = new Thread[_threadCount];
                        for (int i = 0; i < _threads.Length; i++)
                        {
                            _threads[i] = new Thread(DispatchLoop);
                            _threads[i].Start();
                        }
                    }
                }
            }
        }

        private void DispatchLoop()
        {
            while (true)
            {
                WaitCallback work;
                lock (_queue)
                {
                    if (_isTerminated)
                        return;

                    while (_queue.Count == 0)
                    {
                        _threadsWaitingCount++;
                        try
                        {
                            Monitor.Wait(_queue);
                        }
                        finally
                        {
                            _threadsWaitingCount--;
                        }
                        if (_isTerminated)
                            return;
                    }
                    work = _queue.Dequeue();
                }
                work.Invoke(null);
            }
        }
    }
}
