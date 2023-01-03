

using Minio.Exceptions;
using Minio;

namespace MusicDownloaderAPI.MinIO
{
    public class MinIOProvider : IMinIOProvider
    {
        private readonly MinioClient _minioClient;
        private readonly string endpoint = "localhost:9000";
        private readonly string accessKey = "jibajibajibajiba";
        private readonly string secretKey = "urusurusurus";
        private readonly string bucketName = "music";

        public MinIOProvider() 
        {
            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
        }

        public async void UploadFile(Stream stream, string fileName)
        {
            var bucketName = "music";
            var contentType = "binary/octet-stream";

            try
            {
                // Make a bucket on the server, if not already present.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                // Upload a file to bucket.
                using (var filestream = stream)
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(fileName)
                        .WithStreamData(filestream)
                        .WithObjectSize(filestream.Length)
                        .WithContentType(contentType);

                    await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                }
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
        }

        public string GetDownloadLink(string filename) 
        {
            var argss = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(filename)
                .WithExpiry(60 * 60);
            string url = _minioClient.PresignedGetObjectAsync(argss).Result;
            return url;
        }
    }
}
