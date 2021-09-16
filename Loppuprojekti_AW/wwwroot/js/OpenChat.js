"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

// attempt to set scrollbar to bottom of the page
//document.getElementsByClassName('px-4 py-5 chat-box bg-white')[0].scrollTop = document.getElementsByClassName('px-4 py-5 chat-box bg-white')[0].scrollHeight;

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

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

    var receiverUserName = document.getElementById("userNameHeading").innerText;
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
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    //var user = document.getElementById("userInput").value;

    var senderName = document.getElementById("userNameHeading").innerHTML;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = '';
    connection.invoke("SendMessage", senderName, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});