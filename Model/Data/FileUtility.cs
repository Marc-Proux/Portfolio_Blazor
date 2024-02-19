using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

public class FileUtility
{
    // List of common image file extensions
    private static readonly HashSet<string> ImageExtensions = new HashSet<string>
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg" // Add or remove extensions as needed
    };

    public static bool IsImageExtensionValid(string path)
    {
        string extension = Path.GetExtension(path).ToLower();

        return ImageExtensions.Contains(extension);
    }

    public static bool IsImageExists(string path)
    {
        return File.Exists(path);
    }

    public static async Task<string> SaveImageAsync(IBrowserFile file)
    {
        string targetFolder = "wwwroot/img/projects";

        // Ensure the target folder exists
        Directory.CreateDirectory(targetFolder);
        string path;

        int counter = 0;
        string fileName = file.Name;
        string extension = Path.GetExtension(fileName);
        string name = Path.GetFileNameWithoutExtension(fileName);
        path = Path.Combine(targetFolder, fileName);
        bool exists;
        do{
            exists = File.Exists(path);
            if(exists)
            {
                counter++;
                fileName = $"{name}({counter}){extension}";
                path = Path.Combine(targetFolder, fileName);
            }
        } while(exists);

        try
        {
            using (var stream = file.OpenReadStream(15 * 1024 * 1024))
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fs);
            }

            return fileName;
        }
        catch
        {
            return "";
        }
    }

    public static async Task<bool> DeleteImageAsync(string path)
    {
        try
        {
            File.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
