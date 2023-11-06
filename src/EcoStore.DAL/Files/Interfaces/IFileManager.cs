namespace EcoStore.DAL.Files.Interfaces;

public interface IFileManager
{
    Task SaveFileAsync(Stream stream, string fileName);

    void DeleteFile(string fileName);

    FileStream GetFileStream(string fileName);
}