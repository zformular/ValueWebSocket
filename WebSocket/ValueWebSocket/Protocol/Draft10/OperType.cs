using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueWebSocket.Protocol.Draft10
{
    public enum OperType
    {
        Row = 0x0,
        Text = 0x1,
        Binary = 0x2,
        Close = 0x8,
        Ping = 0x9,
        Pong = 0xA,
        Unkown
    };
}
