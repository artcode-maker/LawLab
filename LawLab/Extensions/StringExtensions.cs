using System.Linq;

namespace LawLab.Extensions
{
    public static class StringExtensions
    {
        public static string GetStringWithoutZero(this string str)
        {
            char[] strArray = str.Select(x => x).ToArray();
            string result = string.Empty;
            foreach (char ch in strArray)
            {
                if (char.IsDigit(ch))
                {
                    result += ' ';
                }
                else
                {
                    result += ch;
                }
            }
            return result;
        }
    }
}
