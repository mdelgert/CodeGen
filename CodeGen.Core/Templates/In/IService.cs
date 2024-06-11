<#TemplateFileOut#>I${TableName}Service.cs<#/TemplateFileOut#>
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

namespace CodeGen.Core.Services
{

    public partial interface I${TableName}Service
    {

        #region MethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        DataSourceResult Create(${TableName}Model objectModel);
        

        /// <summary>
        /// Read - record from database.
        /// </summary>
        ${TableName}Domain ReadById(int id);

        
        /// <summary>
        /// Update - record in database.
        /// </summary>
        DataSourceResult Update(${TableName}Model objectModel);
    
    
        /// <summary>
        /// Delete - record from database.
        /// </summary>
        DataSourceResult Delete(${TableName}Model objectModel);
    
    
        /// <summary>
        /// Reads data source result.
        /// </summary>
        DataSourceResult GetDataResult(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter);
    
    
        /// <summary>
        /// Reads all records from database.
        /// </summary>
        IList<${TableName}Domain> GetAll();
    
    
        #endregion

    }

}

${CopyWrite}

<#/ForEachTable#>