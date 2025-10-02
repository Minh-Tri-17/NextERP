namespace NextERP.BLL.Interface
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName, string subFolder);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string subFolder);

        Task DeleteFileAsync(string fileName, string subFolder);
    }
}
