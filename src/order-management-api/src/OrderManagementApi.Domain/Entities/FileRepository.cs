using OrderManagementApi.Domain.Enums;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public sealed class FileRepository : BaseEntity
{
    public FileRepository()
    {
        FileRepositoryId = Guid.NewGuid();
        FileName = string.Empty;
        UniqueFileName = string.Empty;
        IsFileDeleted = false;
        FileExtension = string.Empty;
        FileType = FileType.Others;
        FileStoreAt = FileStoreAt.FileSystem;
    }

    public FileRepository(string fileName, string uniqueFileName, long size,
        FileStoreAt fileStoreAt = FileStoreAt.FileSystem) : this()
    {
        Size = size;
        FileName = fileName;
        UniqueFileName = uniqueFileName;
        FileExtension = Path.GetExtension(fileName).ToUpper();

        if (fileStoreAt != FileStoreAt.FileSystem)
            FileStoreAt = fileStoreAt;

        if (FileRepositoryExtensions.ListOfFileTypeImages.Any(e => e == FileExtension.ToUpper()))
            FileType = FileType.Images;

        if (FileRepositoryExtensions.ListOfFileTypeDocuments.Any(e => e == FileExtension.ToUpper()))
            FileType = FileType.Document;
    }

    public Guid FileRepositoryId { get; set; }
    public string FileName { get; set; }
    public string UniqueFileName { get; set; }
    public string FileExtension { get; set; }
    public long Size { get; set; }

    /// <summary>
    /// Source meaning identifier for business process use case
    /// </summary>
    public string? Source { get; set; }

    public string? Note { get; set; }

    /// <summary>
    /// Default value is <see cref="Enums.FileType.Others">FileType.Others</see>
    /// </summary>
    public FileType FileType { get; set; }

    /// <summary>
    /// Default value is <see cref="Enums.FileStoreAt.FileSystem">FileStoreAt.FileSystem</see>
    /// </summary>
    public FileStoreAt FileStoreAt { get; set; }

    /// <summary>
    /// Default value is false.
    /// </summary>
    public bool IsFileDeleted { get; set; }

    public void DeleteTheFile()
    {
        IsFileDeleted = true;
    }
}