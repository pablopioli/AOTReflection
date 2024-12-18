using System;

namespace Apparatus.AOT.Reflection.Playground
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class TestAttribute : Attribute
    {
        public int Integer
        {
            get;
        }

        public float Float
        {
            get;
        }

        public string Text
        {
            get;
        }

        public string[]? TextArray
        {
            get;
        }

        public Type? Type
        {
            get;
        }

        public TestAttribute(
            int @int = default,
            float @float = default,
            string text = "",
            string[]? textArray = null,
            Type? type = null)
        {
            Integer = @int;
            Float = @float;
            Text = text;
            TextArray = textArray;
            Type = type;
        }

        public TestAttribute(string text = "")
        {
            Text = text;
        }
    }
}