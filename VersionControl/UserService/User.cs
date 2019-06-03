using AmbassadorService;
using Common;
using CQRSService;
using EventSourcingService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService
{
    public class User : IUser
    {
        private static IAmbassador m_IAmbassador;
        private static IReadWrite _readWrite;
        private string putanjaDoRepozitorijumiText = System.IO.Directory.GetCurrentDirectory() + "\\Repozitorijumi.txt";

        public string GetPutanjaDoRepozitorijumiText()
        {
            return putanjaDoRepozitorijumiText;
        }

        public void SetPutanjaDoRepozitorijumiText(string text)
        {
            putanjaDoRepozitorijumiText = text;
        }
        public User()
        {

        }
        public void Inicijalizuj()
        {
            ICQRSRead read = new CQRSRead();
            ICQRSWrite write = new CQRSWrite();
            IEventSourcing es = new EventSourcing(read, write);
            IAmbassador ambassador = new Ambassador(es);
            _readWrite = new ReadWrite();
            IUser iu = new User(ambassador);
        }

        public User(IAmbassador ambasador)
        {
            m_IAmbassador = ambasador;
        }

        public void Meni()
        {
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            do
            {

                Console.WriteLine("\n\n=== MENI ===\n");
                Console.WriteLine("1. Prikaz svih dokumenata u odabranom folderu");
                Console.WriteLine("2. Prikaz revizija koje su se dogodile nad dokumentima u trenutnom folderu");
                Console.WriteLine("3. Odabir zeljene revizije dokumenata");
                Console.WriteLine("4. Pravljenje nove revizije nad dokumentima");
                Console.WriteLine("5. Pretraga revizija");
                Console.WriteLine("6. Izlazak iz programa");
                Console.Write("Vas izbor: ");


                keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        {
                            PrikaziFajlove();
                            break;

                        }
                    case ConsoleKey.D2:
                        {
                            PrikaziSveRevizije();
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            OdaberiReviziju();
                            break;
                        }
                    case ConsoleKey.D4:
                        {
                            NapraviReviziju();
                            break;
                        }

                    case ConsoleKey.D5:
                        {
                            PretragaRevizija();
                            break;
                        }

                    case ConsoleKey.D6:
                        break;
                    default:
                        {
                            Console.WriteLine("\nNepostojeca opcija. Pokusajte ponovo");
                            break;
                        }
                }


            } while (keyInfo.Key != ConsoleKey.D6);
        }
        public void NapraviReviziju()
        {
            string nazivRepozitorijuma = "";
            string putanjaDoRepozitorijumiTxt = "";
            RepozitorijumiInfo repoInfo = new RepozitorijumiInfo();
            int izborKorisnika = 0;

            nazivRepozitorijuma = UnesiRepozitorijum();
            putanjaDoRepozitorijumiTxt = ProveraIKreiranjeLogFajla(GetPutanjaDoRepozitorijumiText());
            repoInfo = ReferenciranjeNaPutanju(putanjaDoRepozitorijumiTxt, nazivRepozitorijuma);

            if (repoInfo == null)
                repoInfo = KreirajRepozitorijum(putanjaDoRepozitorijumiTxt, nazivRepozitorijuma);

            Console.WriteLine("\nIzaberite jednu od opcija za kreiranje nove revizije:\n");
            Console.WriteLine("\t1. Zelim da kreiram novu reviziju nad svim dokumentima\n\t2. Zelim da kreiram novu reviziju nad konkretnim dokumentom\n");
            Console.Write("Vas izbor: ");
            izborKorisnika = Int32.Parse(Console.ReadLine());

            if (izborKorisnika == 1)
            {
                SprovediRevizijuNadSvim(repoInfo.PreuzetaPutanja, repoInfo.NazivAutora, nazivRepozitorijuma);
                return;
            }

            SprovediRevizijuNadKonkretnim(repoInfo.PreuzetaPutanja, repoInfo.NazivAutora, nazivRepozitorijuma);
        }
        public void SprovediRevizijuNadSvim(string putanjaDoRepozitorijuma, string nazivAutora, string nazivRepozitorijuma)
        {
            if (putanjaDoRepozitorijuma == null || nazivAutora == null || nazivRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma, naziv autora i naziv repozitorijuma ne smeju biti nevalidni.\n");
            }
            if (putanjaDoRepozitorijuma == "" || nazivAutora == "" || nazivRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma, naziv autora i naziv repozitorijuma ne smeju biti prazni.\n");
            }

            int redniBrojFajla = 0;
            List<string> fajloviNadKojimSeSprovodiRevizija = new List<string>();
            DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoRepozitorijuma);

            FileInfo[] Files = direktorijum.GetFiles("*.txt");

            Console.WriteLine("\nPrikazana je lista fajlova iz repozitorijuma \"{0}\" autora \"{1}\". Nad svim prikazanim fajlovima bice sprovedena revizija\n", nazivRepozitorijuma, nazivAutora);
            foreach (FileInfo file in Files)
            {
                redniBrojFajla++;
                fajloviNadKojimSeSprovodiRevizija.Add(file.Name);
                Console.WriteLine("\t" + redniBrojFajla + ". " + file.Name);
            }

            m_IAmbassador.NapraviReviziju(putanjaDoRepozitorijuma, nazivAutora, fajloviNadKojimSeSprovodiRevizija);
        }
        public void SprovediRevizijuNadKonkretnim(string putanjaDoRepozitorijuma, string nazivAutora, string nazivRepozitorijuma)
        {
            if (putanjaDoRepozitorijuma == null || nazivAutora == null || nazivRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma, naziv autora i naziv repozitorijuma ne smeju biti nevalidni.\n");
            }
            if (putanjaDoRepozitorijuma == "" || nazivAutora == "" || nazivRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma, naziv autora i naziv repozitorijuma ne smeju biti prazni.\n");
            }

            int izborKorisnika = 0;
            List<string> listaOriginalnihFajlova = new List<string>();
            List<string> fajlNadKojimSeSprovodiRevizija = new List<string>();
            int redniBrojFajla = 0;

            DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoRepozitorijuma);
            FileInfo[] Files = direktorijum.GetFiles("*.txt");

            Console.WriteLine("\nPrikazana je lista fajlova iz repozitorijuma \"{0}\" autora \"{1}\". Potrebno je da izaberete konkretan fajl nad kojim bi da sprovedete reviziju.\n", nazivRepozitorijuma, nazivAutora);
            foreach (FileInfo file in Files)
            {
                redniBrojFajla++;
                listaOriginalnihFajlova.Add(file.Name);
                Console.WriteLine("\t" + redniBrojFajla + ". " + file.Name);

            }

            Console.Write("Vas izbor: ");
            izborKorisnika = Int32.Parse(Console.ReadLine());

            for (int i = 0; i < listaOriginalnihFajlova.Count; i++)
            {
                if ((izborKorisnika - 1) == i)
                {
                    fajlNadKojimSeSprovodiRevizija.Add(listaOriginalnihFajlova[i]);
                    break;
                }
            }

            m_IAmbassador.NapraviReviziju(putanjaDoRepozitorijuma, nazivAutora, fajlNadKojimSeSprovodiRevizija);
        }
        public void UpisiURepozitorijumiTxt(string putanjaDoRepozitorijumiTxt, RepozitorijumiInfo repoInfo)
        {
            if (putanjaDoRepozitorijumiTxt == null || repoInfo == null)
            {
                throw new ArgumentNullException("\nPutanja do log fajla i informacije o autoru ne smeju biti nevalidni.\n");
            }
            if (putanjaDoRepozitorijumiTxt == "" || repoInfo == null)
            {
                throw new ArgumentException("\nPutanja do log fajla i informacije o autoru ne smeju biti prazni.\n");
            }

            using (StreamWriter tw = File.AppendText(putanjaDoRepozitorijumiTxt))
            {
                tw.Write(repoInfo.NazivAutora);
                tw.Write(" ");
                tw.Write(repoInfo.PreuzetaPutanja);
                tw.Write(" ");
                tw.Write(repoInfo.NazivRepozitorijuma);
                tw.WriteLine();
                tw.Close();
            }
        }
        public void OdaberiReviziju()
        {
            int izborKorisnika = 0;
            string nazivIzabranogFajla = "";
            List<string> commiti = new List<string>();
            string nazivRepozitorijuma = "";

            nazivRepozitorijuma = UnesiRepozitorijum();
            RepozitorijumiInfo repoInfo = CitajIzLoga(nazivRepozitorijuma);

            if (repoInfo == null)
                return;

            nazivIzabranogFajla = IzaberiFajlZaPrikazNjegovihRevizija(repoInfo.PreuzetaPutanja, nazivRepozitorijuma, repoInfo.NazivAutora);

            string putanjaDoRepozitorijuma = repoInfo.PreuzetaPutanja;
            repoInfo.PreuzetaPutanja = Path.Combine(repoInfo.PreuzetaPutanja, ".vc");

            commiti = PronadjiRevizijeZaIzabraniFajl(nazivIzabranogFajla, repoInfo.PreuzetaPutanja);

            Console.WriteLine("\nPrikazane su revizije koje postoje za fajl \"{0}\", u repozitorijumu \"{1}\", autora \"{2}\". Izaberite reviziju koju zelite da primenite\n", nazivIzabranogFajla, nazivRepozitorijuma, repoInfo.NazivAutora);
            for (int i = 0; i < commiti.Count(); i++)
            {

                Console.WriteLine("\t" + (i + 1) + ". " + commiti[i]);
            }
            Console.Write("\nVas izbor: ");
            izborKorisnika = Int32.Parse(Console.ReadLine());

            m_IAmbassador.ObradiReviziju(repoInfo.PreuzetaPutanja, nazivIzabranogFajla, izborKorisnika, putanjaDoRepozitorijuma);
        }
        public void UpisiNovuPutanju(string putanjaDoRepozitorijumiTxt, string[] noveKonkretneInformacije)
        {
            if (noveKonkretneInformacije == null || putanjaDoRepozitorijumiTxt == null)
            {
                throw new ArgumentNullException("\nPutanja do log fajla ne sme biti nevalidna.\n");
            }
            if (noveKonkretneInformacije.Count() == 0 || putanjaDoRepozitorijumiTxt == "")
            {
                throw new ArgumentException("\nPutanja do log fajla i podaci o autoru ne smeju biti prazni.\n");
            }
            using (StreamWriter tw = File.AppendText(putanjaDoRepozitorijumiTxt))
            {
                tw.Write(noveKonkretneInformacije[0]);
                tw.Write(" ");
                tw.Write(noveKonkretneInformacije[1]);
                tw.Write(" ");
                tw.Write(noveKonkretneInformacije[2]);
                tw.WriteLine();
                tw.Close();
            }
        }
        public void PrikaziFajlove()
        {
            string nazivRepozitorijuma = "";
            string putanjaDoRepozitorijumiTxt = "";
            RepozitorijumiInfo repoInfo = new RepozitorijumiInfo();

            nazivRepozitorijuma = UnesiRepozitorijum();
            putanjaDoRepozitorijumiTxt = ProveraIKreiranjeLogFajla(GetPutanjaDoRepozitorijumiText());
            repoInfo = ReferenciranjeNaPutanju(putanjaDoRepozitorijumiTxt, nazivRepozitorijuma);

            if (repoInfo.PreuzetaPutanja == null)
                repoInfo = KreirajRepozitorijum(putanjaDoRepozitorijumiTxt, nazivRepozitorijuma);

            Console.WriteLine("\nAutor repozitorijuma \"{0}\" u svom repozitorijumu \"{1}\" ima sledece fajlove:\n", repoInfo.NazivAutora, nazivRepozitorijuma);

            foreach (string dokument in m_IAmbassador.PrikaziSveDokumente(repoInfo.PreuzetaPutanja))
            {
                Console.WriteLine(dokument);
            }
        }
        public void PrikaziRevizijeZaSveDokumente(string putanjaDoVC, string nazivRepozitorijuma, string nazivAutora)
        {
            if (putanjaDoVC == null || nazivRepozitorijuma == null || nazivAutora == null)
            {
                throw new ArgumentNullException("\nPutanja do foldera sa revizijama, naziv repozitorijuma i naziv autora ne smeju biti nevalidni.\n");
            }

            if (putanjaDoVC == "" || nazivRepozitorijuma == "" || nazivAutora == "")
            {
                throw new ArgumentException("\nPutanja do foldera sa revizijama, naziv repozitorijuma i naziv autora ne smeju biti prazni.\n");
            }

            List<string> commiti = new List<string>();
            commiti = m_IAmbassador.VratiSveRevizije(0, putanjaDoVC);

            Console.WriteLine("\nPrikazane su sve revizije koje postoje u repozitorijumu \"{0}\", autora \"{1}\".\n", nazivRepozitorijuma, nazivAutora);
            for (int i = 0; i < commiti.Count(); i++)
            {
                Console.WriteLine("\t" + (i + 1) + ". " + commiti[i]);
            }
        }
        public void PrikaziRevizijeZaKonkretanDokument(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string NazivAutora)
        {
            if (putanjaDoRepozitorijuma == null || nazivRepozitorijuma == null || NazivAutora == null)
            {
                throw new ArgumentNullException("\nPutanja do foldera sa revizijama, naziv repozitorijuma i naziv autora ne smeju biti nevalidni.\n");
            }

            if (putanjaDoRepozitorijuma == "" || nazivRepozitorijuma == "" || NazivAutora == "")
            {
                throw new ArgumentException("\nPutanja do foldera sa revizijama, naziv repozitorijuma i naziv autora ne smeju biti prazni.\n");
            }

            int redniBrojFajla = 0;
            int izborKorisnika = 0;
            List<string> commiti = new List<string>();

            DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoRepozitorijuma);
            FileInfo[] Files = direktorijum.GetFiles("*.txt");

            Console.WriteLine("\nPrikazana je lista fajlova iz repozitorijuma \"{0}\" autora \"{1}\". Potrebno je da izaberete fajl za prikaz njegovih revizija.\n", nazivRepozitorijuma, NazivAutora);
            foreach (FileInfo file in Files)
            {
                redniBrojFajla++;
                Console.WriteLine("\t" + redniBrojFajla + ". " + file.Name);

            }
            Console.Write("Vas izbor: ");
            izborKorisnika = Int32.Parse(Console.ReadLine());

            commiti = m_IAmbassador.VratiSveRevizije(izborKorisnika, putanjaDoRepozitorijuma);

            Console.WriteLine("\nPrikazane su revizije koje postoje za izabrani fajl, u repozitorijumu \"{0}\", autora \"{1}\".\n", nazivRepozitorijuma, NazivAutora);
            for (int i = 0; i < commiti.Count(); i++)
            {
                redniBrojFajla = i + 1;
                Console.WriteLine("\t" + redniBrojFajla + ". " + commiti[i]);
            }
        }
        public void PrikaziSveRevizije()
        {
            string nazivRepozitorijuma = "";
            int izborKorisnika = 0;
            List<string> commiti = new List<string>();

            nazivRepozitorijuma = UnesiRepozitorijum();
            RepozitorijumiInfo repoInfo = CitajIzLoga(nazivRepozitorijuma);

            Console.WriteLine("\nIzaberite jednu od opcija za prikaz revizija:\n");
            Console.WriteLine("\t1. Zelim da prikazem sve revizije koje postoje\n\t2. Zelim da prikazem revizije koje su vezane za konkretan dokument\n");
            Console.Write("Vas izbor: ");
            izborKorisnika = Int32.Parse(Console.ReadLine());

            if (izborKorisnika == 1)
            {
                PrikaziRevizijeZaSveDokumente(repoInfo.PreuzetaPutanja, nazivRepozitorijuma, repoInfo.NazivAutora);
                return;
            }

            PrikaziRevizijeZaKonkretanDokument(repoInfo.PreuzetaPutanja, nazivRepozitorijuma, repoInfo.NazivAutora);
        }
        public void PretragaRevizija()
        {
            string nazivRepozitorijuma = "";
            string izborKorisnika = "";
            int redniBrojCommita = 0;

            nazivRepozitorijuma = UnesiRepozitorijum();
            RepozitorijumiInfo repoInfo = CitajIzLoga(nazivRepozitorijuma);

            Console.Write("\nUnesite jedinstvenu oznaku revizije: ");
            izborKorisnika = Console.ReadLine();

            string[] preuzetiFajlovi = m_IAmbassador.PretraziReviziju(repoInfo.PreuzetaPutanja, izborKorisnika);

            Console.WriteLine("\nPrikazana je lista fajlova iz commit-a \"{0}\", unutar repozitorijuma \"{1}\", autora \"{2}\".\n", izborKorisnika, nazivRepozitorijuma, repoInfo.NazivAutora);
            foreach (string fajl in preuzetiFajlovi)
            {
                redniBrojCommita++;
                Console.WriteLine("\t" + redniBrojCommita + ". " + fajl);

            }
        }
        public string UnesiRepozitorijum()
        {
            Console.WriteLine("\n\nMolimo Vas popunite sledecu formu\n");
            Console.Write("Unesite naziv repozitorijuma: ");
            string nazivRepozitorijuma = Console.ReadLine();

            return nazivRepozitorijuma;
        }
        public string ProveraIKreiranjeLogFajla(string putanjaDoRepozitorijumiTxt)
        {
           if (!File.Exists(putanjaDoRepozitorijumiTxt))
            {
                File.Create(putanjaDoRepozitorijumiTxt).Dispose();
            }

            return putanjaDoRepozitorijumiTxt;
        }
        public string IzaberiFajlZaPrikazNjegovihRevizija(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string nazivAutora)
        {
            if (putanjaDoRepozitorijuma == null || nazivAutora == null || nazivRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do repozitorijuma, naziv autora i naziv repozitorijuma ne smeju biti nevalidni.\n");
            }
            if (putanjaDoRepozitorijuma == "" || nazivAutora == "" || nazivRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do repozitorijuma, naziv autora i naziv repozitorijuma ne smeju biti prazni.\n");
            }
            string nazivIzabranogFajla = "";
            int izborKorisnika = 0;
            int redniBrojFajla = 0;

            DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoRepozitorijuma);
            FileInfo[] Files = direktorijum.GetFiles("*.txt");

            Console.WriteLine("\nPrikazana je lista fajlova iz repozitorijuma \"{0}\" autora \"{1}\". Potrebno je da izaberete fajl za prikaz njegovih revizija.\n", nazivRepozitorijuma, nazivAutora);
            foreach (FileInfo file in Files)
            {
                redniBrojFajla++;
                Console.WriteLine("\t" + redniBrojFajla + ". " + file.Name);

            }

            Console.Write("Vas izbor: ");
            izborKorisnika = Int32.Parse(Console.ReadLine());

            for (int i = 0; i < Files.Length; i++)
            {
                if ((izborKorisnika - 1) == i)
                {
                    nazivIzabranogFajla = Files[i].Name;
                    break;

                }
            }

            return nazivIzabranogFajla;
        }
        public RepozitorijumiInfo CitajIzLoga(string nazivRepozitorijuma)
        {
            if (nazivRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nNaziv repozitorijuma ne sme biti nevalidna vrednost.\n");
            }
            if (nazivRepozitorijuma == "")
            {
                throw new ArgumentException("\nNaziv repozitorijuma ne sme biti prazna vrednost.\n");
            }

            string putanjaDoRepozitorijumiTxt = "";
            string[] listaAutora = new string[] { };
            string[] konkretnaInformacija = new string[] { };

            RepozitorijumiInfo repoInfo = new RepozitorijumiInfo();
            putanjaDoRepozitorijumiTxt = ProveraIKreiranjeLogFajla(GetPutanjaDoRepozitorijumiText());

            listaAutora = File.ReadAllLines(putanjaDoRepozitorijumiTxt);
            foreach (string informacije in listaAutora)
            {
                konkretnaInformacija = informacije.Split(' ');

                if (konkretnaInformacija[2].Equals(nazivRepozitorijuma)) //ako ima taj autor, uzmi putanju do njegovog foldera kako bi izlistao dokumente iz foldera
                {
                    repoInfo.PreuzetaPutanja = konkretnaInformacija[1];
                    repoInfo.NazivAutora = konkretnaInformacija[0];
                }
            }
            return repoInfo;
        }
        public RepozitorijumiInfo KreirajRepozitorijum(string putanjaDoRepozitorijumiTxt, string nazivRepozitorijuma)
        {
            if (putanjaDoRepozitorijumiTxt == null || nazivRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do log txt fajla i naziv repozitorijuma ne mogu biti nevalidni.\n");
            }
            if (putanjaDoRepozitorijumiTxt == "" || nazivRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do log txt fajla i naziv repozitorijuma ne mogu biti prazni.\n");
            }

            RepozitorijumiInfo repoInfo = new RepozitorijumiInfo();

            Console.WriteLine("\nRepozitorijum \"{0}\" nije kreiran. Potrebno je izvrsiti kreiranje repozitorijuma. Molimo Vas popunite sledecu formu.\n", nazivRepozitorijuma);

            Console.WriteLine("\nUnesite putanju do repozitorijuma: ");
            repoInfo.PreuzetaPutanja = Console.ReadLine();

            Console.WriteLine("\nUnesite naziv repozitorijuma: ");
            repoInfo.NazivRepozitorijuma = Console.ReadLine();

            Console.WriteLine("\nUnesite autora repozitorijuma: ");
            repoInfo.NazivAutora = Console.ReadLine();

            UpisiURepozitorijumiTxt(putanjaDoRepozitorijumiTxt, repoInfo);

            Directory.CreateDirectory(Path.Combine(repoInfo.PreuzetaPutanja, ".vc"));

            return repoInfo;
        }
        public RepozitorijumiInfo ReferenciranjeNaPutanju(string putanjaDoRepozitorijumiTxt, string nazivRepozitorijuma)
        {
            if (putanjaDoRepozitorijumiTxt == null || nazivRepozitorijuma == null)
            {
                throw new ArgumentNullException("\nPutanja do log txt fajla i naziv repozitorijuma ne mogu biti nevalidni.\n");
            }

            if (putanjaDoRepozitorijumiTxt == "" || nazivRepozitorijuma == "")
            {
                throw new ArgumentException("\nPutanja do log txt fajla i naziv repozitorijuma ne mogu biti prazni.\n");
            }

            string[] listaAutora = new string[] { };
            string[] konkretnaInformacija = new string[] { };
            RepozitorijumiInfo repoInfo = new RepozitorijumiInfo();

            listaAutora = File.ReadAllLines(putanjaDoRepozitorijumiTxt);
            foreach (string informacija in listaAutora)
            {
                //0. je autor, 1. je putanja 2. je naziv repozitorijuma
                konkretnaInformacija = informacija.Split(' ');
                CitanjePutanjeDoRepozitorijuma(konkretnaInformacija, nazivRepozitorijuma, putanjaDoRepozitorijumiTxt, repoInfo);
            }
            return repoInfo;
        }
        public RepozitorijumiInfo PromenaRepozitorijumaAutora(string[] konkretnaInformacija, string nazivRepozitorijuma, string putanjaDoRepozitorijumiTxt, RepozitorijumiInfo repoInfo)
        {
            string[] noveKonkretneInformacije = new string[] { };
            string[] listaAutora = File.ReadAllLines(putanjaDoRepozitorijumiTxt);

            Console.WriteLine("\nUnesite putanju do novog repozitorijuma: ");
            repoInfo.PreuzetaPutanja = Console.ReadLine();

            foreach (string informacija in listaAutora)
            {
                //0. je autor, 1. je putanja 2. je naziv repozitorijuma
                konkretnaInformacija = informacija.Split(' ');

                if (konkretnaInformacija[2].Equals(nazivRepozitorijuma)) //ako ima taj autor, uzmi putanju do njegovog foldera kako bi izlistao dokumente iz foldera
                {
                    listaAutora = File.ReadAllLines(putanjaDoRepozitorijumiTxt);
                    File.Delete(putanjaDoRepozitorijumiTxt);

                    foreach (string noveInformacije in listaAutora)
                    {
                        noveKonkretneInformacije = noveInformacije.Split(' ');

                        if (noveKonkretneInformacije[0].Equals(repoInfo.NazivAutora))
                        {
                            noveKonkretneInformacije[1] = repoInfo.PreuzetaPutanja;
                        }

                        UpisiNovuPutanju(putanjaDoRepozitorijumiTxt, noveKonkretneInformacije);
                    }
                }
            }
            return repoInfo;
        }
        public RepozitorijumiInfo CitanjePutanjeDoRepozitorijuma(string[] konkretnaInformacija, string nazivRepozitorijuma, string putanjaDoRepozitorijumiTxt, RepozitorijumiInfo repoInfo)
        {
            //int IzborKorisnika = 0;
            if (konkretnaInformacija[2].Equals(nazivRepozitorijuma)) //ako ima taj autor, uzmi putanju do njegovog foldera kako bi izlistao dokumente iz foldera
            {
                repoInfo.NazivAutora = konkretnaInformacija[0];
                repoInfo.PreuzetaPutanja = konkretnaInformacija[1];
                repoInfo.NazivRepozitorijuma = nazivRepozitorijuma;

                Console.WriteLine("\nVi kao autor \"{0}\" trenutno radite nad repozitorijumom \"{1}\" na putanji \"{2}\". Zelite li da prikazete fajlove iz ovog repozitorijuma?", repoInfo.NazivAutora, nazivRepozitorijuma, repoInfo.PreuzetaPutanja);
                Console.WriteLine("\t" + "1. Da" + "\n" + "\t" + "2. Ne\n");
                Console.Write("Vas izbor: ");
                string izborKorisnika = Console.ReadLine();
                //IzborKorisnika = Int32.Parse(Console.ReadLine());

                if (izborKorisnika == "2")
                {
                    PromenaRepozitorijumaAutora(konkretnaInformacija, nazivRepozitorijuma, putanjaDoRepozitorijumiTxt, repoInfo);
                }
            }
            return repoInfo;
        }
        public List<string> PronadjiRevizijeZaIzabraniFajl(string nazivIzabranogFajla, string putanjaDoVC)
        {
            if (nazivIzabranogFajla == null || putanjaDoVC == null)
            {
                throw new ArgumentNullException("\nNaziv fajla i putanja do foldera sa revizijama ne smeju biti nevalidni.\n");
            }
            if (nazivIzabranogFajla == "" || putanjaDoVC == "")
            {
                throw new ArgumentException("\nNaziv fajla i putanja do foldera sa revizijama ne smeju biti prazni.\n");
            }

            List<string> commiti = new List<string>();
            string putanjaDoFajlaSaRazlikama = "";
            string[] listaDirektorijuma = Directory.GetDirectories(putanjaDoVC).Select(Path.GetFileName).ToArray();

            for (int i = 0; i < listaDirektorijuma.Length; i++)
            {
                putanjaDoFajlaSaRazlikama = putanjaDoVC;
                putanjaDoFajlaSaRazlikama = Path.Combine(putanjaDoVC, listaDirektorijuma[i]);

                DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoFajlaSaRazlikama);
                FileInfo[] Files = direktorijum.GetFiles("*");
                foreach (FileInfo file in Files)
                {
                    if (nazivIzabranogFajla.Equals(file.Name))
                    {
                        commiti.Add(listaDirektorijuma[i]);
                        break;
                    }
                }
            }

            return commiti;
        }
    }
}
