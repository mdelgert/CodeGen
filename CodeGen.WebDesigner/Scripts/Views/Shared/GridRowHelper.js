//KendoUI grid reorder function

function MoveGridRow(Id, Direction, UpdateUri, GridName) {

       var vgrid = $("#" + GridName).data("kendoGrid");

        $.ajax({ //Post new new OrderId to server and save.
            type: 'POST',
            url: UpdateUri + '?id=' + Id + "&direction=" + Direction,
            contentType: "application/json",
            async: false,
            success: function () {
                //alert('success');
                vgrid.dataSource.read();
                vgrid.refresh();
            },
        });


}

/*

//Usage:


//1. Add script to page

Example 1: 

<script type="text/javascript" src="@Url.Content("/Scripts/Views/Shared/GridRowHelper.js")"></script>

//2. Add template command to grid in columns section

Example 2:

            {
                title: "&nbsp;", width: "100px", template: '<div onclick="MoveGridRow(\'#=Id#\', \'Up\', \'/Column/ChangeOrder\', \'CodeGenColumns\')" class="k-button"><span class="k-icon k-si-arrow-n"></span></div>' +
                                                           '<div onclick="MoveGridRow(\'#=Id#\', \'Down\', \'/Column/ChangeOrder\', \'CodeGenColumns\')" class="k-button"><span class="k-icon k-si-arrow-s"></span></div>'
            }

//3. Add OrderBy Id to DataSourceResult result

Example 3:

/// <summary>
/// Reads grid records from database.
/// </summary>
public virtual DataSourceResult GetDataResult(int take, int skip, IEnumerable<Sort> sort, CodeGen.DynamicLinq.Filter filter)
{
	try
	{
		using (var db = new CodeGenContext())
		{
		dataSourceResult = (from gp in db.Tokens
		orderby gp.OrderId // <--------------------------------
		select gp).ToDataSourceResult(take, skip, sort, filter);
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

4. Create ChangeOrder method

Example 4.

        public void ChangeOrder(int id, string direction)
        {
            ColumnDomain columnSelected = new ColumnDomain();
            columnSelected = this.ReadById(id);
            int newOrderID = 0;
            var items = GetAllByTableId(columnSelected.TableId);

            if (direction == "Up")
            {
                if (columnSelected.OrderId == 0)
                {
                    return; //Do nothing at the lowest level
                }
                else
                {
                    newOrderID = columnSelected.OrderId - 1;
                }
            }

            if (direction == "Down")
            {
                if (columnSelected.OrderId == items.Count - 1)
                {
                    return; //Do nothing at the highest level
                }
                else
                {
                    newOrderID = columnSelected.OrderId + 1;
                }
            }
            
            foreach (ColumnDomain columnDomain in items)
            {
                if (columnDomain.OrderId == newOrderID)
                {
                    using (var db = new CodeGenContext())
                    {
                        objectDomain = db.Columns.Find(columnDomain.Id);
                        objectDomain.OrderId = columnSelected.OrderId; //switch id with original
                        db.SaveChanges();
                    }
                }
            }

            using (var db = new CodeGenContext())
            {
                objectDomain = db.Columns.Find(columnSelected.Id);
                objectDomain.OrderId = newOrderID; //update the id
                db.SaveChanges();
            }

        }


//Notes:
//alert(uid);alert(Direction);alert(GridName);alert(UpdateUri);
http://stackoverflow.com/questions/16471046/how-to-refresh-the-kendo-ui-grid
http://stackoverflow.com/questions/5320802/post-json-data-to-asmx-webservice
*/

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
* File: GridRowHelper.js
*
* Authored date: 1/3/2015
* 
* Modified date: 1/3/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */