namespace Discord
{
    /// <typeparam name="T">Type of property</typeparam>
    internal class Property<T>
    {
        public bool Set { get; private set; }

        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;

                Set = true;
            }
        }


        public static implicit operator T(Property<T> instance)
        {
            return instance.Value;
        }


        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
