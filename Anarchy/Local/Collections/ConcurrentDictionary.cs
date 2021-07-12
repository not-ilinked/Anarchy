using System;
using System.Collections.Generic;

namespace Anarchy
{
    public class ConcurrentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public object Lock { get; private set; } = new object();

        public new TValue this[TKey key]
        {
            get
            {
                lock (Lock)
                    return base[key];
            }
            set
            {
                lock (Lock)
                    base[key] = value;
            }
        }

        public TKey this[TValue value]
        {
            get
            {
                if (TryGetKey(value, out TKey key))
                    return key;
                else
                    throw new ArgumentException("No item with the specified value was found");
            }
        }

        public ConcurrentDictionary() : base()
        { }

        public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        { }

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

        public bool TryGetKey(TValue value, out TKey key)
        {
            lock (Lock)
            {
                foreach (var item in this)
                {
                    if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
                    {
                        key = item.Key;
                        return true;
                    }
                }
            }

            key = default;
            return false;
        }

        public new bool ContainsKey(TKey key)
        {
            lock (Lock)
                return base.ContainsKey(key);
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
