﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace demoinfosharp
{
    public class BitArrayStream
    {
        BitArray array;

		public int Position { get; private set; }

        public BitArrayStream(byte[] data)
        {
            array = new BitArray(data);
			Position = 0;
        }

        public void Seek(int pos, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                Position = pos;

            if (origin == SeekOrigin.Current)
                Position += pos;

            if (origin == SeekOrigin.End)
                Position = array.Count - pos;
        }

        public uint ReadInt(int numBits)
        {
            uint result = PeekInt(numBits);
            Position += numBits;

            return result;
        }

        public int ReadSignedInt(int numBits)
        {
            int nRet = (int)ReadInt(numBits);


            return (nRet << (32 - numBits)) >> (32 - numBits);
        }

        public uint PeekInt(int numBits)
        {
            uint result = 0;
            int intPos = 0;

            for (int i = 0; i < numBits; i++)
            {
                result |= ((array[i + Position] ? 1u : 0u) << intPos++);
            }

            return result;
        }

        public bool ReadBit()
        {
            return ReadInt(1) == 1;
        }

        public byte ReadByte()
        {
            return (byte)ReadInt(8);
        }

        public byte ReadByte(int numBits)
        {
            
            return (byte)ReadInt(numBits);
        }

        public byte[] ReadBytes(int length)
        {
            byte[] result = new byte[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = this.ReadByte();
            }

            return result;
        }

        public string ReadString(int size)
        {
            List<byte> result = new List<byte>(512);
            int pos=0;
            while (true)
            {
                byte a = ReadByte();
                if (a == 0)
                    break;
                else if (a == 10)
                    break;

                result.Add(a);

                if (++pos == size)
                {
                    break;
                }
            }

            return Encoding.ASCII.GetString(result.ToArray());
        }

        public string ReadString()
        {
            return ReadString(int.MaxValue);
        }

        public uint ReadVarInt()
        {
            int count = 0;
            uint result = 0;

            while(true)
            {
                if (count > 5)
                    throw new InvalidDataException("VarInt32 out of range");


                uint tmpByte = ReadByte();

                result |= (tmpByte & 0x7F) << (7 * count);


                if ((tmpByte & 0x80) == 0)
                    break;

                count++;
            }

            return result;
        }

        public uint ReadUBitInt()
        {
            uint ret = ReadInt(6);
            switch (ret & (16 | 32))
            {
                case 16:
                    ret = (ret & 15) | (ReadInt(4) << 4);
                    break;

                case 32:
                    ret = (ret & 15) | (ReadInt(8) << 4);
                    break;
                case 48:
                    ret = (ret & 15) | (ReadInt(32 - 4) << 4);
                    break;
            }
            return ret;
        }

        public string PeekBools(int length)
        {
            byte[] buffer = new byte[length];

            int idx = 0;
            for (int i = Position; i < Math.Min(Position + length, array.Count); i++)
            {
                if (array[i])
                    buffer[idx++] = 49;
                else
                    buffer[idx++] = 48;
            }

            return Encoding.ASCII.GetString(buffer, 0, Math.Min(length, array.Count - Position));
        }

        
    }
}
