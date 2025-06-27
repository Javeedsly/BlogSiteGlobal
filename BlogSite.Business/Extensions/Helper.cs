using BlogSite.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using System;

public static class Helper
{
    public static string SaveFile(string rootPath, string folder, IFormFile file)
    {
        if (file.ContentType != "image/png" && file.ContentType != "image/jpeg")
            throw new ImageContentException("Fayl formatı düzgün deyil!");
        if (file.Length > 2097152)
            throw new ImageSizeException("Fayl 2 MB-dan böyük ola bilməz!");

        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string folderPath = Path.Combine(rootPath, folder);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string path = Path.Combine(folderPath, fileName);

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        // Yalnız fayl adını deyil, route path-in tamamını qaytarmaq daha yaxşı olar
        return $"/{folder}/{fileName}";
    }

    public static void DeleteFile(string rootPath, string folder, string fileName)
    {
        string path = Path.Combine(rootPath, folder, fileName);

        if (!File.Exists(path))
            throw new BlogSite.Business.Exceptions.FileNotFoundException("Fayl tapılmadı");

        File.Delete(path);
    }
}
