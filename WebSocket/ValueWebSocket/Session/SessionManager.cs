using System;
using System.Net.Sockets;
using System.Collections.Generic;
using ValueWebSocket.Infrastructure;
using ValueWebSocket.Protocol;

namespace ValueWebSocket.Session
{
    public class SessionManager
    {
        private static List<ValueSession> sessions;
        public static List<ValueSession> Sessions { get { return sessions; } }

        private const String cookiesPattern = @"Cookie:\s+(?<cookies>.*)\r\n";

        public SessionManager()
        {
            sessions = new List<ValueSession>();
        }

        public void UpdateSession()
        {
            List<Int32> indexs = new List<Int32>();
            for (int i = 0; i < sessions.Count; i++)
            {
                if (!sessions[i].Socket.Connected)
                    indexs.Add(i);
            }

            foreach (Int32 index in indexs)
            {
                sessions.RemoveAt(index);
            }
        }

        public Boolean CheckSessionExist(Socket socket)
        {
            UpdateSession();

            foreach (ValueSession session in sessions)
            {
                if (session.Socket.RemoteEndPoint == socket.RemoteEndPoint)
                    return true;
            }
            return false;
        }

        public void AddSession(Socket socket, String request)
        {
            ValueSession session = new ValueSession(socket);
            String match = Common.GetRegexValue(request, cookiesPattern)[0].Groups["cookies"].Value;
            String[] cookies = match.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String cook in cookies)
            {
                String[] keyval = cook.Split('=');
                session.AddCookies(keyval[0].Trim(), keyval[1]);
            }
            sessions.Add(session);
        }

        public void RemoveSession(Socket socket)
        {
            for (int i = 0; i < sessions.Count; i++)
            {
                Socket sk = sessions[i].Socket;
                if (sk.RemoteEndPoint == socket.RemoteEndPoint)
                {
                    sessions.RemoveAt(i);
                    break;
                }
            }
        }

        public ValueSession GetSession(String cookiesKey, String cookiesValue)
        {
            foreach (ValueSession session in sessions)
            {
                if (session.Cookies[cookiesKey] == cookiesValue)
                    return session;
            }

            return null;
        }
    }
}
