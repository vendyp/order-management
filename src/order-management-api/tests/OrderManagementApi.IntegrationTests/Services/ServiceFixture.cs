namespace OrderManagementApi.IntegrationTests.Services;

[CollectionDefinition(nameof(ServiceFixture))]
public class ServiceFixture : BaseServiceFixture, ICollectionFixture<ServiceFixture>
{
    public ServiceFixture() : base(nameof(ServiceFixture))
    {
    }
}