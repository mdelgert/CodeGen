var dataSourceTemplateGrid = new kendo.data.DataSource({
    transport: {
        create: {
            url: "/Template/Create",
            contentType: "application/json",
            type: "POST"
        },

        read: {
            url: "/Template/Read",
            contentType: "application/json",
            type: "POST"
        },
        update: {
            url: "/Template/Update",
            contentType: "application/json",
            type: "POST"
        },
        destroy: {
            url: "/Template/Destroy",
            contentType: "application/json",
            type: "POST"
        },
        parameterMap: function (data, operation) {
            return JSON.stringify(data);
        }
    },
    requestEnd: function (e) {
        PopNotify('SectionNotifyGridTemplate', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);
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
                Id: { editable: false, type: "number" },
                TemplateFileName: { type: "string" },
                SingleFile: { type: "boolean" },
                TemplateFileExtension: { type: "string"},
                TemplateText: { type: "string", validation: { required: true } }
            }
        }
    },
    pageSize: 10,
    serverPaging: true,
    serverFiltering: true,
    serverSorting: true
});
