namespace Shopping.Helpers
{
    public interface IImageHelper
    {
        Task<(Guid Id, string Extension)> UploadImageAsync(IFormFile file, string folder);

        void DeleteImage(Guid id, string extension, string folder);
    }
}
