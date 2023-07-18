using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Enums;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class FileRepositoryTests
{
    [Fact]
    public void Ctor_FileRepository_ShouldBe_Correct()
    {
        const string fileName = "test.txt";
        const long size = 123456789;
        var uniqueFileName = $"{Guid.NewGuid()}.txt";

        var fileRepository = new FileRepository(fileName, uniqueFileName, size);
        fileRepository.FileRepositoryId.ShouldNotBe(default);
        fileRepository.FileName.ShouldBe(fileName);
        fileRepository.Size.ShouldBe(size);
        fileRepository.UniqueFileName.ShouldBe(uniqueFileName);
        fileRepository.IsFileDeleted.ShouldBeFalse();
        fileRepository.FileExtension.ShouldBe(".txt".ToUpper());
        fileRepository.FileType.ShouldBe(FileType.Document);
        fileRepository.FileStoreAt.ShouldBe(FileStoreAt.FileSystem);
    }

    [Fact]
    public void FileRepository_Func_DeleteTheFile_ShouldBe_Correct()
    {
        const string fileName = "test.txt";
        const long size = 123456789;
        var uniqueFileName = $"{Guid.NewGuid()}.txt";

        var fileRepository = new FileRepository(fileName, uniqueFileName, size);

        fileRepository.IsFileDeleted.ShouldBeFalse();

        fileRepository.DeleteTheFile();

        fileRepository.IsFileDeleted.ShouldBeTrue();
    }

    [Fact]
    public void Ctor_FileRepository_ShouldBe_Correct_2()
    {
        const string fileName = "test.txt";
        const long size = 123456789;
        const FileStoreAt fileStoreAt = FileStoreAt.AzureBlob;
        var uniqueFileName = $"{Guid.NewGuid()}.txt";

        var fileRepository = new FileRepository(fileName, uniqueFileName, size, fileStoreAt);
        fileRepository.FileStoreAt.ShouldBe(fileStoreAt);
    }
}