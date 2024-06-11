using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeGen.DynamicLinq;
using CodeGen.Core.Services;
using CodeGen.Core.Model;

namespace CodeGen.WebDesigner.Controllers
{
    public class TokenController : Controller
    {
        readonly ITokenService repository = new TokenService();

        public ActionResult Index()
        {
            ViewBag.Message = "Action=Index";
            return View();
        }

        #region CRUD

        /// <summary>
        /// Creates record.
        /// </summary>
        public ActionResult Create(TokenModel objectModel)
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
        /// Returns all records
        /// </summary>

        //public ActionResult GetAll()
        //{
        //    return Json(repository.GetAll(), JsonRequestBehavior.AllowGet);
        //}
        
        public ActionResult GetAll()
        {
            var returnData = repository.GetAll();

            return Json(returnData);
        }

        /// <summary>
        /// Updates record.
        /// </summary>
        public ActionResult Update(TokenModel objectModel)
        {
            return Json(repository.Update(objectModel));
        }
        
        /// <summary>
        /// Deletes record.
        /// </summary>
        public ActionResult Destroy(TokenModel objectModel)
        {
            //if (!objectModel.IsSystemToken)
            //{
            //    return Json(repository.Delete(objectModel));
            //}
            //else
            //{
            //    return null;
            //}

            return Json(repository.Delete(objectModel));

        }
        
        #endregion
    }
}

#region Notes

///// <summary>
///// Updates record.
///// </summary>
//public ActionResult Update([Bind(Prefix = "models")]
//                           IEnumerable<TokenModel> objectModel)
//{
//    foreach (TokenModel item in objectModel)
//    {
//        repository.Update(item);
//    }

//    return this.GetAll();
//}

///// <summary>
///// Deletes record.
///// </summary>
////public ActionResult Destroy(TokenModel objectModel)
////{
////    //return Json(repository.Delete(objectModel));
////    return null;
////}

//public ActionResult Destroy([Bind(Prefix = "models")]
//                            IEnumerable<TokenModel> objectModel)
//{
//    foreach (TokenModel item in objectModel)
//    {
//        repository.Delete(item);
//    }

//    return this.GetAll();
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
 * Authored date: 1/10/2015
 * 
 * Modified date: 1/10/2015
 * 
 * Notes: 
 * 
 * ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */