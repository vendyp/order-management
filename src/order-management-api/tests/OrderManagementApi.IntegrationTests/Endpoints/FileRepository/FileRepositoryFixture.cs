namespace OrderManagementApi.IntegrationTests.Endpoints.FileRepository;

[CollectionDefinition(nameof(FileRepositoryFixture))]
public class FileRepositoryFixture : BaseServiceFixture, ICollectionFixture<FileRepositoryFixture>
{
    public FileRepositoryFixture() : base(nameof(FileRepositoryFixture))
    {
    }
}