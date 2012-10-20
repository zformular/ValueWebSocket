using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using ValueHelper.ValueSocket;
using ValueWebSocket.Session;
using ValueWebSocket.Protocol;
using ValueWebSocket.Infrastructure;

namespace ValueWebSocket
{
    public class ValueWebSocket
    {
        // WebSocket服务端
        private ValueServer server;
        // 解析协议
        private ValueProtocol valueProtocol;
        // 管理在线用户
        private SessionManager sessionManager;

        public ValueWebSocket(String ipAddress, Int32 port)
        {
            valueProtocol = new ValueProtocol();
            sessionManager = new SessionManager();

            server = new ValueServer(ipAddress, port, Encoding.UTF8);
            server.OnReceive += new ValueHelper.ValueSocket.Infrastructure.ReceiveHandler(server_OnReceive);
        }

        private void server_OnReceive(ValueHelper.ValueSocket.SocketEvents.ReceiveEventArgs e)
        {
            // 分析用户是否已存在
            if (sessionManager.CheckSessionExist(e.Socket))
            {
                Message message = valueProtocol.Decode(e.Data);
                if (message.header.Opcode == OperType.Close)
                {
                    removeUser(e.Socket);
                }
                if (message.header.Opcode == OperType.Text)
                {
                    String msg = message.Data.ToString();
                    execMsg(e.Socket, msg);
                }
            }
            else
            {
                // 用户不存在则添加用户
                // 并发送握手信息与客户端建立连接
                String request = Encoding.UTF8.GetString(e.Data);
                Byte[] response = valueProtocol.GetResponse(request);
                server.Send(e.Socket, response);
                sessionManager.AddSession(e.Socket, request);
            }
        }

        // 对消息进行的处理
        private void execMsg(Socket socket, String message)
        {
            String name = String.Empty;
            foreach (ValueSession session in SessionManager.Sessions)
            {
                Socket sk = session.Socket;
                if (sk.Connected)
                {
                    if (sk.RemoteEndPoint == socket.RemoteEndPoint)
                    {
                        name = session.Cookies["name"];
                        break;
                    }
                }
            }

            // 判断私聊还是公共
            String[] separator = message.Split(new String[] { "<separator>" }, StringSplitOptions.None);
            String msg = String.Concat(name, ": ", separator[1]);
            if (separator[0] == "All")
                SendToAll(msg);
            else
            {
                foreach (ValueSession session in SessionManager.Sessions)
                {
                    if (session.Cookies["name"] == separator[0])
                    {
                        sendTo(session.Socket, msg);
                        break;
                    }
                }
            }
        }

        private void removeUser(Socket socket)
        {
            sessionManager.RemoveSession(socket);
        }

        private void SendToAll(String msg)
        {
            foreach (ValueSession session in SessionManager.Sessions)
            {
                sendTo(session.Socket, msg);
            }
        }

        private Boolean sendTo(Socket socket, String msg)
        {
            Byte[] data = valueProtocol.Encode(msg);
            return server.Send(socket, data);
        }

        public void Start()
        {
            server.Start();
        }

        public void Dispose()
        {
            sessionManager.Dispose();
            server.Dispose();
        }
    }
}
