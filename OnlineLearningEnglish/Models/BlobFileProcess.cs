
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace OnlineLearningEnglish.Models
{
    public class BlobFileProcess
    {
        string connString = "DefaultEndpointsProtocol=https;AccountName=nitindemostorage;AccountKey=jH/5WgOZzQ7wSQv6BSZ2fbJW3sSEAzinmOUhorUWFpfJSUKPUopAD5uDIP3lvJujmnw1hBoD2RFSzHlPYt2nLw==;";
        /// <summary>
        /// UploadBlobAsync
        /// </summary>
        /// <param name="imageToUpload"></param>
        /// <returns></returns>
        public async Task<FileInFo> UploadBlobAsync(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            FileInFo fileInfo = new FileInFo();
            MemoryStream s = new MemoryStream();
            //check container test or pro
            string savefolder = "test";

            string destContainer = savefolder.ToLower();

            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(destContainer);
                
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                            //Private 

                        }
                        );
                }
               
               
                string imageName = Guid.NewGuid().ToString() + "-" + imageToUpload.FileName;
              
                string subName = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM");
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(subName+"/"+imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                
               
                await cloudBlockBlob.UploadFromStreamAsync(imageToUpload.InputStream);
                imageFullPath = cloudBlockBlob.Uri.ToString();
                fileInfo.fileName = imageName;
                fileInfo.fileUrl = imageFullPath;
            }
            catch (Exception ex)
            {

            }
            return fileInfo;
        }

        public bool DeleteBlobAsync(string FileName)
        {
       
            //check container test or pro
            string savefolder = "test";
            bool result = false;
            string destContainer = savefolder.ToLower();

            
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(destContainer);

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(FileName);
                cloudBlockBlob.DeleteIfExists();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool upload_ToBlob(string fileToUpload)
        {

            string file_extension,
            filename_withExtension;

            Stream file;

            //check container test or pro
            string savefolder = "test";
            string destContainer = savefolder.ToLower();


            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(destContainer);

            // << reading the file as filestream from local machine >>    
            file = new FileStream(fileToUpload, FileMode.Open);


            //checking the container exists or not  
            if (cloudBlobContainer.CreateIfNotExists())
            {

                cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess =
                  BlobContainerPublicAccessType.Blob
                });

            }

            //reading file name & file extention    
            file_extension = Path.GetExtension(fileToUpload);
            filename_withExtension = Path.GetFileName(fileToUpload);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename_withExtension);
            cloudBlockBlob.Properties.ContentType = file_extension;

            cloudBlockBlob.UploadFromStreamAsync(file); // << Uploading the file to the blob >>  

            return true;

        }

        /// <summary>
        /// download_FromBlob
        /// </summary>
        /// <param name="filetoDownload"></param>
        /// <returns></returns>
        public bool download_FromBlob(string filetoDownload)
        {
            //check container test or pro
            string savefolder = "test";
            string destContainer = savefolder.ToLower();


            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connString);
           // string destContainer = splitExt[1].ToLower();
            string subName = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM");
            
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(destContainer);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(subName + "/" + filetoDownload);


            FileStream file;
            //Stream file;
            if (destContainer == "mbf")
            {
                file = File.OpenWrite(@"D:\MBFBlobFile\" + filetoDownload);
            }
            else
            {
                file = File.OpenWrite(@"D:\MBFTESTBlobFile\" + filetoDownload);
            }
            MemoryStream ms = new MemoryStream();

            //ms.CopyTo(file);
            cloudBlockBlob.DownloadToStream(ms);
            //cloudBlockBlob.DownloadToFile(filetoDownload,FileMode.Create);
            //var s = File(cloudBlockBlob,cloudBlockBlob.Properties.ContentType,file);


            return true;

        }

        //public FileStreamResult GetFile()
        //{
        //    var stream = new MemoryStream();
        //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
        //    blockBlob.DownloadToStream(stream);
        //    blockBlob.Seek(0, SeekOrigin.Begin);
        //    var file = new FileStreamResult(stream, FileMode.Open, FileAccess.Read);
        //    return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain").ToString())
        //    {
        //        FileDownloadName = "someFile.txt"
        //    };
        //}
        public class FileInFo
        {
            public string fileUrl { get; set; }
            public string fileName { get; set; }
        }


   
    }
}
