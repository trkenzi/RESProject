using CQRSService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EventSourcingService
{
    public interface IEventSourcing
    {
        void NapraviReviziju(string preuzetaPutanjaIzLogFajla, string nazivAutora, List<string> fajloviNadKojimaSeSprovodiRevizija);
        void ProdjiKrozFoldereSaRazlikama(string[] putanjeDoCommita, string preuzetaPutanjaIzLogFajla, string referencaNaRepozitorijum, List<string> fajloviNadKojimaSeSprovodiRevizija, string putanjaDoFolderaSvakeRevizije, string nazivAutora, string putanjaDoOriginalnogFajla, string[] nazivOriginalnogSplitovan, int iterator);
        void PrimeniRevizije(string putanjaDoVC, string nazivIzabranogFajla, int izborKorisnika, string putanjaDoRepozitorijuma);
        void UcitajSadrzajSaRazlikama(int izborKorisnika, string nazivIzabranogFajla, string putanjaDoRepozitorijuma, string putanjaDoVC, string[] nazivFajlaSplitovan);
        void PrimenaRazlika(string referencaNaVC, int izborKorisnika, string nazivFajla, XElement sadrzajPrvogCommita, string originalniTekst, string osnovnaPutanja, string[] nazivFajlaSplitovan);
        void NapraviIUpisiRazlike(string referencaNaRepozitorijum, string nazivCommita, string nazivOriginalnogFajla, XElement razlikeIzNarednogCommita, XElement razlikeIzPrvogCommita, string nazivAutora, string putanjaDoFajlaKojiSadrziRazlike, string sadrzajPrveRevizije);
        void PrimeniKonkretneRazlike(IEnumerable<XElement> redoviSaRazlikamaPrviCommit, IEnumerable<XElement> redoviSaRazlikamaNaredniCommit, string putanjaDoFajlaKojiSadrziRazlike);
        void ZameniVisakElemenataIzC1SaPraznimStringom(IEnumerable<XElement> redoviSaRazlikamaPrviCommit, IEnumerable<XElement> redoviSaRazlikamaNaredniCommit, string putanjaDoFajlaKojiSadrziRazlike);
        void UpisiNovoStanje(IEnumerable<XElement> redoviSaRazlikamaPrviCommit, IEnumerable<XElement> redoviSaRazlikamaNaredniCommit, XElement konacneRazlike, string putanjaDoFajlaKojiSadrziRazlike);
        void NapraviMeta(ICQRSWrite write, string nazivAutora, string nazivOriginalnogSplitovan, string fajlURepozitorijumu, string direktorijumRevizije, string razlikaUReviziji);
        void UcitajRazlike(string putanjaDoRepozitorijuma, string nazivOriginalnogFajla, string putanjaDoFajlaSaRazlikama, string referencaNaRepozitorijum, string nazivCommita, string nazivAutora, string putanjaDoFajlaKojiSadrziRazlike);
        string ProdjiKrozCommiteIPrimeniRazlike(string nazivOriginalnogFajla, string putanjaDoVC, string[] splitovanOriginalniTekst, string[] subdirs, IEnumerable<XElement> redSaRazlikamaPrviCommit, int iterator);
        string PrimeniSveRazlike(FileInfo[] files, string nazivFajla, string preuzetaPutanja, string[] splitovanOriginalniTekst, IEnumerable<XElement> redSaRazlikamaPrviCommit, string konacnaVerzija, int iterator);
        string ProveriBrojElemenata(IEnumerable<XElement> redoviSaRazlikamaNarednogCommita, IEnumerable<XElement> konacnaVerzijaXElementRedoviSaRazlikama, string konacnaVerzija, IEnumerator<XElement> enumerator);
        string[] VratiSveDokumente(string putanjaDoRepozitorijuma);
        string[] FajloviRevizije(string putanjaDoRepozitorijuma, string izborKorisnika);
        bool PostojiCommit(string nazivOriginalnogFajla, string putanjaUnutarCommita);
        bool ProveriDaLiPostojiCommit(string putanjaZaFoldereIFajloveSaRazlikama, List<string> fajloviNadKojimaSeSprovodiRevizija, int iterator, string putanjaDoFolderaSvakeRevizije, string putanjaDoOriginalnogFajla, string referencaNaRepozitorijum, string nazivCommita, string nazivAutora, bool dodaoFajlSaRazlikama);
        bool DodajFajlSaRazlikama(List<string> fajloviNadKojimaSeSprovodiRevizija, int iterator, string putanjaZaFoldereIFajloveSaRazlikama, string putanjaDoFolderaSvakeRevizije, string putanjaDoOriginalnogFajla, string referencaNaRepozitorijum, string nazivCommita, string nazivAutora);
        bool DodavanjeFajlaSaRazlikamaUCommitKojiPostoji(bool dodaoFajlUOdgovarajuciCommit, string nazivAutora, string preuzetaPutanja, string[] nazivOriginalnogSplitovan, string[] putanjeDoCommita);
        XElement PripremaFajla(string sadrzajCommita);
        List<string> VratiRevizije(int izborKorisnika, string putanjaDoRepozitorijuma);
    }
}
