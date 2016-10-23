using System.Threading;

namespace GenerationLibrary.Threading
{
    internal class WorkItem
    {
        private readonly WaitCallback _work;
        private readonly object _obj;
        
        internal WorkItem(WaitCallback work, object obj)
        {
            _work = work;
            _obj = obj;
        }

        public void Invoke()
        {
            _work(_obj);
        }
    }
}
