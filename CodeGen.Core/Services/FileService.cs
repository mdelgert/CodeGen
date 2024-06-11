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
    public class FileService : IFileService
    {
        DataSourceResult dataSourceResult = new DataSourceResult();
        FileDomain objectDomain = new FileDomain();

        #region MethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        
        public FileModel Create(FileModel objectModel)
        {
            using (var db = new CodeGenContext())
            {
                objectDomain.TemplateId = objectModel.TemplateId;
                objectDomain.FileName = objectModel.FileName;
                objectDomain.FileInput = objectModel.FileInput;
                objectDomain.FileOutput = objectModel.FileOutput;
                objectDomain.IsDeleted = false;
                objectDomain.CreatedOn = System.DateTime.Now;
                db.Files.Add(objectDomain);
                db.SaveChanges();
                objectModel.Id = objectDomain.Id;
            }

            return objectModel;
        }

        /// <summary>
        /// Create - datasource result record in database.
        /// </summary>
        public DataSourceResult CreateDataSourceResult(FileModel objectModel)
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
        public FileDomain ReadById(int id)
        { 
            using (var db = new CodeGenContext())
            {
                return db.Files.Find(id);
            }
        }
        
        public FileModel Update(FileModel objectModel)
        {
            using (var db = new CodeGenContext())
            {
                objectDomain = db.Files.Find(objectModel.Id);
                objectDomain.TemplateId = objectModel.TemplateId;
                objectDomain.FileName = objectModel.FileName;
                objectDomain.FileInput = objectModel.FileInput;
                objectDomain.FileOutput = objectModel.FileOutput;
                objectDomain.IsDeleted = objectModel.IsDeleted;
                db.SaveChanges();
            }

            return objectModel;
        }

        /// <summary>
        /// Update - record in database.
        /// </summary>
        public DataSourceResult UpdateDataSourceResult(FileModel objectModel)
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
        public DataSourceResult Delete(FileModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    //objectDomain = db.Files.Find(objectModel.Id);
                    //db.Files.Remove(objectDomain);
                    //db.SaveChanges();
                    //Do not really delete only flag item as deleted
                    objectModel.IsDeleted = true;
                    this.Update(objectModel);
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
                    dataSourceResult = (from t in db.Files
                                        orderby t.Id
                                        where t.IsDeleted == false
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
        public virtual IList<FileDomain> GetAll()
        {
            using (var db = new CodeGenContext())
            {
                var query = from t in db.Files
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