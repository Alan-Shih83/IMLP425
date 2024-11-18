using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public class Repository
    {
        Storage<RepositoryMessage> storage = new Storage<RepositoryMessage>(17);

        private static readonly Lazy<Repository> singleton = new Lazy<Repository>(() => new Repository(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        private Repository() { }
        public static Repository Instance { get { return singleton.Value; } }
        public bool ADD(RepositoryMessage repository)
        {
            if (DEL(repository))
            {
                FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewInsertMsg("RepositoryView", repository.Format()));
                return storage.SetRecord(repository);
            }
            else
                return false;
            //Program.formMain.AddData(repository);
            //if (!Equals(repository, default(RepositoryMessage)))
            //{
            //    FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewInsertMsg("RepositoryView", repository.Format()));
            //    return storage.SetRecord(repository);
            //}
            //else
            //    return false;
        }
        public void Clear()
        {
            //Program.formMain.DelDataALL();
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewClearMsg("RepositoryView"));
            storage.Clear();
        }
        public bool DEL(RepositoryMessage repository)
        {
            if (!Equals(repository, default(RepositoryMessage)))
            {
                //Program.formMain.DelData(repository);
                FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewDeleteMsg("RepositoryView", repository.Level.ToString()));
                storage.Remove(record => record.Level == repository.Level);
                return true;
            }                
            else
                return false;
        }
        public RepositoryMessage SEARCH(int Level)
        {
            return storage.Contain(record => record.Level == Level);
        }
        public RepositoryMessage SEARCH(string ID)
        {
            return storage.Contain(record => record.ID == ID);
        }
        public bool Update(RepositoryMessage repository)
        {
            if (DEL(repository))
                return ADD(repository);
            else
                return false;
            //if (!Equals(repository, default(RepositoryMessage)))
            //{
            //    FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewDeleteMsg("RepositoryView", repository.Level.ToString()));
            //    //Program.formMain.DelData(repository);
            //    storage.Remove(record => record.ID == repository.ID);
            //    return ADD(repository);
            //}
            //else
            //    return false;
        }
        public int Count()
        {
            return storage.Count();
        }

        public IEnumerable<RepositoryMessage> GetRepositories()
        {
            return storage.GetRecord(record => true);
        }
    }
}
