using System;
using System.Collections.Generic;
using ValueWebSocket.Infrastructure;

namespace ValueWebSocket.Test.Basic
{
    public class WebSocketManager
    {
        private static ValueWebSocket webSocket;

        public void Init()
        {
            webSocket = new ValueWebSocket("127.0.0.1", 3000);
            webSocket.Start();
        }

        public void Close()
        {
            webSocket.Dispose();
        }

    }
}