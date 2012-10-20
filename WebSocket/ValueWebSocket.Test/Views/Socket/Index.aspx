<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index</title>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var noSupportMessage = "您的浏览器不支持WebSocket!";
        var ws;

        function connectSocketServer() {
            var messageBoard = $("#messageBoard");

            var support = "MozWebSocket" in window ? 'MozWebSocket' : ('WebSocket' in window ? 'WebSocket' : null);

            if (support == null) {
                alert(noSupportMessage);
                messageBoard.append('*' + noSupportMessage + "<br />");
                return;
            }

            messageBoard.append("* Connecting to server..<br />");
            try {
                ws = new WebSocket('ws://localhost:3000');
            } catch (e) {
                alert(e.Message);
            }

            ws.onmessage = function (event) {
                messageBoard.append(event.data + "<br />");
            }

            ws.onopen = function () {
                messageBoard.append('* Connection open<br />');
            }

            ws.onclose = function () {
                messageBoard.append('* Connection closed<br />');
            }
        }

        function sendMessage() {
            if (ws) {
                var mssageBox = document.getElementById("messageBox");
                var user = document.getElementById("users");
                var msg = user.value + "<separator>" + mssageBox.value;
                ws.send(msg);
                mssageBox.value = "";
            } else {
                alert(noSupportMessage);
            }
        }

        window.onload = function () {
            connectSocketServer();
        }
    </script>
    <script type="text/javascript">
        $.ajax({
            type: "POST",
            url: "/Socket/UpdateUser",
            success: function (data) {
                $("#userlist").html(data);
            }
        })
    </script>
</head>
<body>
    <form id="formMain" runat="server">
    <table width="100%" cellspacing="1" bgcolor="#99BBE8" cellpadding="0" border="0">
        <tr>
            <td class="panelHeader">
                Chat Message
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
                <div id="messageBoard">
                </div>
            </td>
        </tr>
        <tr>
            <td bgcolor="#D2E0F0">
                <table width="100%" border="0" height="90" cellpadding="2" cellspacing="1">
                    <tr>
                        <td id="messageBoxCell">
                            <div id="userlist">
                            </div>
                            <textarea id="messageBox" style="height: 80px; border: 1px solid gray;"></textarea>
                        </td>
                        <td width="100" valign="top" align="center">
                            <input type="button" id="btnSend" value="Send" style="width: 84px; height: 64px;"
                                onclick="return sendMessage();" />
                            <span style="font-size: 12px; color: Red;">[Ctrl + Enter]</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
