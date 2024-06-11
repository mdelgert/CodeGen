<#TemplateFileOut#>Map${TableName}Domain.cs<#/TemplateFileOut#>
<#SingleFile#>False<#/SingleFile#>
<#ForEachTable#>

using CodeGen.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace CodeGen.Core.Data
{
    public class Map${TableName}Domain : EntityTypeConfiguration<${TableName}Domain>
    {
        /// <summary>
        /// Database mappings
        /// </summary>
        public Map${TableName}Domain()
        {
            ToTable("${TableName}Column");
            HasKey(m => m.Id); //Need to add logic no primary key so column is not added twice
            <#ForEachColumn#>
            Property(m => m.${ColumnName});
            <#/ForEachColumn#>

        }

    }

}

${CopyWrite}

<#/ForEachTable#>