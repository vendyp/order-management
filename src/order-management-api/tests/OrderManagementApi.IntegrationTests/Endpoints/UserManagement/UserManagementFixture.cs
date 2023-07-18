namespace OrderManagementApi.IntegrationTests.Endpoints.UserManagement;

[CollectionDefinition(nameof(UserManagementFixture))]
public class UserManagementFixture : BaseServiceFixture, ICollectionFixture<UserManagementFixture>
{
    public UserManagementFixture() : base(nameof(UserManagementFixture))
    {
    }
}