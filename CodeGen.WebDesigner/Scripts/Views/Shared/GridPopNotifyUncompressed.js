//Provides pop notification to Kendoui grid CRUD operations

function PopNotify(NotifyArea, PopNotificationMessage, PopNotificationMessageType) {

    var notificationSection = "#" + NotifyArea;
    var appendSection = "#append" + NotifyArea;

    var staticNotification = $(notificationSection).kendoNotification({
        appendTo: appendSection,
        autoHideAfter: 0,
        hideOnClick: false
    }).data("kendoNotification");

    $(appendSection).empty();

    staticNotification.show(PopNotificationMessage, PopNotificationMessageType);

};

/*

//Usage: 

//1. Add to grid div

Example 1:

<div id="GridName">
     <span id="SectionNotifyGridName"></span>
     <div id="appendSectionNotifyGridName" class="k-block"></div>
</div>

// 2. Add to section "schema:"

Example 2.

PopNotificationMessage: "PopNotificationMessage", 
PopNotificationMessageType: "PopNotificationMessageType",

// 3. Add request end function to grid 

Example 3.

requestEnd: function (e) {
    PopNotify('SectionNotifyGridName', e.response.PopNotificationMessage, e.response.PopNotificationMessageType);
    if (e.response.PopNotificationMessageType === "error") {
        this.cancelChanges();
    }
},

*/

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
* Copyright © 2015 Matthew David Elgert mdelgert@vessea.com, All Rights Reserved. 
*
* NOTICE: All information contained herein is, and remains the property of Matthew Elgert. The intellectual and technical concepts contained
* herein are proprietary to Matthew Elgert and may be covered by U.S. and Foreign Patents, patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material is strictly forbidden unless prior written permission is obtained
* from Matthew Elgert. Access to the source code contained herein is hereby forbidden to anyone except Matthew Elgert or contractors who have executed 
* Confidentiality and Non-disclosure agreements explicitly covering such access.
*
* The copyright notice above does not evidence any actual or intended publication or disclosure of this source code, which includes
* information that is confidential and/or proprietary, and is a trade secret, of Matthew Elgert. ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC PERFORMANCE, 
* OR PUBLIC DISPLAY OF OR THROUGH USE OF THIS SOURCE CODE WITHOUT THE EXPRESS WRITTEN CONSENT OF Matthew Elgert IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE 
* LAWS AND INTERNATIONAL TREATIES. THE RECEIPT OR POSSESSION OFTHIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT ITMAY DESCRIBE, IN WHOLE OR IN PART.
*
* Company: VESSEA, LLC.
*
* Author: Matthew David Elgert
*
* File: GridPopNotifyUncompressed.js
*
* Authored date: 1/3/2015
* 
* Modified date: 1/3/2015
* 
* Notes: 
* 
* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- */