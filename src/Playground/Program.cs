using System;

namespace Apparatus.AOT.Reflection.Playground
{
    public static class Program
    {
        public static void Main(string[] _)
        {
            var user = new User();
            var properties = user.GetProperties().Values;
            foreach (var property in properties)
            {
                Console.WriteLine(property.Name);
            }
        }
    }
}