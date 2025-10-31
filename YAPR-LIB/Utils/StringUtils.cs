namespace System
{
    internal static class StringUtils
    {
        internal static string MakeUnixString(string str)
        {
            return str.ReplaceLineEndings("\n");
        }

        internal static string UnixReplace(this string str, string src, string dst)
        {
            return str.Replace(
                MakeUnixString(src),
                MakeUnixString(dst)
            );
        }

        internal static string UnixAppend(this string str, string text)
        {
            return $"{str}{MakeUnixString(text)}";
        }

        internal static string UnixPrepend(this string str, string text)
        {
            return $"{MakeUnixString(text)}{str}";
        }
    }
}
