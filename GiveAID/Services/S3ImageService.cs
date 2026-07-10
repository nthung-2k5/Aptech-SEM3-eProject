using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class S3ImageService : IImageService
{
    private readonly AmazonS3Client s3Client;
    private readonly string bucketName;
    
    private static readonly AmazonS3Config S3Config = new()
    {
        ServiceURL = "http://127.0.0.1:9000",
        ForcePathStyle = true,
        AuthenticationRegion = "us-east-1"
    };

    public S3ImageService(IConfiguration config)
    {
        string? accessKey = config["AWS:AccessKey"];
        string? secretKey = config["AWS:SecretKey"];
        bucketName = config["AWS:BucketName"] ?? "giveaid-bucket";

        s3Client = new AmazonS3Client(accessKey, secretKey, S3Config);
    }

    // public async Task<Uri> GetImageUriAsync(string key)
    // {
    //     if (Uri.TryCreate(key, UriKind.Absolute, out var uri))
    //     {
    //         return uri;
    //     }
    //     
    //     var getRequest = new GetPreSignedUrlRequest
    //     {
    //         BucketName = bucketName,
    //         Key = key,
    //         Verb = HttpVerb.GET,
    //         Protocol = Protocol.HTTP,
    //         Expires = DateTime.UtcNow.AddDays(7)
    //     };
    //     
    //     return new Uri(await s3Client.GetPreSignedURLAsync(getRequest));
    // }

    public async Task EnsureBucketExists()
    {
        var response = await s3Client.ListBucketsAsync();
        if (response.Buckets != null && response.Buckets.Any(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase))) return;
        
        var putBucketRequest = new PutBucketRequest
        {
            BucketName = bucketName
        };

        await s3Client.PutBucketAsync(putBucketRequest);
        
        await s3Client.PutPublicAccessBlockAsync(new PutPublicAccessBlockRequest
        {
            BucketName = bucketName,
            PublicAccessBlockConfiguration = new PublicAccessBlockConfiguration
            {
                BlockPublicAcls = false,
                IgnorePublicAcls = false,
                BlockPublicPolicy = false,
                RestrictPublicBuckets = false
            }
        });

        string bucketPolicy = $$"""
                            {
                                "Version": "2012-10-17",
                                "Statement": [
                                    {
                                        "Sid": "PublicReadGetObject",
                                        "Effect": "Allow",
                                        "Principal": "*",
                                        "Action": "s3:GetObject",
                                        "Resource": "arn:aws:s3:::{{bucketName}}/*"
                                    }
                                ]
                            }
            """;

        await s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest
        {
            BucketName = bucketName,
            Policy = bucketPolicy
        });
    }

    public async Task<string> UploadImageAsync(string folder, string filename, byte[] bytes)
    {
        if (bytes.Length == 0)
        {
            return string.Empty;
        }

        string uniqueFileName = $"{folder}/{filename}";
        
        using var newMemoryStream = new MemoryStream(bytes);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = uniqueFileName,
            BucketName = bucketName
        };

        var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"http://127.0.0.1:9000/{bucketName}/{uniqueFileName}";
    }
}
