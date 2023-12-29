using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class Test3
    {
        [TestMethod]
        public async Task Test3_WordCounter()
        {
            string workingDirectory = @$"{Environment.CurrentDirectory}\..\..\..\..\TestingFolders\test3";
            await WordCounter.Program.Main(new string[] { workingDirectory });
        }
        
        [TestMethod]
        public async Task Test3_WordCounter_Alternative()
        {
            string workingDirectory = @$"{Environment.CurrentDirectory}\..\..\..\..\TestingFolders\test3";
            await WordCounter_Alternative.Program.Main(new string[] { workingDirectory });
        }
        
        [TestMethod]
        public async Task Test3_WordCounter_SingleThread()
        {
            string workingDirectory = @$"{Environment.CurrentDirectory}\..\..\..\..\TestingFolders\test3";
            await WordCounter_SingleThread.Program.Main(new string[] { workingDirectory });
        }

    }
}