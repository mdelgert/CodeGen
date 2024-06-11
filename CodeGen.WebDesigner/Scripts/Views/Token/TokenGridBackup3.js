
function onCancel(e) {

    $("#GridName").data("kendoGrid").refresh();

}

$("#TokenGrid").kendoGrid({
    dataSource:
        {


            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: {
                field: "Token",
                dir: "asc"
            },
            pageSize: 10,
            requestEnd: function (e) {
                PopNotify('SectionNotifyGridToken', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);

                if (e.response.PopNotificationMessageType === "error") {

                    this.cancelChanges();
                    //alert('error');

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

                        Token: {
                            type: "string",
                            defaultValue: "${YourTokenName}",
                            validation: {
                                required: true
                            }
                        },

                        TokenValue: {
                            type: "string",
                            validation: {
                                required: true
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
            transport: {
                create: {
                    url: "/Token/Create",
                    contentType: "application/json",
                    type: "POST"
                },
                read: {
                    url: "/Token/Read",
                    contentType: "application/json",
                    type: "POST"
                },
                update: {
                    url: "/Token/Update",
                    contentType: "application/json",
                    type: "POST"
                },
                destroy: {
                    url: "/Token/Destroy",
                    contentType: "application/json",
                    type: "POST"
                },
                parameterMap: function (data, operation) {
                    return JSON.stringify(data);
                }
            }
        },

    columns: [
        {
            field: "Token",
            title: "Token:",
            width: "100px"
        },
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
            command: ["edit", "destroy"],
            //title : "&nbsp;",
            title: "Commands:",
            width: "90px"
        }


    ],

    //editable : {
    //	mode : "popup",
    //	confirmation : false,
    //},

    editable: {
        mode: "popup",
        template: kendo.template($("#popup-editor").html())
    },


    //cancel: function(e) {
    //    alert('test');
    //},

    //editable: true,

    pageable: true,

    sortable: true,

    filterable: true,

    //toolbar: ["create", "save", "cancel"]

    toolbar: ["create"]
        ,


    save: function (e) {
        //alert("save event captured");
        console.log("save event captured");
    },


    //http://tiku.io/questions/909795/how-capture-destroy-event-in-kendo-ui-grids-when-click-is-done
    remove: function (e) {
        //alert("delete event captured");

        //alert(e.IsSystemToken);

        console.log("delete event captured");

        console.log(e.model.IsSystemToken);

        if (e.model.IsSystemToken === true) {
            javascript: void (0);
            this.cancelChanges();
            console.log("System tokens can not be deleted.");
            //PopNotify('SectionNotifyGridToken', 'System tokens can not be deleted.', 'error');
        }


    },


    //http://stackoverflow.com/questions/20491191/applying-k-state-disabled-class-to-text-inputs-kendo-ui
    //http://www.telerik.com/forums/how-do-i-conditional-set-the-visibility-of-the-command-edit-button-on-a-row-by-row-basis-
    dataBound: function (e) {
        //console.log("dataBound");

        //Selects all delete buttons
        $("#TokenGrid tbody tr .k-grid-delete").each(function () {
            var currentDataItem = $("#TokenGrid").data("kendoGrid").dataItem($(this).closest("tr"));

            //Check in the current dataItem if the row is deletable
            if (currentDataItem.IsSystemToken === true) {

                //$(this).remove();
                //$(this).disabled();

                //e.preventDefault();
                //$(this).unbind("click");

                $(this).addClass('k-state-disabled');

                //$(this).detach();

                //$(this).off("click");

                //$(this).unbind();
                //$(this).unbind("click");
                //$(this).addClass('red');
            }
        })


    }

}).data("kendoGrid");

