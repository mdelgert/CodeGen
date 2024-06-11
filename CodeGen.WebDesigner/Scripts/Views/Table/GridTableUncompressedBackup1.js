$(document).ready(function () {
    $("#CodeGenTablesGrid").kendoGrid({
        dataSource: {
            transport: {
                create: {
                    url: "/Table/Create",
                    contentType: "application/json",
                    type: "POST"
                },

                read: {
                    url: "/Table/Read",
                    contentType: "application/json",
                    type: "POST"
                },
                update: {
                    url: "/Table/Update",
                    contentType: "application/json",
                    type: "POST"
                },
                destroy: {
                    url: "/Table/Destroy",
                    contentType: "application/json",
                    type: "POST"
                },
                parameterMap: function (data, operation) {
                    return JSON.stringify(data);
                }
            },
            requestEnd: function (e) {
                PopNotify('SectionNotifyGridCodeGenTables', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);
                if (e.response.PopNotificationMessageType === "error") {
                    this.cancelChanges();
                }
            },
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
                        TableName: {
                            type: "string",
                            validation: {
                                required: true
                            }
                        }

                    }
                }

            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        },

        columns: [{
            field: "TableName",
            title: "Table",
            width: "110px"
        }, {
            command: ["edit", "destroy"],
            title: "&nbsp;",
            width: "100px"
        }, {
            command: {
                text: "Columns",
                click: ShowColumns
            },
            title: " ",
            width: "180px"
        }
        ],

        sortable: true,
        pageable: true,
        filterable: true,
        toolbar: ["create", "save", "cancel"],
        editable: {
            mode: "popup",
            confirmation: false,
        }

    }).data("kendoGrid");

});
