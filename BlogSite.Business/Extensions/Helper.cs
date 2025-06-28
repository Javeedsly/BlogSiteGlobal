using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BlogSite.Business.Exceptions;

public static class Helper
{
    // folderPath misal üçün "uploads/portfolios"
    public static string SaveImage(IWebHostEnvironment env, IFormFile file, string folderPath)
    {
        // 1) Təzə content-type yoxlaması
        if (file == null || !file.ContentType.StartsWith("image/"))
            throw new ImageContentException("Fayl formatı düzgün deyil!");

        // 2) Maksimum ölçü: 2 MB
        const long maxBytes = 2 * 1024 * 1024;
        if (file.Length > maxBytes)
            throw new ImageSizeException("Fayl 2 MB-dan böyük ola bilməz!");

        // 3) Fiziki qovluğu hazırla: wwwroot/uploads/portfolios
        string uploadsRoot = Path.Combine(env.WebRootPath, folderPath);
        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        // 4) Unikal ad + uzantı
        string ext = Path.GetExtension(file.FileName);
        string fileName = $"{Guid.NewGuid()}{ext}";
        string fullPath = Path.Combine(uploadsRoot, fileName);

        // 5) Yaz
        using (var fs = new FileStream(fullPath, FileMode.Create))
            file.CopyTo(fs);

        // 6) Browser üçün relative URL: "uploads/portfolios/abcd.jpg"
        var relative = Path.Combine(folderPath, fileName)
                           .Replace("\\", "/");  // windows slash-ları əvəz et
        return "/" + relative;       // -> "/uploads/portfolios/abcd.jpg"
    }

    public static void DeleteImage(IWebHostEnvironment env, string relativeUrl)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl))
            return;

        // "/uploads/portfolios/abcd.jpg" -> "uploads/portfolios/abcd.jpg"
        var trimmed = relativeUrl.TrimStart('/', '\\');
        var fullPath = Path.Combine(env.WebRootPath, trimmed);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
        // yoxdursa sakitcə çıx (və ya özünüz exception atın)
    }
}
