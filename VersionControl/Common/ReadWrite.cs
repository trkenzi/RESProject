using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ReadWrite : IReadWrite
    {
        public string UnesiSaKonzole()
        {
            string input = Console.ReadLine();
            return input;
        }
    }
}
