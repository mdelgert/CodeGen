using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using CodeGen.Core.Domain;
using CodeGen.Core.Model;
using CodeGen.DynamicLinq;
using CodeGen.Core.Data;

namespace CodeGen.Core.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IFileService _fileService = new FileService();
        private readonly ITableService _tableService = new TableService();
        private readonly ITokenService _tokenService = new TokenService();
        private readonly IColumnService _columnService = new ColumnService();
        private readonly IColumnTypeService _columnTypeService = new ColumnTypeService();
        private DataSourceResult _dataSourceResult = new DataSourceResult();
        private TemplateDomain _objectDomain = new TemplateDomain();

        #region MethodsTemplates

        /// <summary>
        /// Loads template files
        /// </summary>
        public string LoadTemplates()
        {
            var tokenDictionary = this.TokenDictionary(); //Load the token dictionary

            string msg = "";

            try
            {
                string[] fileEntries = Directory.GetFiles(tokenDictionary["TemplateInDirectory"]);

                foreach (string file in fileEntries)
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        String templateText = sr.ReadToEnd();

                        TemplateModel templateModel = new TemplateModel
                        {
                            TemplateFileName = file.Replace(tokenDictionary["TemplateInDirectory"], ""),
                            TemplateFileExtension = Path.GetExtension(file),
                            TemplateText = templateText
                        };

                        templateModel.SingleFile =
                            bool.Parse(GetSection(templateModel.TemplateText,
                                @"(?s)(?<=<#SingleFile#>).*?(?=<#/SingleFile#>)"));

                        templateModel.Id = this.GetTemplateId(templateModel);

                        if (templateModel.Id == 0)
                        {
                            this.Create(templateModel);
                        }
                        else
                        {
                            this.Update(templateModel);
                        }
                    }
                }

                msg = "Success loading files!";
            }
            catch (Exception e)
            {
                msg = e.ToString();
            }

            return msg;
        }

        /// <summary>
        /// Runs all templates
        /// </summary>
        public string RunAllTemplates()
        {
            string responseMsg = "cmd=RunAllTemplates";


            try
            {
                var listTemplates = this.GetAll();

                foreach (var template in listTemplates)
                {
                    RunTemplate(template);
                }

            }
            catch (Exception e)
            {
                responseMsg = responseMsg + "&Exception=" + e;
            }

            return responseMsg;

        }

        /// <summary>
        /// Run template
        /// </summary>
        public string RunTemplate(TemplateDomain templateDomain)
        {
            string responseMsg = "";

            bool hasSectionTemplateFileOut = HasSection(templateDomain.TemplateText,
                @"(?s)(?<=<#TemplateFileOut#>).*?(?=<#/TemplateFileOut#>)");

            bool hasSectionSingleFile = HasSection(templateDomain.TemplateText,
                @"(?s)(?<=<#SingleFile#>).*?(?=<#/SingleFile#>)");

            bool hasSectionForEachTable = HasSection(templateDomain.TemplateText,
                @"(?s)(?<=<#ForEachTable#>).*?(?=<#/ForEachTable#>)");

            bool hasSectionForEachColumn = HasSection(templateDomain.TemplateText,
                @"(?s)(?<=<#ForEachColumn#>).*?(?=<#/ForEachColumn#>)");

            if (hasSectionTemplateFileOut == false)
            {
                responseMsg = "Template does not have section TemplateFileOut unable to process";
            }

            if (hasSectionTemplateFileOut == false)
            {
                responseMsg = "Template does not have section SectionSingleFile unable to process";
            }

            if (hasSectionTemplateFileOut & hasSectionTemplateFileOut)
            {
                if (hasSectionForEachTable)
                {
                    if (templateDomain.SingleFile) //Write a single file
                    {
                        var tokenDictionary = TokenDictionary();

                        FileModel fileModel = new FileModel
                        {
                            TemplateId = templateDomain.Id,
                            FileInput = templateDomain.TemplateText,
                            FileOutput = templateDomain.TemplateText
                        };

                        Regex regex = new Regex(@"(?s)(?<=<#ForEachTable#>).*?(?=<#/ForEachTable#>)",
                            RegexOptions.Singleline);

                        MatchCollection matches = regex.Matches(templateDomain.TemplateText);

                        foreach (Match match in matches)
                        {
                            var listTables = _tableService.GetAll();
                            StringBuilder tableLines = new StringBuilder();

                            foreach (var table in listTables)
                            {
                                if (tokenDictionary.ContainsKey("TableName"))
                                {
                                    tokenDictionary.Remove("TableName");
                                }

                                tokenDictionary.Add("TableName", table.TableName);

                                tableLines.Append(ReplaceToken(match.Value, tokenDictionary));
                            }

                            string escapedString = Regex.Escape(match.Value);
                            fileModel.FileOutput = Regex.Replace(fileModel.FileOutput, escapedString,
                                tableLines.ToString());
                        }

                        WriteFile(fileModel, tokenDictionary);
                    }

                    else //Write a file for every table name
                    {
                        var listTables = _tableService.GetAll();

                        foreach (var table in listTables)
                        {
                            var tokenDictionary = TokenDictionary();

                            FileModel fileModel = new FileModel
                            {
                                TemplateId = templateDomain.Id,
                                FileInput = templateDomain.TemplateText,
                                FileOutput = templateDomain.TemplateText
                            };

                            if (tokenDictionary.ContainsKey("TableName"))
                            {
                                tokenDictionary.Remove("TableName");
                            }

                            tokenDictionary.Add("TableName", table.TableName);

                            fileModel.FileOutput = ReplaceToken(fileModel.FileOutput, tokenDictionary);

                            if (hasSectionForEachColumn) //Replace column section
                            {
                                fileModel.FileOutput = ReplaceColumns(tokenDictionary, fileModel, table).FileOutput;
                            }

                            WriteFile(fileModel, tokenDictionary);
                        }
                    }
                }
            }

            responseMsg = responseMsg +
                          "TemplateId=" + templateDomain.Id +
                          "?HasSectionTemplateFileOut=" + hasSectionTemplateFileOut +
                          "&HasSectionSingleFile=" + hasSectionSingleFile +
                          "&HasSectionForEachTable=" + hasSectionForEachTable +
                          "&HasSectionForEachColumn=" + hasSectionForEachColumn;

            return responseMsg;
        }

        /// <summary>
        /// Replace columns
        /// </summary>
        private FileModel ReplaceColumns(Dictionary<string, string> tokenDictionary, FileModel fileModel,
            TableDomain tableDomain)
        {
            var listColumns = _columnService.GetAllByTableId(tableDomain.Id);

            Regex regex = new Regex(@"(?s)(?<=<#ForEachColumn#>).*?(?=<#/ForEachColumn#>)", RegexOptions.Singleline);

            MatchCollection matches = regex.Matches(fileModel.FileOutput);

            foreach (Match match in matches)
            {
                StringBuilder columnLines = new StringBuilder();

                foreach (var column in listColumns)
                {
                    if (tokenDictionary.ContainsKey("ColumnName"))
                    {
                        tokenDictionary.Remove("ColumnName");
                    }

                    if (tokenDictionary.ContainsKey("ColumnType"))
                    {
                        tokenDictionary.Remove("ColumnType");
                    }

                    tokenDictionary.Add("ColumnType", _columnTypeService.ReadById(column.ColumnTypeId).ColumnType);

                    tokenDictionary.Add("ColumnName", column.ColumnName);

                    columnLines.Append(ReplaceToken(match.Value, tokenDictionary));
                }

                string escapedString = Regex.Escape(match.Value);
                fileModel.FileOutput = Regex.Replace(fileModel.FileOutput, escapedString, columnLines.ToString());
            }

            return fileModel;
        }

        /// <summary>
        /// Write file
        /// </summary>
        private void WriteFile(FileModel fileModel, Dictionary<string, string> tokenDictionary)
        {
            string templateOutDirectory = tokenDictionary["TemplateOutDirectory"];

            //Get file name
            fileModel.FileName = GetSection(fileModel.FileOutput,
                @"(?s)(?<=<#TemplateFileOut#>).*?(?=<#/TemplateFileOut#>)");

            //Remove file name
            fileModel.FileOutput = ReplaceValue(fileModel.FileOutput, fileModel.FileName, "");

            //Remove section single file
            fileModel.FileOutput = Regex.Replace(fileModel.FileOutput, @"<#SingleFile#>(.|\n)*?<#/SingleFile#>",
                string.Empty);

            //Remove white spaces from file name
            fileModel.FileName =
                Regex.Replace(templateOutDirectory + fileModel.FileName, @"^\s*$\n", string.Empty,
                    RegexOptions.Multiline).TrimEnd();

            //Replace empty tags
            fileModel.FileOutput = Regex.Replace(fileModel.FileOutput, @"<#(.|\n)*?#>", string.Empty);

            //Replace white spaces
            fileModel.FileOutput =
                Regex.Replace(fileModel.FileOutput, @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();

            _fileService.Create(new FileModel()
            {
                TemplateId = fileModel.TemplateId,
                FileInput = fileModel.FileInput,
                FileOutput = fileModel.FileOutput,
                FileName = fileModel.FileName.Replace(templateOutDirectory, "")
                //FileName = fileModel.FileName
            });

            File.WriteAllText(fileModel.FileName, fileModel.FileOutput);
            //File.WriteAllText(fileModel.FileName, string.Format("//Timestamp:{0}\r\n{1}", System.DateTime.Now, fileModel.FileOutput));
        }

        /// <summary>
        /// Safely replaces string and checks for null values
        /// </summary>
        private string ReplaceValue(string inValue, string oldValue, string newValue)
        {
            string replaceValue = null;

            if (inValue != null & oldValue != null & newValue != null)
            {
                replaceValue = inValue.Replace(oldValue, newValue);
            }

            return replaceValue;
        }

        /// <summary>
        /// Determines if a template has a section for given type
        /// </summary>
        private bool HasSection(string templateText, String pattern)
        {
            bool rValue = false;

            if (templateText == null)
            {
                return false;
            }

            Regex rx = new Regex(pattern, RegexOptions.Singleline);

            Match m = rx.Match(templateText);

            if (m.Success)
            {
                rValue = true;
            }

            return rValue;
        }

        /// <summary>
        /// Returns template section
        /// </summary>
        //http://msdn.microsoft.com/en-us/library/h5181w5w(v=vs.110).aspx
        private string GetSection(string templateText, String pattern)
        {
            if (templateText == null)
            {
                return null;
            }

            string section = null;

            Regex rx = new Regex(pattern, RegexOptions.Singleline);

            Match m = rx.Match(templateText);

            if (m.Success)
            {
                section = m.Value;
            }

            return section;
        }

        /// <summary>
        /// Returns token dictionary
        /// </summary>
        private Dictionary<string, string> TokenDictionary()
        {
            Dictionary<string, string> tokenDictionary = new Dictionary<string, string>();
            foreach (TokenDomain token in _tokenService.GetAll())
            {
                string tokenName = token.Token.Replace("${", "");
                tokenName = tokenName.Replace("}", "");
                tokenDictionary.Add(tokenName, token.TokenValue);
                //tokenDictionary.Add(token.Token, token.TokenValue);
            }

            return tokenDictionary;
        }

        /// <summary>
        /// Regular expression replaces token with value
        /// </summary>
        private string ReplaceToken(string replacementText, Dictionary<string, string> dictionary)
        {
            if (replacementText == null)
            {
                return null;
            }

            var rex = new Regex(_tokenService.ReadById(1).TokenValue); //Gets ${ExpressionReplaceToken} value

            return (rex.Replace(replacementText, delegate(Match m)
            {
                string key = m.Groups[1].Value;
                string rep = dictionary.ContainsKey(key) ? dictionary[key] : m.Value;
                return (rep);
            }));
        }

        #endregion

        #region MethodsCRUD

        /// <summary>
        /// Create - record in database.
        /// </summary>
        public TemplateModel Create(TemplateModel objectModel)
        {
            using (var db = new CodeGenContext())
            {
                //Domain object to model mapping make sure to change update and create methods
                _objectDomain.TemplateFileName = objectModel.TemplateFileName;
                _objectDomain.SingleFile = objectModel.SingleFile;
                _objectDomain.TemplateFileExtension = objectModel.TemplateFileExtension;
                _objectDomain.TemplateText = objectModel.TemplateText;
                //Domain object to model mapping make sure to change update and create methods

                _objectDomain.CreatedOn = System.DateTime.Now;

                db.Templates.Add(_objectDomain);

                db.SaveChanges();
                objectModel.Id = _objectDomain.Id;
            }

            return objectModel;
        }

        /// <summary>
        /// Create - datasource result record in database.
        /// </summary>
        public DataSourceResult CreateDataSourceResult(TemplateModel objectModel)
        {
            try
            {
                _dataSourceResult.Data = new[] {this.Create(objectModel)}.ToList();
                _dataSourceResult.PopNotificationMessage = "Create success!";
                _dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                _dataSourceResult.PopNotificationMessage = "Error! " + e;
                _dataSourceResult.PopNotificationMessageType = "error";
            }

            return _dataSourceResult;
        }

        /// <summary>
        /// Read - record from database.
        /// </summary>
        public TemplateDomain ReadById(int id)
        {
            using (var db = new CodeGenContext())
            {
                return db.Templates.Find(id);
            }
        }

        /// <summary>
        /// Determines if template has been loaded
        /// </summary>
        public int GetTemplateId(TemplateModel objectModel)
        {
            int id = 0;

            using (var db = new CodeGenContext())
            {
                var template = db.Templates
                    .Where(t => t.TemplateFileName == objectModel.TemplateFileName)
                    .FirstOrDefault();

                if (template != null)
                {
                    id = template.Id;
                }
            }

            return id;
        }

        public TemplateModel Update(TemplateModel objectModel)
        {
            using (var db = new CodeGenContext())
            {
                _objectDomain = db.Templates.Find(objectModel.Id);

                //Domain object to model mapping make sure to change update and create methods
                _objectDomain.TemplateFileName = objectModel.TemplateFileName;
                _objectDomain.SingleFile = objectModel.SingleFile;
                _objectDomain.TemplateFileExtension = objectModel.TemplateFileExtension;
                _objectDomain.TemplateText = objectModel.TemplateText;
                //Domain object to model mapping make sure to change update and create methods

                db.SaveChanges();
            }

            return objectModel;
        }

        /// <summary>
        /// Update - record in database.
        /// </summary>
        public DataSourceResult UpdateDataSourceResult(TemplateModel objectModel)
        {
            try
            {
                _dataSourceResult.Data = new[] {this.Update(objectModel)}.ToList();
                _dataSourceResult.PopNotificationMessage = "Update success!";
                _dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                _dataSourceResult.PopNotificationMessage = "Error! " + e;
                _dataSourceResult.PopNotificationMessageType = "error";
            }

            return _dataSourceResult;
        }

        /// <summary>
        /// Delete - record from database.
        /// </summary>
        public DataSourceResult Delete(TemplateModel objectModel)
        {
            try
            {
                using (var db = new CodeGenContext())
                {
                    _objectDomain = db.Templates.Find(objectModel.Id);
                    db.Templates.Remove(_objectDomain);
                    db.SaveChanges();
                }

                _dataSourceResult.PopNotificationMessage = "Delete success!";
                _dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                _dataSourceResult.PopNotificationMessage = "Error! " + e;
                _dataSourceResult.PopNotificationMessageType = "error";
            }

            return _dataSourceResult;
        }

        /// <summary>
        /// Reads grid records from database.
        /// </summary>
        public virtual DataSourceResult GetDataResult(int take, int skip, IEnumerable<Sort> sort,
            CodeGen.DynamicLinq.Filter filter)
        {
            try
            {
                using (var db = new CodeGenContext())
                {
                    //dataSourceResult = (from t in db.Templates
                    //                    orderby t.Id
                    //                    select t).ToDataSourceResult(take, skip, sort, filter);
                    _dataSourceResult = (from t in db.Templates.Select(n => new TemplateModel
                    {
                        Id = n.Id,
                        TemplateFileName = n.TemplateFileName,
                        TemplateFileExtension = n.TemplateFileExtension,
                        SingleFile = n.SingleFile,
                        TemplateText = n.TemplateText,
                        CreatedOn = n.CreatedOn
                    })
                        orderby t.Id
                        select t)
                        .ToDataSourceResult(take, skip, sort, filter);
                }

                _dataSourceResult.PopNotificationMessage = "Query success!";
                _dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                _dataSourceResult.PopNotificationMessage = "Error! " + e;
                _dataSourceResult.PopNotificationMessageType = "error";
            }

            return _dataSourceResult;
        }

        /// <summary>
        /// Reads all records from database.
        /// </summary>
        public virtual IList<TemplateDomain> GetAll()
        {
            using (var db = new CodeGenContext())
            {
                var query = from t in db.Templates
                    orderby t.Id
                    select t;

                return query.ToList();
            }
        }

        #endregion

    }

}