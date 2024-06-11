var dataSource = new kendo.data.DataSource({

        serverPaging: true,

        serverFiltering : true,

        serverSorting : true,

		//sort: {
		//    field: "Id",
		//    dir: "desc"
		//},


        sort: {
            field: "Token",
            dir: "asc"
        },

		pageSize : 10,

       requestEnd: function (e) {
	    	PopNotify('SectionNotifyGridToken', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);
	    	if (e.response.PopNotificationMessageType === "error") {
	    		this.cancelChanges();
	    	}
	    },

		schema : {
			data : "Data",
			total : "Total",
			PopNotificationMessage : "PopNotificationMessage",
			PopNotificationMessageType : "PopNotificationMessageType", //Type of notification
			model : {
				id : "Id",
				fields : {

					Id : {
						editable : false,
						type : "number"
					},
					
					Token : {
					    type: "string",
					    defaultValue: "${YourTokenName}",
						validation : {
							required : true
						}
					},
					
					TokenValue : {
						type : "string",
						validation : {
							required : true
						}
					},

					TokenNote: {
					    type: "string"
					},

					IsSystemToken: {
					    type: "boolean",
					    defaultValue: false,
					},

				}

			}

		},

		batch: false,

		transport : { //use HTTP POST request as by default GET is not allowed by ASP.NET MVC

			create : {
				url : "/Token/Create",
				contentType : "application/json",
				type : "POST"

			},

			read : {
				url : "/Token/Read",
				contentType : "application/json",
				type : "POST"

			},

			update : {
				url : "/Token/Update",
				contentType : "application/json",
				type : "POST"
			},

			destroy : {
				url : "/Token/Destroy",
				contentType : "application/json",
				type : "POST"
			},

			parameterMap : function (data, operation) {
				return JSON.stringify(data);
			}

		}

	});


//var grid = $("#TokenGrid").kendoGrid({

$("#TokenGrid").kendoGrid({

    dataSource : dataSource,
    columns: [

        //{
        //	field : "Id",
        //	width : "100px"
        //}

        {
            field: "Token",
            title: "Token:",
            width : "100px"
        },

        //{
        //    field: "TokenValue",
        //    title: "Value:",
        //    width : "100px"
        //},

        {
            field: "TokenNote",
            title: "Note:",
            width: "100px"
        },

        {
            field: "IsSystemToken",
            title: "System Token:",
            width: "60px"
        },


        {
            command : ["edit", "destroy"],
            //title : "&nbsp;",
            title: "Commands:",
            width : "90px"
        }


        //{ command: ["edit", { text: "Delete", imageClass: "k-icon k-i-custom ob-icon-only", click: DeleteToken }], title: "&nbsp", width: "180px" }


        //{ command: ["edit", { text: "Delete", imageClass: "", click: DeleteToken }], title: "&nbsp", width: "180px" }


        //http://stackoverflow.com/questions/5003867/how-to-call-javascript-function-instead-of-href-in-html
        //http://jsfiddle.net/FcWBQ/
        //http://www.telerik.com/forums/how-to-conditionally-format-a-grid-cell
        //http://www.telerik.com/forums/styling-individual-cells-of-grid-with-a-template
		//http://www.telerik.com/forums/kendo-ui-style-grid-rows
		
		
		
        //{ field: "IsSystemToken", title: "Delete", template: "<a class='k-icon k-i-custom ob-icon-only' href='javascript:void(0);' onclick='DeleteToken();'>Delete</a>", width: "180px" }




        //{
        //    field: "IsSystemToken", title: "Delete", template:
        //      "<a class='k-button k-button-icontext k-grid-Delete' href='javascript:void(0);' onclick='DeleteToken();'><span class='k-icon k-i-custom ob-icon-only'></span>Delete</a>"
        //    , width: "180px"
        //}





        ],

		//editable : {
		//	mode : "popup",
		//	confirmation : false,
		//},


        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },

		//editable: true,

		pageable: true,

        sortable: true,

		filterable: true,

		//toolbar: ["create", "save", "cancel"]

		toolbar: ["create"]

        ,

		//dataBinding: function (e) {
		//    dataBound(e);
		//}

        //http://stackoverflow.com/questions/20491191/applying-k-state-disabled-class-to-text-inputs-kendo-ui
        //http://www.telerik.com/forums/how-do-i-conditional-set-the-visibility-of-the-command-edit-button-on-a-row-by-row-basis-
		dataBound: function (e) {
		    //console.log("dataBound");

		    //Selects all delete buttons
		    $("#TokenGrid tbody tr .k-grid-delete").each(function () {
		        var currentDataItem = $("#TokenGrid").data("kendoGrid").dataItem($(this).closest("tr"));

		        //Check in the current dataItem if the row is deletable
		        if (currentDataItem.IsSystemToken == true) {
		            //$(this).remove();
		            //$(this).addClass('k-state-disabled');
		            $(this).addClass('red');
		            $(this).click(DeleteToken);
		        }
		    })


		}



	}).data("kendoGrid");


