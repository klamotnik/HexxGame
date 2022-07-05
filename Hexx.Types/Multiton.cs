using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Hexx.Types
{
    public abstract class Multiton<K, V> where V : Multiton<K, V>
    {
        private static Dictionary<K, V> instances = new Dictionary<K, V>();

        protected Multiton()
        {
            StackTrace stackTrace = new StackTrace();
            if(!stackTrace.GetFrames().Any(p => p.GetMethod().Name == "GetInstance" && p.GetMethod().DeclaringType.Name.StartsWith("Multiton")))
                throw new Exception("Use GetInstance method to create an instance of the multiton.");
        }

        public static V GetInstance(K key)
        {
            V instance;
            if (instances.ContainsKey(key))
                instance = instances[key];
            else
                instances.Add(key, (instance = (V)Activator.CreateInstance(typeof(V), new object[] { })));
            return instance;
        }
    }
}
