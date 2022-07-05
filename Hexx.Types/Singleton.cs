using System;
using System.Diagnostics;
using System.Linq;

namespace Hexx.Types
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        protected static T instance;

        protected Singleton() : this(false)
        {
        }

        protected Singleton(bool customSingleton)
        {
            if (!customSingleton)
            {
                StackTrace stackTrace = new StackTrace();
                if (!stackTrace.GetFrames().Any(p => p.GetMethod().Name == "GetInstance" && p.GetMethod().DeclaringType.Name.StartsWith("Singleton")))
                    throw new Exception("Use GetInstance method to create an instance of the singleton.");
            }
        }

        public static T GetInstance()
        {
            if (instance == null)
                instance = (T)Activator.CreateInstance(typeof(T), new object[] { });
            return instance;
        }
    }
}
