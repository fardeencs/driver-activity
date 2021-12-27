namespace DriverActivityWeb.Helper
{
    public static class AppUtility
    {
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
    }
}
