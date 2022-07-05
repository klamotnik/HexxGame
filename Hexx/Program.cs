using Hexx.Connection;
using Hexx.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx
{
    class Program
    {
        static void Main(string[] args)
        {
            SEngine engine = new SEngine();
            engine.Start();
            return;
        }
    }

    interface A
    {
        void M1();
    }
    interface B
    {
        void M1();
    }
    interface C : A, B
    {
    }
}
