<#TemplateFileOut#>${TableName}Service.cs<#/TemplateFileOut#>
<#SingleFile#>False<#/SingleFile#>
<#ForEachTable#>

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
    public class ${TableName}Service : I${TableName}Service
    {
        DataSourceResult dataSourceResult = new DataSourceResult();
        ${TableName}Domain objectDomain = new ${TableName}Domain();

        #region PublicMethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        public DataSourceResult Create(${TableName}Model objectModel)
        {
            try
            { 
                using (var db = new CodeGenContext())
                {
                    <#ForEachColumn#>
                    //Create
                    objectDomain.${ColumnName} = objectModel.${ColumnName};
                    <#/ForEachColumn#>
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
        /// Update - record in database.
        /// </summary>
        public DataSourceResult Update(${TableName}Model objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.Columns.Find(objectModel.Id);
                    <#ForEachColumn#>
                    //Update
                    objectDomain.${ColumnName} = objectModel.${ColumnName};
                    <#/ForEachColumn#>
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
        /// Read - record from database.
        /// </summary>
        public ${TableName}Domain ReadById(int id)
        { 
            using (var db = new CodeGenContext())
            {
                return db.${TableName}s.Find(id);
            }
        }

        /// <summary>
        /// Delete - record from database.
        /// </summary>
        public DataSourceResult Delete(${TableName}Model objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.${TableName}s.Find(objectModel.Id);
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
                objectDomain = db.${TableName}s.Find(id);
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
                    dataSourceResult = (from t in db.${TableName}s
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
        public IList<${TableName}Domain> GetAll()
        {
            using (var db = new CodeGenContext())
            {
                return (from t in db.${TableName}s
                        orderby t.Id
                        select t).ToList();
            }
        }
        
        #endregion

    }

}

${CopyWrite}

<#/ForEachTable#>