using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGen.Core.Domain;
using CodeGen.Core.Model;
using CodeGen.DynamicLinq;
using CodeGen.Core.Data;

namespace CodeGen.Core.Services
{
    public class ColumnService : IColumnService
    {
        DataSourceResult dataSourceResult = new DataSourceResult();
        ColumnDomain objectDomain = new ColumnDomain();

        #region PrivateMethods

        /// <summary>
        /// Returns the next OrderId
        /// </summary>
        private int GetNextOrderId(ColumnModel objectModel)
        {
            int orderId = 0; //Set default OrderId to 0

            var query = new List<ColumnDomain>();

            using (var db = new CodeGenContext())
            {
                query = (from t in db.Columns
                         where t.TableId == objectModel.TableId
                         select t).ToList();
            }

            if (query.Count != 0)
            {
                orderId = query.Count; //Return the next order Id. First arrary value starting at 0.
            }

            return orderId;
        }

        /// <summary>
        /// Reindex all OrderId after delete
        /// </summary>
        private void ReIndexOrderId(ColumnModel objectModel)
        {
            int orderId = 0;
            var items = GetAllByTableId(objectModel.TableId);

            foreach (ColumnDomain columnDomain in items)
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.Columns.Find(columnDomain.Id);
                    objectDomain.OrderId = orderId;
                    db.SaveChanges();
                }

