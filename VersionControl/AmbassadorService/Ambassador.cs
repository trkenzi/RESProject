using EventSourcingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbassadorService
{
    public class Ambassador : IAmbassador
    {
        private static IEventSourcing m_IEventSourcing;
        public Ambassador()
        {

        }
        public Ambassador(IEventSourcing es)
        {
            m_IEventSourcing = es;
        }
        public void NapraviReviziju(string putanjaDoRepozitorijuma, string nazivAutora, List<string> fajloviNadKojimaSeSprovodiRevizija)
        {
            if (putanjaDoRepozitorijuma == null || nazivAutora == null || fajloviNadKojimaSeSprovodiRevizija == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma i lista fajlova ne smeju biti nevalidni.\n");
            }
            if (putanjaDoRepozitorijuma == "" || nazivAutora == "" || fajloviNadKojimaSeSprovodiRevizija == null)
            {
                throw new ArgumentException("\nPutanja do repozitorijuma i lista fajlova ne smeju biti prazni.\n");
            }
            m_IEventSourcing.NapraviReviziju(putanjaDoRepozitorijuma, nazivAutora, fajloviNadKojimaSeSprovodiRevizija);
        }
        public void ObradiReviziju(string putanjaDoVC, string nazivIzabranogFajla, int izborKorisnika, string putanjaDoOriginalnogFajla)
        {
            if (putanjaDoVC == null || nazivIzabranogFajla == null || putanjaDoOriginalnogFajla == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma i naziv fajla ne mogu biti nevalidni.\n");
            }
            if (putanjaDoVC == "" || nazivIzabranogFajla == "" || izborKorisnika <= 0 || putanjaDoOriginalnogFajla == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma i naziv fajla ne smeju biti prazni.\n");
            }

            m_IEventSourcing.PrimeniRevizije(putanjaDoVC, nazivIzabranogFajla, izborKorisnika, putanjaDoOriginalnogFajla);
        }
        public string[] PrikaziSveDokumente(string putanjaDoRepozitorijuma)
        {
            if (putanjaDoRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma ne sme biti prazna.\n");
            }
            if (putanjaDoRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma ne sme biti nevalidna.\n");
            }

            return m_IEventSourcing.VratiSveDokumente(putanjaDoRepozitorijuma);
        }
        public string[] PretraziReviziju(string putanjaDoVC, string izborKorisnika)
        {
            if (putanjaDoVC == null || izborKorisnika == null)
            {
                throw new ArgumentNullException("\nPutanja do foldera sa revizijama i oznaka revizije ne smeju biti nevalidni.\n");
            }
            if (putanjaDoVC == "" || izborKorisnika == "")
            {
                throw new ArgumentException("\nPutanja do foldera sa revizijama i oznaka revizije ne smeju biti prazni.\n");
            }

            return m_IEventSourcing.FajloviRevizije(putanjaDoVC, izborKorisnika);
        }
        public List<string> VratiSveRevizije(int izborKorisnika, string putanjaDoRepozitorijuma)
        {
            if (putanjaDoRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma ne sme biti nevalidna.\n");
            }
            if (izborKorisnika < 0 || putanjaDoRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma i izabrana revizija ne smeju biti prazni.\n");
            }

            return m_IEventSourcing.VratiRevizije(izborKorisnika, putanjaDoRepozitorijuma);
        }
    }
}
