using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using ValueHelper.Infrastructure;

namespace ValueWebSocket.Infrastructure
{
    public class ValueSession
    {
        private Socket socket;
        public Socket Socket { get { return socket; } }

        public ValueSession(Socket socket)
        {
            this.socket = socket;
            cookies = new KeyvalList<string, string>();
        }
        private KeyvalList<String, String> cookies;
        public KeyvalList<String, String> Cookies { get { return cookies; } }

        public void AddCookies(String key, String value)
        {
            cookies.Add(new Keyval<String, String> { Key = key, Value = value });
        }
    }
}
