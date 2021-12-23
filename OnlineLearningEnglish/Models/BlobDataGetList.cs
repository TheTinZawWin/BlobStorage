//// =============================================================================
//// <summary>
//// モデル
//// </summary>
//// <copyright file="SCRRM005A02_AutoRepaymentDetail.cs" company="Works">
//// Copyright(C) Works Co.,Ltd.
//// </copyright>
//// <author>
//// Works
//// </author>
//// =============================================================================

//namespace HybridCapOh.Common.Models.Customer
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Web.Mvc;
//    using HybridCapOh.Common;
//    using HybridCapOh.Common.Common;
//    using HybridCapOh.Common.Const;
//    using HybridCapOh.Common.Models.Base;

//    /// <summary>
//    /// モデル
//    /// </summary>
//    public class BlobDataGetList : ModelBase
//    {
//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        public BlobDataGetList()
//        {
//            this.BlobDataStorageEntity = new BaseTB_BlobDataStorageEntity();
//        }

//        /// <summary>
//        /// BaseTB_BlobDataStorageEntity
//        /// </summary>
//        public BaseTB_BlobDataStorageEntity BlobDataStorageEntity { get; set; }

//        /// <summary>
//        /// 画面ID
//        /// </summary>
//        public override string ScreenId { get { return "SCRCS021"; } }

        

//        /// <summary>
//        /// 初期値を設定する
//        /// </summary>
//        public override void InitialValueSetting()
//        {
//            base.InitialValueSetting();
//        }

//        /// <summary>
//        /// パラメータを設定する
//        /// </summary>
//        /// <param name="src">src</param>
//        /// <param name="bankPaymentHeaderSeq">bankPaymentHeaderSeq</param>
//        public void SetParameters(string src, string BlobDataStorageSeq)
//        {
//            this.Src = src;

//            if (string.IsNullOrWhiteSpace(BlobDataStorageSeq))
//            {
//                throw new ArgumentException(string.Format("blobDataStorageSeq:[{0}]", BlobDataStorageSeq));
//            }

//            this.BlobDataStorageEntity.BlobDataStorageSeq = int.Parse(BlobDataStorageSeq);
//            this.GetHeaderInfo();
//        }

//        /// <summary>
//        /// Validate
//        /// </summary>
//        /// <param name="modelState">modelState</param>
//        public override void Validate(ref ModelStateDictionary modelState)
//        {
//        }

//        /// <summary>
//        /// 銀行入金明細データを取得します
//        /// </summary>
//        /// <returns>SearchResultObject</returns>
//        public SearchResultObject GetDataList()
//        {
//            var m = new BaseTB_BlobDataStorage();
//            var whereParams = new Dictionary<string, object>();
//            var whereSql = new StringBuilder();
//            DataBaseFoundation.AddWhereParameter(
//                nameof(m.ContractNo),
//                this.BlobDataStorageEntity.ContractNo,
//                whereSql,
//                whereParams);
//            var data = m.GetDataList(whereSql.ToString(), whereParams, $"{nameof(m.ContractNo)} ASC");
//            int totalCount = data.Count;

//            SearchResultObject sro = new SearchResultObject
//            {
//                totalCount = totalCount,
//                records = new List<SearchResultRecordBase>()
//            };

           
//            int recid = 1;
//            foreach (var r in data)
//            {
               
                
//                sro.records.Add(new SearchResultRecord()
//                {
//                    recid = recid.ToString(),
//                    BlobDataStorageSeq = r.BlobDataStorageSeq,
//                    Name = r.Name,
//                    StorageData = r.StorageData
//                });

//                recid++;
//            }

//            return sro;
//        }

//        /// <summary>
//        /// GetDataListByJson
//        /// </summary>
//        /// <param name="bankPaymentHeaderSeq">bankPaymentHeaderSeq</param>
//        /// <param name="limit">limit</param>
//        /// <param name="offset">offset</param>
//        /// <param name="sort">order</param>
//        /// <returns>JsonString</returns>
//        public string GetDataListByJson(string ContractNo, string limit, string offset, List<W2uiSearchSortCondtion> sort)
//        {
//            var m = new BaseTB_BlobDataStorage();
//            var whereParams = new Dictionary<string, object>();
//            var whereSql = new StringBuilder();
//            DataBaseFoundation.AddWhereParameter(
//                nameof(m.ContractNo),
//                ContractNo,
//                whereSql,
//                whereParams);
//            DataBaseFoundation.AddWhereParameter(
//               nameof(m.StatusCode),
//               "1",
//               whereSql,
//               whereParams);
//            // Get total count
//            int totalCount = m.GetCount(whereSql.ToString(), whereParams);

//            // Get limited data
//            //var order = DataBaseFoundation.GetLimitOffsetOrder(sort, int.Parse(offset), int.Parse(limit));
//            var data = m.GetDataList(whereSql.ToString(), whereParams, "");
//             BaseTB_User userCreate = new BaseTB_User();
            
//             BaseTB_UserEntity usrCreate = new BaseTB_UserEntity();
//            if (data.Count > 0)
//            {
//                usrCreate = userCreate.GetData(data.FirstOrDefault().CreatedBy);
//            }
           
//            var sro = new SeachInfiniteResultObject
//            {
//                status = "scuccess",
//                total = totalCount,
//                records = new List<SearchResultRecordBase>()
//            };

            
//            int recid = int.Parse(offset) + 1;
//            foreach (var r in data)
//            {
               
//                sro.records.Add(new SearchResultRecord()
//                {
//                    recid = recid.ToString(),
//                    BlobDataStorageSeq=r.BlobDataStorageSeq,
//                    UploadedDateTime=r.CreatedAt,
//                    Name=r.Name,
//                    StaffName = userCreate.NameE,
//                    StorageData =r.StorageData,
                   


//                });

//                recid++;
//            }

//            return Newtonsoft.Json.JsonConvert.SerializeObject(sro);
//        }

//        /// <summary>
//        /// GetHeaderInfo
//        /// </summary>
//        private void GetHeaderInfo()
//        {
//            var m1 = new BaseTB_BlobDataStorage();
//           // var header = m1.GetData(this.BlobDataStorageEntity.ContractNo);
//            var m2 = new BaseTB_PaymentMethod();
           
//            this.BlobDataStorageEntity.Name = "AA";
            
//        }

//        /// <summary>
//        /// ScreenParam
//        /// </summary>
//        public class ScreenParam
//        {
//            /// <summary>
//            /// ContractNo
//            /// </summary>
//            public const string ContractNo = "contractNo";

         
//        }

//        /// <summary>
//        /// 検索結果となるレコード
//        /// </summary>
//        public class SearchResultRecord : SearchResultRecordBase
//        {
//            /// <summary>
//            /// BlobDataStorageSeq
//            /// </summary>
//            public int BlobDataStorageSeq { get; set; }


//            /// <summary>
//            /// Name
//            /// </summary>
//            public string Name { get; set; }

           
//            /// <summary>
//            /// StorageData
//            /// </summary>
//            public string StorageData { get; set; }

//            /// <summary>
//            /// RegistrationDateTime
//            /// </summary>
//            public DateTime UploadedDateTime { get; set; }

//            /// <summary>
//            /// PaymentMethodName
//            /// </summary>
//            public string StaffName { get; set; }


//        }
//    }
//}
