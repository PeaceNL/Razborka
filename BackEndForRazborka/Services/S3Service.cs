using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using static System.Net.WebRequestMethods;


namespace BackEndForRazborka.Services
{


    public class S3Service
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _bucketName;
        private readonly RegionEndpoint _region;

        private IAmazonS3 _s3Client;

        public S3Service(string accessKey, string secretKey, string bucketName, string region)
        {
            if (string.IsNullOrWhiteSpace(accessKey) ||
            string.IsNullOrWhiteSpace(secretKey) ||
            string.IsNullOrWhiteSpace(bucketName) ||
            string.IsNullOrWhiteSpace(region))
            {
                throw new ArgumentException("Одно или несколько значений конфигурации отсутствуют.");
            }
            _accessKey = accessKey;
            _secretKey = secretKey;
            _bucketName = bucketName;
            _region = RegionEndpoint.GetBySystemName(region);

            if (_region == null)
            {
                throw new ArgumentException("Неправильный регион.");
            }

            _s3Client = new AmazonS3Client(_accessKey, _secretKey, _region);
        }

        // Загрузка файла в S3
        public async Task<List<string>> UploadFileAsync(List<IFormFile> files)
        {
            var fileUrls = new List<string>();
            var fileTransferUtility = new TransferUtility(_s3Client);
            foreach (var file in files) 
            {
                using var fileStream = file.OpenReadStream();
                var keyName = $"{Guid.NewGuid()}_{file.FileName}";
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fileStream,
                    BucketName = _bucketName,
                    Key= keyName,
                    CannedACL = S3CannedACL.PublicRead
                };

                await fileTransferUtility.UploadAsync(uploadRequest);

                // Получаем URL загруженного файла
                var fileUrl = $"https://{_bucketName}.s3.amazonaws.com/{keyName}";
                fileUrls.Add(fileUrl);
            }
            return fileUrls;
        }

        // Удаление файла из S3
        public async Task DeleteFileAsync(List<string> keyNames)
        {
            try
            {
                if (keyNames.Count == 0){
                    return;
                }
                foreach (var keyName in keyNames)
                {
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = _bucketName,                        
                        Key = keyName[46..],
                    };                    
                    await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                    Console.WriteLine($"Файл {keyName} успешно удален из бакета {_bucketName}.");
                }
                
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Ошибка при удалении файла: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Общая ошибка: {e.Message}");
            }
        }
    }
}