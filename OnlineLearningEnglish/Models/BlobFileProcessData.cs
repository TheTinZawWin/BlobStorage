using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineLearningEnglish.Models.Customer
{
    
    public class BlobFileProcessData
    {
      
        public int BlobStorageDataSeq { get; set; }
        public HttpPostedFileBase BlobFile { get; set; }
    }

    public class BlobData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Storagename { get; set; }

        public string StorageData { get; set; }

        public string Statuscode { get; set; }
        
    }
}
