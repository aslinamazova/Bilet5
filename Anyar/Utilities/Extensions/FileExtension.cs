using System;
namespace Bizland.Utilities.Extensions;  

	public static class FileExtension
	{
    public static bool CheckContentType(this IFormFile file, string contentType)
    {
        return file.ContentType.ToLower().Trim().Contains(contentType.ToLower().Trim());
    }

    public static bool CheckFileSize(this IFormFile file, double size)
    {
        return file.Length / 1024 < size;
    }


    public async static Task<string> SaveAsync(this IFormFile file, string rootPath)
    { 
        string fileName = Guid.NewGuid().ToString() + file.FileName;
        using (FileStream fileStream = new FileStream(Path.Combine(rootPath, fileName), FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return fileName;
    }
}

