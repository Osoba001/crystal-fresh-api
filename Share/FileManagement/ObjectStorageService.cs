using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using static Share.ActionResponse;
using Microsoft.AspNetCore.Http;
using Share;
using Share.FileManagement;

namespace Share.FileManagement
{
    internal class ObjectStorageService(ObjectStorageConfiguration configuration) : IFileManagementService
    {
        private readonly ObjectStorageConfiguration _storageConfiguration = configuration;

        public async Task<ActionResponse> DeleteFilesFromCloudAsync(IEnumerable<string> keys)
        {
            try
            {
                Account account = new(
                    _storageConfiguration.StorageName,
                    _storageConfiguration.AccessKey,
                    _storageConfiguration.SecretKey
                );

                Cloudinary cloudinary = new(account);
                List<string> deletedFiles = [];
                List<string> failedFiles = [];

                foreach (var key in keys)
                {
                    var deletionParams = new DeletionParams(key);
                    var deletionResult = await cloudinary.DestroyAsync(deletionParams);

                    if (deletionResult.Result == "ok")
                    {
                        deletedFiles.Add(key);
                    }
                    else
                    {
                        failedFiles.Add(key);
                    }
                }

                return new ActionResponse
                {
                    PayLoad = new
                    {
                        deletedFiles,
                        failedFiles,
                        message = "File deletion process completed."
                    }
                };
            }
            catch (Exception ex)
            {
                return ServerExceptionError(ex);
            }
        }


        public async Task<ActionResponse> UploadFileToCloudAsync(IFormFile file, string key)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                Account account = new(
                  _storageConfiguration.StorageName,
                  _storageConfiguration.AccessKey,
                  _storageConfiguration.SecretKey
              );

                Cloudinary cloudinary = new(account);
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, new MemoryStream(imageBytes)),
                    PublicId = key,
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                string imageUrl = uploadResult.SecureUri.ToString();
                return new ActionResponse { PayLoad = new { imageUrl } };
            }
            catch (Exception ex)
            {

                return ServerExceptionError(ex);
            }
        }

        public ActionResponse ValidateImage(IFormFile file)
        {
            var fileExt = Path.GetExtension(file.FileName)?.TrimStart('.').ToLower();
            if (string.IsNullOrWhiteSpace(fileExt) || !IsValidImageExtension(fileExt))
                return BadRequestResult("Invalid image type.");
            return SuccessResult();
        }
        private static readonly HashSet<string> ValidImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
             "png", "jpeg", "webp", "bmp", "pbm", "tga", "gif", "tiff","psd","raw","heif","indd","jfi","jfif","jpe","jif","jpg"
        };

        private static bool IsValidImageExtension(string extension)
        {
            return ValidImageExtensions.Contains(extension);
        }
    }
}