//http://jsfiddle.net/FcWBQ/

function DeleteToken(e) {

    
    alert('System tokens can not be deleted.');

}



function dataBound(e) {


    
    ////$('.k-grid-delete').hide();


    var grid = $("#TokenGrid").data("kendoGrid");
    var gridData = grid.dataSource.view();

    for (var i = 0; i < gridData.length; i++) {

        var currentUid = gridData[i].uid;

        //alert(gridData[i].Flex);

        //alert(gridData[i].IsSystemToken);
        
        if (gridData[i].IsSystemToken === true) {

            //var currentRow = grid.table.find("tr[data-uid='" + currentUid + "']");

            //http://www.telerik.com/forums/kendo-ui-style-grid-rows
            $("#TokenGrid tbody").find("tr[data-uid=" + currentUid + "]").addClass("Red");

            //currentRow.addClass('Red');

            //var tr = grid.table.find("tr[data-uid='" + currentUid + "']");

            //var cell = $("td:nth-child(2)", tr);

            //cell.addClass("style=background-color: Red;");

            //grid.removeRow(grid.tbody.find("tr")[i]);

            //var createUserButton = $(currentRow).find(".k-grid-delete");

            //$(currentRow).find(".k-button .k-button-icontext .k-grid-delete").hide();

            //<a class="k-button k-button-icontext k-grid-delete" href="#"><span class="k-icon k-delete"></span>Delete</a>

            //createUserButton.hide();

            //createUserButton.remove();

            //alert(currentUid);

            //$(currentRow).parent().parent().children('td > label').hide();

        }



    }

}


/*

//http://www.telerik.com/forums/hide-edit-and-delete-button-based-on-the-status-of-each-record

function onDataBound(e) {
    ////Selects all edit buttons

    //alert('test');
    ////currentDataItem.IsSystemToken === true

    ////Selects all delete buttons
    //$("#TokenGrid tbody tr .k-grid-delete").each(function () {
    //    var currentDataItem = $("#TokenGrid").data("kendoGrid").dataItem($(this).closest("tr"));

    //    alert(currentDataItem.IsSystemToken);

    //    //Check in the current dataItem if the row is deletable
    //    if (currentDataItem.IsSystemToken == true) {
    //        $(this).remove();
    //    }
    //})

    //alert('test');

    //alert(e.IsSystemToken);

    //if (e.IsSystemToken == true) {
    //    $(this).remove();
    //}

    //http://www.telerik.com/forums/conditional-custom-command-display-in-kendo-ui-grid-

    var html = 'test';
    document.getElementById("debuText").innerHTML = html;

    //http://www.telerik.com/forums/conditional-custom-command-display-in-kendo-ui-grid-

    var grid = $("#TokenGrid").data("kendoGrid");
    var gridData = grid.dataSource.view();
    for (var i = 0; i < gridData.length; i++) {
        
        var currentUid = gridData[i].uid;

        alert(gridData[i].IsSystemToken);
        
        //if (gridData[i].IsSystemToken = true) {
            
        //    //var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
        //    //var editButton = $(currenRow).find("k-grid-custom");
                    
        //    //alert(gridData[i].Flex);                  
        //    //editButton.hide();
        
        //}

    }}



    //var Kgrid = $("#TokenGrid").data("kendoGrid");
    //var gridData = Kgrid.dataSource.view();

    //for (var i = 0; i < gridData.length; i++) {

    //    var currentUid = gridData[i].uid;

    //    //alert(gridData[i].IsSystemToken);

    //    if (gridData[i].IsSystemToken === true) {


    //        alert('hide this');

    //        //var editButton = $(currenRow).find(".k-grid-delete");

    //        //var obj = $("#TokenGrid tbody").find("tr[data-uid=" + dataView[i].uid + "]");

    //        //var currentRow = Kgrid.table.find("tr[data-uid='" + currentUid + "']");

    //        //var deleteBtn = $(currentRow).find(".k-grid-delete");



    //        //deleteBtn.hide();
    //        //deleteBtn.remove();

    //        //html = html + deleteBtn;

    //    }

    //}

    


}

*/