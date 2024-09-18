using Share;
using Microsoft.AspNetCore.Http;

namespace Share.FileManagement
{
    public interface IFileManagementService
    {
        Task<ActionResponse> DeleteFilesFromCloudAsync(IEnumerable<string> keys);
        Task<ActionResponse> UploadFileToCloudAsync(IFormFile file, string key);
        ActionResponse ValidateImage(IFormFile file);
    }
}