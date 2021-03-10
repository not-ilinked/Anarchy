using System;
using System.Collections.Generic;

namespace Anarchy
{
    internal class ConcurrentList<T> : List<T>
    {
        public object Lock { get; private set; } = new object();

        public ConcurrentList() : base()
        { }

        public ConcurrentList(int capacity) : base(capacity)
        { }

        public ConcurrentList(IEnumerable<T> items) : base(items)
        { }

        public new T this[int i]
        {
            get
            {
                lock (Lock)
                    return base[i];
            }
            set
            {
                lock (Lock)
                    base[i] = value;
            }
        }

        public List<T> CreateCopy()
        {
            lock (Lock)
                return new List<T>(this);
        }

        public new void Add(T item)
        {
            lock (Lock)
                base.Add(item);
        }

        public new void Remove(T item)
        {
            lock (Lock)
                base.Remove(item);
        }

        public new void AddRange(IEnumerable<T> items)
        {
            lock (Lock)
                base.AddRange(items);
        }

        public bool ReplaceFirst(Predicate<T> match, T newItem)
        {
            lock (Lock)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (match.Invoke(base[i]))
                    {
                        base[i] = newItem;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool RemoveFirst(Predicate<T> match)
        {
            lock (Lock)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (match.Invoke(base[i]))
                    {
                        base.Remove(base[i]);

                        return true;
                    }
                }
            }

            return false;
        }

        public new int FindIndex(Predicate<T> match)
        {
            lock (Lock)
                return base.FindIndex(match);
        }

        public new void Clear()
        {
            lock (Lock)
                base.Clear();
        }
    }
}
