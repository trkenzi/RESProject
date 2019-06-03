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
    public class EventSourcing : IEventSourcing
    {
        private static ICQRSWrite m_CQRSWrite;
        private static ICQRSRead m_CQRSRead;
        public EventSourcing()
        {

        }
        public EventSourcing(ICQRSRead read, ICQRSWrite write)
        {
            m_CQRSRead = read;
            m_CQRSWrite = write;
        }

        public void NapraviReviziju(string preuzetaPutanjaIzLogFajla, string nazivAutora, List<string> fajloviNadKojimaSeSprovodiRevizija)
        {
            string putanjaDoOriginalnogFajla = "";
            string putanjaDoRepozitorijuma = preuzetaPutanjaIzLogFajla;
            string putanjaDoFolderaSvakeRevizije = preuzetaPutanjaIzLogFajla;

            for (int iterator = 0; iterator < fajloviNadKojimaSeSprovodiRevizija.Count(); iterator++)
            {
                string[] nazivOriginalnogSplitovan = fajloviNadKojimaSeSprovodiRevizija[iterator].Split('.');
                string[] putanjeDoCommita = m_CQRSWrite.KreirajFolderZaC1(putanjaDoRepozitorijuma, fajloviNadKojimaSeSprovodiRevizija[iterator]);

                preuzetaPutanjaIzLogFajla = putanjeDoCommita[3];
                string referencaNaRepozitorijum = preuzetaPutanjaIzLogFajla;
                putanjaDoOriginalnogFajla = putanjeDoCommita[0];

                if (!putanjeDoCommita[4].Equals(""))
                {
                    NapraviMeta(m_CQRSWrite, nazivAutora, nazivOriginalnogSplitovan[0], putanjeDoCommita[2], putanjeDoCommita[1], putanjeDoCommita[0]);
                }
                else
                {
                    ProdjiKrozFoldereSaRazlikama(putanjeDoCommita, preuzetaPutanjaIzLogFajla, referencaNaRepozitorijum, fajloviNadKojimaSeSprovodiRevizija, putanjaDoFolderaSvakeRevizije, nazivAutora, putanjaDoOriginalnogFajla, nazivOriginalnogSplitovan, iterator);
                }
            }
        }

        public void ProdjiKrozFoldereSaRazlikama(string[] putanjeDoCommita, string preuzetaPutanjaIzLogFajla, string referencaNaRepozitorijum, List<string> fajloviNadKojimaSeSprovodiRevizija, string putanjaDoFolderaSvakeRevizije, string nazivAutora, string putanjaDoOriginalnogFajla, string[] nazivOriginalnogSplitovan, int iterator)
        {
            bool dodaoFajlSaRazlikama = false;
            bool dodaoFajlUOdgovarajuciCommit = false;
            string putanjaZaFoldereIFajloveSaRazlikama = "";
            int redniBrojRevizije = 0;
            string[] listaCommita = Directory.GetDirectories(putanjeDoCommita[3]).Select(Path.GetFileName).ToArray();//preuzimam sve foldere iz Commit tj C1, C2, itd.

            for (int j = 0; j < listaCommita.Length; j++)
            {
                if (dodaoFajlSaRazlikama)
                    return;

                putanjaZaFoldereIFajloveSaRazlikama = referencaNaRepozitorijum;
                preuzetaPutanjaIzLogFajla = referencaNaRepozitorijum;
                preuzetaPutanjaIzLogFajla = Path.Combine(preuzetaPutanjaIzLogFajla, listaCommita[j]);

                if (PostojiCommit(fajloviNadKojimaSeSprovodiRevizija[iterator], preuzetaPutanjaIzLogFajla))
                {
                    redniBrojRevizije = 1 + Int32.Parse(listaCommita[j].Substring(1));
                    string nazivCommita = "C" + redniBrojRevizije.ToString();
                    putanjaZaFoldereIFajloveSaRazlikama = Path.Combine(putanjaZaFoldereIFajloveSaRazlikama, nazivCommita);

                    dodaoFajlSaRazlikama = ProveriDaLiPostojiCommit(putanjaZaFoldereIFajloveSaRazlikama, fajloviNadKojimaSeSprovodiRevizija, iterator, putanjaDoFolderaSvakeRevizije, putanjaDoOriginalnogFajla, referencaNaRepozitorijum, nazivCommita, nazivAutora, dodaoFajlSaRazlikama);
                }
                else
                {
                    dodaoFajlUOdgovarajuciCommit = DodavanjeFajlaSaRazlikamaUCommitKojiPostoji(dodaoFajlUOdgovarajuciCommit, nazivAutora, preuzetaPutanjaIzLogFajla, nazivOriginalnogSplitovan, putanjeDoCommita);
                }
            }
        }

        public void PrimeniRevizije(string putanjaDoVC, string nazivIzabranogFajla, int izborKorisnika, string putanjaDoRepozitorijuma)
        {
            if (putanjaDoVC == null || nazivIzabranogFajla == null || putanjaDoRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma, naziv fajla i oznaka revizije ne smeju biti nevalidni.\n");
            }
            if (putanjaDoVC == "" || nazivIzabranogFajla == "" || izborKorisnika <= 0 || putanjaDoRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma, naziv fajla i oznaka revizije ne smeju biti prazni.\n");
            }

            string sadrzajPrvogCommita = "";
            string[] nazivFajlaSplitovan = nazivIzabranogFajla.Split('.');

            if (izborKorisnika == 1)
            {
                sadrzajPrvogCommita = m_CQRSRead.CitajCeoFajl(Path.Combine(Path.Combine(putanjaDoVC, "C1"), nazivIzabranogFajla)); // cita iz c1
                nazivIzabranogFajla = nazivFajlaSplitovan[0] + "_C1" + "." + nazivFajlaSplitovan[1];
                putanjaDoRepozitorijuma = Path.Combine(putanjaDoRepozitorijuma, "FinalneVerzije");
                Directory.CreateDirectory(putanjaDoRepozitorijuma);
                putanjaDoRepozitorijuma = Path.Combine(putanjaDoRepozitorijuma, nazivIzabranogFajla); // pise u "razlike" u c1

                File.WriteAllText(putanjaDoRepozitorijuma, sadrzajPrvogCommita);
                return;
            }

            UcitajSadrzajSaRazlikama(izborKorisnika, nazivIzabranogFajla, putanjaDoRepozitorijuma, putanjaDoVC, nazivFajlaSplitovan);
        }

        public void UcitajSadrzajSaRazlikama(int izborKorisnika, string nazivIzabranogFajla, string putanjaDoRepozitorijuma, string putanjaDoVC, string[] nazivFajlaSplitovan)
        {
            if (nazivIzabranogFajla == null || putanjaDoRepozitorijuma == null || putanjaDoVC == null || nazivFajlaSplitovan == null)
            {
                throw new ArgumentNullException("\nNaziv fajla sa razlikama i putanje do repozitorijuma i tog fajla ne smeju biti nevalidne.\n");
            }

            if (izborKorisnika <= 0 || nazivIzabranogFajla == "" || putanjaDoRepozitorijuma == "" || putanjaDoVC == "" || nazivFajlaSplitovan.Count() == 0)
            {
                throw new ArgumentException("\nNaziv fajla sa razlikama i putanje do repozitorijuma i tog fajla ne smeju biti prazni.\n");
            }

            string referencaNaVC = "";
            XElement sadrzajPrvogCommita;
            string originalniTekst = "";

            referencaNaVC = putanjaDoVC;
            putanjaDoVC = putanjaDoVC + "\\C1" + "\\" + nazivIzabranogFajla;
            string textIzFajlaC1 = m_CQRSRead.CitajCeoFajl(putanjaDoVC); //ucitaj ceo tekst iz fajla
            originalniTekst = m_CQRSRead.CitajCeoFajl(putanjaDoVC);
            sadrzajPrvogCommita = PripremaFajla(textIzFajlaC1);

            PrimenaRazlika(referencaNaVC, izborKorisnika, nazivIzabranogFajla, sadrzajPrvogCommita, originalniTekst, putanjaDoRepozitorijuma, nazivFajlaSplitovan);
        }

        public void PrimenaRazlika(string referencaNaVC, int izborKorisnika, string nazivFajla, XElement sadrzajPrvogCommita, string originalniTekst, string osnovnaPutanja, string[] nazivFajlaSplitovan)
        {
            string[] listaDirektorijuma = Directory.GetDirectories(referencaNaVC).Select(Path.GetFileName).ToArray();
            string konacnaVerzija = "";

            string[] charSeparators = new string[] { "\r\n" };
            string[] splitovanOriginalniTekst = originalniTekst.Split(charSeparators, StringSplitOptions.None);
            IEnumerable<XElement> redSaRazlikamaPrviCommit = sadrzajPrvogCommita.Elements("Red");

            for (int iterator = 1; iterator < izborKorisnika; iterator++)
            {
                string preuzetaPutanja = Path.Combine(referencaNaVC, listaDirektorijuma[iterator]);
                FileInfo[] files = new DirectoryInfo(preuzetaPutanja).GetFiles("*");

                if (iterator > 1)
                {
                    splitovanOriginalniTekst = konacnaVerzija.Split(charSeparators, StringSplitOptions.None);
                }

                konacnaVerzija = PrimeniSveRazlike(files, nazivFajla, preuzetaPutanja, splitovanOriginalniTekst, redSaRazlikamaPrviCommit, konacnaVerzija, iterator);
            }

            nazivFajla = nazivFajlaSplitovan[0] + "_C" + izborKorisnika + "." + nazivFajlaSplitovan[1];
            osnovnaPutanja = Path.Combine(osnovnaPutanja, "FinalneVerzije");
            Directory.CreateDirectory(osnovnaPutanja);
            osnovnaPutanja = Path.Combine(osnovnaPutanja, nazivFajla);
            File.WriteAllText(osnovnaPutanja, konacnaVerzija);
        }

        public void NapraviIUpisiRazlike(string referencaNaRepozitorijum, string nazivCommita, string nazivOriginalnogFajla, XElement razlikeIzNarednogCommita, XElement razlikeIzPrvogCommita, string nazivAutora, string putanjaDoFajlaKojiSadrziRazlike, string sadrzajPrveRevizije)
        {
            string[] nazivOriginalnogSplitovan = nazivOriginalnogFajla.Split('.');
            string[] listaDirektorijuma = Directory.GetDirectories(referencaNaRepozitorijum).Select(Path.GetFileName).ToArray();

            IEnumerable<XElement> redoviSaRazlikamaPrviCommit = razlikeIzPrvogCommita.Elements("Red");
            IEnumerable<XElement> redoviSaRazlikamaNaredniCommit = razlikeIzNarednogCommita.Elements("Red");

            IEnumerator<XElement> iteratorKrozNarediCommitSaRazlikama = redoviSaRazlikamaNaredniCommit.GetEnumerator();
            string konacnaVerzija = "";
            string[] charSeparators = new string[] { "\r\n" };
            string[] splitovanSadrzajPrveRevizije = sadrzajPrveRevizije.Split(charSeparators, StringSplitOptions.None);

            for (int i = 1; i < listaDirektorijuma.Length; i++)
            {

                if (listaDirektorijuma[i].Equals(nazivCommita)) //znaci da sam dosao do trenutnog commita u kojem treba da sacuvam razlike sa prethodnim verzijama
                {
                    string metaPodaci = nazivOriginalnogSplitovan[0] + "_metaPodaci" + '.' + nazivOriginalnogSplitovan[1];
                    m_CQRSWrite.napraviMetaPodatke(Path.Combine(Path.Combine(referencaNaRepozitorijum, listaDirektorijuma[i]), metaPodaci), nazivAutora, DateTime.Now, nazivCommita, Int32.Parse(listaDirektorijuma[i].Substring(1)));

                    if (redoviSaRazlikamaPrviCommit.Count() > redoviSaRazlikamaNaredniCommit.Count())
                    {
                        ZameniVisakElemenataIzC1SaPraznimStringom(redoviSaRazlikamaPrviCommit, redoviSaRazlikamaNaredniCommit, putanjaDoFajlaKojiSadrziRazlike);
                        return;
                    }
                    else
                    {
                        PrimeniKonkretneRazlike(redoviSaRazlikamaPrviCommit, redoviSaRazlikamaNaredniCommit, putanjaDoFajlaKojiSadrziRazlike);
                        return;
                    }
                }
                else
                {
                    if (i > 1)
                    {
                        splitovanSadrzajPrveRevizije = konacnaVerzija.Split(charSeparators, StringSplitOptions.None);
                    }

                    konacnaVerzija = ProdjiKrozCommiteIPrimeniRazlike(nazivOriginalnogFajla, referencaNaRepozitorijum, splitovanSadrzajPrveRevizije, listaDirektorijuma, redoviSaRazlikamaPrviCommit, i);

                    XElement promenljivaZaResetovanjeSadrzajaPrvogCommita = new XElement("Fajl1");
                    razlikeIzPrvogCommita = promenljivaZaResetovanjeSadrzajaPrvogCommita;
                    splitovanSadrzajPrveRevizije = konacnaVerzija.Split(charSeparators, StringSplitOptions.None);
                    foreach (string razlike in splitovanSadrzajPrveRevizije)
                    {
                        XElement redSaRazlikama = new XElement("Red");
                        foreach (string konkretnaRazlika in razlike.Split(' '))
                            redSaRazlikama.Add(new XElement("rec", konkretnaRazlika));
                        razlikeIzPrvogCommita.Add(redSaRazlikama);
                    }
                    redoviSaRazlikamaPrviCommit = razlikeIzPrvogCommita.Elements("Red");
                }
            }
        }
        public void PrimeniKonkretneRazlike(IEnumerable<XElement> redoviSaRazlikamaPrviCommit, IEnumerable<XElement> redoviSaRazlikamaNaredniCommit, string putanjaDoFajlaKojiSadrziRazlike)
        {
            XElement konacneRazlike = new XElement("Razlike");
            IEnumerator<XElement> redoviSaRazlikamaNaredniCommitIterator = redoviSaRazlikamaNaredniCommit.GetEnumerator();
            redoviSaRazlikamaNaredniCommitIterator.MoveNext();
            foreach (XElement razlika in redoviSaRazlikamaPrviCommit)
            {
                XElement redRazlikaPrviCommit = new XElement("Red");

                IEnumerable<XElement> staroStanje = razlika.Elements("rec");
                IEnumerable<XElement> novoStanje = redoviSaRazlikamaNaredniCommitIterator.Current.Elements("rec");

                IEnumerator<XElement> iteratorNovoStanje = novoStanje.GetEnumerator();
                iteratorNovoStanje.MoveNext();
                bool IteratorNovoStanjeDosaoDoKraja = true;
                foreach (XElement konkretnaStaraRazlika in staroStanje)
                {
                    if (IteratorNovoStanjeDosaoDoKraja)
                    {
                        if (konkretnaStaraRazlika.Value != iteratorNovoStanje.Current.Value)
                        {
                            XElement noviElement = new XElement("Razlika", iteratorNovoStanje.Current.Value);
                            noviElement.SetAttributeValue("inicijalno", konkretnaStaraRazlika.Value);
                            redRazlikaPrviCommit.Add(noviElement);
                        }
                    }
                    else
                    {
                        XElement noviElement = new XElement("Razlika", "");
                        noviElement.SetAttributeValue("inicijalno", konkretnaStaraRazlika.Value);
                        redRazlikaPrviCommit.Add(noviElement);
                    }
                    IteratorNovoStanjeDosaoDoKraja = iteratorNovoStanje.MoveNext();


                }
                if (IteratorNovoStanjeDosaoDoKraja)
                {
                    do
                    {
                        XElement noviElement = new XElement("Razlika", " " + iteratorNovoStanje.Current.Value);
                        noviElement.SetAttributeValue("inicijalno", "");
                        redRazlikaPrviCommit.Add(noviElement);
                    }
                    while (iteratorNovoStanje.MoveNext());
                }


                konacneRazlike.Add(redRazlikaPrviCommit);
                redoviSaRazlikamaNaredniCommitIterator.MoveNext();

            }
            if (redoviSaRazlikamaPrviCommit.Count() < redoviSaRazlikamaNaredniCommit.Count())
            {
                UpisiNovoStanje(redoviSaRazlikamaPrviCommit, redoviSaRazlikamaNaredniCommit, konacneRazlike, putanjaDoFajlaKojiSadrziRazlike);
                return;
            }

            konacneRazlike.Save(putanjaDoFajlaKojiSadrziRazlike);
            return;
        }
        public void ZameniVisakElemenataIzC1SaPraznimStringom(IEnumerable<XElement> redoviSaRazlikamaPrviCommit, IEnumerable<XElement> redoviSaRazlikamaNaredniCommit, string putanjaDoFajlaKojiSadrziRazlike)
        {
            XElement konacneRazlike = new XElement("Razlike");
            IEnumerator<XElement> redoviSaRazlikamaNaredniCommitIterator = redoviSaRazlikamaNaredniCommit.GetEnumerator();
            int brojRedovaSaRazlikama = redoviSaRazlikamaNaredniCommit.Count() + 1;
            redoviSaRazlikamaNaredniCommitIterator.MoveNext();

            foreach (XElement razlika in redoviSaRazlikamaPrviCommit)
            {
                XElement redRazlikaPrviCommit = new XElement("Red");
                IEnumerable<XElement> staroStanje = razlika.Elements("rec");
                IEnumerable<XElement> novoStanje = redoviSaRazlikamaNaredniCommitIterator.Current.Elements("rec");
                brojRedovaSaRazlikama--;
                IEnumerator<XElement> iteratorNovoStanje = novoStanje.GetEnumerator();
                iteratorNovoStanje.MoveNext();
                bool IteratorNovoStanjeDosaoDoKraja = true;

                foreach (XElement konkretnaStaraRazlika in staroStanje)
                {
                    if (IteratorNovoStanjeDosaoDoKraja && brojRedovaSaRazlikama >= 1)
                    {
                        if (konkretnaStaraRazlika.Value != iteratorNovoStanje.Current.Value)
                        {
                            XElement noviElement = new XElement("Razlika", iteratorNovoStanje.Current.Value);
                            noviElement.SetAttributeValue("inicijalno", konkretnaStaraRazlika.Value);
                            redRazlikaPrviCommit.Add(noviElement);
                        }
                    }

                    else
                    {
                        XElement noviElement = new XElement("Razlika", "");
                        noviElement.SetAttributeValue("inicijalno", konkretnaStaraRazlika.Value);
                        redRazlikaPrviCommit.Add(noviElement);
                    }
                    IteratorNovoStanjeDosaoDoKraja = iteratorNovoStanje.MoveNext();
                }

                if (IteratorNovoStanjeDosaoDoKraja && brojRedovaSaRazlikama >= 1)
                {
                    do
                    {
                        XElement noviElement = new XElement("Razlika", " " + iteratorNovoStanje.Current.Value);
                        noviElement.SetAttributeValue("inicijalno", "");
                        redRazlikaPrviCommit.Add(noviElement);
                    }
                    while (iteratorNovoStanje.MoveNext());
                }

                konacneRazlike.Add(redRazlikaPrviCommit);
                redoviSaRazlikamaNaredniCommitIterator.MoveNext();
            }

            if (redoviSaRazlikamaPrviCommit.Count() < redoviSaRazlikamaNaredniCommit.Count())
            {
                UpisiNovoStanje(redoviSaRazlikamaPrviCommit, redoviSaRazlikamaNaredniCommit, konacneRazlike, putanjaDoFajlaKojiSadrziRazlike);
                return;
            }

            konacneRazlike.Save(putanjaDoFajlaKojiSadrziRazlike);
            return;
        }

        public void UpisiNovoStanje(IEnumerable<XElement> redoviSaRazlikamaPrviCommit, IEnumerable<XElement> redoviSaRazlikamaNaredniCommit, XElement konacneRazlike, string putanjaDoFajlaKojiSadrziRazlike)
        {
            IEnumerator<XElement> enumerator = redoviSaRazlikamaNaredniCommit.GetEnumerator();
            for (int j = 0; j < redoviSaRazlikamaPrviCommit.Count(); j++)
                enumerator.MoveNext();

            while (enumerator.MoveNext())
            {
                XElement redoviSaRazlikama = new XElement("Red");

                foreach (XElement razlika in enumerator.Current.Elements("rec"))
                {
                    XElement noviElement = new XElement("Razlika", razlika.Value);
                    noviElement.SetAttributeValue("inicijalno", "");
                    redoviSaRazlikama.Add(noviElement);
                }
                konacneRazlike.Add(redoviSaRazlikama);
            }

            konacneRazlike.Save(putanjaDoFajlaKojiSadrziRazlike);

        }

        public void NapraviMeta(ICQRSWrite write, string nazivAutora, string nazivOriginalnogSplitovan, string fajlURepozitorijumu, string direktorijumRevizije, string razlikaUReviziji)
        {
            write.napraviMetaPodatke(Path.Combine(direktorijumRevizije, nazivOriginalnogSplitovan + "_metaPodaci" + ".txt"), nazivAutora, DateTime.Now, "C1", 1);
            write.NapraviPrvuReviziju(fajlURepozitorijumu, razlikaUReviziji);
        }

        public void UcitajRazlike(string putanjaDoRepozitorijuma, string nazivOriginalnogFajla, string putanjaDoFajlaSaRazlikama, string referencaNaRepozitorijum, string nazivCommita, string nazivAutora, string putanjaDoFajlaKojiSadrziRazlike)
        {
            if (putanjaDoRepozitorijuma == null || nazivOriginalnogFajla == null || putanjaDoFajlaSaRazlikama == null || referencaNaRepozitorijum == null || nazivCommita == null || nazivAutora == null || putanjaDoFajlaKojiSadrziRazlike == null)
            {
                throw new ArgumentNullException("\nPutanje do foldera sa razlikama ne smeju biti nevalidne.\n");
            }
            if (putanjaDoRepozitorijuma == "" || nazivOriginalnogFajla == "" || putanjaDoFajlaSaRazlikama == "" || referencaNaRepozitorijum == "" || nazivCommita == "" || nazivAutora == "" || putanjaDoFajlaKojiSadrziRazlike == "")
            {
                throw new ArgumentException("\nPutanje do foldera sa razlikama ne smeju biti prazne.\n");
            }

            string putanjaDoOriginalnog = Path.Combine(putanjaDoRepozitorijuma, nazivOriginalnogFajla);
            string sadrzajOriginalnogFajla = File.ReadAllText(putanjaDoOriginalnog); //ucitaj ceo tekst iz originalnog fajla
            string sadrzajPrveRevizije = File.ReadAllText(putanjaDoFajlaSaRazlikama); //ucitaj ceo tekst iz C1 posto je tu prva verzija koja nije menjana

            NapraviIUpisiRazlike(referencaNaRepozitorijum, nazivCommita, nazivOriginalnogFajla, PripremaFajla(sadrzajOriginalnogFajla), PripremaFajla(sadrzajPrveRevizije), nazivAutora, putanjaDoFajlaKojiSadrziRazlike, sadrzajPrveRevizije);
        }

        public string ProdjiKrozCommiteIPrimeniRazlike(string nazivOriginalnogFajla, string putanjaDoVC, string[] splitovanOriginalniTekst, string[] subdirs, IEnumerable<XElement> redSaRazlikamaPrviCommit, int iterator)
        {
            string konacnaVerzija = "";
            string preuzetaPutanja = Path.Combine(putanjaDoVC, subdirs[iterator]);
            bool zamenjenoStaroStanje = false;

            FileInfo[] Files = new DirectoryInfo(preuzetaPutanja).GetFiles("*");
            foreach (FileInfo file in Files)
            {
                if (nazivOriginalnogFajla.Equals(file.Name))
                {
                    preuzetaPutanja = Path.Combine(preuzetaPutanja, nazivOriginalnogFajla);
                    XElement razlikeIzNarednogCommita = m_CQRSRead.CitajRazlike(preuzetaPutanja);

                    IEnumerable<XElement> redoviSaRazlikamaNarednogCommita = razlikeIzNarednogCommita.Elements("Red");
                    IEnumerator<XElement> enumerator = redoviSaRazlikamaNarednogCommita.GetEnumerator();
                    enumerator.MoveNext();
                    for (int j = 0; j < splitovanOriginalniTekst.Length; j++)
                    {
                        IEnumerable<XElement> stareNoveRazlike = enumerator.Current.Elements("Razlika");

                        foreach (XElement razlika in stareNoveRazlike)
                        {
                            string staraRazlika = razlika.Attribute("inicijalno").Value;
                            string novaRazlika = razlika.Value;
                            if (staraRazlika != string.Empty)
                            {
                                splitovanOriginalniTekst[j] = ZameniPrvi(splitovanOriginalniTekst[j], staraRazlika, novaRazlika);
                                splitovanOriginalniTekst[j] = splitovanOriginalniTekst[j].Trim(null);
                            }
                            if (staraRazlika == string.Empty && novaRazlika != string.Empty)
                                splitovanOriginalniTekst[j] += novaRazlika; //+ ' ' je bilo

                            zamenjenoStaroStanje = true;
                        }
                        enumerator.MoveNext();

                    }
                    if (iterator == 1)
                    {
                        konacnaVerzija = String.Join(Environment.NewLine, splitovanOriginalniTekst);
                        konacnaVerzija += Environment.NewLine;
                    }
                    if (zamenjenoStaroStanje)
                    {
                        konacnaVerzija = String.Join(Environment.NewLine, splitovanOriginalniTekst);
                        konacnaVerzija += Environment.NewLine;
                    }
                    XElement konacnaVerzijaXElement = PripremaFajla(konacnaVerzija);
                    IEnumerable<XElement> konacnaVerzijaXElementRedoviSaRazlikama = konacnaVerzijaXElement.Elements("Red");

                    konacnaVerzija = ProveriBrojElemenata(redoviSaRazlikamaNarednogCommita, konacnaVerzijaXElementRedoviSaRazlikama, konacnaVerzija, enumerator);
                }
            }
            return konacnaVerzija;
        }

        public string PrimeniSveRazlike(FileInfo[] files, string nazivFajla, string preuzetaPutanja, string[] splitovanOriginalniTekst, IEnumerable<XElement> redSaRazlikamaPrviCommit, string konacnaVerzija, int iterator)
        {
            bool dodaoRazliku = false;
            foreach (FileInfo file in files)
            {
                if (nazivFajla.Equals(file.Name))
                {
                    preuzetaPutanja = Path.Combine(preuzetaPutanja, nazivFajla);
                    XElement razlikeIzNarednogCommita = m_CQRSRead.CitajRazlike(preuzetaPutanja);

                    IEnumerable<XElement> redoviSaRazlikamaNarednogCommita = razlikeIzNarednogCommita.Elements("Red");
                    IEnumerator<XElement> enumerator = redoviSaRazlikamaNarednogCommita.GetEnumerator();
                    enumerator.MoveNext();

                    for (int i = 0; i < splitovanOriginalniTekst.Length; i++)
                    {
                        IEnumerable<XElement> stareNoveRazlike = enumerator.Current.Elements("Razlika");

                        foreach (XElement razlika in stareNoveRazlike)
                        {
                            string staraRazlika = razlika.Attribute("inicijalno").Value;
                            string novaRazlika = razlika.Value;
                            if (staraRazlika != string.Empty)
                            {
                                splitovanOriginalniTekst[i] = ZameniPrvi(splitovanOriginalniTekst[i], staraRazlika, novaRazlika);
                                splitovanOriginalniTekst[i] = splitovanOriginalniTekst[i].Trim(null);
                            }
                            if (staraRazlika == string.Empty && novaRazlika != string.Empty)
                                splitovanOriginalniTekst[i] += novaRazlika; //+ ' ' je bilo

                            dodaoRazliku = true;
                        }
                        enumerator.MoveNext();

                    }
                    if (iterator == 1)
                    {
                        konacnaVerzija = String.Join(Environment.NewLine, splitovanOriginalniTekst);
                        konacnaVerzija += Environment.NewLine;
                    }
                    if (dodaoRazliku)
                    {
                        konacnaVerzija = String.Join(Environment.NewLine, splitovanOriginalniTekst);
                        konacnaVerzija += Environment.NewLine;
                    }
                    XElement konacnaVerzijaXElement = PripremaFajla(konacnaVerzija);
                    IEnumerable<XElement> konacnaVerzijaXElementRedoviSaRazlikama = konacnaVerzijaXElement.Elements("Red");


                    konacnaVerzija = ProveriBrojElemenata(redoviSaRazlikamaNarednogCommita, konacnaVerzijaXElementRedoviSaRazlikama, konacnaVerzija, enumerator);
                }
            }
            return konacnaVerzija;
        }

        public string ProveriBrojElemenata(IEnumerable<XElement> redoviSaRazlikamaNarednogCommita, IEnumerable<XElement> konacnaVerzijaXElementRedoviSaRazlikama, string konacnaVerzija, IEnumerator<XElement> enumerator)
        {
            if (redoviSaRazlikamaNarednogCommita.Count() >= konacnaVerzijaXElementRedoviSaRazlikama.Count())
            {
                do
                {
                    foreach (XElement razlika in enumerator.Current.Elements("Razlika"))
                    {
                        if (enumerator.Current.Value != String.Empty)
                            konacnaVerzija += razlika.Value + ' ';
                    }
                    konacnaVerzija += Environment.NewLine;

                } while (enumerator.MoveNext());
            }

            return konacnaVerzija;
        }

        public string[] VratiSveDokumente(string putanjaDoRepozitorijuma)
        {
            if (putanjaDoRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma ne sme biti nevalidna.\n");
            }
            if (putanjaDoRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma ne sme biti prazna.\n");
            }

            DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoRepozitorijuma);
            FileInfo[] Files = direktorijum.GetFiles("*.txt");
            string[] dokumenti = new string[Files.Length];

            for (int i = 0; i < Files.Length; i++)
            {
                dokumenti[i] = "\t" + (i + 1) + ". " + Files[i].Name;
            }
            return dokumenti;
        }

        public string[] FajloviRevizije(string putanjaDoRepozitorijuma, string izborKorisnika)
        {
            if (putanjaDoRepozitorijuma == null || izborKorisnika == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma i oznaka revizije ne smeju biti nevalidni.\n");
            }
            if (putanjaDoRepozitorijuma == "" || izborKorisnika == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma i oznaka revizije ne smeju biti prazni.\n");
            }
            return m_CQRSRead.VratiFajloveIzRevizije(Path.Combine(putanjaDoRepozitorijuma, ".vc"), izborKorisnika);
        }

        public bool PostojiCommit(string nazivOriginalnogFajla, string putanjaUnutarCommita)
        {
            if (nazivOriginalnogFajla == null || putanjaUnutarCommita == null)
            {
                throw new ArgumentNullException("\nPutanja do fajla sa revizijama ne sme biti nevalidna.\n");
            }
            if (nazivOriginalnogFajla == "" || putanjaUnutarCommita == "")
            {
                throw new ArgumentException("\nPutanja do fajla sa revizijama ne sme biti prazna.\n");
            }

            DirectoryInfo direktorijum = new DirectoryInfo(putanjaUnutarCommita);

            FileInfo[] Files = direktorijum.GetFiles("*");

            foreach (FileInfo file in Files)
            {
                if (nazivOriginalnogFajla.Equals(file.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ProveriDaLiPostojiCommit(string putanjaZaFoldereIFajloveSaRazlikama, List<string> fajloviNadKojimaSeSprovodiRevizija, int iterator, string putanjaDoFolderaSvakeRevizije, string putanjaDoOriginalnogFajla, string referencaNaRepozitorijum, string nazivCommita, string nazivAutora, bool dodaoFajlSaRazlikama)
        {
            if (!Directory.Exists(putanjaZaFoldereIFajloveSaRazlikama))
            {
                Directory.CreateDirectory(putanjaZaFoldereIFajloveSaRazlikama);

                putanjaZaFoldereIFajloveSaRazlikama = Path.Combine(putanjaZaFoldereIFajloveSaRazlikama, fajloviNadKojimaSeSprovodiRevizija[iterator]);
                File.Create(putanjaZaFoldereIFajloveSaRazlikama).Dispose();
                string putanjaGdeTrebaSacuvatiRazlike = putanjaZaFoldereIFajloveSaRazlikama; //putanja to .txt fajla unutar foldera npr. C2

                UcitajRazlike(putanjaDoFolderaSvakeRevizije, fajloviNadKojimaSeSprovodiRevizija[iterator], putanjaDoOriginalnogFajla, referencaNaRepozitorijum, nazivCommita, nazivAutora, putanjaGdeTrebaSacuvatiRazlike);
            }

            else
            {
                dodaoFajlSaRazlikama = DodajFajlSaRazlikama(fajloviNadKojimaSeSprovodiRevizija, iterator, putanjaZaFoldereIFajloveSaRazlikama, putanjaDoFolderaSvakeRevizije, putanjaDoOriginalnogFajla, referencaNaRepozitorijum, nazivCommita, nazivAutora);
            }

            return dodaoFajlSaRazlikama;
        }

        public bool DodajFajlSaRazlikama(List<string> fajloviNadKojimaSeSprovodiRevizija, int iterator, string putanjaZaFoldereIFajloveSaRazlikama, string putanjaDoFolderaSvakeRevizije, string putanjaDoOriginalnogFajla, string referencaNaRepozitorijum, string nazivCommita, string nazivAutora)
        {
            if (PostojiCommit(fajloviNadKojimaSeSprovodiRevizija[iterator], putanjaZaFoldereIFajloveSaRazlikama))
                return false;

            Directory.CreateDirectory(putanjaZaFoldereIFajloveSaRazlikama);

            putanjaZaFoldereIFajloveSaRazlikama = Path.Combine(putanjaZaFoldereIFajloveSaRazlikama, fajloviNadKojimaSeSprovodiRevizija[iterator]);
            File.Create(putanjaZaFoldereIFajloveSaRazlikama).Dispose();
            string putanjaDoFajlaKojiSadrziRazlike = putanjaZaFoldereIFajloveSaRazlikama; //putanja to .txt fajla unutar foldera npr. C2
            UcitajRazlike(putanjaDoFolderaSvakeRevizije, fajloviNadKojimaSeSprovodiRevizija[iterator], putanjaDoOriginalnogFajla, referencaNaRepozitorijum, nazivCommita, nazivAutora, putanjaDoFajlaKojiSadrziRazlike);

            return true;
        }

        public bool DodavanjeFajlaSaRazlikamaUCommitKojiPostoji(bool dodaoFajlUOdgovarajuciCommit, string nazivAutora, string preuzetaPutanja, string[] nazivOriginalnogSplitovan, string[] putanjeDoCommita)
        {
            if (dodaoFajlUOdgovarajuciCommit)
                return true;

            Directory.CreateDirectory(preuzetaPutanja);
            NapraviMeta(m_CQRSWrite, nazivAutora, nazivOriginalnogSplitovan[0], putanjeDoCommita[2], preuzetaPutanja, putanjeDoCommita[0]);
            return true;
        }

        public XElement PripremaFajla(string sadrzajCommita)
        {
            string[] charSeparators = new string[] { "\r\n" };
            string[] splitovanSadrzajCommita = sadrzajCommita.Split(charSeparators, StringSplitOptions.None);

            XElement sadrzajCommitaXElement = new XElement("Fajl1");
            foreach (string redSaRazlikama in splitovanSadrzajCommita)
            {
                XElement staroNovoStanje = new XElement("Red");
                foreach (string konkretnaRazlika in redSaRazlikama.Split(' '))
                    staroNovoStanje.Add(new XElement("rec", konkretnaRazlika));
                sadrzajCommitaXElement.Add(staroNovoStanje);
            }
            return sadrzajCommitaXElement;
        }

        public List<string> VratiRevizije(int izborKorisnika, string putanjaDoRepozitorijuma)
        {
            if (putanjaDoRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma ne sme biti nevalidna.\n");
            }
            if (izborKorisnika < 0 || putanjaDoRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma i jedinstvena oznaka revizije ne smeju biti prazni.\n");
            }

            string nazivIzabranogFajla = "";

            if (izborKorisnika == 0)
                return m_CQRSRead.PosaljiSveRevizije(Path.Combine(putanjaDoRepozitorijuma, ".vc"));

            DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoRepozitorijuma);
            FileInfo[] Files = direktorijum.GetFiles("*.txt");

            for (int i = 0; i < Files.Length; i++)
            {
                if ((izborKorisnika - 1) == i)
                {
                    nazivIzabranogFajla = Files[i].Name;
                    break;

                }
            }

            return m_CQRSRead.PosaljiRevizije(Path.Combine(putanjaDoRepozitorijuma, ".vc"), nazivIzabranogFajla);
        }

        //pomocna koja se ne testira
        private static string ZameniPrvi(string text, string stringZaZamenu, string stringSaKojimMenja)
        {
            int pocetniIndexStringa = text.IndexOf(stringZaZamenu);

            if (pocetniIndexStringa < 0)
            {
                return text;
            }

            return text.Substring(0, pocetniIndexStringa) + stringSaKojimMenja + text.Substring(pocetniIndexStringa + stringZaZamenu.Length);
        }
    }
}
