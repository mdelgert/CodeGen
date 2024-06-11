<#TemplateFileOut#>${TableName}Domain.cs<#/TemplateFileOut#>
<#SingleFile#>False<#/SingleFile#>
<#ForEachTable#>

using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ${ProjectNameSpace}.Domain
{
    ${ClassSummary}
    public class ${TableName}Domain
    {
        <#ForEachColumn#>
        ${FieldSummary}
        public ${ColumnType} ${ColumnName} { get; set; }
        <#/ForEachColumn#>
    }

}

${CopyWrite}

<#/ForEachTable#>