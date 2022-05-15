using System;
using System.Collections.Generic;

namespace JPLab2.Infrastructure
{

    public static class StringExtension
    {
        /// <summary>
        /// コレクションのメンバーを連結します。各メンバーの間には、指定した区切り記号が挿入されます。
        /// </summary>
        public static string ConcatenateString<T>(this IEnumerable<T> values, string sepalator) => String.Join(sepalator, values);
    }
}

