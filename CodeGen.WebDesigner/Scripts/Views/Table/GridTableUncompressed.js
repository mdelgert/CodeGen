$(document).ready(function () {
    $("#CodeGenTablesGrid").kendoGrid({
        dataSource: dataSourceTableGrid,
        columns: [

            { field: "TableName", title: "Table", width: "110px" },

            //{ command: ["edit", "destroy"], title: "&nbsp;", width: "100px" },

            //http://demos.telerik.com/kendo-ui/styling/icons

            //http://stackoverflow.com/questions/19900334/kendoui-using-icons-instead-of-buttons-for-custom-commands

            { command: ["edit", "destroy", { text: "Columns", imageClass: "k-icon k-i-custom ob-icon-only", click: ShowColumns }], title: "&nbsp", width: "180px" }],

        sortable: true,
        pageable: true,
        filterable: true,
        //toolbar: ["create", "save", "cancel"],
        toolbar: ["create"],
        editable: {
            mode: "popup",
            confirmation: false,
        }

    }).data("kendoGrid");

});


/*

http://stackoverflow.com/questions/19900334/kendoui-using-icons-instead-of-buttons-for-custom-commands

But if you don't want to overwrite Kendo UI style, you still can do:

$("#grid").kendoGrid({
    dataSource: myDataSource,
    columns: [
        {
            command: { 
                name: "Edit",
                text:"",
                imageClass: "k-icon k-i-pencil ob-icon-only",
                click: function(e) {
                    //some code
                }
            }
        },
        ...
    ],
});

*/