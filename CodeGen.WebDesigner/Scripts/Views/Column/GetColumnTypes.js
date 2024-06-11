//get column types and rebind the grid


var columnTypes = [];

//get categories and rebind the grid
$.getJSON("/ColumnType/GetAll",
    function (data) {
    columnTypes = data;
    dataSource.fetch();
});

function columnTypeDropDownEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
      .appendTo(container)
      .kendoDropDownList({
          dataTextField: "ColumnType",
          dataValueField: "Id",
          dataSource: columnTypes
      });
}

function getColumnTypeName(ColumnTypeId) {
    for (var idx = 0, length = columnTypes.length; idx < length; idx++) {
        if (columnTypes[idx].Id === ColumnTypeId) {
            return columnTypes[idx].ColumnType;
        }
    }
}


/*

////get categories and rebind the grid
//$.getJSON("/ColumnType/GetAll",
//    function (data) {

//        columnTypes = data;

//        ColumnDataSource.fetch();

//    });

<div id="debug"></div>

var columntypes = [];

var html = ' ';

$.ajax({ //Post new new OrderId to server and save.
    type: 'POST',
    url: "/ColumnType/GetAll",
    contentType: "application/json",
    async: false,
    success: function (data) {


        var arrayLength = data.length;

        for (var i = 0; i < arrayLength; i++) {

            var obj = data[i];

            

            html = html + ' ' + obj.ColumnType;

        }

        document.getElementById("debug").innerHTML = html;

    },

});

*/


//html = html + obj.Id;


//$.getJSON("/ColumnType/GetAll",

//    function (data) {

//        columntypes = data;
//        //dataSource.fetch();

//    var arrayLength = columntypes.length;

//    for (var i = 0; i < arrayLength; i++) {

//        //alert(myStringArray[i]);

//        html = html + columntypes[i];
//        //Do something
//    }

//    document.getElementById("debug").innerHTML = html;


//    });






//for(var item in columntypes)
//{
//    //Do something

//}


//alert('success');
//document.getElementById("debug").innerHTML = "success";


//http://stackoverflow.com/questions/5320802/post-json-data-to-asmx-webservice

//http://stackoverflow.com/questions/18238173/javascript-loop-through-json-array


//Kendoui dropdown json - http://jsbin.com/obufum/5/edit?html,css,js,output, http://www.telerik.com/forums/adding-dropdown-list-to-a-grid-column

//Kendoui dropdown inline - http://jsfiddle.net/Eh8GL/, http://stackoverflow.com/questions/18980325/show-dropdown-in-cell-of-kendo-grid-in-template, http://demos.telerik.com/kendo-ui/grid/editing-inline