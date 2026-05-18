using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System.Text.Json;

namespace EcommerceStore.Services.StorageService
{
    public interface IStorageService
    {
        Task InitializeMainBucket();
        Task<string> UploadProductImage(string shopSlug, IFormFile file);
        Task<(string url, string key)> UploadImage(string folder, IFormFile file);
        Task DeleteByKey(string key);
        Task DeleteFile(string fileUrl);
    }

    public class StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _publicEndpoint;

        public StorageService(IConfiguration configuration)
        {
            _bucketName = configuration["S3:BucketName"] ?? "platform-bucket";
            _publicEndpoint = configuration["S3:PublicEndpoint"] ?? "http://localhost:9000";

            var internalEndpoint = configuration["S3:InternalEndpoint"] ?? "http://localhost:9000";
            var accessKey = configuration["S3:AccessKey"] ?? "minioadmin";
            var secretKey = configuration["S3:SecretKey"] ?? "minioadmin";

            var config = new AmazonS3Config
            {
                ServiceURL = internalEndpoint,
                ForcePathStyle = true, // Requisito obligatorio para MinIO
                UseHttp = true
            };

            _s3Client = new AmazonS3Client(accessKey, secretKey, config);
        }

        public async Task InitializeMainBucket()
        {
            try
            {
                // Verifica si existe antes de intentar crearlo
                bool bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
                if (!bucketExists)
                {
                    await _s3Client.PutBucketAsync(new PutBucketRequest { BucketName = _bucketName });
                    Console.WriteLine($"Bucket '{_bucketName}' creado.");
                }
                else
                {
                    Console.WriteLine($"Servicio de almacenamiento listo. Bucket '{_bucketName}'.");
                }

                // Configurar Política de Lectura Pública
                var policy = new
                {
                    Version = "2012-10-17",
                    Statement = new[]
                    {
                        new
                        {
                            Sid = "PublicReadGetObject",
                            Effect = "Allow",
                            Principal = "*",
                            Action = "s3:GetObject",
                            Resource = $"arn:aws:s3:::{_bucketName}/*"
                        }
                    }
                };

                var putPolicyRequest = new PutBucketPolicyRequest
                {
                    BucketName = _bucketName,
                    Policy = JsonSerializer.Serialize(policy)
                };

                await _s3Client.PutBucketPolicyAsync(putPolicyRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar el bucket: {ex.Message}");
            }
        }

        public async Task<string> UploadProductImage(string shopSlug, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var key = $"{shopSlug}/products/{fileName}";

            // Abrimos el Stream del archivo (equivale al buffer en Node)
            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(request);

            return $"{_publicEndpoint}/{_bucketName}/{key}";
        }

        public async Task<(string url, string key)> UploadImage(string folder, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var key = $"{folder}/{fileName}";

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(request);

            return ($"{_publicEndpoint}/{_bucketName}/{key}", key);
        }

        public async Task DeleteByKey(string key)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error borrando archivo en MinIO: {ex.Message}");
            }
        }

        public async Task DeleteFile(string fileUrl)
        {
            try
            {
                var separator = $"{_bucketName}/";
                var urlParts = fileUrl.Split(separator);

                if (urlParts.Length >= 2)
                {
                    var key = urlParts[1];
                    await DeleteByKey(key);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extrayendo key para borrar: {ex.Message}");
            }
        }
    }
}