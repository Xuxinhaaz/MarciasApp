using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using MarciaApi.Domain.Data.Cloud;

namespace MarciaApi.Infrastructure.Data.Cloud;

public class Cloudflare : ICloudflare
{
    public readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public Cloudflare(IConfiguration configuration)
    {
        _configuration = configuration;
        var credentials = new BasicAWSCredentials(_configuration["Cloudflare:AccessKey"], _configuration["Cloudflare:SecretKey"]);
        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config()
        {
            ServiceURL = $"https://{_configuration["Cloudflare:AccountID"]}.r2.cloudflarestorage.com",
            SignatureVersion = "4"
        });
    }

    public IAmazonS3 S3Client => _s3Client;
    
    public async Task AddFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception();
        }
        
        var allowedContentTypes = new List<string> { "image/jpeg", "image/png", "application/pdf" };
        if (!allowedContentTypes.Contains(file.ContentType))
        {
            throw new InvalidOperationException("Unsupported file type.");
        }

        PutObjectRequest putObjectRequest = new PutObjectRequest()
        {
            BucketName = _configuration["Cloudflare:Buckets:Main"],
            Key = file.FileName,
            ContentType = file.ContentType,
            DisablePayloadSigning = true
        };
        
        using (var stream = file.OpenReadStream())
        {
            putObjectRequest.InputStream = stream;

            await _s3Client.PutObjectAsync(putObjectRequest);
        }
    }

    public async Task<string> GetFile(string nameFile)
    {
        GetObjectRequest request = new()
        {
            BucketName = _configuration["Cloudflare:Buckets:Main"],
            Key = nameFile
        };
        
        using var result = await 
            _s3Client.GetObjectAsync(request);

        return result.ETag;
    }
}