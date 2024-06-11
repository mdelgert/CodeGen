// ##################    2/23/2015 Backup and cleanup    ############################

/*

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
    public class TemplateServiceBackup : ITemplateService
    {
        private readonly IFileService _fileService = new FileService();
        private readonly ITableService _tableService = new TableService();
        private readonly ITokenService _tokenService = new TokenService();
        private readonly IColumnService _columnService = new ColumnService();
        private readonly IColumnTypeService _columnTypeService = new ColumnTypeService();
        private DataSourceResult _dataSourceResult = new DataSourceResult();
        private TemplateDomain _objectDomain = new TemplateDomain();
        //private FileModel _fileModel = new FileModel();

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
        /// Generates code files
        /// </summary>
        public string GenerateCode()
        {
            string msg = "";

            var listTemplates = this.GetAll();

            foreach (var template in listTemplates)
            {
                RunTemplate(template);
            }

            //try
            //{
            //    var listTemplates = this.GetAll();

            //    foreach (var template in listTemplates)
            //    {
            //        msg = "<br />" + Environment.NewLine + msg + RunTemplate(template);
            //    }

            //    //msg = "Success generating code!";
            //}
            //catch (Exception e)
            //{
            //    msg = "<br />" + Environment.NewLine + e.ToString();
            //}

            return msg;
        }

        /// <summary>
        /// Run template
        /// </summary>
        public string RunTemplate(TemplateDomain templateDomain)
        {
            string msg = "TEST";

            var tokenDictionary = TokenDictionary(); //Load the token dictionary

            FileModel fileModel = new FileModel
            {
                TemplateId = templateDomain.Id,
                FileInput = templateDomain.TemplateText
            };

            if (templateDomain.SingleFile)
            {
                //Write a single file for every table
                fileModel = ReplaceTables(tokenDictionary, templateDomain, fileModel);
                WriteFile(fileModel, templateDomain, tokenDictionary);
            }
            else
            {
                //Write a new file for every table
                ReplaceTables(tokenDictionary, templateDomain, fileModel);
            }

            return msg;
        }

        /// <summary>
        /// Replace tables
        /// </summary>
        private FileModel ReplaceTables(Dictionary<string, string> tokenDictionary, TemplateDomain templateDomain,
            FileModel fileModel)
        {
            var listTables = _tableService.GetAll();

            Regex regex = new Regex(@"(?s)(?<=<#ForEachTable#>).*?(?=<#/ForEachTable#>)", RegexOptions.Singleline);

            MatchCollection matches = regex.Matches(templateDomain.TemplateText);

            foreach (Match match in matches)
            {
                StringBuilder tableLines = new StringBuilder();

                foreach (var table in listTables)
                {
                    if (tokenDictionary.ContainsKey("TableName"))
                    {
                        tokenDictionary.Remove("TableName");
                    }

                    tokenDictionary.Add("TableName", table.TableName);

                    if (templateDomain.SingleFile)
                    {
                        tableLines.Append(ReplaceToken(match.Value, tokenDictionary));
                    }
                    else
                    {
                        fileModel.FileInput = templateDomain.TemplateText;


                        if (ReplaceColumns(tokenDictionary, templateDomain, fileModel, table).FileOutput != null)
                        {
                            fileModel.FileOutput =
                                ReplaceColumns(tokenDictionary, templateDomain, fileModel, table).FileOutput;
                        }
                        else
                        {
                            //fileModel.FileOutput = fileModel.FileInput;

                            string escapedString = Regex.Escape(match.Value);
                            fileModel.FileOutput = Regex.Replace(fileModel.FileInput, escapedString,
                                tableLines.ToString());
                            fileModel.FileInput = fileModel.FileOutput;
                        }


                        WriteFile(fileModel, templateDomain, tokenDictionary);
                    }
                }

                if (templateDomain.SingleFile & matches.Count != 0)
                {
                    string escapedString = Regex.Escape(match.Value);
                    fileModel.FileOutput = Regex.Replace(fileModel.FileInput, escapedString, tableLines.ToString());
                    fileModel.FileInput = fileModel.FileOutput;
                }
            }

            return fileModel;
        }

        /// <summary>
        /// Replace columns
        /// </summary>
        private FileModel ReplaceColumns(Dictionary<string, string> tokenDictionary, TemplateDomain templateDomain,
            FileModel fileModel, TableDomain tableDomain)
        {
            var listColumns = _columnService.GetAllByTableId(tableDomain.Id);

            Regex regex = new Regex(@"(?s)(?<=<#ForEachColumn#>).*?(?=<#/ForEachColumn#>)", RegexOptions.Singleline);

            MatchCollection matches = regex.Matches(fileModel.FileInput);

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


                if (matches.Count != 0)
                {
                    string escapedString = Regex.Escape(match.Value);
                    fileModel.FileOutput = Regex.Replace(fileModel.FileInput, escapedString, columnLines.ToString());
                    fileModel.FileInput = fileModel.FileOutput;
                }
            }

            return fileModel;
        }

        /// <summary>
        /// Write file
        /// </summary>
        private void WriteFile(FileModel fileModel, TemplateDomain templateDomain,
            Dictionary<string, string> tokenDictionary)
        {
            string templateOutDirectory = tokenDictionary["TemplateOutDirectory"];

            //Replace tokens
            fileModel.FileOutput = ReplaceToken(fileModel.FileOutput, tokenDictionary);

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
                FileName = fileModel.FileName
            });

            //fileModel.FileOutput = fileModel.FileOutput + "//Timestamp:" + System.DateTime.Now;

            //File.WriteAllText(fileModel.FileName, fileModel.FileOutput);

            File.WriteAllText(fileModel.FileName,
                string.Format("//Timestamp:{0}\r\n{1}", System.DateTime.Now, fileModel.FileOutput));
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


        ///// <summary>
        ///// Returns template section
        ///// </summary>
        ////http://msdn.microsoft.com/en-us/library/h5181w5w(v=vs.110).aspx
        //private string GetSection(string templateText, String pattern)
        //{
        //    string section = null;

        //    Regex rx = new Regex(pattern, RegexOptions.Singleline);

        //    Match m = rx.Match(templateText);

        //    if (m.Success)
        //    {
        //        section = m.Value;
        //    }

        //    return section;
        //}

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


        ///// <summary>
        ///// Regular expression replaces token with value
        ///// </summary>
        //private string ReplaceToken(string replacementText, Dictionary<string, string> dictionary)
        //{
        //    var rex = new Regex(_tokenService.ReadById(1).TokenValue); //Gets ${ExpressionReplaceToken} value

        //    return (rex.Replace(replacementText, delegate(Match m)
        //    {
        //        string key = m.Groups[1].Value;
        //        string rep = dictionary.ContainsKey(key) ? dictionary[key] : m.Value;
        //        return (rep);
        //    }));
        //}

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

#region Notes

//    //int line = m.Index;
//else
//{
//    //section = null;
//}

//Regex rx = new Regex(pattern);

//Regex rx = new Regex(pattern, RegexOptions.Multiline);

//{TABLE.COLUMNS(?<selection> (ALL|PRIMARY|NOPRIMARY))?}(?<column>.+?){/TABLE.COLUMNS}

//(?s)(?<={TemplateFileOut}).+(?={\/TemplateFileOut})


//ReplaceTables(templateDomain);

///// <summary>
///// Replace tables
///// </summary>
//private void ReplaceTables(TemplateDomain templateDomain)
//{
//    var tokenDictionary = this.TokenDictionary(); //Load the token dictionary


//    var listTables = _tableService.GetAll();

//    Regex regex = new Regex(@"(?s)(?<=<#ForEachTable#>).*?(?=<#/ForEachTable#>)", RegexOptions.Singleline);

//    MatchCollection matches = regex.Matches(templateDomain.TemplateText);

//    foreach (Match match in matches)
//    {


//        foreach (var table in listTables)
//        {
//            if (tokenDictionary.ContainsKey("TableName"))
//            {
//                tokenDictionary.Remove("TableName");
//            }

//            tokenDictionary.Add("TableName", table.TableName);

//            FileModel fileModel = new FileModel();

//            fileModel.TemplateId = templateDomain.Id;

//            fileModel.FileInput = templateDomain.TemplateText;

//            fileModel.FileOutput = ReplaceColumns(table, tokenDictionary, fileModel).FileOutput;


//            WriteFile(fileModel, templateDomain, tokenDictionary);
//        }
//    }
//}


// StringBuilder tableLines = new StringBuilder();

//if (!templateDomain.SingleFile)
//{

//}
//else
//{
//    tableLines.Append(ReplaceToken(match.Value, tokenDictionary));
//}

//if (!templateDomain.SingleFile)
//{
//    string escapedString = Regex.Escape(match.Value);
//    fileModel.FileOutput = Regex.Replace(fileModel.FileInput, escapedString, tableLines.ToString());
//    fileModel.FileInput = fileModel.FileOutput;
//}

//MatchCollection matches = regex.Matches(fileModel.FileInput);


//http://stackoverflow.com/questions/787932/using-c-sharp-regular-expressions-to-remove-html-tags
//input = Regex.Replace(input, "<style>(.|\n)*?</style>",string.Empty);
//input = Regex.Replace(input, @"<xml>(.|\n)*?</xml>", string.Empty); // remove all <xml></xml> tags and anything inbetween.  
//return Regex.Replace(input, @"<(.|\n)*?>", string.Empty); // remove any tags but not there content "<p>bob<span> johnson</span></p>" becomes "bob johnson"

//Dictionary<string, string> tokenDictionary = new Dictionary<string, string>();

//http://msdn.microsoft.com/en-us/library/system.io.path.getextension(v=vs.110).aspx
//How to: Read Text from a File - http://msdn.microsoft.com/en-us/library/db5x7c0d(v=vs.110).aspx

//templateModel.TemplateFileName = file.Replace(tokenDictionary["TemplateInDirectory"], "");

//_fileModel.FileOutput = ReplaceTables(templateDomain, tokenDictionary, _fileModel).FileOutput; //Replace tables


//Regex regex = new Regex(@"(?s)(?<={ForEachColumn}).*?(?={\/ForEachColumn})", RegexOptions.Singleline);


//Regex regex = new Regex(@"(?s)(?<={ForEachColumn}).*?(?={\/ForEachColumn})", RegexOptions.Singleline);


//http://stackoverflow.com/questions/8869134/finding-text-between-tags-and-replacing-it-along-with-the-tags
//Regex regex = new Regex(@"\{ForEachColumn\}(.*?)\{/ForEachColumn}", RegexOptions.Singleline);

//(?s)(?<={ForEachColumn}).+(?={\/ForEachColumn})
//\${([^}]+)}

//string pattern = @"(?s)(?<={ForEachColumn})(.|\n)*?(?={\/ForEachColumn})";


//MatchCollection matches = regex.Matches(fileModel.FileOutput);


//_fileModel.FileOutput = ReplaceColumns(table, tokenDictionary, _fileModel);

//Dictionary<string, string> tokenDictionary = new Dictionary<string, string>();

//string templateText = ReplaceToken(template.TemplateText, tokenDictionary);


//string sectionTable = GetSection(templateText, tokenDictionary["ExpressionSectionTable"]);

//fileText = ReplaceValue(fileText, @"{ForEachColumn}", "");
//fileText = ReplaceValue(fileText, @"{/ForEachColumn}", "");

//if (sectionColumn != null)
//{
//    //fileText = ReplaceValue(fileText, sectionColumn, columnLines.ToString());
//    //fileText = ReplaceValue(fileText, @"{ForEachColumn}", "");
//    //fileText = ReplaceValue(fileText, @"{/ForEachColumn}", "");

//private string GetMultiSection(string templateText, String pattern)
//{
//    Regex regex = new Regex(pattern,RegexOptions.Singleline);
//    MatchCollection matches = regex.Matches(templateText);			
//    foreach(Match match in matches)
//    {

//    }

//    return null;

//}

//fileText = fileText.Replace(sectionColumn, columnLines.ToString());


//How to delete an object without retrieving it - http://blogs.msdn.com/b/alexj/archive/2009/03/27/tip-9-deleting-an-object-without-retrieving-it.aspx
//CRUD using Entity Framework in .NET Framework 5.0 - (Delete) Print Print Email Email - https://support.microsoft.com/kb/2802240/en-us?wa=wsignin1.0

#endregion


*/