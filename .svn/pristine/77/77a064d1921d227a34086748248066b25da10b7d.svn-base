using Microsoft.Extensions.FileProviders;
using SERP.Domain.Common.Constants;

namespace SERP.Api.Common.FileServers;

public abstract class FileServerHelper
{
    public static void ConfigureFileProviders(WebApplication app, IConfiguration configuration)
    {
        var resourceFilePath = Path.Combine(app.Environment.ContentRootPath, DomainConstant.ResourceFolder);
        if (!Directory.Exists(resourceFilePath))
        { 
            Directory.CreateDirectory(resourceFilePath);
        }
        
        var mediaFileProvider = new PhysicalFileProvider(resourceFilePath);
        var requestPath = configuration["Media:RequestPath"];
        var enableDirectoryBrowsing = Convert.ToBoolean(configuration["Media:EnableDirectoryBrowsing"]);
        var webRootProvider = new PhysicalFileProvider(app.Environment.ContentRootPath);
    
        app.Environment.WebRootFileProvider = new CompositeFileProvider(webRootProvider, mediaFileProvider);

        app.UseStaticFiles();
        app.UseFileServer(new FileServerOptions
        {
            FileProvider = mediaFileProvider,
            RequestPath = requestPath,
            EnableDirectoryBrowsing = enableDirectoryBrowsing
        });
    }
    
    public static string SaveToResourceFolder(string rootFolder, IFormFile file, string fileDir, string staticFolder = "Resources")
    {
        var rootStaticFolder = Path.Combine(rootFolder, staticFolder, fileDir);

        if (!Directory.Exists(rootStaticFolder))
            Directory.CreateDirectory(rootStaticFolder);

        // Create new file name
        var extension = Path.GetExtension(file.FileName);
        var newFileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmssfffff}";
        var fileFullName = newFileName + extension;

        var filePath = Path.Combine(fileDir, $"{Guid.NewGuid()}_{fileFullName}");
        var fullFilePath = Path.Combine(rootFolder, staticFolder, filePath);

        using var fs = File.Create(fullFilePath);
        file.CopyTo(fs);

        return Path.Combine("\\", filePath);
    }

    public static void RemoveResourceFile(string rootFolder, string fileDir, string fileName, string staticFolder = "Resources")
    {
        var deleteFile = Path.Combine(rootFolder, staticFolder, fileDir, Path.GetFileName(fileName));
        if (!File.Exists(deleteFile))
        {
            return;
        }

        File.Delete(deleteFile);
    }

    // Method to convert file to IFormFile
    //public static IFormFile ConvertFileToIFormFile(string rootFolder, string fileDir, string fileName, string staticFolder = "Resources")
    //{
    //    var filePath = Path.Combine(rootFolder, staticFolder, fileDir, Path.GetFileName(fileName));
    //    if (!File.Exists(filePath))
    //    {
    //        throw new FileNotFoundException("File does not exist.", filePath);
    //    }

    //    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    //    var formFile = new FormFile(fileStream, 0, fileStream.Length, Path.GetFileNameWithoutExtension(filePath), Path.GetFileName(filePath))
    //    {
    //        Headers = new HeaderDictionary(),
    //        ContentType = "application/octet-stream" // Set appropriate MIME type if known
    //    };

    //    return formFile;
    //}
    public static byte[] ConvertFileToIFormFile(string rootFolder, string fileDir, string fileName, string staticFolder = "Resources")
    {
        var filePath = Path.Combine(rootFolder, staticFolder, fileDir, Path.GetFileName(fileName));
        if (!File.Exists(filePath))
        {
            //throw new FileNotFoundException("File does not exist.", filePath);
            return null;
        }

        byte[] fileBytes;
        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
        }

        return fileBytes;
    }
    
}