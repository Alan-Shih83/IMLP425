using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{

    public interface ICreater<T>
    {
        T Pull(params object[] param);
        void Push(T _T);
        void Clear();
    }

    public interface ICreator
    {
        bool Reuse(params object[] param);
        void Release();
    }

    public abstract class Creater<T> : ICreater<T> where T : ICreator
    {
        HashSet<T> Creators = new HashSet<T>();

        readonly object Lock = new object();

        private T GetCreator<U>(params object[] param) where U : T
        {
            lock (Lock)
            {
                T creator = Creators.FirstOrDefault(_creator => _creator.GetType() == typeof(U));
                if (!Equals(creator, default))
                {
                    Creators.Remove(creator);
                    if (!creator.Reuse(param))
                        creator = default;
                }

                if (Equals(creator, default))
                    creator = (U)Activator.CreateInstance(typeof(U), param);

                return creator;
            }
        }

        public T Pull(params object[] param)
        {
            return GetCreator<T>(param);
        }

        public T Pull<U>(params object[] param) where U : T
        {
            return GetCreator<U>(param);
        }

        public void Push(T creator)
        {
            lock (Lock)
            {
                if (!Creators.Contains(creator))
                {
                    creator.Release();
                    Creators.Add(creator);
                }
            }
        }

        public void Clear()
        {
            lock (Lock)
            {
                Creators.Clear();
            }
        }

        public int Count()
        {
            lock (Lock)
            {
                return Creators.Count;
            }
        }

        ~Creater()
        {
            Creators.Clear();
            //IEnumerator enumerator = Creators.GetEnumerator();
            //while(enumerator.MoveNext())
            //{
            //    try
            //    {
            //        (enumerator.Current as ICreator).Release();
            //    }
            //    catch (Exception) { }
            //}
        }
    }

 
}
