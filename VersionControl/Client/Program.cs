using AmbassadorService;
using CQRSService;
using EventSourcingService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserService;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            IUser iu = new User();
            iu.Inicijalizuj();
            iu.Meni();
        }
    }
}
