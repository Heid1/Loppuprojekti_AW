"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// attempt to set scrollbar to bottom of the page
//document.getElementsByClassName('px-4 py-5 chat-box bg-white')[0].scrollTop = document.getElementsByClassName('px-4 py-5 chat-box bg-white')[0].scrollHeight;

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (userId, message, sendTime, sendDate) {

    var div1 = document.createElement('div');
    div1.className = 'media w-50 ml-auto mb-3';

    var div2 = document.createElement('div');
    div2.className = 'media-body';

    var div3 = document.createElement('div');
    div3.className = 'bg-primary rounded py-2 px-3 mb-2';

    var msgContent = document.createElement('p');
    msgContent.className = 'text-small mb-0 text-white';
    msgContent.appendChild(document.createTextNode(`${message}`));

    var msgInfo = document.createElement('p');
    msgInfo.className = 'small text-muted';
    msgInfo.appendChild(document.createTextNode(`${sendTime} | ${sendDate}`));

    div3.appendChild(msgContent);
    div2.appendChild(div3);
    div2.appendChild(msgInfo);
    div1.appendChild(div2);
    document.getElementById("chatBox").appendChild(div1);

    // example structure of one message created above
    //<div class="media w-50 ml-auto mb-3">
    //    <div class="media-body">
    //        <div class="bg-primary rounded py-2 px-3 mb-2">
    //            <p class="text-small mb-0 text-white">Hey!</p>
    //        </div>
    //        <p class="small text-muted">12:00 | Syyskuu 10</p>
    //    </div>
    //</div>

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    //var user = document.getElementById("userInput").value;
    
    var userId = "44";
    var senderId = document.get
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = '';
    connection.invoke("SendMessage", userId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});