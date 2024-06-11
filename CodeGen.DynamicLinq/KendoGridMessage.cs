//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Collections;

//namespace CodeGen.DynamicLinq
//{
//    public class KendoGridMessage
//    {
//         /// <summary>
//        /// Create a message for the grid
//        /// </summary>
        
//        public DataSourceResult GenerateGridMessage(IEnumerable data, string message, KendoNotification.PopUpMessageType popUpMessageType)
//        {
            
//            DataSourceResult dataSourceResult = new DataSourceResult();

//            dataSourceResult.Data = data;
//            dataSourceResult.NotificationMessage = message;

//            if (popUpMessageType == KendoNotification.PopUpMessageType.Info)
//            {
//                dataSourceResult.NotificationMessageType = "info";
//            }

//            if (popUpMessageType == KendoNotification.PopUpMessageType.Success)
//            {
//                dataSourceResult.NotificationMessageType = "upload-success";
//            }

//            if (popUpMessageType == KendoNotification.PopUpMessageType.Error)
//            {
//                dataSourceResult.NotificationMessageType = "error";
//            }
            

//            return dataSourceResult;

//        }

//    }

//}