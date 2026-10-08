namespace Shopping.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<(Guid Id, string Extension)> UploadImageAsync(IFormFile file, string folder)
        {
            string extension = Path.GetExtension(file.FileName);
            Guid id = Guid.NewGuid();
            string folderPath = Path.Combine(_env.WebRootPath, "images", folder);
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, $"{id}{extension}");

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (id, extension);
        }

        public void DeleteImage(Guid id, string extension, string folder)
        {
            if (id == Guid.Empty)
            {
                return;
            }

            string filePath = Path.Combine(_env.WebRootPath, "images", folder, $"{id}{extension}");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
