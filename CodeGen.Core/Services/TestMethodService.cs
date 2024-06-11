using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGen.Core.Domain;
using CodeGen.Core.Model;
using CodeGen.DynamicLinq;
using CodeGen.Core.Data;

namespace CodeGen.Core.Services
{
    public class TestMethodService : ITestMethodService
    {
        TableModel tableModel = new TableModel();
        readonly ITokenService tokenService = new TokenService();
        readonly ITableService tableService = new TableService();
        readonly IColumnService columnService = new ColumnService();
        readonly IColumnTypeService columnTypeService = new ColumnTypeService();

        #region TestMethods
        
        //########################################################################################
        //For testing only comment out region before release
        //########################################################################################
        
        public void RunTest(string testName)
        { 
            if (testName == "GenerateAllTestData()")
            {
                //Insert column types into DB
                columnTypeService.Create(new ColumnTypeModel() { ColumnType = "int" });

                columnTypeService.Create(new ColumnTypeModel() { ColumnType = "string" });

                columnTypeService.Create(new ColumnTypeModel() { ColumnType = "DateTime" });

                columnTypeService.Create(new ColumnTypeModel() { ColumnType = "bool" });

                columnTypeService.Create(new ColumnTypeModel() { ColumnType = "double" });
                
                //######################## Generate tables and columns data #########################################################################################################

                tableModel = tableService.Create(new TableModel() { TableName = "Table" });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TableName", ColumnTypeId = 2 });

                tableModel = tableService.Create(new TableModel() { TableName = "Column" });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "OrderId", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TableId", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "ColumnName", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "ColumnTypeId", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "ColumnType", ColumnTypeId = 2 });

                tableModel = tableService.Create(new TableModel() { TableName = "ColumnType" });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "ColumnType", ColumnTypeId = 2 });

                tableModel = tableService.Create(new TableModel() { TableName = "Token" });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Token", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TokenValue", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TokenExpression", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TokenNote", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "bool", ColumnTypeId = 2 });

                tableModel = tableService.Create(new TableModel() { TableName = "Template" });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TemplateFileName", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TemplateFileExtension", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TemplateText", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "CreatedOnUtc", ColumnTypeId = 3 });

                tableModel = tableService.Create(new TableModel() { TableName = "File" });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TemplateId", ColumnTypeId = 3 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "FileTextIn", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "FileTextOut", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "FileName", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "FilePathDestination", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "CreatedOnUtc", ColumnTypeId = 3 });

                tableModel = tableService.Create(new TableModel() { TableName = "Log" });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "LogLevelId", ColumnTypeId = 1 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "ShortMessage", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "FullMessage", ColumnTypeId = 2 });
                columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "CreatedOnUtc", ColumnTypeId = 3 });
                
                //######################## Generate tables and columns data #########################################################################################################

                tokenService.CreateSystemTokens();
                tokenService.CreateSampleTokens();
                

            }

            if (testName == "TruncateTables()")
            {
                using (var db = new CodeGenContext())
                {
                    string sql = "truncate table [dbo].[CodeGenColumn]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "truncate table [dbo].[CodeGenTable]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "truncate table [dbo].[CodeGenToken]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "truncate table [dbo].[CodeGenColumnType]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "truncate table [dbo].[CodeGenTemplate]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "truncate table [dbo].[CodeGenFile]"; 
                    db.Database.ExecuteSqlCommand(sql);
                }
            }

            if (testName == "DropTables()")
            {
                using (var db = new CodeGenContext())
                {
                    string sql = "drop table [dbo].[__MigrationHistory]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "drop table [dbo].[CodeGenColumn]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "drop table [dbo].[CodeGenTable]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "drop table [dbo].[CodeGenToken]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "drop table [dbo].[CodeGenColumnType]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "drop table [dbo].[CodeGenTemplate]"; 
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "drop table [dbo].[CodeGenFile]"; 
                    db.Database.ExecuteSqlCommand(sql);
                }
            }


        }
        //if (testName == "GenerateDefaultTokens()")
        //{
        //    tokenService.Create(new TokenModel() { Token = "${ProjectNameSpace}", TokenValue = "CodeGen.Core" });
        //    tokenService.Create(new TokenModel() { Token = "${CopyWrite}", TokenValue = "//Copy write @ 2015 your company name" });
        //    tokenService.Create(new TokenModel() { Token = "${ClassSummary}", TokenValue = "//Class summary" });
        //    tokenService.Create(new TokenModel() { Token = "${FieldSummary}", TokenValue = "//Field summary" });
        //    tokenService.Create(new TokenModel() { Token = "${EntityContext}", TokenValue = "CodeGenContext" });
        //}
        //if (testName == "GenerateTableColumnTestData()")
        //{
        //    tableModel = tableService.Create(new TableModel() { TableName = "Table" });
        //    columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnType = "int", OrderId = 0 });
        //    columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TableName", ColumnType = "string", OrderId = 1 });
        //    tableModel = tableService.Create(new TableModel() { TableName = "Column" });
        //    columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "Id", ColumnType = "int", OrderId = 0 });
        //    columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "OrderId", ColumnType = "int", OrderId = 1 });
        //    columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "TableId", ColumnType = "int", OrderId = 2 });
        //    columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "ColumnName", ColumnType = "string", OrderId = 3 });
        //    columnService.Create(new ColumnModel() { TableId = tableModel.Id, ColumnName = "ColumnType", ColumnType = "string", OrderId = 4 });
        //    tokenService.Create(new TokenModel() { Token = "${Project}", TokenValue = "CodeGen"});
        //    tokenService.Create(new TokenModel() { Token = "${ProjectNameSpace}", TokenValue = "CodeGen.Core"});
        //    tokenService.Create(new TokenModel() { Token = "${CopyWrite}", TokenValue = "//Copy write @ 2015 your company name"});
        //    tokenService.Create(new TokenModel() { Token = "${ClassSummary}", TokenValue = "//Class summary"});
        //    tokenService.Create(new TokenModel() { Token = "${FieldSummary}", TokenValue = "//Field summary"});
        //    tokenService.Create(new TokenModel() { Token = "${EntityContext}", TokenValue = "CodeGenContext"});
        //}
        //########################################################################################
        //For testing only comment out region before release
        //########################################################################################
        
        #endregion
    }
}

#region Notes

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