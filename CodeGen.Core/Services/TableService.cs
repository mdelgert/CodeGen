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
    public class TableService : ITableService
    {
        readonly IColumnService columnService = new ColumnService();
        DataSourceResult dataSourceResult = new DataSourceResult();
        TableDomain objectDomain = new TableDomain();

        #region MethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        
        public TableModel Create(TableModel objectModel)
        {
            using (var db = new CodeGenContext())
            {
                objectDomain.TableName = objectModel.TableName;
                objectDomain.CreatedOn = System.DateTime.Now;
                db.Tables.Add(objectDomain);
                db.SaveChanges();
                objectModel.Id = objectDomain.Id;
            }

            return objectModel;
        }

        /// <summary>
        /// Create - datasource result record in database.
        /// </summary>
        public DataSourceResult CreateDataSourceResult(TableModel objectModel)
        { 
            try
            {
                dataSourceResult.Data = new[] { this.Create(objectModel) }.ToList();
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
        public TableDomain ReadById(int id)
        { 
            using (var db = new CodeGenContext())
            {
                return db.Tables.Find(id);
            }
        }
        
        public TableModel Update(TableModel objectModel)
        {
            using (var db = new CodeGenContext())
            {
                objectDomain = db.Tables.Find(objectModel.Id);
                objectDomain.TableName = objectModel.TableName;
                db.SaveChanges();
            }

            return objectModel;
        }

        /// <summary>
        /// Update - record in database.
        /// </summary>
        public DataSourceResult UpdateDataSourceResult(TableModel objectModel)
        { 
            try
            {
                dataSourceResult.Data = new[] { this.Update(objectModel) }.ToList();
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
        public DataSourceResult Delete(TableModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.Tables.Find(objectModel.Id);
                    db.Tables.Remove(objectDomain);
                    db.SaveChanges();
                }

                using (var db = new CodeGenContext())
                {
                    //How to delete an object without retrieving it - http://blogs.msdn.com/b/alexj/archive/2009/03/27/tip-9-deleting-an-object-without-retrieving-it.aspx
                    //CRUD using Entity Framework in .NET Framework 5.0 - (Delete) Print Print Email Email - https://support.microsoft.com/kb/2802240/en-us?wa=wsignin1.0
                    var columnRecords = from t in db.Columns
                                        orderby t.Id
                                        where t.TableId == objectModel.Id
                                        select t;

                    foreach (ColumnDomain record in columnRecords)
                    {
                        columnService.DeleteById(record.Id);
                    }
                }

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
        /// Reads grid records from database.
        /// </summary>
        public virtual DataSourceResult GetDataResult(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter)
        {
            try
            {
                using (var db = new CodeGenContext())
                {
                    dataSourceResult = (from t in db.Tables
                                        orderby t.Id
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
        public virtual IList<TableDomain> GetAll()
        {
            using (var db = new CodeGenContext())
            {
                var query = from t in db.Tables
                            orderby t.Id
                            select t;

                return query.ToList();
            }
        }
        
        #endregion
    }
}

#region Notes

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