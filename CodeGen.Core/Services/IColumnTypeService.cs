using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.DynamicLinq;
using CodeGen.Core.Domain;
using CodeGen.Core.Model;

namespace CodeGen.Core.Services
{
    /// <summary>
    /// IColumnTypeServices interface class
    /// </summary>
    
    public partial interface IColumnTypeService
    {

        #region MethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        DataSourceResult Create(ColumnTypeModel objectModel);
        
        /// <summary>
        /// Read - record from database.
        /// </summary>
        ColumnTypeDomain ReadById(int id);
        
        /// <summary>
        /// Update - record in database.
        /// </summary>
        DataSourceResult Update(ColumnTypeModel objectModel);
        
        /// <summary>
        /// Delete - record from database.
        /// </summary>
        DataSourceResult Delete(ColumnTypeModel objectModel);
        
        /// <summary>
        /// Reads all grid records from database.
        /// </summary>
        DataSourceResult GetDataResult(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter);
        
        /// <summary>
        /// Reads all records from database.
        /// </summary>
        IList<ColumnTypeDomain> GetAll();
    
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