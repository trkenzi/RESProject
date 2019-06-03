using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CQRSService
{
    public class CQRSRead : ICQRSRead
    {
        public CQRSRead()
        {

        }

        public string CitajCeoFajl(string putanjaDoOriginalnogFajla)
        {
            if (putanjaDoOriginalnogFajla == null)
            {
                throw new ArgumentNullException("\nPutanja ne sme biti prazna\n.");
            }

            if (!File.Exists(putanjaDoOriginalnogFajla) || putanjaDoOriginalnogFajla == "")
            {
                throw new ArgumentException("\nNe postoji putanja do dokumenta ciji sadrzaj treba ucitati\n.");
            }

            return File.ReadAllText(putanjaDoOriginalnogFajla);
        }
        public string[] VratiFajloveIzRevizije(string putanjaDoVC, string izabraniFajl)
        {
            if (putanjaDoVC == "" || izabraniFajl == "")
            {
                throw new ArgumentException("\nPutanja do foldera sa revizijama ili naziv fajla ne smeju biti prazni.\n");
            }
            if (putanjaDoVC == null || izabraniFajl == null)
            {
                throw new ArgumentNullException("\nPutanja do foldera sa revizijama ili naziv fajla ne smeju biti nevalidni.\n");
            }

            string[] fajlovi = new string[] { };
            string[] subdirs = Directory.GetDirectories(putanjaDoVC).Select(Path.GetFileName).ToArray();

            for (int i = 0; i < subdirs.Length; i++)
            {
                if (subdirs[i] == izabraniFajl)
                {
                    putanjaDoVC = Path.Combine(putanjaDoVC, izabraniFajl);
                    DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoVC);
                    FileInfo[] Files = direktorijum.GetFiles("*");

                    fajlovi = new string[Files.Length];
                    for (int j = 0; j < fajlovi.Length; j++)
                    {
                        fajlovi[j] = Files[j].Name;
                    }
                }
            }
            return fajlovi;
        }
        public List<string> PosaljiSveRevizije(string putanjaDoVC)
        {
            if (putanjaDoVC == "")
            {
                throw new ArgumentException("\nPutanja do foldera sa revizijama ne sme biti prazna.\n");
            }
            if (putanjaDoVC == null)
            {
                throw new ArgumentNullException("\nPutanja do foldera sa revizijama ne sme biti nevalidna.\n");
            }

            List<string> commiti = new List<string>();
            string[] subdirs = Directory.GetDirectories(putanjaDoVC).Select(Path.GetFileName).ToArray();

            for (int i = 0; i < subdirs.Length; i++)
            {
                commiti.Add(subdirs[i]);
            }
            return commiti;
        }
        public List<string> PosaljiRevizije(string putanjaDoVC, string nazivFajla)
        {
            if (putanjaDoVC == null || nazivFajla == null)
            {
                throw new ArgumentNullException("\nPutanja do foldera sa revizijama ili naziv fajla ne smeju biti nevalidni.\n");
            }

            if (putanjaDoVC == "" || nazivFajla == "")
            {
                throw new ArgumentException("\nPutanja do foldera sa revizijama ili naziv fajla ne smeju biti nevalidni.\n");
            }
            string putanjaDoFolderaSaRazlikama = "";
            List<string> commiti = new List<string>();

            string[] subdirs = Directory.GetDirectories(putanjaDoVC).Select(Path.GetFileName).ToArray();

            for (int i = 0; i < subdirs.Length; i++)
            {
                putanjaDoFolderaSaRazlikama = putanjaDoVC;
                putanjaDoFolderaSaRazlikama = Path.Combine(putanjaDoVC, subdirs[i]);
                DirectoryInfo direktorijum = new DirectoryInfo(putanjaDoFolderaSaRazlikama);
                FileInfo[] Files1 = direktorijum.GetFiles("*");
                foreach (FileInfo f in Files1)
                {
                    if (nazivFajla.Equals(f.Name))
                    {
                        commiti.Add(subdirs[i]);
                        break;

                    }
                }
            }
            return commiti;
        }
        public XElement CitajRazlike(string putanjaDoFajlaSaRazlikama)
        {
            if (putanjaDoFajlaSaRazlikama == null)
            {
                throw new ArgumentNullException("\nPutanja ne sme biti prazna\n.");
            }
            if (!File.Exists(putanjaDoFajlaSaRazlikama) || putanjaDoFajlaSaRazlikama == "")
            {
                throw new ArgumentException("\nNe postoji putanja do dokumenta sa razlikama.\n");
            }

            return XElement.Load(putanjaDoFajlaSaRazlikama);
        }

    }
}
