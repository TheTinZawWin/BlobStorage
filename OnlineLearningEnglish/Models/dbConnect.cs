using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineLearningEnglish.Models
{
    public class dbConnect
    {
        DbContextTransaction dbContextTransaction;
        public bool isConnected(ref BlobEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                //RBS_dbEntities dbContext = new RBS_dbEntities();
                if (db.Database.Exists())
                {
                    dbContextTransaction = db.Database.BeginTransaction();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;

        }
        public bool dbCommit(BlobEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                db.SaveChanges();
                dbContextTransaction.Commit();
                //db.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            finally
            {
                db.Database.Connection.Close();
                db.Dispose();
            }
            return result;

        }
        public bool dbClose(BlobEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                db.Database.Connection.Close();
                db.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;

        }
        public bool dbRollback(ref string returnMessage)
        {
            bool result = false;
            try
            {
                dbContextTransaction.Rollback();
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;

        }
    }
}