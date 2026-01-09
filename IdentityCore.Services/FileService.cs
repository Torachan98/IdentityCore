using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using IdentityCore.EFs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IdentityCore.Services
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFileAsync(FileType fileType, IFormFile file)
        {
            string path = await SaveFileServer(file);

            switch(fileType) 
            {
                case FileType.Image:
                    path = uploadImage(path,file.FileName);
                break;

                case FileType.File:
                break;

                default: 
                break;
            }

            return path;
        }

        public async Task<MemoryStream> ViewImageAsync(string fileId)
        {
            try
            {
                GoogleCredential credential = GoogleCredential
                    .GetApplicationDefault()
                    .CreateScoped(DriveService.Scope.Drive);

                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Identity Admin"
                });

                var request = service.Files.Get(fileId);
                var stream = new MemoryStream();

                request.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    Console.WriteLine(progress.BytesDownloaded);
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    Console.WriteLine("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    Console.WriteLine("Download failed.");
                                    break;
                                }
                        }
                    };

                await request.DownloadAsync(stream);
                stream.Position = 0;

                return stream;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        private async Task<string> SaveFileServer(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        private string uploadImage(string filePath,string fileName)
        {
            try
            {
                GoogleCredential credential = GoogleCredential.GetApplicationDefault().CreateScoped(DriveService.Scope.Drive);
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Identity Admin"
                });

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    Parents = ["1BexVSCG8y8IgppjKXz8DJmjOPi6jEY6V"]
                };

                FilesResource.CreateMediaUpload request;

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    request = service.Files.Create(fileMetadata, stream, "image/jpeg");
                    request.Fields = "id";
                    request.Upload();
                }

                var file = request.ResponseBody;

                if(request.ResponseBody == null)
                {
                    throw request.GetProgress().Exception;
                }

                var permission = new Google.Apis.Drive.v3.Data.Permission
                {
                    Type = "anyone",
                    Role = "reader"
                };

                service.Permissions.Create(permission, file.Id).Execute();

                return file.Id;
            }
            catch (Exception ex) 
            {
                if (ex is AggregateException)
                {
                    throw new FriendlyException(StatusCodes.Status403Forbidden, "Credential Not found");
                }
                else if (ex is FileNotFoundException)
                {
                    throw new FriendlyException(StatusCodes.Status403Forbidden, "File not found");
                }
                else
                {
                    throw new FriendlyException(StatusCodes.Status404NotFound, ex.ToString());
                }
            }
        }
    }
}
