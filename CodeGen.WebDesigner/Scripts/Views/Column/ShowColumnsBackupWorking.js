
//################### Get column types DD ################################################

//http://jsbin.com/obufum/5/edit?html,css,js,output

var columnTypes = [];

$.ajax({ //Get column types
    type: 'POST',
    url: "/ColumnType/GetAll",
    contentType: "application/json",
    async: false,
    success: function (data) {
        columnTypes = data;
        //alert('success');
    },

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

//################### Get column types DD ################################################

var wnd,
detailsTemplate;

wnd = $("#details")
    .kendoWindow({
        //title: "Columns",
        modal: true,
        visible: false,
        resizable: false,
        width: 800,
        height: 500
    }).data("kendoWindow");

detailsTemplate = kendo.template($("#template").html());

function ShowColumns(e) {

    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    wnd.content(detailsTemplate(dataItem));

    wnd.title(dataItem.TableName);

    wnd.center().open();

    var GridTableId = dataItem.Id;

    var ColumnDataSource = new kendo.data.DataSource({
        transport: {

            create: {
                url: "/Column/Create",
                contentType: "application/json",
                type: "POST"
            },


            read: {
                url: "/Column/Read",
                contentType: "application/json",
                type: "POST"
            },

            update: {
                url: "/Column/Update",
                contentType: "application/json",
                type: "POST"
            },

            destroy: {
                url: "/Column/Destroy",
                contentType: "application/json",
                type: "POST"
            },

            parameterMap: function (data, operation) {
                return JSON.stringify(data);
            }

        },

        requestEnd: function (e) {

            //alert(e.type);

            PopNotify('SectionNotifyCodeGenColumns', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);

            if (e.response.PopNotificationMessageType === "error") {
                this.cancelChanges();
            }

            if (e.type === "create") {
                this.read(); //refresh after create to show the order in DB
            }


        },

        //autoSync: true,

        schema: {

            data: "Data",

            total: "Total",

            PopNotificationMessage: "PopNotificationMessage",
            PopNotificationMessageType: "PopNotificationMessageType",

            model: {
                id: "Id",
                fields: {
                    Id: {
                        editable: false,
                        type: "number"
                    },
                    OrderId: {
                        type: "number"
                    },

                    TableId: {
                        //editable: true,
                        type: "number",
                        defaultValue: GridTableId
                    },

                    ColumnName: {
                        type: "string",
                        validation: {
                            required: true
                        }
                    },

                    ColumnType: {
                        type: "string"
                    },

                    //ColumnTypeId: {
                    //    type: "number"
                    //},

                    ColumnTypeId: { field: "ColumnTypeId", defaultValue: 2 },

                }
            }

        },

        //cache: false,

        batch: false,

        pageSize: 10,

        serverPaging: true,

        serverFiltering: true,

        serverSorting: true,

        filter: {
            field: "TableId",
            operator: "eq",
            value: GridTableId
        }

    });

    $("#CodeGenColumns").kendoGrid({
        dataSource: ColumnDataSource,

        columns: [

            //{
            //    field: "TableId",
            //    title: "TableId",
            //    width: "110px"
            //},

            //{
            //    field: "OrderId",
            //    title: "OrderId",
            //    width: "110px"
            //},

            {
                field: "ColumnName",
                title: "Column",
                width: "110px"
            },

            //{
            //    field: "ColumnType",
            //    title: "Column Type",
            //    width: "110px"
            //},

            //{
            //    field: "ColumnTypeId",
            //    title: "ColumnTypeId",
            //    width: "110px"
            //},

            {
                field: "ColumnTypeId", width: "150px",
                editor: columnTypeDropDownEditor,
                title: "Column Type",
                template: "#=getColumnTypeName(ColumnTypeId)#"
            },

            {
                command: ["edit", "destroy"],
                title: "&nbsp;",
                width: "100px"
            },

            {
                title: "&nbsp;", width: "100px", template: '<div onclick="MoveGridRow(\'#=Id#\', \'Up\', \'/Column/ChangeOrder\', \'CodeGenColumns\')" class="k-button"><span class="k-icon k-si-arrow-n"></span></div>' +
                                                           '<div onclick="MoveGridRow(\'#=Id#\', \'Down\', \'/Column/ChangeOrder\', \'CodeGenColumns\')" class="k-button"><span class="k-icon k-si-arrow-s"></span></div>'
            }

        ],

        //autoBind: false,

        batch: false,

        sortable: false,

        pageable: false,

        filterable: true,

        scrollable: false,

        //toolbar: ["create", "save", "cancel"],

        toolbar: ["create"],

        //editable: true

        editable: {
            mode: "popup",
            confirmation: false,
        }

    }).data("kendoGrid");

}
