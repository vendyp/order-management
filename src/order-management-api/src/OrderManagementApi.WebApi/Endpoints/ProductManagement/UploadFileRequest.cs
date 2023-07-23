namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class UploadFileRequest
{
    public IFormFile File { get; set; } = null!;
}