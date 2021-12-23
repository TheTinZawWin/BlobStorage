using Newtonsoft.Json;
using OnlineLearningEnglish.Models;
using OnlineLearningEnglish.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using static OnlineLearningEnglish.Models.BlobFileProcess;
using static OnlineLearningEnglish.Models.CommonEnum;

namespace OnlineLearningEnglish.Controllers
{
    public class BlobController : Controller
    {
        // GET: Blob
        public ActionResult Index()
        {
            List<BlobData> blobList = new List<BlobData>();
            dbConnect dbConnect = new dbConnect();
            BlobEntities db = new BlobEntities();
            string ret = string.Empty;
            bool result = false;
            if (dbConnect.isConnected(ref db, ref ret))
            {
                try
                {
                     blobList = (from blob in db.TB_BlobDataStorage
                                    select new BlobData
                                    {
                                      //  id = blob.BlobDataStorageSeq,
                                      //  name = blob.Name

                                    }).ToList();

                   
                }



                catch (Exception ex)
                {

                }
            }
            return View(blobList);
        }

        /// <summary>
        /// BlobStorageData
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> BlobStorageData(BlobFileProcessData data)
        {
            bool ret = false;
            BlobFileProcess blobFile = new BlobFileProcess();
            FileInFo fileInFo;
            fileInFo = await blobFile.UploadBlobAsync(data.BlobFile);
            if (!String.IsNullOrEmpty(fileInFo.fileUrl))
            {
                if (InsertIntoBlotDataStorage(data, fileInFo))
                {
                    ret = true;
                }
            }


            return ret;
        }
        /////// <summary>
        /////// BlobFileDowload
        /////// </summary>
        /////// <param name="BlobStorageDataSeq"></param>
        /////// <returns></returns>
        ////public bool BlobFileDowload(int BlobStorageDataSeq)
        ////{
        ////    bool ret = false;
        ////    BlobFileProcess blobFileProcess = new BlobFileProcess();

        ////    BaseTB_BlobDataStorage blobData = new BaseTB_BlobDataStorage();

        ////    BaseTB_BlobDataStorageEntity blobList = new BaseTB_BlobDataStorageEntity();

        ////    blobList = blobData.GetData(BlobStorageDataSeq);


        ////    if (blobFileProcess.download_FromBlob(blobList.StorageName))
        ////    {
        ////        ret = true;
        ////    }



        ////    return ret;
        ////}
        ///// <summary>
        ///// BlobFileUpdate
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        public async Task<bool> BlobFileUpdate(BlobFileProcessData data)
        {
            bool ret = false;
            dbConnect dbConnect = new dbConnect();
            BlobEntities db = new BlobEntities();
            BlobFileProcess blobFileProcess = new BlobFileProcess();
            db.Configuration.AutoDetectChangesEnabled = false;
            string returnMessage = string.Empty;
            BlobFileProcess blobFile = new BlobFileProcess();
            var blobData = (from blob in db.TB_BlobDataStorage
                            where blob.BlobDataStorageSeq == data.BlobStorageDataSeq
                            select blob).FirstOrDefault();

            if (blobFileProcess.DeleteBlobAsync(blobData.Name))
            {
                blobData.StatusCode = Convert.ToInt32(Datastatus.delete);
                UpdateBlob(blobData, ref db, ref returnMessage);
                FileInFo fileInFo;
                fileInFo = await blobFile.UploadBlobAsync(data.BlobFile);
                if (!String.IsNullOrEmpty(fileInFo.fileUrl))
                {
                    if (InsertIntoBlotDataStorage(data, fileInFo))
                    {
                        ret = true;
                    }
                }

            }
            return ret;
        }
       
        public JsonResult BlobFileDelete(int id)
        {
            dbConnect dbConnect = new dbConnect();
            BlobEntities db = new BlobEntities();
            BlobFileProcess blobFileProcess = new BlobFileProcess();
            db.Configuration.AutoDetectChangesEnabled = false;
            string ret = string.Empty;
            string returnMessage = string.Empty;
            bool result = false;
            if (dbConnect.isConnected(ref db, ref ret))
            {
                try
                {
                    var blobData = (from blob in db.TB_BlobDataStorage
                                    where blob.BlobDataStorageSeq == id
                                    select blob).FirstOrDefault();

                    if (blobFileProcess.DeleteBlobAsync(blobData.Name))
                    {
                      

                        blobData.StatusCode = Convert.ToInt32(Datastatus.delete);
                        UpdateBlob(blobData, ref db, ref returnMessage);
                        if (!dbConnect.dbCommit(db, ref returnMessage))
                        {
                            dbConnect.dbRollback(ref returnMessage);
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        ret = "delete fail";
                    }
                   
                }



                catch (Exception ex)
                {
                    ret = ex.Message.ToString();
                }
            }


            return Json(ret, JsonRequestBehavior.AllowGet);
        }

      
        public JsonResult GetBlobStorageDataList()
        {
            Data data = new Data();
            dbConnect dbConnect = new dbConnect();
            BlobEntities db = new BlobEntities();
            string ret = string.Empty;
            bool result = false;
            if (dbConnect.isConnected(ref db, ref ret))
            {
                try
                {
                  var  blobList = (from blob in db.TB_BlobDataStorage
                                   where blob.StatusCode!=(int)Datastatus.delete
                                 select new BlobData
                                 {
                                    Id=blob.BlobDataStorageSeq,
                                    Name=blob.Name,
                                    Storagename=blob.StorageName,
                                    StorageData=blob.StorageData,
                                    Statuscode=blob.StatusCode.ToString()
                                   
                                 }).ToList();
                   
                    data.status = "success";
                    data.total = blobList.Count;
                    data.records = blobList;
                  
                    
                }



                catch (Exception ex)
                {

                }
            }


            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

      
        private bool InsertIntoBlotDataStorage(BlobFileProcessData data, FileInFo fileInFo)
        {
            
            dbConnect dbConnect = new dbConnect();
            BlobEntities db = new BlobEntities();
            string returnMessage = string.Empty;
            bool result = false;
            if (dbConnect.isConnected(ref db, ref returnMessage))
            {
                try
                {
                    
                    TB_BlobDataStorage blob = new TB_BlobDataStorage();
                    blob.StorageData = fileInFo.fileUrl;
                    blob.StorageName = fileInFo.fileName;
                    blob.Name = fileInFo.fileName;
                    blob.StatusCode = (int)Datastatus.insert;
                    db.TB_BlobDataStorage.Add(blob);
                    if (!dbConnect.dbCommit(db, ref returnMessage))
                    {
                        dbConnect.dbRollback(ref returnMessage);
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }

                }

                catch (Exception ex)
                {
                    returnMessage = ex.Message;
                }
            }
            return result;
        }
        public bool UpdateBlob(TB_BlobDataStorage blob, ref BlobEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
              
                    db.TB_BlobDataStorage.Attach(blob);
                    db.Entry(blob).State = System.Data.Entity.EntityState.Modified;
                
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;
        }
        internal class Data
        {
            public string status { get; set; } // Status of operation
            public int total { get; set; } // total number of records
            public List<BlobData> records { get; set; } //records
        }
    }
}