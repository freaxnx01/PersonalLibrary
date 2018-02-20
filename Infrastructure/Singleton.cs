using System;
using System.Reflection;

namespace Infrastructure
{
    public class Singleton<T> where T : class
    {
        // volatile: ensure that assignment to the instance variable completes before the instance variable can be accessed.
        private static volatile T instance;
        
        private static object syncRoot = new object();

        public static T Instance
        {
            get
            {
                // Double-checked locking
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            //- Public
                            ConstructorInfo ctorInfo = typeof(T).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                            if (ctorInfo == null)
                            {
                                throw new InvalidOperationException("Class must contain a private constructor.");
                            }

                            instance = (T)ctorInfo.Invoke(null);
                        }
                    }
                }

                return instance;
            }
        }
    }
}
