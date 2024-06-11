using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CodeGen.ConsoleRegex
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.WriteLine(ExampleOne());

            GetColumnSection(ExampleOne());

            Console.ReadKey();
        }


        //Returns template section
        public static void GetColumnSection(string templateText)
        {
            //Does not work with multisections
            //Regex regex = new Regex(@"(?s)(?<={ForEachColumn}).+(?={\/ForEachColumn})",RegexOptions.Singleline);

            //Get XML Tags
            //http://www.regexlib.com/REDetails.aspx?regexp_id=433
            //Regex regex = new Regex(@"<(\w+?).*?(/>|>.*?</\1>)", RegexOptions.Singleline);

            //http://stackoverflow.com/questions/801575/regular-expression-to-get-all-the-value-between-custom-tag
            //Regex regex = new Regex(@"<ForEachColumn[^>]*>(.*?)</ForEachColumn>", RegexOptions.Singleline);

            //Regex regex = new Regex(@"{ForEachColumn[^>]*}(.*?){/ForEachColumn}", RegexOptions.Singleline);

            //Regex regex = new Regex(@"(?s)(?<={ForEachTable}).*?(?={\/ForEachTable})", RegexOptions.Singleline);

            Regex regex = new Regex(@"(?s)(?<={ForEachColumn}).*?(?={\/ForEachColumn})", RegexOptions.Singleline);

            MatchCollection matches = regex.Matches(templateText);

            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value.ToString());
            }
        }

        public static string ExampleOne()
        {
            string rValue = @"

{ForEachTable} 

{ForEachColumn}
ColumnSectionOne
{/ForEachColumn}

{ForEachColumn}
ColumnSectionTwo
{/ForEachColumn}

{/ForEachTable}

";

            return rValue;
        }


                public static string ExampleTwo()
        {
            string rValue = @"

<ForEachTable>XXXX

<ForEachColumn>
ColumnSectionOne
</ForEachColumn>

<ForEachColumn>
ColumnSectionTwo
</ForEachColumn>

</ForEachTable>

";

            return rValue;
        }

    }
}

#region Notes

//{TABLE.COLUMNS(?<selection> (ALL|PRIMARY|NOPRIMARY))?}(?<column>.+?){/TABLE.COLUMNS}

//(?s)(?<={TemplateFileOut}).+(?={\/TemplateFileOut})


//http://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex(v=vs.110).aspx
//http://www.rexegg.com/regex-csharp.html
//http://www.rexegg.com/regex-best-trick.html
//http://msdn.microsoft.com/en-us/library/xwewhkd1(v=vs.110).aspx
//http://msdn.microsoft.com/en-us/library/ewy2t5e0(v=vs.110).aspx
//http://msdn.microsoft.com/en-us/library/e7f5w83z(v=vs.110).aspx
//http://msdn.microsoft.com/en-us/library/hs600312(v=vs.110).aspx
//http://msdn.microsoft.com/en-us/library/t9e807fx(v=vs.110).aspx
//http://www.regexlib.com/DisplayPatterns.aspx?cattabindex=3&categoryId=4
//http://regexlab.codeplex.com/
//C#: token replacement using regex - http://blogs.msdn.com/b/akirsman/archive/2011/09/13/c-token-replacement-using-regex.aspx
//http://www.tutorialspoint.com/csharp/csharp_regular_expressions.htm
//http://www.dotnetperls.com/regex-match
//http://msdn.microsoft.com/en-us/library/ms228595.aspx

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
* Authored date: 1/19/2015
* 
* Modified date: 1/19/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */