namespace DriverActivityWeb.Helper
{
    public static class AppExtention
    {
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

    }
}
