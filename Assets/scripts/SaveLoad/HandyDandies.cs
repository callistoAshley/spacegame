using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame.SaveLoad
{
    public static class HandyDandies
    {
        public static char GetTypeCode(Type type)
        {
            // this should be all i'll need to serialize
            Dictionary<Type, char> typeCodes = new Dictionary<Type, char>
            {
                {typeof(int), 'I' },
                {typeof(float), 'F' },
                {typeof(string), 'S' },
                {typeof(bool), 'B' },
                {typeof(Vector2), 'V' },
                {typeof(Dictionary<string, bool>), 'D' },
                {typeof(Dictionary<string, int>), 'd' },
                {typeof(Follower), 'f' }
            };

            if (!typeCodes.TryGetValue(type, out char code))
                throw new Exception($"no type code for type \"{type.Name}\"");
            return code;
        }
    }
}
