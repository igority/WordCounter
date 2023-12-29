using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public async Task Test1_WordCounter()
        {
            string workingDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\TestingFolders\test1");
            await WordCounter.Program.Main(new string[] { workingDirectory });
        }
        
        [TestMethod]
        public async Task Test1_WordCounter_Alternative()
        {
            string workingDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\TestingFolders\test1");
            await WordCounter_Alternative.Program.Main(new string[] { workingDirectory });
        }

        [TestMethod]
        public async Task Test1_WordCounter_SingleThread()
        {
            string workingDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\TestingFolders\test1");
             await WordCounter_SingleThread.Program.Main(new string[] { workingDirectory });
        }

    }
}