$(document).ready(function () {
    $("#CodeGenTemplatesGrid").kendoGrid({
        dataSource: dataSourceTemplateGrid,
        columns: [
            //{ field: "Id", title: "Id", width: "110px" },

            { field: "TemplateFileName", title: "File", width: "100px" },

            { field: "SingleFile", title: "SingleFile", width: "100px" },

            //{ field: "TemplateFileExtension", title: "Extension", width: "110px" },

            //{ field: "TemplateText", title: "Template Code", width: "110px" },

            { command: ["edit", "destroy", { text: "Run", imageClass: "k-icon k-i-custom ob-icon-only", click: RunTemplate }], title: "&nbsp", width: "200px" }],

        sortable: true,
        pageable: true,
        filterable: true,
        toolbar: ["create"],

        //editable: {
        //    mode: "popup",
        //    confirmation: false,
        //}

        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        }

    }).data("kendoGrid");

});

function RunTemplate(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    var templateId = dataItem.Id;

    console.log(templateId);

    //var datastring = $(dataItem).serialize();

    var datastring = JSON.stringify(dataItem);

    //url: "/Template/RunTemplate?templateId=" + templateId,

    $.ajax({
        type: 'POST',
        url: "/Template/RunTemplate",
        data: datastring,
        contentType: "application/json",
        async: false,
        success: function (data) {
            document.getElementById("debug").innerHTML = data;

            $('#CodeGenTemplatesGrid').data('kendoGrid').dataSource.read();
            $('#CodeGenTemplatesGrid').data('kendoGrid').refresh();

        },
        error: function (msg) {
            alert('failure');
            alert(msg);
        }
    });

}

function GetTokens()
{
    alert('test');

    var dd = $("#tokens").kendoDropDownList({
        dataTextField: "Token",
        dataValueField: "Id",
        dataSource: {
            transport: {
                read: {
                    url: "/Token/GetAll",
                    contentType: "application/json",
                    type: "POST"
                }
            }
        }
    });

    //dd.dataSource.read();
    
}
  

//how to get kendoui grid popup add/edit form created from kendo template, show the correct title for add and edit operations?

//http://stackoverflow.com/questions/19374775/how-to-get-kendoui-grid-popup-add-edit-form-created-from-kendo-template-show-th


//{ command: ["edit", "destroy", { text: "Columns", imageClass: "k-icon k-i-custom ob-icon-only", click: EditTemplate }], title: "&nbsp", width: "180px" }],