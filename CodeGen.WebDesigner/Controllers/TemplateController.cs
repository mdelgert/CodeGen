using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeGen.Core.Domain;
using CodeGen.DynamicLinq;
using CodeGen.Core.Services;
using CodeGen.Core.Model;

namespace CodeGen.WebDesigner.Controllers
{
    public class TemplateController : Controller
    {
        private readonly ITemplateService templateService = new TemplateService();

        // GET: Template
        public ActionResult Index(string action)
        {
            ViewBag.Message = "Action=" + action;
            return View();
        }

        public ActionResult RunCmd(string cmd)
        {
            string response = "";

            if (cmd == "loadFiles")
            {
                response = templateService.LoadTemplates();
            }

            if (cmd == "RunAllTemplates")
            {
                response = templateService.RunAllTemplates();
            }

            return Json(response);
        }

        public ActionResult RunTemplate(TemplateDomain objectDomain)
        {
            string response = templateService.RunTemplate(objectDomain);
            return Json(response);
        }

        #region CRUD

        /// <summary>
        /// Creates table record.
        /// </summary>
        public ActionResult Create(TemplateModel objectModel)
        {
            return Json(templateService.CreateDataSourceResult(objectModel));
        }

        /// <summary>
        /// Reads table record.
        /// </summary>
        public ActionResult Read(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter)
        {
            return Json(templateService.GetDataResult(take, skip, sort, filter));
        }

        /// <summary>
        /// Returns all records
        /// </summary>
        public ActionResult GetAll()
        {
            return Json(templateService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates table record.
        /// </summary>
        public ActionResult Update(TemplateModel objectModel)
        {
            return Json(templateService.UpdateDataSourceResult(objectModel));
        }

        /// <summary>
        /// Deletes table record.
        /// </summary>
        public ActionResult Destroy(TemplateModel objectModel)
        {
            return Json(templateService.Delete(objectModel));
        }

        #endregion
    }
}

#region Notes

//"Run templateId=" + objectDomain.Id.ToString();
//string response = templateService.RunTemplate(objectDomain);

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