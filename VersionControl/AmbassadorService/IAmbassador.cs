using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbassadorService
{
    public interface IAmbassador
    {
        void NapraviReviziju(string putanjaDoRepozitorijuma, string nazivAutora, List<string> fajloviNadKojimaSeSprovodiRevizija);
        void ObradiReviziju(string putanjaDoVC, string nazivIzabranogFajla, int izborKorisnika, string putanjaDoOriginalnogFajla);
        string[] PretraziReviziju(string putanjaDoVC, string izborKorisnika);
        string[] PrikaziSveDokumente(string putanjaDoRepozitorijuma);
        List<string> VratiSveRevizije(int izborKorisnika, string putanjaDoRepozitorijuma);
    }
}
