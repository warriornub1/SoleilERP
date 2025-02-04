using Microsoft.AspNetCore.Http;

namespace SERP.Application.Common.Helper
{
    public class FileHelper
    {
        public static bool IsExcelFile(string contentType)
        {
            switch (contentType.ToLower())
            {
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return true;
                default:
                    return false;
            }
        }

        public static async Task<string> SaveFileToTemporaryFolder(IFormFile file, string fileName, string tempFolder)
        {
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            var tempFilePath = Path.Combine(tempFolder, fileName);
            await using var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(fileStream);

            return tempFilePath;
        }
    }
}
