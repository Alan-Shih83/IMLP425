using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public class ProcessManager
    {
        Storage<IProcessContainer> containers = new Storage<IProcessContainer>();

        private static readonly Lazy<ProcessManager> singleton = new Lazy<ProcessManager>(() => new ProcessManager(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        private ProcessManager() { }
        public static ProcessManager Instance { get { return singleton.Value; } }

        public void attach(IProcessContainer container)
        {
            containers.SetRecord(container);
        }

        public IProcessContainer Query<T>() where T : IProcessContainer
        {
            return containers.QueryItem<T>();
        }
    }
}
