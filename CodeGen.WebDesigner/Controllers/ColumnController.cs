using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeGen.DynamicLinq;
using CodeGen.Core.Services;
using CodeGen.Core.Model;
using CodeGen.Core.Domain;

namespace CodeGen.WebDesigner.Controllers
{
    public class ColumnController : Controller
    {
        readonly IColumnService repository = new ColumnService();

        public ActionResult Index()
        {
            ViewBag.Message = "Action=Index";
            return View();
        }

        #region CRUD

        /// <summary>
        /// Creates record.
        /// </summary>
        public ActionResult Create(ColumnModel objectModel)
        {
            return Json(repository.Create(objectModel));
        }

        /// <summary>
        /// Reads record.
        /// </summary>
        public ActionResult Read(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter)
        {
            return Json(repository.GetDataResult(take, skip, sort, filter));
        }

        /// <summary>
        /// Updates record.
        /// </summary>
        public ActionResult Update(ColumnModel objectModel)
        {
            return Json(repository.Update(objectModel));
        }

        /// <summary>
        /// Deletes record.
        /// </summary>
        public ActionResult Destroy(ColumnModel objectModel)
        {
            return Json(repository.Delete(objectModel));
        }
        
        #endregion

        /// <summary>
        /// Move order Id
        /// </summary>
        public ActionResult ChangeOrder(string id, string direction)
        {
            repository.ChangeOrder(int.Parse(id), direction);
            return null;
        }







    }
}

#region Notes

//if (objectModel != null && ModelState.IsValid)
//{
//    var result = Json(repository.Create(objectModel));
//    return result;
//}
//else
//{
//    DataSourceResult result = new DataSourceResult();
//    IEnumerable Errors = ModelState.ToDictionary(kvp => kvp.Key,
//                                        kvp => kvp.Value
//                                                  .Errors
//                                                  .Select(e => e.ErrorMessage)
//                                                  .ToArray())
//                                   .Where(m => m.Value.Count() > 0);

//    string msg = null;

//    foreach (string item in Errors)
//    {
                    
//    }

//    result.Data = Errors;
//    result.NotificationMessage = "Error " + msg;
//    result.NotificationMessageType = "error";

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
* Authored date: 1/10/2015
* 
* Modified date: 1/10/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */