using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSService
{
    public interface ICQRSWrite
    {
        void UpisiReviziju(string putanja, string staroStanje, string novoStanje);
        void NapraviPrvuReviziju(string izvornaPutanja, string odredisnaPutanja);
        void napraviMetaPodatke(string putanjaDoCommitaSaRazlikama, string nazivAutora, DateTime dt, string oznakaRevizije, int redniBrojRevizije);
        string[] KreirajFolderZaC1(string preuzetaPutanja, string nazivOriginalnogFajla);
        string[] VratiPovratnePutanje(string putanjaZaFolderSvakeRevizije, string putanjaDoRazlika, string pomocnaReferencaNaVC, string putanjaDoOriginalnogFajla);
        bool ProveriPutanju(string putanjaDoRazlika);
    }
}
