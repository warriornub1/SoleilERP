using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Common.Helper;
using SERP.Application.Transactions.FilesTracking.DTOs;
using SERP.Application.Transactions.FilesTracking.Interfaces;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Domain.Transactions.FilesTracking;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using ImageMagick;
using SERP.Domain.Masters.Items;
using static SERP.Application.Common.Constants.ApplicationConstant;

namespace SERP.Application.Transactions.FilesTracking.Services
{
    internal class FilesTrackingService : IFilesTrackingService
    {
        private readonly string FILE_SERVER_NAME;
        private readonly IFileTrackingRepository _fileTrackingRepository;
        private readonly IConfiguration _configuration;

        public FilesTrackingService(
            IFileTrackingRepository fileTrackingRepository,
            IConfiguration configuration)
        {
            _fileTrackingRepository = fileTrackingRepository;
            _configuration = configuration;
            FILE_SERVER_NAME = _configuration["FileDirectoryPath"];
        }
        #region upload to File Server
        public async Task<int> ProcessingAndUploadImageFileAsync(string userId, string localPath, IUnitOfWork unitOfWork, IFormFile imageFile)
        {
            // Initialize variables
            var currentTime = DateTime.UtcNow;

            // Build file info before process
            var fileUploadTrackingId = Guid.NewGuid();

            // Create new file name
            string newFileName = $"{fileUploadTrackingId}_{currentTime:yyyyMMddTHHmmssfffff}";
            string fileFullName = newFileName + Path.GetExtension(imageFile.FileName);
            string thumbnailFileFullName = "thumbnail_" + newFileName + Path.GetExtension(imageFile.FileName);

            // Create upload path
            string s3UploadPath = localPath + $"/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";
            string localFilePath =
                $"App_Data/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/{fileUploadTrackingId}";

            try
            {
                // Save file to temporary folder
                await FileHelper.SaveFileToTemporaryFolder(imageFile, fileFullName, localFilePath);

                var inputFilePath = Path.Combine(localFilePath, fileFullName);
                var resizedImagePath = "resized_" + fileFullName;
                var resizedImageOutputPath = Path.Combine(FILE_SERVER_NAME, resizedImagePath);
                var thumbnailImagePath = "thumbnail_" + fileFullName;
                var thumbnailImageOutputPath = Path.Combine(FILE_SERVER_NAME, thumbnailImagePath);

                // Resize image and save to File Server
                var resizeImageResult = await ResizeImageAsync(imageFile.ContentType, inputFilePath, resizedImageOutputPath);
                var thumbnailImageResult = await ResizeThumbnailImageAsync(imageFile.ContentType, inputFilePath, thumbnailImageOutputPath);


                var fileUploadTracking = new FileTracking
                {
                    file_name = imageFile.FileName,
                    file_type = imageFile.ContentType,
                    url_path = resizedImagePath,
                    url_path_thumbnail = thumbnailImagePath,
                    upload_source = "todelete",
                    //height = resizeImageResult.Height,
                    //width = resizeImageResult.Width,
                    file_size = Utilities.ConvertFileLengthToMegabytes(imageFile.Length),
                    created_by = userId,
                    created_on = DateTime.Now
                };

                await _fileTrackingRepository.CreateAsync(fileUploadTracking);
                await unitOfWork.SaveChangesAsync();

                return fileUploadTracking.id;

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Directory.Delete(localFilePath, true);
            }
        }
        public async Task RemoveFileFromFileServerAsync(IUnitOfWork unitOfWork, List<int> fileIds)
        {
            var fileTracking = await _fileTrackingRepository.GetDictionaryAsync(x => fileIds.Contains(x.id));

            if (fileTracking is null)
            {
                throw new NotFoundException(ErrorCodes.FileTrackingNotFound, ErrorMessages.FileTrackingNotFound);
            }

            var filePath = fileTracking.Values.Select(x => x.url_path).ToList();
            var thumbnailPath = fileTracking.Values.Where(x => !string.IsNullOrEmpty(x.url_path_thumbnail)).Select(x => x.url_path_thumbnail).ToList();
            await _fileTrackingRepository.DeleteRangeAsync(fileTracking.Values);
            await unitOfWork.SaveChangesAsync();

            foreach (var path in filePath)
            {
                var deleteFile = Path.Combine(FILE_SERVER_NAME, path);
                if (!File.Exists(deleteFile))
                    continue;

                File.Delete(deleteFile);
            }
            foreach (var path in thumbnailPath)
            {
                var deleteFile = Path.Combine(FILE_SERVER_NAME, path);
                if (!File.Exists(deleteFile))
                    continue;

                File.Delete(deleteFile);
            }
        }


