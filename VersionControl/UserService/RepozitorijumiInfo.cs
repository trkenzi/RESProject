using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService
{
    public class RepozitorijumiInfo
    {
        private string nazivAutora;
        private string preuzetaPutanja;
        private string nazivRepozitorijuma;
        public RepozitorijumiInfo()
        {

        }

        public string NazivAutora { get => nazivAutora; set => nazivAutora = value; }
        public string PreuzetaPutanja { get => preuzetaPutanja; set => preuzetaPutanja = value; }
        public string NazivRepozitorijuma { get => nazivRepozitorijuma; set => nazivRepozitorijuma = value; }
    }
}
