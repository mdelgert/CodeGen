<#TemplateFileOut#>${TableName}Controller.cs<#/TemplateFileOut#>
<#SingleFile#>False<#/SingleFile#>
<#ForEachTable#>

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
    public class ${TableName}Controller : Controller
    {
        readonly I${TableName}Service repository = new ${TableName}Service();

        public ActionResult Index()
        {
            ViewBag.Message = "Action=Index";
            return View();
        }

        #region CRUD

        /// <summary>
        /// Create record.
        /// </summary>
        public ActionResult Create(${TableName}Model objectModel)
        {
            return Json(repository.Create(objectModel));
        }

        /// <summary>
        /// Read record.
        /// </summary>
        public ActionResult Read(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter)
        {
            return Json(repository.GetDataResult(take, skip, sort, filter));
        }

        /// <summary>
        /// Update record.
        /// </summary>
        public ActionResult Update(${TableName}Model objectModel)
        {
            return Json(repository.Update(objectModel));
        }

        /// <summary>
        /// Delete record.
        /// </summary>
        public ActionResult Destroy(${TableName}Model objectModel)
        {
            return Json(repository.Delete(objectModel));
        }
        
        #endregion

    }

}

${CopyWrite}

<#/ForEachTable#>