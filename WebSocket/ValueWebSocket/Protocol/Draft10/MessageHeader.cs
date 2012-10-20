using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueWebSocket.Protocol.Draft10
{
    public class MessageHeader
    {
        public Char FIN;
        public Char RSV1;
        public Char RSV2;
        public Char RSV3;
        public OperType Opcode;
        public Char MASK;
        public UInt64 Payloadlen;
        public Byte[] Maskey;
        public Int32 PayloadDataStartIndex;
    }
}