                orderId++;
            }
        }

        #endregion

        public void ChangeOrder(int id, string direction)
        {
            ColumnDomain columnSelected = new ColumnDomain();
            columnSelected = this.ReadById(id);
            int newOrderID = 0;
            var items = GetAllByTableId(columnSelected.TableId);

            if (direction == "Up")
            {
                if (columnSelected.OrderId == 0)
                {
                    return; //Do nothing at the lowest level
                }
                else
                {
                    newOrderID = columnSelected.OrderId - 1;
                }
            }

            if (direction == "Down")
            {
                if (columnSelected.OrderId == items.Count - 1)
                {
                    return; //Do nothing at the highest level
                }
                else
                {
                    newOrderID = columnSelected.OrderId + 1;
                }
            }
            
            foreach (ColumnDomain columnDomain in items)
            {
                if (columnDomain.OrderId == newOrderID)
                {
                    using (var db = new CodeGenContext())
                    {
                        objectDomain = db.Columns.Find(columnDomain.Id);
                        objectDomain.OrderId = columnSelected.OrderId; //switch id with original
                        db.SaveChanges();
                    }
                }
            }

            using (var db = new CodeGenContext())
            {
                objectDomain = db.Columns.Find(columnSelected.Id);
                objectDomain.OrderId = newOrderID; //update the id
                db.SaveChanges();
            }
        }

        #region PublicMethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        public DataSourceResult Create(ColumnModel objectModel)
        {
            try
            { 
                using (var db = new CodeGenContext())
                {
                    //Look at using auto mapper here
                    objectDomain.ColumnName = objectModel.ColumnName;
                    objectDomain.TableId = objectModel.TableId;
                    objectDomain.ColumnTypeId = objectModel.ColumnTypeId;
                    objectDomain.OrderId = this.GetNextOrderId(objectModel);
                    objectDomain.CreatedOn = System.DateTime.Now;
                    db.Columns.Add(objectDomain);
                    db.SaveChanges();
                }

                dataSourceResult.Data = new[] { objectDomain }.ToList();
                dataSourceResult.PopNotificationMessage = "Create success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Read - record from database.
        /// </summary>
        public ColumnDomain ReadById(int id)
        { 
            using (var db = new CodeGenContext())
            {
                return db.Columns.Find(id);
            }
        }
        
        /// <summary>
        /// Update - record in database.
        /// </summary>
        public DataSourceResult Update(ColumnModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.Columns.Find(objectModel.Id);
                    objectDomain.TableId = objectModel.TableId;
                    objectDomain.OrderId = objectModel.OrderId;
                    objectDomain.ColumnTypeId = objectModel.ColumnTypeId;
                    objectDomain.ColumnName = objectModel.ColumnName;
                    db.SaveChanges();
                }
                
                dataSourceResult.Data = new[] { objectDomain }.ToList();
                dataSourceResult.PopNotificationMessage = "Update success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Delete - record from database.
        /// </summary>
        public DataSourceResult Delete(ColumnModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.Columns.Find(objectModel.Id);
                    db.Columns.Remove(objectDomain);
                    db.SaveChanges();
                }

                this.ReIndexOrderId(objectModel);

                dataSourceResult.PopNotificationMessage = "Delete success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Delete - record from database by id.
        /// </summary>
        public void DeleteById(int id)
        { 
            using (var db = new CodeGenContext())
            {
                objectDomain = db.Columns.Find(id);
                db.Columns.Remove(objectDomain);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Reads grid records from database.
        /// </summary>
        public virtual DataSourceResult GetDataResult(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter)
        {
            try
            {
                using (var db = new CodeGenContext())
                {
                    dataSourceResult = (from t in db.Columns
                                        orderby t.OrderId
                                        select t).ToDataSourceResult(take, skip, sort, filter);
                }
                
                dataSourceResult.PopNotificationMessage = "Query success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Reads all records from database.
        /// </summary>
        public IList<ColumnDomain> GetAll()
        {
            using (var db = new CodeGenContext())
            {
                return (from t in db.Columns
                        orderby t.Id
                        select t).ToList();
            }
        }
        
        /// <summary>
        /// Reads all records from database by tableId.
        /// </summary>
        public IList<ColumnDomain> GetAllByTableId(int tableId)
        {
            using (var db = new CodeGenContext())
            {
                return (from t in db.Columns
                        where t.TableId == tableId
                        orderby t.OrderId
                        select t).ToList();
            }
        }
        
        #endregion
    }
}

#region Notes

//"Query success! Debug; ?take=" + take.ToString() + "&skip=" + skip.ToString() + "&sort=" + sort.First().ToString() + "&filter=" + filter.Field.First().ToString(),

//using (var db = new CodeGenContext())
//{
//    dataSourceResult = db.Columns.Select(n => new ColumnModel
//    {
//        Id = n.Id,
//        TableID = n.TableID,
//        ColumnName = n.ColumnName,
//    }).ToDataSourceResult(take, skip, sort, filter);
//}

//public void ChangeOrder(int id, string direction)
//{
//    ColumnDomain columnSelected = new ColumnDomain();

//    columnSelected = this.ReadById(id);

//    var items = GetAllByTableId(columnSelected.TableId);

//    int oldOrderId = columnSelected.OrderId;
//    int newOrderId = 0;
//    int i = 0;

//    if (direction == "Up")
//    {
//        if (columnSelected.OrderId != 0)
//        {
//            newOrderId = columnSelected.OrderId - 1;
//        }
//        else
//        {
//            newOrderId = 0; //Can not move higher than 0
//        }
//    }

//    if (direction == "Down")
//    {
//        if (columnSelected.OrderId > items.Count) 
//        {
//            newOrderId = columnSelected.OrderId + 1;
//        }
//        else
//        {
//            newOrderId = items.Count - 1;
//        }
//    }

//    using (var db = new CodeGenContext())
//    {
//        objectDomain = db.Columns.Find(columnSelected.Id);
//        objectDomain.OrderId = newOrderId;    
//        db.SaveChanges();
//    }

//    if(newOrderId == 0)
//    {
//        i = 1;
//    }

//    foreach (ColumnDomain columnDomain in items)
//    {
//        if (i != newOrderId & columnDomain.Id != columnSelected.Id)
//        {
//            using (var db = new CodeGenContext())
//            {
//                objectDomain = db.Columns.Find(columnDomain.Id);
//                objectDomain.OrderId = i;    
//                db.SaveChanges();
//            }
//        }

//        i++;
//    }

//}

#endregion

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
* Copyright © 2015 Matthew David Elgert mdelgert@vessea.com, All Rights Reserved. 
*
* NOTICE: All information contained herein is, and remains the property of Matthew Elgert. The intellectual and technical concepts contained
* herein are proprietary to Matthew Elgert and may be covered by U.S. and Foreign Patents, patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material is strictly forbidden unless prior written permission is obtained
* from Matthew Elgert. Access to the source code contained herein is hereby forbidden to anyone except Matthew Elgert or contractors who have executed 
* Confidentiality and Non-disclosure agreements explicitly covering such access.
*
* The copyright notice above does not evidence any actual or intended publication or disclosure of this source code, which includes
* information that is confidential and/or proprietary, and is a trade secret, of Matthew Elgert. ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC PERFORMANCE, 
* OR PUBLIC DISPLAY OF OR THROUGH USE OF THIS SOURCE CODE WITHOUT THE EXPRESS WRITTEN CONSENT OF Matthew Elgert IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE 
* LAWS AND INTERNATIONAL TREATIES. THE RECEIPT OR POSSESSION OFTHIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT ITMAY DESCRIBE, IN WHOLE OR IN PART.
*
* Company: VESSEA, LLC.
*
* Author: Matthew David Elgert
*
* Project: CodeGen.Core
*
* Authored date: 1/10/2015
* 
* Modified date: 1/10/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */