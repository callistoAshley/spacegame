using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame.SaveLoad
{
    public class SaveReader : BinaryReader, IDisposable
    {
        private Dictionary<string, object> stuffInTheSaveFile = new Dictionary<string, object>();

        public SaveReader(Stream output) : this(output, Encoding.UTF8, false) { }

        public SaveReader(Stream output, Encoding encoding) : this(output, encoding, false) { }

        public SaveReader(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
            
        }

        public bool VerifyHeader(out byte[] header)
        {
            // this should be "spacegamesave" in ascii
            header = ReadBytes(13);
            return header.SequenceEqual(Encoding.ASCII.GetBytes(Constants.Meta.SAVE_HEADER)); 
        }

        public string GetVersion(bool resetPosition)
        {
            // get the position before setting it 
            long oldPosition = BaseStream.Position;
            BaseStream.Position = 13;
            string ret = ReadString();
            // reset the stream position back to what it was before
            if (resetPosition) BaseStream.Position = oldPosition;
            return ret;
        }

        public object GetObject(string name)
        {
            if (stuffInTheSaveFile.Keys.Count == 0)
                throw new Exception("save file hasn't been read yet");
            if (!stuffInTheSaveFile.TryGetValue(name, out object value))
                throw new Exception($"no object in the save file with name {name}");
            return value;
        }

        public T[] GetObjectsOfType<T>()
        {
            IEnumerable<T> stuff = (from obj in stuffInTheSaveFile.Values where obj.GetType() is T select obj).Cast<T>();
            return stuff.ToArray();
        }

        // this needs to be called manually before any GetObject or GetObjectsOfType calls
        public void ReadTheStuff()
        {
            while (BaseStream.Position < BaseStream.Length)
            {
                char code = ReadChar();
                switch (code)
                {
                    // int32
                    case 'I':
                        stuffInTheSaveFile.Add(ReadString(), ReadInt32());
                        break;
                    // float
                    case 'F':
                        stuffInTheSaveFile.Add(ReadString(), ReadSingle());
                        break;
                    // string
                    case 'S':
                        stuffInTheSaveFile.Add(ReadString(), ReadString());
                        break;
                    // bool
                    case 'B':
                        stuffInTheSaveFile.Add(ReadString(), ReadBoolean());
                        break;
                    // vector2
                    case 'V':
                        stuffInTheSaveFile.Add(ReadString(), new Vector2(ReadSingle(), ReadSingle()));
                        break;
                    // Dictionary<string, bool>
                    case 'D':
                        Dictionary<string, bool> dict = new Dictionary<string, bool>();
                        string dictName = ReadString();

                        // the dictionary ends at a null byte, so keep checking if the byte ahead is one
                        while (PeekChar() != 0x0)
                        {
                            dict.Add(ReadString(), ReadBoolean());
                        }
                        // then advance the stream position forward one byte
                        ReadByte();

                        stuffInTheSaveFile.Add(dictName, dict);
                        break;
                    // Dictionary<string, int>
                    case 'd':
                        Dictionary<string, int> dict2 = new Dictionary<string, int>();
                        string dictName2 = ReadString();

                        // the dictionary ends at a null byte, so keep checking if the byte ahead is one
                        while (PeekChar() != 0x0)
                        {
                            dict2.Add(ReadString(), ReadInt32());
                        }
                        // then advance the stream position forward one byte
                        ReadByte();

                        stuffInTheSaveFile.Add(dictName2, dict2);
                        break;
                    // followers
                    case 'f':
                        string name = ReadString();
                        GameObject prefab = null;

                        // the next string will contain the name of the follower
                        switch (ReadString())
                        {
                            // just add sam by default
                            // prevents players from being naughty and changing any old npc to a follower,
                            // which would break the save file
                            default:
                                prefab = PrefabManager.instance.GetPrefab("microwave man sam--follower");
                                break;
                        }

                        stuffInTheSaveFile.Add(name, prefab);
                        break;
                    default:
                        //throw new Exception($"unrecognized type code \"{code}\" at stream position {BaseStream.Position}");
                        break;
                }
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            Close();
        }
    }
}
