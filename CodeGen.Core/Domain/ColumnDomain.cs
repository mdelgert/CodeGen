﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeGen.Core.Domain
{
    public class ColumnDomain
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets ColumnTypeId
        /// </summary>
        [Required] 
        public int ColumnTypeId { get; set; }

        /// <summary>
        /// Gets or sets OrderId
        /// </summary>
        [Required] 
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets TableId
        /// </summary>
        [Required] 
        public int TableId { get; set; }

        /// <summary>
        /// Gets or sets ColumnName
        /// </summary>
        [Required] 
        [MaxLength(50)]
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets IsPrimaryKey flag
        /// </summary>
        [Required] 
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        [Required] 
        public DateTime CreatedOn { get; set; }
    }
}

#region Notes

// Code First Data Annotations - http://msdn.microsoft.com/en-us/data/jj591583.aspx

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