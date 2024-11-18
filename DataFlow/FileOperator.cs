using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace DataFlow
{
    //chcp 437 english
    //chcp 950 chinese
    public class FileOperator
    {
        private const int Lock_THRESHOLD = 10;
        private const int READ_THRESHOLD = 10;
        private object Lock = new object();
        private Dictionary<string, ReaderWriterLockSlim> FileLock = new Dictionary<string, ReaderWriterLockSlim>();
        private ReaderWriterLockSlim GetLock(string filepath)
        {
            lock(Lock)
            {
                try
                {
                    if (FileLock.Count > Lock_THRESHOLD)
                        FileLock.Clear();

                    ReaderWriterLockSlim rwls;
                    if (FileLock.TryGetValue(filepath, out rwls))
                        return rwls;
                    else
                    {
                        rwls = new ReaderWriterLockSlim();
                        FileLock.Add(filepath, rwls);
                        return rwls;
                    }
                }
                catch(Exception)
                {
                    return default;
                }
            }
        }

        private bool DelLock(string filepath)
        {
            lock (Lock)
            {
                try
                {
                    ReaderWriterLockSlim rwls;
                    if (FileLock.TryGetValue(filepath, out rwls))
                    {
                        //Console.WriteLine(rwls.GetHashCode());
                        rwls.Dispose();
                    }
                    return FileLock.Remove(filepath);
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

        public bool Write(string path, string data, FileMode mode = FileMode.OpenOrCreate)
        {
            //try
            //{
            //    using (var fs = new FileStream(path, mode, FileAccess.Write, FileShare.ReadWrite))
            //    {
            //        using (var sw = new StreamWriter(fs, new UTF8Encoding(false)))
            //        {
            //            fs.Seek(0, SeekOrigin.End);
            //            await sw.WriteLineAsync(data);
            //            sw.Close();
            //            return true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + ex.Message);
            //    return false;
            //}
            //==============================================================================================================
            ReaderWriterLockSlim rwls = GetLock(path);
            if (Equals(rwls, default) || rwls.IsWriteLockHeld || rwls.IsReadLockHeld || !rwls.TryEnterWriteLock(0))
                return false;
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(data))
                    {
                        if(File.Exists(path))
                        {
                            File.WriteAllText(path, String.Empty);
                            return true;
                        }
                        else
                            return false;
                    }
                        
                    using (var fs = new FileStream(path, mode, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (var sw = new StreamWriter(fs, new UTF8Encoding(false)))
                        {
                            fs.Seek(0, SeekOrigin.End);
                            sw.Write(data);
                            sw.Close();
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + ex.Message);
                    return false;
                }
                finally
                {
                    if (rwls.IsWriteLockHeld)
                        rwls.ExitWriteLock();
                }
            }
            //==============================================================================================================

            //ReaderWriterLockSlim rwls = GetLock(path);
            //try
            //{
            //    if (!rwls.TryEnterWriteLock(5000))
            //        return false;
            //    else
            //    {
            //        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            //        {
            //            using (var sw = new StreamWriter(fs, new UTF8Encoding(false)))
            //            {
            //                fs.Seek(0, SeekOrigin.End);
            //                await sw.WriteLineAsync(data);
            //                sw.Close();
            //                return true;
            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return false;
            //}
            //finally
            //{
            //    rwls?.ExitWriteLock();
            //}
        }

        public string Read(string path)
        {
            //try
            //{
            //    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //    using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            //    {
            //        char[] result = new char[sr.BaseStream.Length];
            //        await sr.ReadAsync(result, 0, (int)sr.BaseStream.Length);
            //        sr.Close();
            //        return new string(result.Where(indexer => indexer != '\0').ToArray());
            //    }
            //}
            //catch (Exception)
            //{
            //    return default;
            //}
            //=====================================================================================================================
            ReaderWriterLockSlim rwls = GetLock(path);
            if (Equals(rwls, default) || rwls.CurrentReadCount > READ_THRESHOLD || rwls.IsWriteLockHeld || !rwls.TryEnterReadLock(0))
                return default;
            else
            {
                try
                {
                    if (!File.Exists(path))
                        return default(string);

                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    {
                        char[] result = new char[sr.BaseStream.Length];
                        sr.Read(result, 0, (int)sr.BaseStream.Length);
                        sr.Close();
                        return new string(result.Where(indexer => indexer != '\0').ToArray());
                    }
                }
                catch (Exception ex)
                {
                    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + ex.Message);
                    return default;
                }
                finally
                {
                    if (rwls.IsReadLockHeld)
                        rwls.ExitReadLock();
                }
            }
            //=====================================================================================================
            //ReaderWriterLockSlim rwls = GetLock(path);
            //try
            //{
            //    rwls.EnterReadLock();
            //    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //    using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            //    {
            //        char[] result = new char[sr.BaseStream.Length];
            //        await sr.ReadAsync(result, 0, (int)sr.BaseStream.Length);
            //        sr.Close();
            //        return new string(result.Where(indexer => indexer != '\0').ToArray());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return default;
            //}
            //finally
            //{
            //    rwls?.ExitReadLock();
            //}
        }
        

       
    }
}
