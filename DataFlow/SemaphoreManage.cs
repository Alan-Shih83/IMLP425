using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{

    public class SemaphoreManage
    {
        private Semaphore semaphore;

        private volatile int sharedStorage = 0;

        private object Lock = new object();

        public SemaphoreManage(int initialCount, int maximumCount)
        {
            semaphore = new Semaphore(initialCount, maximumCount);
        }

        ~SemaphoreManage()
        {
            semaphore.Dispose();
        }

        public void WaitOne()
        {
            semaphore.WaitOne();
            lock(Lock)
            {
                Interlocked.Increment(ref sharedStorage);
            }
            
        }

        public void Release(int releaseCount = 1)
        {
            try
            {
                lock(Lock)
                {
                    if (releaseCount - sharedStorage >= 0 && sharedStorage > 0)
                    {
                        semaphore.Release(sharedStorage);
                        Interlocked.Exchange(ref sharedStorage, 0);
                    }
                    else if (releaseCount < sharedStorage && releaseCount > 0)
                    {
                        Interlocked.Exchange(ref sharedStorage, sharedStorage - releaseCount);
                        semaphore.Release(releaseCount);
                    }
                }
            }
            catch (Exception ex) 
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + ex.ToString());
            }
        }

    }
}
