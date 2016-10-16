using System;
using System.Collections.Generic;
using System.Threading;

namespace DTOGenerationLibrary.ThreadPool
{
    internal class ThreadPool : IDisposable
    {
        private readonly int _threadCount;
        private readonly Queue<WorkItem> _queue = new Queue<WorkItem>();
        private Thread[] _threads;
        private int _threadsWaiting;
        private bool _isTerminated;

        internal ThreadPool(int threadCount)
        {
            _threadCount = threadCount;
        }

        internal void QueueUserWorkItem(WaitCallback work)
        {
            QueueUserWorkItem(work, null);
        }

        internal void QueueUserWorkItem(WaitCallback work, object obj)
        {
            var workItem = new WorkItem(work, obj);

            EnsureStarted();

            lock (_queue)
            {
                _queue.Enqueue(workItem);
                if (_threadsWaiting > 0)
                    Monitor.Pulse(_queue);
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
                WorkItem workItem;
                lock (_queue)
                {
                    if (_isTerminated)
                        return;

                    while (_queue.Count == 0)
                    {
                        _threadsWaiting++;
                        try
                        {
                            Monitor.Wait(_queue);
                        }
                        finally
                        {

                            _threadsWaiting--;
                        }
                        if (_isTerminated)
                            return;
                    }
                    workItem = _queue.Dequeue();
                }
                workItem.Invoke();
            }
        }

        public void Dispose()
        {
            _isTerminated = true;
            lock (_queue)
            {
                Monitor.PulseAll(_queue);
            }
            foreach (Thread t in _threads)
            {
                t.Join();
            }
        }
    }
}
