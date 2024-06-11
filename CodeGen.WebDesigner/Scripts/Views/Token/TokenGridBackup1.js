var dataSource = new kendo.data.DataSource({


    serverPaging: true,

    serverFiltering: true,

    //serverSorting : true,

    //sort: {
    //    field: "Id",
    //    dir: "desc"
    //},

    pageSize: 10,

    requestEnd: function (e) {
        PopNotify('SectionNotifyGridToken', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);
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
                position: {
                    type: "number"
                },
                Token: {
                    type: "string",
                    validation: {
                        required: true
                    }
                },
                TokenValue: {
                    type: "string",
                    validation: {
                        required: true
                    }
                }
            }
        }
    },

    batch: false,

    transport: { //use HTTP POST request as by default GET is not allowed by ASP.NET MVC

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

});

var grid = $("#grid").kendoGrid({
    dataSource: dataSource,
    columns: [

        //{
        //	field : "Id",
        //	width : "100px"
        //}, {
        //	field : "position",
        //	width : "100px"
        //},




        {
            field: "Token",
            width: "100px"
        }, {
            field: "TokenValue",
            width: "300px"
        }, {
            command: ["edit", "destroy"],
            title: "&nbsp;",
            width: "200px"
        },


           //{ template: '<button onclick="return up(\'#=uid#\')">up</button><button onclick="return down(\'#=uid#\')">down</button>' }

           //http://demos.telerik.com/kendo-ui/styling/icons


            //{ template: '<div onclick="return up(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrow-n"></span>Up</div>' },
            //{ template: '<div onclick="return down(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrow-s"></span>Down</div>' }


            //{ template: '<div onclick="return up(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrow-n"></span></div>Up<div onclick="return down(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrow-s"></span>Down</div>' },


            //{ template: '<div onclick="return up(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrow-n"></span></div><div onclick="return down(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrow-s"></span></div>' },

            //{ template: '<div onclick="return up(\'#=uid#\')" class="k-button"><span class="k-icon k-i-seek-n"></span></div><div onclick="return down(\'#=uid#\')" class="k-button"><span class="k-icon k-i-seek-s"></span></div>' },

            //{ template: '<div onclick="return up(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrowhead-n"></span></div><div onclick="return down(\'#=uid#\')" class="k-button"><span class="k-icon k-i-arrowhead-s"></span></div>' },


            {
                width: "55px", template: '<div onclick="return up(\'#=uid#\')" class="k-button"><span class="k-icon k-si-arrow-n"></span></div>' + '<br />' +
                                          '<div onclick="return down(\'#=uid#\')" class="k-button"><span class="k-icon k-si-arrow-s"></span></div>'
            },



    ],

    editable: {
        mode: "popup",
        confirmation: false,
    },

    //editable: true,

    pageable: true,

    //sortable: true,

    filterable: true,

    toolbar: ["create", "save", "cancel"]

}).data("kendoGrid");

//http://stackoverflow.com/questions/5320802/post-json-data-to-asmx-webservice
//http://stackoverflow.com/questions/16471046/how-to-refresh-the-kendo-ui-grid


function up(uid) {

    //alert('up');

    //alert(uid);

    //event = this.dataSource.getByUid(this.select().data("uid"));

    //var uid = this.dataSource.getByUid(this.select().data("uid"));

    document.getElementById("debuText").innerHTML = uid;

    var dataItem = grid.dataSource.getByUid(uid);
    var index = grid.dataSource.indexOf(dataItem);
    var newIndex = Math.max(0, index - 1);

    if (newIndex !== index) {
        grid.dataSource.remove(dataItem);
        grid.dataSource.insert(newIndex, dataItem);
    }

    var vgrid = $("#grid").data("kendoGrid");
    var items = vgrid.dataSource.data();

    for (i = 0; i < items.length; i++) {
        var item = items[i];

        item.position = i;

        $.ajax({
            type: 'POST',
            url: "/Token/Update",
            data: JSON.stringify(item),
            contentType: "application/json"
        });

    }

    document.getElementById("debuText").innerHTML = htmlText;

    return false;

}

