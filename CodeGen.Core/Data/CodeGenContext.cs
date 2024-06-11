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
            modelBuilder.Configurations.Add(new MapColumnDomain());
            modelBuilder.Configurations.Add(new MapColumnTypeDomain());
            modelBuilder.Configurations.Add(new MapFileDomain());
            modelBuilder.Configurations.Add(new MapTableDomain());
            modelBuilder.Configurations.Add(new MapTemplateDomain());
            modelBuilder.Configurations.Add(new MapTokenDomain());

            //http://aspboss.blogspot.com/2012/03/model-backing-mydbcontext-context-has.html
            //disable EdmMetadata generation
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            base.OnModelCreating(modelBuilder);
            //Database.SetInitializer<CodeGenContext>(new DropCreateDatabaseIfModelChanges<CodeGenContext>());
        }


        public DbSet<ColumnDomain> Columns { get; set; }
        public DbSet<ColumnTypeDomain> ColumnTypes { get; set; }
        public DbSet<FileDomain> Files { get; set; }
        public DbSet<TableDomain> Tables { get; set; }
        public DbSet<TemplateDomain> Templates { get; set; }
        public DbSet<TokenDomain> Tokens { get; set; }

    }
    
}

#region Notes

//How to Enable Migration to update my database in MVC4? - http://stackoverflow.com/questions/17922945/how-to-enable-migration-to-update-my-database-in-mvc4

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