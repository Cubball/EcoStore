using EcoStore.DAL.Files.Exceptions;

namespace EcoStore.DAL.Files;

public class FileManager : IFileManager
{
    private readonly string _folderPath;

    public FileManager(string folderPath)
    {
        _folderPath = folderPath;
    }

    public void DeleteFile(string fileName)
    {
        var filePath = Path.Combine(_folderPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public FileStream GetFileStream(string fileName)
    {
        var filePath = Path.Combine(_folderPath, fileName);
        return File.Exists(filePath)
            ? new FileStream(fileName, FileMode.Open)
            : throw new Exceptions.FileNotFoundException($"Не вдалося знайти файл за шляхом: {filePath}");
    }

    public async Task SaveFileAsync(Stream stream, string fileName)
    {
        try
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            var filePath = Path.Combine(_folderPath, fileName);
            using var fs = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fs);
        }
        catch (Exception e)
        {
            throw new FileUploadFailedException("Під час завантаження файлу сталася помилка", e);
        }
    }
}