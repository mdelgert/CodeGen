<#TemplateFileOut#>${TableName}Model.cs<#/TemplateFileOut#>
<#SingleFile#>False<#/SingleFile#>
<#ForEachTable#>

using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ${ProjectNameSpace}.Model
{
    ${ClassSummary}
    public class ${TableName}Model
    {
        <#ForEachColumn#>
        ${FieldSummary}
        public ${ColumnType} ${ColumnName} { get; set; }
        <#/ForEachColumn#>
    }

}
${CopyWrite}

<#/ForEachTable#>