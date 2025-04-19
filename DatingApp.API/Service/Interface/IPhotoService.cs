using CloudinaryDotNet.Actions;

namespace DatingApp.API.Service.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotosAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
