using System;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ModalParameterAttribute : Attribute
    {
        public string Id { get; }
        public string Text { get; }
        public TextInputStyle Style { get; }
        public uint MinLength { get; }
        public uint MaxLength { get; }
        public string PlaceHolder { get; }
        public bool Required { get; }

        public ModalParameterAttribute(string name, string placeHolder = null, bool required = false, TextInputStyle style = TextInputStyle.Short, uint minLength = 0, uint maxLength = 4000)
        {
            Id = name;
            Text = name;
            PlaceHolder = placeHolder;
            Required = required;
            Style = style;
            MinLength = minLength;
            MaxLength = maxLength;           
        }
    }
}