function down(uid) {

    //document.getElementById("debuText").innerHTML = 'Down';

    document.getElementById("debuText").innerHTML = uid;

    var dataItem = grid.dataSource.getByUid(uid);
    var index = grid.dataSource.indexOf(dataItem);
    var newIndex = Math.min(grid.dataSource.total() - 1, index + 1);

    if (newIndex !== index) {
        grid.dataSource.remove(dataItem);
        grid.dataSource.insert(newIndex, dataItem);
    }

    var vgrid = $("#grid").data("kendoGrid");
    var items = vgrid.dataSource.data();


    for (i = 0; i < items.length; i++) {
        var item = items[i];

        item.position = i;

        $.ajax({
            type: 'POST',
            url: "/Token/Update",
            data: JSON.stringify(item),
            contentType: "application/json"
        });

    }

    document.getElementById("debuText").innerHTML = htmlText;

    return false;
}










//{ template: '<div onclick="return up(\'#=uid#\')" class="k-button"><span class="k-icon k-edit"></span></div><button onclick="return up(\'#=uid#\')">up</button><button onclick="return down(\'#=uid#\')">down</button>' }



//,{ command: [{ name: "edit", template: "<div class='k-button'><span class='k-icon k-edit'></span></div>" }], title: " ", width: 100 }



//{ command: { text: "UP", click: 'return up(uid)' }, title: "&nbsp", width: "180px" }



//{
//    command: {
//        name: "Edit",
//        text: "Up",
//        imageClass: "k-icon k-i-pencil ob-icon-only",
//        click: function (e) {
//            //some code
//            //return up(e.dataItem.uid);
//            up($uid);
//        }
//    }
//}






//var htmlText = '';
//htmlText = htmlText + 'id=' + item.id + ' position=' + item.position + '<br />';

////var data = grid.dataSource.data();

//$.each(data, function (i, row) {
//    //do something

//    htmlText = htmlText + row.position + '<br />';


//    $.ajax({
//        type: 'POST',
//        url: "/Token/Update",
//        data: JSON.stringify(grid.dataItem),
//        contentType: "application/json"
//    });


//});





//var grid = $("#grid").data("kendoGrid");

//dataItem.position = newIndex;

//$.ajax({
//    type: 'POST',
//    url: "/Token/Update",
//    data: JSON.stringify(dataItem),
//    contentType: "application/json"
//});

////http://stackoverflow.com/questions/16471046/how-to-refresh-the-kendo-ui-grid
//grid.dataSource.read();



////http://stackoverflow.com/questions/5320802/post-json-data-to-asmx-webservice

//dataItem.position = newIndex;

//$.ajax({
//    type: 'POST',
//    url: "/Token/Update",
//    data: JSON.stringify(dataItem),
//    contentType : "application/json"
//});

////http://stackoverflow.com/questions/16471046/how-to-refresh-the-kendo-ui-grid
//grid.dataSource.read();





//function up(uid) {

//    var dataItem = grid.dataSource.getByUid(uid);
//    var newIndex = (dataItem.position - 1);

//    dataItem.position = newIndex;

//    dataItem.MoveDirection = 'Up';

//    $.ajax({
//        type: 'POST',
//        url: "/Token/Update",
//        data: JSON.stringify(dataItem),
//            contentType: "application/json",
//            async: false,
//            success: function (msg) {
//                //alert(msg.d);
//                //http://stackoverflow.com/questions/5320802/post-json-data-to-asmx-webservice
//                //http://stackoverflow.com/questions/16471046/how-to-refresh-the-kendo-ui-grid
//                grid.dataSource.read();
//            },
//            error: function (msg) {
//                alert('failure');
//                alert(msg);
//            }
//        });


//    return false;
//}

//function down(uid) {

//    var dataItem = grid.dataSource.getByUid(uid);
//    var newIndex = (dataItem.position + 1);

//        dataItem.position = newIndex;

//    dataItem.MoveDirection = 'Down';

//        $.ajax({
//            type: 'POST',
//            url: "/Token/Update",
//            data: JSON.stringify(dataItem),
//            contentType: "application/json",
//            async: false,
//            success: function (msg) {
//                //alert(msg.d);
//                //http://stackoverflow.com/questions/5320802/post-json-data-to-asmx-webservice
//                //http://stackoverflow.com/questions/16471046/how-to-refresh-the-kendo-ui-grid
//                grid.dataSource.read();
//            },
//            error: function (msg) {
//                alert('failure');
//                alert(msg);
//            }
//        });


//    return false;
//}











