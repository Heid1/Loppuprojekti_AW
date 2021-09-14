"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// attempt to set scrollbar to bottom of the page
//document.getElementsByClassName('px-4 py-5 chat-box bg-white')[0].scrollTop = document.getElementsByClassName('px-4 py-5 chat-box bg-white')[0].scrollHeight;

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

function getMessagesApi() {
            return fetch(`/api/ChatApi/${currentUserId}/${otherPartyId}`)
                .then(res => res.json())
}

function getmessages(event) {

    // console.dir(this);
    // console.dir(event);
    //console.log("get messages!");
    getMessagesApi().then(messages => {
        console.dir(messages);
        let tempstring = "";
        for (let m of messages) {
            //var receiverUserName = m.r
            var div1ClassName;
            var div2ClassName;
            var div3ClassName;

            // shows messages differently depending on who has sent the message
            if (m.senderName != otherPartyId) {
                div1ClassName = "media w-50 ml-auto mb-3";
                div2ClassName = "media-body";
                div3ClassName = "bg-primary rounded py-2 px-3 mb-2";
                text = "text-small mb-0 text-white";
            } else {
                div1ClassName = "media w-50 mb-3";
                div2ClassName = "media-body ml-3";
                div3ClassName = "bg-light rounded py-2 px-3 mb-2";
                text = "text-small mb-0 text-muted";
            }

            tempstring += `<div class=${div1ClassName}><div class=${div2ClassName}><div class=${div3ClassName}><p class="text-small mb-0 text-white">${m.messagebody}</p></div><p class="small text-muted">${m.sendtime.toString('t')} | ${ m.sendtime.toString('dd.MM.yyy')}</p></div></div>`;
        }
        chatbox.innerHTML = tempstring;
    }).catch((error) => {
        console.error('Error:', error);
    });
}

function updateMessages() {
        var otherPartyId = this.id;
        var chatbox = document.getElementById("chatBox");
        chatbox.innerHTML = "";
        getmessages();
    }

var currentUserId = document.getElementById("userId").innerText;

// CREATING ELEMENTS OF EXISTING CHATS
var usersMessagedWith = document.getElementsByClassName("enduserName");
var usersMessagedWithIds = document.getElementsByClassName("enduserId");
for (let i = 0; i < usersMessagedWith.length; i++) {

    //<a class="list-group-item list-group-item-action active text-white rounded-0">
     //    <div class="media">
     //       @*<img src="" alt="user" width="50" class="rounded-circle">*@
     //         <div class="media-body ml-4">
     //             <div class="d-flex align-items-center justify-content-between mb-1">
     //                 <h6 class="mb-0">Jason Doe</h6><small class="small font-weight-bold">25 Dec</small>
     //              </div>
     //                  <p class="font-italic mb-0 text-small">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore.</p>
     //          </div>
     //    </div>
     //</a>

    var aElement = document.createElement('a');
    aElement.className = "list-group-item list-group-item-action active text-white rounded-0";
    aElement.id = usersMessagedWithIds[i].innerHTML;

   
    // function update chat window on when chat with different user is opened
    aElement.addEventListener("click", updateMessages);
    //aElement.onclick = function (element) {
    //    var otherPartyId = this.id;
    //    var chatbox = document.getElementById("chatBox");
    //    chatbox.innerHTML = "";
        
    //}

    //    function getMessagesApi() {
    //        return fetch(`/api/ChatApi/${currentUserId}/${otherPartyId}`)
    //            .then(res => res.json())
    //    }

    //    function getmessages(event) {
         
    //        // console.dir(this);
    //        // console.dir(event);
    //        //console.log("get messages!");
    //        getMessagesApi().then(messages => {
    //            console.dir(messages);
    //            let tempstring = "";
    //            for (let m of messages) {
    //                //var receiverUserName = m.r
    //                var div1ClassName;
    //                var div2ClassName;
    //                var div3ClassName;

    //                // shows messages differently depending on who has sent the message
    //                if (m.senderName != otherPartyId) {
    //                    div1ClassName = "media w-50 ml-auto mb-3";
    //                    div2ClassName = "media-body";
    //                    div3ClassName = "bg-primary rounded py-2 px-3 mb-2";
    //                    text = "text-small mb-0 text-white";
    //                } else {
    //                    div1ClassName = "media w-50 mb-3";
    //                    div2ClassName = "media-body ml-3";
    //                    div3ClassName = "bg-light rounded py-2 px-3 mb-2";
    //                    text = "text-small mb-0 text-muted";
    //                }

    //                tempstring += `<div class=${div1ClassName}><div class=${div2ClassName}><div class=${div3ClassName}><p class="text-small mb-0 text-white">${m.messagebody}</p></div><p class="small text-muted">${m.sendtime.toString('t')} | ${ m.sendtime.toString('dd.MM.yyy')}</p></div></div>`;

    //            }
    //            chatbox.innerHTML = tempstring;
    //        }).catch((error) => {
    //            console.error('Error:', error);
    //        });
    //    }



        
   
    //}

    var div1 = document.createElement('div');
    div1.className = "media";

    var div2 = document.createElement('div');
    div2.className = "media-body ml-4";

    var div3 = document.createElement('div');
    div3.className = "d-flex align-items-center justify-content-between mb-1";

    var chatWithUserName = document.createElement('h6');
    chatWithUserName.className = "mb-0";
    chatWithUserName.appendChild(document.createTextNode(`${usersMessagedWith[i].textContent}`));

    // pitää välittää viimeisimmän viestin pvm
  


    div3.appendChild(chatWithUserName);
    div2.appendChild(div3);
    div1.appendChild(div2);
    aElement.appendChild(div1);
    document.getElementById("chats").appendChild(aElement);
}





