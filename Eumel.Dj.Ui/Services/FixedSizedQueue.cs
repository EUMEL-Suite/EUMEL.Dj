using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Eumel.Dj.Ui.Services
{
    public class FixedSizedQueue<T>
    {
        private readonly ConcurrentQueue<T> _q = new();
        private readonly object _lockObject = new();

        public FixedSizedQueue(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; }
        public void Enqueue(T obj)
        {
            _q.Enqueue(obj);
            lock (_lockObject)
            {
                while (_q.Count > Limit && _q.TryDequeue(out _)) { }
            }
        }

        public IEnumerable<T> ToArray()
        {
            return _q.ToArray();
        }
    }
}