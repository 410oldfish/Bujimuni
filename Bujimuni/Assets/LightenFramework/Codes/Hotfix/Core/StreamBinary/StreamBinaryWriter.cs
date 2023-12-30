using System;
using System.Net;
using System.Text;
using UnityEngine;

namespace Lighten
{
    // 二进制流的写
    public class StreamBinaryWriter : IDisposable
    {
        private byte[] m_buffer;
        private int m_offset;
        private int m_capacity;
        private int m_growSize;

        public StreamBinaryWriter(int capacity = 1024, int growSize = 1024)
        {
            m_capacity = capacity;
            m_growSize = growSize;
            m_buffer = new byte[capacity];
            m_offset = 0;
        }
        
        public void Dispose()
        {
            m_buffer = null;
            m_offset = 0;
        }
        
        public new string ToString()
        {
            return Convert.ToBase64String(m_buffer);
        }
        public byte[] GetData()
        {
            if (m_offset < 1)
                return null;
            var data = new byte[m_offset];
            Array.Copy(m_buffer, 0, data, 0, m_offset);
            return data;
        }
        public void Reset()
        {
            m_offset = 0;
        }
        public void WriteByte(byte value)
        {
            if (m_offset + 1 > m_buffer.Length)
            {
                GrowSize(1);
            }
            m_buffer[m_offset++] = value;
        }
        public void WriteInt16(short value)
        {
            if (m_offset + 2 > m_buffer.Length)
            {
                GrowSize(2);
            }
            short temp = IPAddress.HostToNetworkOrder(value);
            byte[] bytes = BitConverter.GetBytes((ushort)temp);

            m_buffer[m_offset++] = bytes[0];
            m_buffer[m_offset++] = bytes[1];
        }
        public void WriteInt32(int value)
        {
            if (m_offset + 4 > m_buffer.Length)
            {
                GrowSize(4);
            }
            int temp = IPAddress.HostToNetworkOrder(value);
            byte[] bytes = BitConverter.GetBytes((uint)temp);

            m_buffer[m_offset++] = bytes[0];
            m_buffer[m_offset++] = bytes[1];
            m_buffer[m_offset++] = bytes[2];
            m_buffer[m_offset++] = bytes[3];
        }
        public void WriteFloat(float value)
        {
            if (m_offset + 4 > m_buffer.Length)
            {
                GrowSize(4);
            }
            byte[] bytes = BitConverter.GetBytes(value);
            m_buffer[m_offset++] = bytes[0];
            m_buffer[m_offset++] = bytes[1];
            m_buffer[m_offset++] = bytes[2];
            m_buffer[m_offset++] = bytes[3];
        }
        public void WriteString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteInt32(0);
                return;
            }
            var bytes = Encoding.UTF8.GetBytes(value);
            int len = bytes.Length;
            WriteInt32(len);
            if (m_offset + len > m_buffer.Length)
            {
                GrowSize(len);
            }
            Array.Copy(bytes, 0, m_buffer, m_offset, len);
            m_offset += len;
        }

        private void GrowSize(int size)
        {
            //
            int length = m_buffer.Length;
            while (m_offset + size > length)
            {
                length += m_growSize;
            }
            //
            byte[] buffer = new byte[length];
            Array.Copy(m_buffer, 0, buffer, 0, m_offset);
            m_buffer = buffer;
        }
    }


}
