using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Lighten
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IdStruct
    {
        public uint Time; // 30bit
        public int Process; // 18bit
        public ushort Value; // 16bit

        public long ToLong()
        {
            ulong result = 0;
            result |= this.Value;
            result |= (ulong)this.Process << 16;
            result |= (ulong)this.Time << 34;
            return (long)result;
        }

        public IdStruct(uint time, int process, ushort value)
        {
            this.Process = process;
            this.Time = time;
            this.Value = value;
        }

        public IdStruct(long id)
        {
            ulong result = (ulong)id;
            this.Value = (ushort)(result & ushort.MaxValue);
            result >>= 16;
            this.Process = (int)(result & IdGenerater.Mask18bit);
            result >>= 18;
            this.Time = (uint)result;
        }

        public override string ToString()
        {
            return $"process: {this.Process}, time: {this.Time}, value: {this.Value}";
        }
    }

    public static class IdGenerater
    {
        public const int Mask18bit = 0x03ffff;

        private static long m_epoch1970;
        private static long m_epoch2020;
        private static ushort m_value;
        private static uint m_lastIdTime;

        static IdGenerater()
        {
            m_epoch1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
            m_epoch2020 = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000 - m_epoch1970;
            
            m_lastIdTime = TimeSince2020();
            if (m_lastIdTime <= 0)
            {
                Debug.LogWarning($"lastIdTime less than 0: {m_lastIdTime}");
                m_lastIdTime = 1;
            }
        }
        
        private static uint TimeSince2020()
        {
            var currentTime = DateTime.UtcNow.Ticks / 10000 - m_epoch1970;
            return (uint)((currentTime - m_epoch2020) / 1000);
        }
        
        public static long GenerateId()
        {
            uint time = TimeSince2020();

            if (time > m_lastIdTime)
            {
                m_lastIdTime = time;
                m_value = (ushort)(time & 1);
            }
            else
            {
                ++m_value;

                if (m_value > ushort.MaxValue - 1)
                {
                    m_value = 0;
                    ++m_lastIdTime; // 借用下一秒
                    Debug.LogWarning($"id count per sec overflow: {time} {m_lastIdTime}");
                }
            }

            IdStruct idStruct = new IdStruct(m_lastIdTime, 0, m_value);
            return idStruct.ToLong();
        }
    }
}