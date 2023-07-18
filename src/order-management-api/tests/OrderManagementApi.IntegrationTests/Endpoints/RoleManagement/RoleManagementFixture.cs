namespace OrderManagementApi.IntegrationTests.Endpoints.RoleManagement;

[CollectionDefinition(nameof(RoleManagementFixture))]
public class RoleManagementFixture : BaseServiceFixture, ICollectionFixture<RoleManagementFixture>
{
    public RoleManagementFixture() : base(nameof(RoleManagementFixture))
    {
    }
}