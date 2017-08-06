using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineageIIServerEmulator.LoginServer;

namespace TestOutPut
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginServer.GetInstance();
            Console.ReadLine();
        }
    }
}
