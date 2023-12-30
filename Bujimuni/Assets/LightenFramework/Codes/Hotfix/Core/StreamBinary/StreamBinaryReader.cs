using System;
using System.Net;
using System.Text;

namespace Lighten
{

    // 二进制流的读
    public class StreamBinaryReader : IDisposable
    {
        private byte[] m_buffer;
        private int m_offset;

        public StreamBinaryReader()
        {
            m_offset = 0;
        }
        public void Dispose()
        {
            m_buffer = null;
            m_offset = 0;
        }
        public void SetBytes(byte[] data)
        {
            m_buffer = data;
        }
        public void Reset()
        {
            m_offset = 0;
        }

        public byte ReadByte()
        {
            if (m_offset + 1 > m_buffer.Length)
                return 0;
            return m_buffer[m_offset++];
        }
        public short ReadInt16()
        {
            if (m_offset + 2 > m_buffer.Length)
                return 0;
            short value = (short)BitConverter.ToUInt16(m_buffer, m_offset);
            m_offset += 2;
            return IPAddress.NetworkToHostOrder(value);
        }
        public int ReadInt32()
        {
            if (m_offset + 4 > m_buffer.Length)
                return 0;
            int value = (int)BitConverter.ToUInt32(m_buffer, m_offset);
            m_offset += 4;
            return IPAddress.NetworkToHostOrder(value);
        }
        public float ReadFloat()
        {
            if (m_offset + 4 > m_buffer.Length)
                return 0;
            float value = BitConverter.ToSingle(m_buffer, m_offset);
            m_offset += 4;
            return value;
        }
        public string ReadString()
        {
            int len = ReadInt32();
            if (len < 1)
                return string.Empty;
            var bytes = new byte[len];
            Array.Copy(m_buffer, m_offset, bytes, 0, len);
            m_offset += len;
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