        #endregion

        #region TO REMOVE
        public async Task<List<FileTracking>> UploadMultipleFilesAsync(string userId, IUnitOfWork unitOfWork, List<FileTrackingRequestDto> requests)
        {
            var fileTrackingToInsert = new List<FileTracking>();

            foreach (var item in requests)
            {
                var fileTracking = new FileTracking
                {
                    created_by = userId,
                    file_type = item.file.ContentType,
                    file_name = item.file.FileName,
                    upload_source = item.upload_source,
                    document_type = item.document_type,
                    url_path = item.url_path,
                    file_size = Utilities.ConvertFileLengthToMegabytes(item.file.Length),
                };

                fileTrackingToInsert.Add(fileTracking);
            }

            if (fileTrackingToInsert.Count == 0)
            {
                return [];
            }
            await _fileTrackingRepository.CreateRangeAsync(fileTrackingToInsert);
            await unitOfWork.SaveChangesAsync();

            return fileTrackingToInsert;
        }
        public async Task<FileTracking> UploadSingleFileAsync(string userId, IUnitOfWork unitOfWork, FileInfoRequestDto request, string uploadSource, string documentType)
        {
            var fileTracking = new FileTracking
            {
                created_by = userId,
                file_type = request.file.ContentType,
                file_name = request.file.FileName,
                upload_source = uploadSource,
                document_type = documentType,
                url_path = request.url_path,
                file_size = Utilities.ConvertFileLengthToMegabytes(request.file.Length),
            };

            await _fileTrackingRepository.CreateAsync(fileTracking);
            await unitOfWork.SaveChangesAsync();

            return fileTracking;
        }
        public async Task<List<string>> RemoveFileAsync(IUnitOfWork unitOfWork, List<int> fileIds)
        {
            var fileTracking = await _fileTrackingRepository.GetDictionaryAsync(x => fileIds.Contains(x.id));

            if (fileTracking is null)
            {
                throw new NotFoundException(ErrorCodes.FileTrackingNotFound, ErrorMessages.FileTrackingNotFound);
            }

            var filePath = fileTracking.Values.Select(x => x.url_path).ToList();
            await _fileTrackingRepository.DeleteRangeAsync(fileTracking.Values);
            await unitOfWork.SaveChangesAsync();
            return filePath;
        }

        #endregion
        #region Private method
        async Task<(int Height, int Width)> ResizeImageAsync(string imageType, string inputPath, string outPath)
        {
            switch (imageType)
            {
                case "image/gif":
                    {
                        using Image image = await Image.LoadAsync(inputPath);
                        var imageDimension = GetOriginalSize(image.Height, image.Width);
                        image.Mutate(x => x.Resize(imageDimension.Width, imageDimension.Height));
                        await image.SaveAsGifAsync(outPath);
                        return (image.Height, image.Width);
                    }
                case "image/png":
                case "image/apng":
                    {
                        using var image = new MagickImage(inputPath);
                        var imageDimension = GetOriginalSize((int)image.Height, image.Width);
                        image.Format = MagickFormat.Png;
                        image.Quality = 80;
                        image.Resize(imageDimension.Width, imageDimension.Height);
                        byte[] data = image.ToByteArray();
                        await File.WriteAllBytesAsync(outPath, data);
                        return (image.Height, image.Width);
                    }
                case "image/heic":
                    {
                        using var image = new MagickImage(inputPath);
                        var imageDimension = GetOriginalSize(image.Height, image.Width);
                        image.Format = MagickFormat.Jpg;
                        image.Quality = 70;
                        image.Resize(imageDimension.Width, imageDimension.Height);
                        byte[] data = image.ToByteArray();
                        await File.WriteAllBytesAsync(outPath, data);
                        return (image.Height, image.Width);
                    }
                default:
                    {
                        using var image = new MagickImage(inputPath);
                        var thumbnailDimension = GetOriginalSize(image.Height, image.Width);
                        image.Format = MagickFormat.Jpg;
                        image.Quality = 80;
                        image.Resize(thumbnailDimension.Width, thumbnailDimension.Height);
                        byte[] data = image.ToByteArray();
                        await File.WriteAllBytesAsync(outPath, data);
                        return (image.Height, image.Width);
                    }
            }
        }