//senderName, message, sendTime, sDate
connection.on("ReceiveMessage", function (senderName, message, sendTime, sendDate) {

    // receiver and sender message examples
    //< !--Sender Message-- >
    //    <div class="media w-50 mb-3">
    //        @*<img src="" alt="user" width="50" class="rounded-circle">*@
    //                    <div class="media-body ml-3">
    //                <div class="bg-light rounded py-2 px-3 mb-2">
    //                    <p class="text-small mb-0 text-muted">Hello there!</p>
    //                </div>
    //                <p class="small text-muted">12:00 | Syyskuu 10</p>
    //            </div>
    //                </div>

    //        <!-- Reciever Message -->
    //          <div class="media w-50 ml-auto mb-3">
    //            <div class="media-body">
    //                <div class="bg-primary rounded py-2 px-3 mb-2">
    //                    <p class="text-small mb-0 text-white">testi!</p>
    //                </div>
    //                <p class="small text-muted">12:00 | Syyskuu 10</p>
    //            </div>
    //        </div>

    // document.getElementById("userName").innerText;
    var receiverUserName = document.getElementById("userName").innerText;
    var div1ClassName;
    var div2ClassName;
    var div3ClassName;
    var text;

    // shows messages differently depending on who has sent the message
    if (receiverUserName === senderName) {
        div1ClassName = "media w-50 ml-auto mb-3";
        div2ClassName = "media-body";
        div3ClassName = "bg-primary rounded py-2 px-3 mb-2";
        text = "text-small mb-0 text-white";
    } else {
        div1ClassName = "media w-50 mb-3";
        div2ClassName = "media-body ml-3";
        div3ClassName = "bg-light rounded py-2 px-3 mb-2";
        text = "text-small mb-0 text-muted";
    }
    var div1 = document.createElement('div');
    div1.className = div1ClassName;

    var div2 = document.createElement('div');
    div2.className = div2ClassName;

    var div3 = document.createElement('div');
    div3.className = div3ClassName;

    var msgContent = document.createElement('p');
    msgContent.className = text;
    msgContent.appendChild(document.createTextNode(`${message}`));

    var msgInfo = document.createElement('p');
    msgInfo.className = 'small text-muted';
    msgInfo.appendChild(document.createTextNode(`${sendTime} | ${sendDate}`));

    var sender = document.createElement('p');
    sender.className = 'small text-muted';
    sender.appendChild(document.createTextNode(`${senderName}`));

    div3.appendChild(msgContent);
    div2.appendChild(sender);
    div2.appendChild(div3);
    div2.appendChild(msgInfo);
    div1.appendChild(div2);
    document.getElementById("chatBox").appendChild(div1);


    //var div1 = document.createElement('div');
    //div1.className = 'media w-50 ml-auto mb-3';

    //var div2 = document.createElement('div');
    //div2.className = 'media-body';

    //var div3 = document.createElement('div');
    //div3.className = 'bg-primary rounded py-2 px-3 mb-2';

    //var msgContent = document.createElement('p');
    //msgContent.className = 'text-small mb-0 text-white';
    //msgContent.appendChild(document.createTextNode(`${message}`));

    //var msgInfo = document.createElement('p');
    //msgInfo.className = 'small text-muted';
    //msgInfo.appendChild(document.createTextNode(`${sendTime} | ${sendDate}`));

    //div3.appendChild(msgContent);
    //div2.appendChild(div3);
    //div2.appendChild(msgInfo);
    //div1.appendChild(div2);
    //document.getElementById("chatBox").appendChild(div1);

    //// example structure of one message created above
    ////<div class="media w-50 ml-auto mb-3">
    ////    <div class="media-body">
    ////        <div class="bg-primary rounded py-2 px-3 mb-2">
    ////            <p class="text-small mb-0 text-white">Hey!</p>
    ////        </div>
    ////        <p class="small text-muted">12:00 | Syyskuu 10</p>
    ////    </div>
    ////</div>

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    //var user = document.getElementById("userInput").value;
    
    var senderId = document.getElementById("userId").innerText;
    var senderName = document.getElementById("userName").innerText;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = '';
    connection.invoke("SendMessage", senderId, senderName, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

    function changeChat(userId) {
        document.getElementById("demo").style.color = "red";
    }
