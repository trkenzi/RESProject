using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CQRSService
{
    public interface ICQRSRead
    {
        string CitajCeoFajl(string putanjaDoOriginalnogFajla);
        string[] VratiFajloveIzRevizije(string putanjaDoVC, string izabraniFajl);
        List<string> PosaljiSveRevizije(string putanjaDoVC);
        List<string> PosaljiRevizije(string putanjaDoVC, string nazivFajla);
        XElement CitajRazlike(string putanjaDoFajlaSaRazlikama);
    }

}
