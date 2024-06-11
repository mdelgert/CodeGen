using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.DynamicLinq;
using CodeGen.Core.Domain;
using CodeGen.Core.Model;
using CodeGen.Core.Data;

namespace CodeGen.Core.Services
{
    /// <summary>
    /// ColumnTypeServices class
    /// </summary>

    public class ColumnTypeService : IColumnTypeService
    {

       DataSourceResult dataSourceResult = new DataSourceResult();
       ColumnTypeDomain objectDomain = new ColumnTypeDomain();

        #region PublicMethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        public DataSourceResult Create(ColumnTypeModel objectModel)
        {
            try
            { 
                using (var db = new CodeGenContext())
                {

                    //Domain object to model mapping make sure to change update and create methods
                    objectDomain.ColumnType = objectModel.ColumnType;
                    objectDomain.CreatedOn = System.DateTime.Now;
                    //Domain object to model mapping make sure to change update and create methods

                    db.ColumnTypes.Add(objectDomain);
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
        public ColumnTypeDomain ReadById(int id)
        { 
            using (var db = new CodeGenContext())
            {
                return db.ColumnTypes.Find(id);
            }
        }
        
        /// <summary>
        /// Update - record in database.
        /// </summary>
        public DataSourceResult Update(ColumnTypeModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.ColumnTypes.Find(objectModel.Id);

                    //Domain object to model mapping make sure to change update and create methods
                    objectDomain.ColumnType = objectModel.ColumnType;
                    //Domain object to model mapping make sure to change update and create methods
                    
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
        public DataSourceResult Delete(ColumnTypeModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.ColumnTypes.Find(objectModel.Id);
                    db.ColumnTypes.Remove(objectDomain);
                    db.SaveChanges();
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
                    dataSourceResult = (from t in db.ColumnTypes
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
        public IList<ColumnTypeDomain> GetAll()
        {
            using (var db = new CodeGenContext())
            {
                return (from t in db.ColumnTypes
                        orderby t.Id
                        select t).ToList();
            }
        }
        
        #endregion

        #region PrivateMethods

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