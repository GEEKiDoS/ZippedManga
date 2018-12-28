using SharpCompress.Archives;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZippedManga
{
    class ZippedMangaHelper
    {
        public static byte[] GetMangaPreview(string fp)
        {
            var needPW = true;
            var iter = App.KeyChain.GetEnumerator();
            iter.MoveNext();

            using (var file = File.Open(fp, FileMode.Open, FileAccess.Read))
            {
                string pw = null;

                while (needPW)
                {
                    needPW = false;
                    using (var archive = ArchiveFactory.Open(file, pw == null ? null : new ReaderOptions { Password = pw }))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            if (entry.IsDirectory)
                                continue;

                            try
                            {
                                iter.Dispose();
                                byte[] bytes = null;
                                using (var binaryReader = new BinaryReader(entry.OpenEntryStream()))
                                {
                                    bytes = binaryReader.ReadBytes(Convert.ToInt32(entry.Size));
                                }

                                return bytes;
                            }
                            catch
                            {
                                pw = iter.Current ?? throw new Exception("No vaild password found in KeyChain");
                                iter.MoveNext();
                                needPW = true;
                                break;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
