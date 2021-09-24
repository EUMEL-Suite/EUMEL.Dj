using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Eumel.Dj.Ui.Services
{
    public class FixedSizedQueue<T> : IReadOnlyCollection<T>
    {
        private readonly object _lockObject = new();
        private readonly ConcurrentQueue<T> _q = new();

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
                while (_q.Count > Limit && _q.TryDequeue(out _))
                {
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _q.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _q.Count;
    }
}