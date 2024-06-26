﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeGen.DynamicLinq
{
    /// <summary>
    /// Describes the result of Kendo DataSource read operation. 
    /// </summary>
    [KnownType("GetKnownTypes")]
    public class DataSourceResult
    {
        /// <summary>
        /// Represents a single page of processed data.
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// The total number of records available.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Represents a requested aggregates.
        /// </summary>
        public object Aggregates { get; set; }

        /// <summary>
        /// Used by the KnownType attribute which is required for WCF serialization support
        /// </summary>
        /// <returns></returns>
        private static Type[] GetKnownTypes()
        {
            var assembly = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .FirstOrDefault(a => a.FullName.StartsWith("DynamicClasses"));

            if (assembly == null)
            {
                return new Type[0];
            }

            return assembly.GetTypes()
                           .Where(t => t.Name.StartsWith("DynamicClass"))
                           .ToArray();
        }

        /// <summary>
        /// Message to the grid
        /// </summary>
        public string PopNotificationMessage { get; set; }

        /// <summary>
        /// Pop notification message type: valid types "info", "upload-success", "error"
        /// </summary>
        public string PopNotificationMessageType { get; set; }

    }

}

#region Notes

//Kendo.DynamicLinq - https://github.com/kendo-labs/dlinq-helpers

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
* Project: CodeGen.DynamicLinq
*
* Authored date: 1/10/2015
* 
* Modified date: 1/10/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */
