using Amazon.S3;

namespace MarciaApi.Domain.Data.Cloud;

public interface ICloudflare
{
    IAmazonS3 S3Client { get; }
    Task AddFile(IFormFile file);
    Task<string> GetFile(string nameFile);
}