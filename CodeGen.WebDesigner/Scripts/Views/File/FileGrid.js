var dataSource = new kendo.data.DataSource({

    serverPaging: true,

    serverFiltering: true,

    serverSorting: true,

    sort: {
        field: "Id",
        dir: "desc"
    },

    pageSize: 10,

    requestEnd: function (e) {
        PopNotify('SectionNotifyGridFile', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);
        if (e.response.PopNotificationMessageType === "error") {
            this.cancelChanges();
        }
    },

    schema: {
        data: "Data",
        total: "Total",
        PopNotificationMessage: "PopNotificationMessage",
        PopNotificationMessageType: "PopNotificationMessageType", //Type of notification
        model: {
            id: "Id",
            fields: {

                Id: {
                    editable: false,
                    type: "number"
                },

                FileName: {
                    type: "string"
                },

                FileText: {
                    type: "string"
                },

            }

        }

    },

    batch: false,

    transport: { //use HTTP POST request as by default GET is not allowed by ASP.NET MVC

        create: {
            url: "/File/Create",
            contentType: "application/json",
            type: "POST"

        },

        read: {
            url: "/File/Read",
            contentType: "application/json",
            type: "POST"

        },

        update: {
            url: "/File/Update",
            contentType: "application/json",
            type: "POST"
        },

        destroy: {
            url: "/File/Destroy",
            contentType: "application/json",
            type: "POST"
        },

        parameterMap: function (data, operation) {
            return JSON.stringify(data);
        }

    }

});


var grid = $("#FileGrid").kendoGrid({
    dataSource: dataSource,
    columns: [

        //{
        //	field : "Id",
        //	width : "100px"
        //}

        {
            field: "FileName",
            title: "FileName:",
            width: "100px"
        },

        {
            command: ["edit", "destroy"],
            //title : "&nbsp;",
            title: "Commands:",
            width: "90px"
        }


    ],

    //editable: {
    //    mode: "popup",
    //    confirmation: false,
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

}).data("kendoGrid");
