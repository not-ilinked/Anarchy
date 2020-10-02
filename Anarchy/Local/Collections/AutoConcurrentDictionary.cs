using System;

namespace Anarchy
{
    internal class AutoConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _valueCreate;

        public AutoConcurrentDictionary(Func<TKey, TValue> valueCreate)
        {
            _valueCreate = valueCreate;
        }

        public new TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                    return value;
                else
                    return this[key] = _valueCreate(key);
            }
            set { base[key] = value; }
        }

        public TValue this[TKey key, bool doNotCreate]
        {
            get
            {
                if (doNotCreate)
                    return base[key];
                else
                    return this[key];
            }
        }
    }
}
