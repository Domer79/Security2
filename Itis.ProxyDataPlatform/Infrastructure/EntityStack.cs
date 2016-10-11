using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Itis.ProxyDataPlatform.Infrastructure
{
    public class EntityStack
    {
        private readonly Dictionary<object, object> _heap = new Dictionary<object, object>();
        private static readonly Dictionary<Thread, EntityStack> StacksOfThreadDictionary = new Dictionary<Thread, EntityStack>();
        private static readonly object Locker = new object();
        private void _Push(object key, object value)
        {
            if (_heap.ContainsKey(key))
            {
                _heap[key] = value;
                return;
            }

            _heap.Add(key, value);
        }

        private void _Pop()
        {
            _heap.Remove(_heap.Keys.Last());
        }

        private object _GetEntity()
        {
            return _heap[_heap.Keys.Last()];
        }

        private object _GetEntity(object key)
        {
            return _heap[key];
        }

        private bool _ExistKey(object key)
        {
            return _heap.ContainsKey(key);
        }

        private void _Set(object key, object value)
        {
            _heap[key] = value;
        }

        #region Static Singletone Members

        private static EntityStack Instance
        {
            get
            {
//                return _instance ?? (_instance = new EntityStack());
                lock (Locker)
                {
                    if (!StacksOfThreadDictionary.ContainsKey(Thread.CurrentThread))
                        StacksOfThreadDictionary.Add(Thread.CurrentThread, new EntityStack());

                    return StacksOfThreadDictionary[Thread.CurrentThread];
                }
            }
        }

        public static void Push(object key, object value)
        {
            Instance._Push(key, value);
        }

        public static void Pop()
        {
            Instance._Pop();
        }

        public static bool ExistKey(object key)
        {
            return Instance._ExistKey(key);
        }

        public static object GetEntity()
        {
            return Instance._GetEntity();
        }

        public static object GetEntity(object key)
        {
            return Instance._GetEntity(key);
        }

        public static void Set(object key, object value)
        {
            Instance._Set(key, value);
        }

        #endregion
    }
}
