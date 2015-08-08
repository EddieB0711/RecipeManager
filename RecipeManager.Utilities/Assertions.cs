namespace RecipeManager.Utilities
{
    using System;

    public static class Assertions
    {
        public static T CheckNull<T>(T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        public static string CheckNull(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }

        public static T? CheckNull<T>(T? value, string name) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }
    }
}