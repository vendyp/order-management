namespace OrderManagementApi.WebApi.Endpoints.FileRepository;

public class UploadFileRequest
{
    public IFormFile File { get; set; } = null!;
    public string Source { get; set; } = null!;
}