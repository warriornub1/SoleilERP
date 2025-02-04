using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Common
{
    public abstract class Utilities
    {
        public static bool ParseBool(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            switch (input.ToLower())
            {
                case "1":
                case "e":
                case "E":
                case "y":
                case "yes":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// parse string to bool
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool MapStatusToBool(string source)
        {
            return ParseBool(source);
        }

        public static string MapBoolToStatus(bool source, bool isStatus = false)
        {
            if (isStatus)
            {
                return source ? "E" : "D";
            }

            return source ? "1" : "0";
        }

        public static Task<Dictionary<int, List<string>>> GetDictionaryAsync<T>(
            IQueryable<T> query,
            Func<T, int> idSelector,
            Func<T, List<string>> valueSelector)
        {
            var result = query
                .Select(x => new
                {
                    Id = idSelector(x),
                    Value = valueSelector(x)
                }).ToDictionary(x => x.Id, y => y.Value);

            return Task.FromResult(result);
        }

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

        public static decimal ConvertFileLengthToMegabytes(long fileSize)
        {
            var value = (decimal)fileSize / (1024 * 1024);
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
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

        public static List<(string Name, object? Value)> GetPropertyNameAndValues<T>(T instance, HashSet<string> excludedProperties)
        {
            var propertyValues = new List<(string Name, object? Value)>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (excludedProperties.Contains(property.Name)) // Exclude by property name
                {
                    continue;
                }

                var value = property.GetValue(instance);
                propertyValues.Add((property.Name, value));
            }

            return propertyValues;
        }

        public static int? GetMaxLength(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                return null;
            }

            var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
            return stringLengthAttribute?.MaximumLength;
        }

        /// <summary>
        /// Checks if the file size of the provided file exceeds the maximum size in megabytes.
        /// </summary>
        /// <param name="file">The file to check the size of.</param>
        /// <param name="maxSizeInMb">The maximum allowable size in megabytes.</param>
        /// <returns>True if the file size is within the limit, false if it exceeds the maximum size.</returns>
        public static bool ValidateFileSize(IFormFile? file, int? maxSizeInMb)
        {
            if (file == null || maxSizeInMb == null)
                return false;

            // Calculate the file size in MB
            var fileSizeInMb = ConvertFileLengthToMegabytes(file.Length);

            return fileSizeInMb <= maxSizeInMb;
        }

        /// <summary>
        /// Validates the file extension based on the allowed extensions provided.
        /// </summary>
        /// <param name="file">The file to validate.</param>
        /// <param name="allowedExtensions">The allowed file extensions separated by commas.</param>
        /// <returns>True if the file extension is valid, false otherwise.</returns>
        public static bool ValidateFileExtension(IFormFile? file, string? allowedExtensions)
        {
            if (file == null) return false;

            // Check if allowed extensions are provided
            if (string.IsNullOrEmpty(allowedExtensions)) return true;

            var allowedImageExtensions = allowedExtensions
                .Split(',')
                .Select(ext => ext.Trim().ToLowerInvariant()) // Normalize extensions
                .ToArray();

            var fileExtension = Path.GetExtension(file.FileName)?.TrimStart('.').ToLowerInvariant();
            return allowedImageExtensions.Contains(fileExtension);
        }

        /// <summary>
        /// Validates a list of IFormFile objects based on file size and extension.
        /// </summary>
        /// <param name="files">The list of files to validate.</param>
        /// <param name="maxSizeInMb">The maximum allowable size in megabytes.</param>
        /// <param name="allowedExtensions">The allowed file extensions separated by commas.</param>
        /// <returns>A list of validation errors. If the list is empty, all files are valid.</returns>
        public static List<string> ValidateFiles(List<IFormFile> files, int? maxSizeInMb, string? allowedExtensions)
        {
            var errors = new List<string>();

            foreach (var file in files)
            {
                if (!ValidateFileSize(file, maxSizeInMb))
                {
                    errors.Add($"File '{file.FileName}' exceeds the maximum size of {maxSizeInMb} MB.");
                }

                if (!ValidateFileExtension(file, allowedExtensions))
                {
                    errors.Add($"File '{file.FileName}' has an invalid extension. Allowed extensions: {allowedExtensions}");
                }
            }

            return errors;
        }

        public static async Task<string> SaveFileUpload(IFormFile file)
        {
            var localFilePath = $"{ResourceFolder}/{Resources.POFile}/";

            // Create new file name
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmssfffff}";
            var fileFullName = newFileName + extension;

            await Utilities.SaveFileToTemporaryFolder(file, fileFullName, localFilePath);

            var urlPath = Path.Combine(Resources.POFile, fileFullName);
            return urlPath;
        }
    }
}
