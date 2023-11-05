namespace EcoStore.DAL.Files;

public interface IFileManager
{
    Task SaveFileAsync(Stream stream, string fileName);

    void DeleteFile(string fileName);

    FileStream GetFileStream(string fileName);
}