        async Task<(int Height, int Width)> ResizeThumbnailImageAsync(string imageType, string inputPath, string outPath)
        {
            switch (imageType)
            {
                case "image/gif":
                    {
                        using Image image = await Image.LoadAsync(inputPath);
                        var thumbnailDimension = GetThumbnailSize(image.Height, image.Width);
                        image.Mutate(x => x.Resize(thumbnailDimension.Width, thumbnailDimension.Height));
                        await image.SaveAsGifAsync(outPath);
                        return (image.Height, image.Width);
                    }
                case "image/png":
                case "image/apng":
                    {
                        using Image image = await Image.LoadAsync(inputPath);
                        var thumbnailDimension = GetThumbnailSize(image.Height, image.Width);
                        image.Mutate(x => x.Resize(thumbnailDimension.Width, thumbnailDimension.Height));
                        await image.SaveAsync(outPath, new PngEncoder());
                        return (image.Height, image.Width);
                    }
                case "image/heic":
                    {
                        using var image = new MagickImage(inputPath);
                        var thumbnailDimension = GetThumbnailSize(image.Height, image.Width);
                        image.Format = MagickFormat.Jpg;
                        image.Quality = 80;
                        image.Resize(thumbnailDimension.Width, thumbnailDimension.Height);
                        byte[] data = image.ToByteArray();
                        await File.WriteAllBytesAsync(outPath, data);
                        return (image.Height, image.Width);
                    }
                default:
                    {
                        using var image = new MagickImage(inputPath);
                        var thumbnailDimension = GetThumbnailSize(image.Height, image.Width);
                        image.Format = MagickFormat.Jpg;
                        image.Quality = 80;
                        image.Resize(thumbnailDimension.Width, thumbnailDimension.Height);
                        byte[] data = image.ToByteArray();
                        await File.WriteAllBytesAsync(outPath, data);
                        return (image.Height, image.Width);
                    }
            }
        }
        (int Height, int Width) GetThumbnailSize(int height, int width)
        {
            if (height > width)
            {
                double heightRatio = height * 1.0 / 500;
                double widthRatio = width * 1.0 / 261;
                double ratio = heightRatio < widthRatio ? heightRatio : widthRatio;
                if (heightRatio <= 1 || widthRatio <= 1)
                {
                    return (height, width);
                }
                else
                {
                    height = (int)(height / ratio / 2) * 2;
                    width = (int)(width / ratio / 2) * 2;
                }
            }
            else
            {
                double heightRatio = height / 1.0 / 261;
                double widthRatio = width / 1.0 / 500;
                double ratio = heightRatio < widthRatio ? heightRatio : widthRatio;
                if (ratio <= 1)
                {
                    return (height, width);
                }
                else
                {
                    height = (int)(height / ratio / 2) * 2;
                    width = (int)(width / ratio / 2) * 2;
                }
            }

            return (height, width);
        }

        (int Height, int Width) GetOriginalSize(int height, int width)
        {
            if (height > width)
            {
                double heightRatio = height * 1.0 / 1920;
                double widthRatio = width * 1.0 / 1080;
                double ratio = heightRatio < widthRatio ? heightRatio : widthRatio;
                if (ratio <= 1)
                {
                    return (height, width);
                }
                else
                {
                    height = (int)(height / ratio / 2) * 2;
                    width = (int)(width / ratio / 2) * 2;
                }
            }
            else
            {
                double heightRatio = height * 1.0 / 1080;
                double widthRatio = width * 1.0 / 1920;
                double ratio = heightRatio < widthRatio ? heightRatio : widthRatio;
                if (ratio <= 1)
                {
                    return (height, width);
                }
                else
                {
                    height = (int)(height / ratio / 2) * 2;
                    width = (int)(width / ratio / 2) * 2;
                }
            }

            return (height, width);
        }

        #endregion
    }
}
