var dataSourceTableGrid = new kendo.data.DataSource({
		transport : {
			create : {
				url : "/Table/Create",
				contentType : "application/json",
				type : "POST"
			},

			read : {
				url : "/Table/Read",
				contentType : "application/json",
				type : "POST"
			},
			update : {
				url : "/Table/Update",
				contentType : "application/json",
				type : "POST"
			},
			destroy : {
				url : "/Table/Destroy",
				contentType : "application/json",
				type : "POST"
			},
			parameterMap : function (data, operation) {
				return JSON.stringify(data);
			}
		},
		requestEnd : function (e) {
			PopNotify('SectionNotifyGridCodeGenTables', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);
			if (e.response.PopNotificationMessageType === "error") {
				this.cancelChanges();
			}
		},
		schema : {
			data : "Data",
			total : "Total",
			PopNotificationMessage : "PopNotificationMessage",
			PopNotificationMessageType : "PopNotificationMessageType",
			model : {
				id : "Id",
				fields : {
				    Id: { editable: false, type: "number" },
				    TableName: { type: "string", validation: { required: true } }

				}
			}

		},
		pageSize : 10,
		serverPaging : true,
		serverFiltering : true,
		serverSorting : true
	});
