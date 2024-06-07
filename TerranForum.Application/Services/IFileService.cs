namespace TerranForum.Application.Services
{
    public interface IFileService
    {
        string ContentPath { get; }
        string UploadedImagesPath { get; }
    }
}
