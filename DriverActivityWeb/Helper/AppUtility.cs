namespace DriverActivityWeb.Helper
{
    public static class AppUtility
    {
        private const string EMPTY_MSG = "{0} can't be empty.";

        public static bool IsEmpty(string? val)
        {
            if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val))
                return true;
            else
                return false;
        }

        public static bool IsNotEmpty(string? val)
        {
            if (!string.IsNullOrEmpty(val) && !string.IsNullOrWhiteSpace(val))
                return true;
            else
                return false;
        }

        public static string RequiredMsg(string key)
        {
            return string.Format(EMPTY_MSG, key);
        }
    }
}
