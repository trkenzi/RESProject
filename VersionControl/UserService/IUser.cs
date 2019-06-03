using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService
{
    public interface IUser
    {
        void Inicijalizuj();
        void NapraviReviziju();
        void Meni();
        void OdaberiReviziju();
        void PrikaziFajlove();
        void PrikaziSveRevizije();
        void SprovediRevizijuNadSvim(string putanjaDoRepozitorijuma, string nazivAutora, string nazivRepozitorijuma);
        void SprovediRevizijuNadKonkretnim(string putanjaDoRepozitorijuma, string nazivAutora, string nazivRepozitorijuma);
        void UpisiURepozitorijumiTxt(string putanjaDoRepozitorijumiTxt, RepozitorijumiInfo repoInfo);
        void UpisiNovuPutanju(string putanjaDoRepozitorijumiTxt, string[] noveKonkretneInformacije);
        void PrikaziRevizijeZaSveDokumente(string putanjaDoVC, string nazivRepozitorijuma, string nazivAutora);
        void PrikaziRevizijeZaKonkretanDokument(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string NazivAutora);
        void PretragaRevizija();
        string ProveraIKreiranjeLogFajla(string putanjaDoRepozitorijumiTxt);
        string UnesiRepozitorijum();
        string IzaberiFajlZaPrikazNjegovihRevizija(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string nazivAutora);
        void SetPutanjaDoRepozitorijumiText(string text);
        string GetPutanjaDoRepozitorijumiText();
        RepozitorijumiInfo CitajIzLoga(string nazivRepozitorijuma);
        RepozitorijumiInfo KreirajRepozitorijum(string putanjaDoRepozitorijumiTxt, string nazivRepozitorijuma);
        RepozitorijumiInfo ReferenciranjeNaPutanju(string putanjaDoRepozitorijumiTxt, string nazivRepozitorijuma);
        RepozitorijumiInfo PromenaRepozitorijumaAutora(string[] konkretnaInformacija, string nazivRepozitorijuma, string putanjaDoRepozitorijumiTxt, RepozitorijumiInfo repoInfo);
        RepozitorijumiInfo CitanjePutanjeDoRepozitorijuma(string[] konkretnaInformacija, string nazivRepozitorijuma, string putanjaDoRepozitorijumiTxt, RepozitorijumiInfo repoInfo);
        List<string> PronadjiRevizijeZaIzabraniFajl(string nazivIzabranogFajla, string putanjaDoVC);
    }
}
