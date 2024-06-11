//Deprecated see GridRowHelper.js

function MoveGridRow(uid, Direction, GridName, UpdateUri) {

	var newIndex;

	var vgrid = $("#" + GridName).data("kendoGrid");

	var dataItem = vgrid.dataSource.getByUid(uid);

    //
	//dataItem.dirty = false;

	var index = vgrid.dataSource.indexOf(dataItem);

	if (Direction === 'Up') {
	    newIndex = Math.max(0, index - 1);
	}

	if (Direction === 'Down') {
	    newIndex = Math.min(vgrid.dataSource.total() - 1, index + 1);
	}

	if (newIndex !== index) {

		//Remove the old record.
	    vgrid.dataSource.remove(dataItem);

		//Insert the new record.
	    vgrid.dataSource.insert(newIndex, dataItem);

	    var items = vgrid.dataSource.data();

		//Loop through grid and update OrderId.
		for (i = 0; i < items.length; i++) {

			var item = items[i]; //Get all items in grid.

			item.OrderId = i; //Set the new OrderId.

			$.ajax({ //Post new new OrderId to server and save.
				type : 'POST',
				url : UpdateUri,
				data : JSON.stringify(item),
				contentType : "application/json"
			});

		}


	}


	return false;

}


/*

//Usage:


//1. Add script to page

Example 1: 

<script type="text/javascript" src="@Url.Content("/Scripts/Views/Shared/GridRowHelper.js")"></script>

//2. Add template command to grid in columns section

Example 2:

{title: "Row Order", width: "100px", template: '<div onclick="return MoveGridRow(\'#=uid#\', \'Up\', \'grid\', \'/Token/Update\')" class="k-button"><span class="k-icon k-si-arrow-n"></span></div>' + 
                                               '<div onclick="return MoveGridRow(\'#=uid#\', \'Down\', \'grid\', \'/Token/Update\')" class="k-button"><span class="k-icon k-si-arrow-s"></span></div>'}],

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
		orderby gp.OrderId
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
 */

/*

//Notes:
//alert(uid);alert(Direction);alert(GridName);alert(UpdateUri);
http://stackoverflow.com/questions/16471046/how-to-refresh-the-kendo-ui-grid
http://stackoverflow.com/questions/5320802/post-json-data-to-asmx-webservice
http://www.telerik.com/forums/manually-set-grid-row-as-dirty
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
* Authored date: 1/1/2015
* 
* Modified date: 1/1/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */