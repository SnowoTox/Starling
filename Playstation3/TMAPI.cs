/*
 * 
 * Written by Starman. Copyright (C) 2019.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starling.TMAPI
{
    /// <summary>
    /// Provides easier syntax for the Target Manager API.
    /// </summary>
    public class TMAPI
    {
        private struct SysInfos
        {
            public static uint[] ProcessList;
            public static uint ProcessID;
            public static int Target = 0xFF;

            public static PS3TMAPI.ProcessInfo procinfo;
            public static PS3TMAPI.ConnectStatus status;
            
        }

        /// <summary>
        /// Returns true when a function has been executed successfully.
        /// </summary>
        /// <param name="Function"></param>
        /// <returns></returns>
        public bool Succeeded(int Function)
        {
            if (Function == 0)
                return true;
            else return false;
        }

        /// <summary>
        /// Returns true when a function has not been executed successfully.
        /// </summary>
        /// <param name="Function"></param>
        /// <returns></returns>
        public bool Failed(int Function)
        {
            if (!Succeeded(Function))
                return true;
            else return false;
        }

        #region BASIC COMMANDS

        /// <summary>
        /// Initializes TMAPI for debugging, modding etc.
        /// </summary>
        /// <returns></returns>
        public int InitTargetComms()
        {
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.InitTargetComms()))
                return 0;
            else return -1;
        }

        /// <summary>
        /// Closes TMAPI connections and activities. This function does not return.
        /// </summary>
        public void CloseTargetComms()
        {
            PS3TMAPI.CloseTargetComms();
        }

        /// <summary>
        /// Connects to the specified target.
        /// </summary>
        /// <param name="TargetIndex"></param>
        /// <returns></returns>
        public int ConnectTarget(int TargetIndex = 0)
        {
            SysInfos.Target = TargetIndex;
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.Connect(SysInfos.Target, null)))
                return 0;
            else return -1;
        }

        /// <summary>
        /// Connects to the specified target. pszApplication must be null if you want a regular connection.
        /// </summary>
        /// <param name="TargetIndex"></param>
        /// <param name="pszApplication"></param>
        /// <returns></returns>
        public int ConnectTarget(int TargetIndex = 0, string pszApplication = null)
        {
            SysInfos.Target = TargetIndex;
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.Connect(SysInfos.Target, pszApplication)))
                return 0;
            else return -1;
        }

        public int DisconnectTarget()
        {
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.Disconnect(SysInfos.Target)))
            {
                SysInfos.Target = 0xFF;
                return 0;
            }
            else return -1;
        }

        public int PowerOff(bool forced)
        {
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.PowerOff(SysInfos.Target, forced)))
                return 0;
            else return -1;
        }

        public int PowerOn(int Target)
        {
            SysInfos.Target = Target;
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.PowerOn(SysInfos.Target)))
                return 0;
            else return -1;
        }

        public int AttachProcess()
        {
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.GetProcessList(SysInfos.Target, out SysInfos.ProcessList)))
            {
                if (SysInfos.ProcessList.Length > 0)
                {
                    UInt64 uProc = SysInfos.ProcessList[0];
                    UInt32 ProcID = Convert.ToUInt32(uProc);
                    SysInfos.ProcessID = ProcID;

                    if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessAttach(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID)))
                        return 0;
                    else return -1;
                }
                else return -1;
            }
            else return -1;
        }

        /// <summary>
        /// Attach to the a process.
        /// </summary>
        /// <param name="processID"></param>
        /// <returns></returns>
        public int AttachProcess(UInt32 processID)
        {
            Int32 result = -1;
            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.GetProcessList(SysInfos.Target, out SysInfos.ProcessList)))
            {
                if (SysInfos.ProcessList.Length > 0)
                {
                    for (Int32 i = 0; i < SysInfos.ProcessList.Length; i++)
                    {
                        if (SysInfos.ProcessList[i] == processID)
                        {
                            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessAttach(SysInfos.Target, PS3TMAPI.UnitType.PPU, processID)))
                            {
                                result = 0;
                                break;
                            }
                            else result = -1;
                            break;
                        }
                    }
                    if (SysInfos.ProcessList[0] == processID)
                    {
                        result = 0;
                    }
                    else result = -1;
                }
                else result = -1;
            }
            else result = -1;

            return result;
        }


        public enum ProcessInfo
        {
            ParentProcessID,
            ELFPath,
            AmountOfSPUThreads,
            AmountOfPPUThreads,
            Status,
            MaxMemorySize
        }


        /// <summary>
        /// Returns the process info. Depending on what you select.
        /// </summary>
        /// <returns></returns>
        public string GetProcessInfo(ProcessInfo procInfo)
        {
            string output = String.Empty;

            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.GetProcessInfo(SysInfos.Target, SysInfos.ProcessID, out SysInfos.procinfo)))
            {
                if (procInfo == ProcessInfo.AmountOfPPUThreads)
                {
                    output = Convert.ToString(SysInfos.procinfo.Hdr.NumPPUThreads) + " PPU Threads";
                }
                else if (procInfo == ProcessInfo.AmountOfSPUThreads)
                {
                    output = Convert.ToString(SysInfos.procinfo.Hdr.NumSPUThreads) + " SPU Threads";
                }
                else if (procInfo == ProcessInfo.ELFPath)
                {
                    output = SysInfos.procinfo.Hdr.ELFPath;
                }
                else if (procInfo == ProcessInfo.ParentProcessID)
                {
                    output = Convert.ToString(SysInfos.procinfo.Hdr.ParentProcessID);
                }
                else if (procInfo == ProcessInfo.MaxMemorySize)
                {
                    output = Convert.ToString(SysInfos.procinfo.Hdr.MaxMemorySize);
                }
                else if (procInfo == ProcessInfo.Status)
                {
                    output = Convert.ToString(SysInfos.procinfo.Hdr.Status);
                }

                return output;
            }
            else return "Failed to load " + procInfo;
        }

        public string GetConnectionStatus()
        {
            string usage = String.Empty;

            if (PS3TMAPI.SUCCEEDED(PS3TMAPI.GetConnectStatus(SysInfos.Target, out SysInfos.status, out usage)))
                return SysInfos.status.ToString();
            return "Unknown";
        }
        
        #endregion


        /// <summary>
        /// Provides functions to write values to the memory and read values from the memory. This also includes extended functions such as writing/reading with several datatypes.
        /// </summary>
        public class Memory
        {
            /// <summary>
            /// Writes an array of buffers in the memory. This will write into the PPU.
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="buffer"></param>
            /// <returns></returns>
            public int SetMemory(ulong Offset, byte[] buffer)
            {
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a string to the buffer of the specified Offset. PPU only.
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_String(ulong Offset, string Input)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(Input);
                Array.Resize(ref buffer, buffer.Length + 1);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a signed byte (8 bit) to the buffer of the specified Offset. PPU only.
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_SByte(ulong Offset, sbyte Input)
            {
                byte[] buffer = new byte[1];
                buffer[0] = (byte)Input;
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a short integer (16 bit) to the buffer of the Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_Int16(ulong Offset, short Input)
            {
                byte[] buffer = new byte[2];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 2);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes an integer (32 bit) to the buffer of the Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_Int32(ulong Offset, int Input)
            {
                byte[] buffer = new byte[4];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 4);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a long integer (64 bit) to the buffer of the Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_Int64(ulong Offset, long Input)
            {
                byte[] buffer = new byte[8];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 8);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes an unsigned short integer (16 bit) to the buffer of the Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_UInt16(ulong Offset, ushort Input)
            {
                byte[] buffer = new byte[2];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 2);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes an unsigned integer (32 bit) to the buffer of the specified Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_UInt32(ulong Offset, uint Input)
            {
                byte[] buffer = new byte[4];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 4);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes an unsigned long integer (64 bit) to the buffer of the specified Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_UInt64(ulong Offset, ulong Input)
            {
                byte[] buffer = new byte[8];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 8);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a float to the buffer of the specified Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_Float(ulong Offset, float Input)
            {
                byte[] buffer = new byte[4];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 4);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a double in the buffer of the specified Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_Double(ulong Offset, double Input)
            {
                byte[] buffer = new byte[8];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 8);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a byte in the buffer of the specified Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_Byte(ulong Offset, byte Input)
            {
                byte[] buffer = new byte[1];
                BitConverter.GetBytes(Input).CopyTo(buffer, 0);
                Array.Reverse(buffer, 0, 1);
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes a boolean (true or false) in the buffer of the specified Offset. PPU only
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_Bool(ulong Offset, bool Input)
            {
                byte[] buffer = new byte[1];
                buffer[0] = Input ? (byte)1 : (byte)0;
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, buffer)))
                    return 0;
                else return -1;
            }

            /// <summary>
            /// Writes an array of floats in the buffer of the specified Offset. PPU only.
            /// </summary>
            /// <param name="Offset"></param>
            /// <param name="Input"></param>
            /// <returns></returns>
            public int SetMemory_FloatArray(ulong Offset, float[] Input)
            {
                int result = -1;
                byte[] buffer = new byte[4];
                for(int i = 0; i < Input.Length; i++)
                {
                    BitConverter.GetBytes(Input[i]).CopyTo(buffer, 0);
                    Array.Reverse(buffer, 0, 4);
                    if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessSetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset + ((uint)i * 4), buffer)))
                        result = 0;
                    else result = -1;
                }
                return result;
            }

            public int GetMemory(ulong Offset, byte[] buffer)
            {
                if (PS3TMAPI.SUCCEEDED(PS3TMAPI.ProcessGetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, ref buffer)))
                    return 0;
                else return -1;
            }

            public byte[] GetBytes(ulong Offset, uint length)
            {
                byte[] buffer = new byte[length];
                PS3TMAPI.ProcessGetMemory(SysInfos.Target, PS3TMAPI.UnitType.PPU, SysInfos.ProcessID, 0, Offset, ref buffer);
                return buffer;
            }

            public short GetMemory_Int16(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 2);
                Array.Reverse(buffer, 0, 2);
                return BitConverter.ToInt16(buffer, 0);
            }

            public int GetMemory_Int32(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 4);
                Array.Reverse(buffer, 0, 4);
                return BitConverter.ToInt32(buffer, 0);
            }

            public long GetMemory_Int64(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 8);
                Array.Reverse(buffer, 0, 8);
                return BitConverter.ToInt64(buffer, 0);
            }

            public ushort GetMemory_UInt16(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 2);
                Array.Reverse(buffer, 0, 2);
                return BitConverter.ToUInt16(buffer, 0);
            }

            public uint GetMemory_UInt32(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 4);
                Array.Reverse(buffer, 0, 4);
                return BitConverter.ToUInt32(buffer, 0);
            }

            public ulong GetMemory_UInt64(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 8);
                Array.Reverse(buffer, 0, 8);
                return BitConverter.ToUInt64(buffer, 0);
            }

            public float GetMemory_Float(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 4);
                Array.Reverse(buffer, 0, 4);
                return BitConverter.ToSingle(buffer, 0);
            }

            public double GetMemory_Double(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 8);
                Array.Reverse(buffer, 0, 8);
                return BitConverter.ToDouble(buffer, 0);
            }

            public string GetMemory_String(ulong Offset)
            {
                int blocksize = 40;
                int scalesize = 0;
                string str = string.Empty;

                while(!str.Contains('\0'))
                {
                    byte[] buffer = GetBytes(Offset + (uint)scalesize, (uint)blocksize);
                    str += Encoding.UTF8.GetString(buffer);
                    scalesize += blocksize;
                }

                return str.Substring(0, str.IndexOf('\0'));
            }

            public float[] GetMemory_FloatArray(ulong Offset, int ArrayLength = 3)
            {
                float[] vector = new float[ArrayLength];

                for(int i = 0; i < ArrayLength; i++)
                {
                    byte[] buf = GetBytes(Offset + (uint)i * 4, 4);
                    Array.Reverse(vector, 0, 4);
                    vector[i] = BitConverter.ToSingle(buf, 0);
                }

                return vector;
            }

            public byte GetMemory_Byte(ulong Offset)
            {
                byte[] buffer = GetBytes(Offset, 1);
                return buffer[0];
            }
        }
    }
}
