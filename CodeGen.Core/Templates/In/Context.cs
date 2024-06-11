<#TemplateFileOut#>Context.cs<#/TemplateFileOut#>
<#SingleFile#>True<#/SingleFile#>

using CodeGen.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Migrations;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace CodeGen.Core.Data
{
    public class CodeGenContext : DbContext
    {
        public CodeGenContext() : base("name=CodeGenContext")
        {
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            <#ForEachTable#>
            //Section model
            modelBuilder.Configurations.Add(new Map${TableName}Domain());
            <#/ForEachTable#>

            base.OnModelCreating(modelBuilder);
        }

        <#ForEachTable#>
        //Section dbset
        public DbSet<${TableName}Domain> ${TableName}s { get; set; }
        <#/ForEachTable#>

    }
    
}
