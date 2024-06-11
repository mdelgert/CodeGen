using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.Core.Domain;
using CodeGen.Core.Model;
using CodeGen.DynamicLinq;
using CodeGen.Core.Data;

namespace CodeGen.Core.Services
{
    public class TokenService : ITokenService
    {
        DataSourceResult dataSourceResult = new DataSourceResult();
        TokenDomain objectDomain = new TokenDomain();

        #region PublicMethodsCRUD
        
        /// <summary>
        /// Create - record in database.
        /// </summary>
        public DataSourceResult Create(TokenModel objectModel)
        {
            objectModel.Id = this.GetTokenId(objectModel);

            if (objectModel.Id != 0)
            {
                throw new System.ArgumentException("Duplicate tokens can not be created. Token Service.");
            }
            
            try
            { 
                using (var db = new CodeGenContext())
                {
                    //Domain object to model mapping make sure to change update and create methods
                    objectDomain.Token = objectModel.Token;
                    objectDomain.TokenValue = objectModel.TokenValue;
                    objectDomain.TokenNote = objectModel.TokenNote;
                    objectDomain.IsSystemToken = objectModel.IsSystemToken;
                    objectDomain.CreatedOn = System.DateTime.Now;
                    //Domain object to model mapping make sure to change update and create methods

                    db.Tokens.Add(objectDomain);
                    db.SaveChanges();
                }

                dataSourceResult.Data = new[] { objectDomain }.ToList();
                dataSourceResult.PopNotificationMessage = "Create success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Read - record from database.
        /// </summary>
        public TokenDomain ReadById(int id)
        { 
            using (var db = new CodeGenContext())
            {
                return db.Tokens.Find(id);
            }
        }
        
        /// <summary>
        /// Update - record in database.
        /// </summary>
        public DataSourceResult Update(TokenModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.Tokens.Find(objectModel.Id);

                    //Domain object to model mapping make sure to change update and create methods
                    objectDomain.Token = objectModel.Token;
                    objectDomain.TokenValue = objectModel.TokenValue;
                    objectDomain.TokenNote = objectModel.TokenNote;
                    objectDomain.IsSystemToken = objectModel.IsSystemToken;
                    //Domain object to model mapping make sure to change update and create methods
                    
                    db.SaveChanges();
                }
                
                dataSourceResult.Data = new[] { objectDomain }.ToList();
                dataSourceResult.PopNotificationMessage = "Update success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Delete - record from database.
        /// </summary>
        public DataSourceResult Delete(TokenModel objectModel)
        { 
            try
            {
                using (var db = new CodeGenContext())
                {
                    objectDomain = db.Tokens.Find(objectModel.Id);

                    if (objectDomain.IsSystemToken)
                    {
                        throw new System.ArgumentException("System tokens can not be deleted.", "Token Service.");
                    }
                    db.Tokens.Remove(objectDomain);
                    db.SaveChanges();
                }

                dataSourceResult.PopNotificationMessage = "Delete success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Reads grid records from database.
        /// </summary>
        public virtual DataSourceResult GetDataResult(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter)
        {
            try
            {
                using (var db = new CodeGenContext())
                {
                    dataSourceResult = (from t in db.Tokens
                                        orderby t.Id
                                        select t).ToDataSourceResult(take, skip, sort, filter);
                }
                
                dataSourceResult.PopNotificationMessage = "Query success!";
                dataSourceResult.PopNotificationMessageType = "info";
            }
            catch (Exception e)
            {
                dataSourceResult.PopNotificationMessage = "Error! " + e;
                dataSourceResult.PopNotificationMessageType = "error";
            }

            return dataSourceResult;
        }
        
        /// <summary>
        /// Reads all records from database.
        /// </summary>
        public IList<TokenDomain> GetAll()
        {
            using (var db = new CodeGenContext())
            {
                return (from t in db.Tokens
                        orderby t.Id
                        select t).ToList();
            }
        }
        
        #endregion
        
        #region PrivateMethods

        /// <summary>
        /// Determines if token was created
        /// </summary>
        private int GetTokenId(TokenModel objectModel)
        {
            int id = 0;

            using (var db = new CodeGenContext())
            {
                var tokenDomain = db.Tokens
                                    .Where(t => t.Token == objectModel.Token)
                                    .FirstOrDefault();

                if (tokenDomain != null)
                {
                    id = tokenDomain.Id;
                }
            }

            return id;
        }

        #endregion

        #region SystemTokens

        public void CreateSystemTokens()
        {
            string systemMessage = "System Tokens can not be deleted.";
            this.Create(new TokenModel() { Token = "${ExpressionReplaceToken}", TokenValue = @"\${([^}]+)}", IsSystemToken = true, TokenNote = systemMessage, }); //Must be Id=1 (TemplateService.ReplaceToken) method requires this Id
            this.Create(new TokenModel() { Token = "${ExpressionSectionTemplate}", TokenValue = @"(?s)(?<={Template}).+(?={\/Template})", IsSystemToken = true, TokenNote = systemMessage, });
            this.Create(new TokenModel() { Token = "${ExpressionSectionTemplateFileOut}", TokenValue = @"(?s)(?<={TemplateFileOut}).+(?={\/TemplateFileOut})", IsSystemToken = true, TokenNote = systemMessage, });
            this.Create(new TokenModel() { Token = "${ExpressionSectionTable}", TokenValue = @"(?s)(?<={ForEachTable}).+(?={\/ForEachTable})", IsSystemToken = true, TokenNote = systemMessage, });
            this.Create(new TokenModel() { Token = "${ExpressionSectionColumn}", TokenValue = @"(?s)(?<={ForEachColumn}).+(?={\/ForEachColumn})", IsSystemToken = true, TokenNote = systemMessage, });
            this.Create(new TokenModel() { Token = "${TemplateInDirectory}", TokenValue = @"C:\Source\Git\VESSEA\CodeGen\CodeGen.WebDesigner\bin\Templates\In\", IsSystemToken = true, TokenNote = systemMessage });
            this.Create(new TokenModel() { Token = "${TemplateOutDirectory}", TokenValue = @"C:\Source\Git\VESSEA\CodeGen\CodeGen.WebDesigner\bin\Templates\Out\", IsSystemToken = true, TokenNote = systemMessage });
        }
        
        public void CreateSampleTokens()
        {
            this.Create(new TokenModel() { Token = "${Application}", TokenValue = "CodeGen", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${Version}", TokenValue = "1.0", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${Author}", TokenValue = "Matthew Elgert", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${Company}", TokenValue = "VESSEA, LLC.", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${ClassHeader}", TokenValue = "//Class header", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${ClassFooter}", TokenValue = "//Class footer", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${ClassSummary}", TokenValue = "//Class summary", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${FieldSummary}", TokenValue = "//Field summary", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${Project}", TokenValue = "CodeGen", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${ProjectNameSpace}", TokenValue = "CodeGen.Core", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${WebProjectNameSpace}", TokenValue = "CodeGen.WebDesigner", IsSystemToken = false });
            this.Create(new TokenModel() { Token = "${EntityContext}", TokenValue = "CodeGenContext", IsSystemToken = false });
            this.Create(new TokenModel()
            {
                Token = "${CopyWrite}",
                IsSystemToken = false,
                TokenValue = @"
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
* Authored date: 1/3/2015
* 
* Modified date: 1/3/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */"
            });
        }
        
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