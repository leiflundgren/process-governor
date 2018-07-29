using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Memory_User
{
    class Program
    {
        static void Main(string[] args)
        {
            int b = 0, loops = 0;
                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    long mbUsed = 0;
            var ls = new LinkedList<byte[]>();
            try
            {

                for (; ; ++loops)
                {
                    currentProcess.Refresh();
                    mbUsed = currentProcess.WorkingSet64 / 1048576;
                    System.Console.Write("Has run {0} loops, so far allocated {1} MB, memory use {2}\n", loops, b, mbUsed);

                    byte[] buf = new byte[1048576];
                    // Fill buffer with values so memory manager doesn't optimize and share mem
                    for (int i = 0; i < 1048576; ++i)
                        buf[i] = (byte)(i % 256);
                    ls.AddLast(buf);
                    ++b;
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch ( OutOfMemoryException )
            {
                try
                {
                    mbUsed = currentProcess.WorkingSet64 / 1048576;
                }
                catch { }
            }
            finally
            {
                ls = null;
                GC.Collect();
            }

            System.Console.Write("Ran {0} loops, allocated {1} MB, peak memory use {2}\n", loops, b, mbUsed);
            System.Threading.Thread.Sleep(4000);
        }
    }
}
