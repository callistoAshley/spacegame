using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;

namespace spacegame.SaveLoad
{
    public class SaveWriter : BinaryWriter, IDisposable
    {
        public SaveWriter(Stream output) : this(output, Encoding.UTF8, false) { }

        public SaveWriter(Stream output, Encoding encoding) : this(output, encoding, false) { }

        public SaveWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
            // go ahead and write the header
            WriteHeader();
        }

        public void Dump(object obj, string name)
        {
            char code = HandyDandies.GetTypeCode(obj.GetType());
            Write(code);
            Write(name);

            switch (code)
            {
                // 32-bit signed integer
                case 'I':
                    Write((int)obj);
                    break;
                // float
                case 'F':
                    Write((float)obj);
                    break;
                // string
                case 'S':
                    Write(obj.ToString());
                    break;
                // bool
                case 'B':
                    Write((bool)obj);
                    break;
                case 'V':
                    Vector2 v = (Vector2)obj;
                    Write(v.x);
                    Write(v.y);
                    break;
                // Dictionary<string, bool>
                case 'D':
                    Dictionary<string, bool> dictionary = (Dictionary<string, bool>)obj;
                    foreach (string key in dictionary.Keys)
                    {
                        Write(key);
                        Write(dictionary[key]);
                    }
                    // end the dictionary by writing a null byte
                    Write(0x0);
                    break;
                // Dictionary<string, int>
                case 'd':
                    Dictionary<string, int> dictionary2 = (Dictionary<string, int>)obj;
                    foreach (string key in dictionary2.Keys)
                    {
                        Write(key);
                        Write(dictionary2[key]);
                    }
                    // end the dictionary by writing a null byte
                    Write(0x0);
                    break;
                // follower
                case 'f':
                    // the save reader will interpret this by instantiating the prefab with the corresponding name,
                    // and adding it as a follower
                    Write(((Follower)obj).gameObject.name + "--follower");
                    break;
                default:
                    throw new Exception("can you not");
            }
        }

        private void WriteHeader()
        {
            // the header contains the ascii string "spacegamesave," followed immediately by the version
            Write(Encoding.ASCII.GetBytes(Constants.Meta.SAVE_HEADER));
            Write(Constants.Meta.VERSION);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            Close();
        }
    }
}
