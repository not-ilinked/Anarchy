using System;
using System.Collections.Generic;

namespace Anarchy
{
    public class ConcurrentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public object Lock { get; private set; } = new object();

        public new TValue this[TKey i]
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

        public TKey this[TValue i]
        {
            get
            {
                lock (Lock)
                {
                    foreach (var item in this)
                    {
                        if (EqualityComparer<TValue>.Default.Equals(item.Value, i))
                            return item.Key;
                    }
                }

                throw new ArgumentException("No item with the specified value was found");
            }
        }

        public Dictionary<TKey, TValue> CreateCopy()
        {
            lock (Lock)
                return new Dictionary<TKey, TValue>(this);
        }

        public new bool TryGetValue(TKey key, out TValue value)
        {
            lock (Lock)
                return base.TryGetValue(key, out value);
        }

        public new bool Remove(TKey key)
        {
            lock (Lock)
                return base.Remove(key);
        }

        public new void Clear()
        {
            lock (Lock)
                base.Clear();
        }
    }
}
