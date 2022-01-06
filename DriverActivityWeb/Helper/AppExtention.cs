using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Helper
{
    public static class AppExtention
    {
        private const string EMPTY_MSG = "{0} can't be empty.";

        public static byte[] ConvertToByteArray(this IFormFile file)
        {
            byte[]? imageBytes = null;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string imageStr = Convert.ToBase64String(fileBytes);
                imageBytes = Convert.FromBase64String(imageStr);
            }
            return imageBytes;
        }


        public static byte[]? ConvertToByteArray(this string? stringFile)
        {
            if (AppUtility.IsEmpty(stringFile))
                return null;

            string[] splitSignUrl = stringFile.Split(';');
            string signStr = splitSignUrl[1].Replace("base64,", "");

            return Convert.FromBase64String(signStr);
        }

        public static bool IsEmpty(this string val)
        {
            if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val))
                return true;
            else
                return false;
        }

        public static bool IsNotEmpty(this string val)
        {
            if (!string.IsNullOrEmpty(val) && !string.IsNullOrWhiteSpace(val))
                return true;
            else
                return false;
        }

        public static string RequiredMsg(this string key)
        {
            return string.Format(EMPTY_MSG, key);
        }

    }
}
