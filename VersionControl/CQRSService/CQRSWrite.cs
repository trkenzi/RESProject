using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSService
{
    public class CQRSWrite : ICQRSWrite
    {
        public CQRSWrite()
        {

        }

        public string[] KreirajFolderZaC1(string preuzetaPutanja, string nazivOriginalnogFajla)
        {
            if (preuzetaPutanja == null || nazivOriginalnogFajla == null)
            {
                throw new ArgumentNullException("\nPolja Putanja/Naziv fajla, ne smeju biti prazni\n.");
            }

            if (preuzetaPutanja == "" || nazivOriginalnogFajla == "")
            {
                throw new ArgumentException("\nPolja Putanja/Naziv fajla su nepostojeci\n.");
            }

            string putanjaDoVC = preuzetaPutanja;
            string referencaNaVC = preuzetaPutanja;
            string putanjaZaFolderSvakeRevizije = "";
            string putanjaDoRazlika = "";
            string putanjaDoOriginalnogFajla = "";
            string[] povratnaVrednost = new string[5];

            referencaNaVC = Path.Combine(preuzetaPutanja, ".vc");
            Directory.CreateDirectory(preuzetaPutanja);
            string pomocnaReferencaNaVC = referencaNaVC;

            putanjaZaFolderSvakeRevizije = Path.Combine(referencaNaVC, "C1");
            putanjaDoRazlika = Path.Combine(putanjaZaFolderSvakeRevizije, nazivOriginalnogFajla);

            putanjaDoOriginalnogFajla = Path.Combine(putanjaDoVC, nazivOriginalnogFajla); //"s" je putanja do repozitorijuma, putanja je do originalnog fajla

            return VratiPovratnePutanje(putanjaZaFolderSvakeRevizije, putanjaDoRazlika, pomocnaReferencaNaVC, putanjaDoOriginalnogFajla);
        }
        public string[] VratiPovratnePutanje(string putanjaZaFolderSvakeRevizije, string putanjaDoRazlika, string pomocnaReferencaNaVC, string putanjaDoOriginalnogFajla)
        {
            string[] povratnaVrednost = new string[5];

            if (putanjaZaFolderSvakeRevizije == null || putanjaDoRazlika == null || pomocnaReferencaNaVC == null || putanjaDoOriginalnogFajla == null)
            {
                throw new ArgumentNullException("\nParametri koji predstavljaju putanje do fajlova ne smeju biti nevalidni.\n");
            }
            if (putanjaZaFolderSvakeRevizije == "" || putanjaDoRazlika == "" || pomocnaReferencaNaVC == "" || putanjaDoOriginalnogFajla == "")
            {
                throw new ArgumentException("\nParametri koji predstavljaju putanje do fajlova ne smeju biti prazni.\n");
            }

            if (!Directory.Exists(putanjaZaFolderSvakeRevizije) || !File.Exists(putanjaDoRazlika))
            {
                Directory.CreateDirectory(putanjaZaFolderSvakeRevizije);
            }
            else
            {
                povratnaVrednost[3] = pomocnaReferencaNaVC;
                povratnaVrednost[0] = putanjaDoRazlika;
                povratnaVrednost[1] = putanjaZaFolderSvakeRevizije;
                povratnaVrednost[2] = putanjaDoOriginalnogFajla;
                povratnaVrednost[4] = "";
                return povratnaVrednost;
            }

            povratnaVrednost[0] = putanjaDoRazlika;
            povratnaVrednost[1] = putanjaZaFolderSvakeRevizije;
            povratnaVrednost[2] = putanjaDoOriginalnogFajla;
            povratnaVrednost[3] = pomocnaReferencaNaVC;
            povratnaVrednost[4] = "ima";

            return povratnaVrednost;
        }
        public void napraviMetaPodatke(string putanjaDoCommitaSaRazlikama, string nazivAutora, DateTime dt, string oznakaRevizije, int redniBrojRevizije)
        {
            
            if (putanjaDoCommitaSaRazlikama == null || nazivAutora == null || dt == null || oznakaRevizije == null)
            {
                throw new ArgumentNullException("\nPodaci koji predstavljaju meta podatke ne smeju biti nevalidni.\n");
            }
            if (putanjaDoCommitaSaRazlikama == "" || nazivAutora == "" || oznakaRevizije == "" || redniBrojRevizije <= 0)
            {
                throw new ArgumentException("\nPodaci koji predstavljaju meta podatke ne smeju biti prazni.\n");
            }

            using (StreamWriter sw = File.AppendText(putanjaDoCommitaSaRazlikama))
            {
                sw.WriteLine("Autor: " + nazivAutora);
                sw.WriteLine("Datum i vreme kreiranja revizije: " + dt);
                sw.WriteLine("Jedinstvena oznaka revizije: " + oznakaRevizije);
                sw.WriteLine("Redni broj revizije: " + redniBrojRevizije);
            }

            UpisiUBazu(nazivAutora, dt, oznakaRevizije, redniBrojRevizije);
            //CitajIzBaze();
        }
        public void NapraviPrvuReviziju(string izvornaPutanja, string odredisnaPutanja)
        {
            if (izvornaPutanja == null || odredisnaPutanja == null)
            {
                throw new ArgumentNullException("\nIzvorna/Odredisna putanja ne smeju biti nepostojeci.\n");
            }

            if (izvornaPutanja == "" || odredisnaPutanja == "")
            {
                throw new ArgumentException("\nIzvorna/Odredisna putanja ne smeju biti prazni.\n");
            }

            File.Copy(izvornaPutanja, odredisnaPutanja);
        }
        public void UpisiReviziju(string putanjaDoRazlika, string staroStanje, string novoStanje)
        {
            if (putanjaDoRazlika == null || staroStanje == null || novoStanje == null)
            {
                throw new ArgumentNullException("\nPutanja do dokumenta sa razlikama, stari/novi sadrzaj dokumenta, ne smeju biti nevalidni.\n");
            }

            if (putanjaDoRazlika == "" || staroStanje == "" || novoStanje == "")
            {
                throw new ArgumentException("\nPutanja do dokumenta sa razlikama, stari/novi sadrzaj dokumenta, ne smeju biti prazni.\n");
            }

            if (!ProveriPutanju(putanjaDoRazlika))
            {
                throw new ArgumentException("\nPutanja do foldera sa razlikama nije validna\n.");
            }

            using (StreamWriter tw = File.AppendText(putanjaDoRazlika))
            {
                tw.Write(staroStanje + " " + novoStanje);
                tw.WriteLine();
                tw.Close();
            }
        }
        public bool ProveriPutanju(string putanjaDoRazlika)
        {
            string[] splitovan = putanjaDoRazlika.Split('\\');

            foreach (string s in splitovan)
            {
                if (s.Equals(".vc"))
                    return true;
            }

            return false;
        }

        private static void UpisiUBazu(string nazivAutora, DateTime dt, string oznakaRevizije, int redniBrojRevizije)
        {
            RESProjekatEntities context = new RESProjekatEntities();
            MetaPodaci metaPodaci = new MetaPodaci()
            {
                AutorRevizije = nazivAutora,
                DatumKreiranjaRevizije = dt,
                RedniBrojRevizije = redniBrojRevizije,
                JedinstvenaOznakaRevizije = oznakaRevizije
            };
            context.MetaPodaci.Add(metaPodaci);
            context.SaveChanges();

        }

        private static void CitajIzBaze()
        {
            int brojac = 0;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RESProjekatEntities"].ConnectionString;
            EntityConnectionStringBuilder build = new EntityConnectionStringBuilder(connectionString);

            using (SqlConnection connection = new SqlConnection(build.ProviderConnectionString))
            using (SqlCommand command = new SqlCommand("select * from RESProjekat.dbo.MetaPodaci", connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        brojac++;
                        Console.WriteLine("\nMeta podataka iz baze broj " + brojac);
                        Console.WriteLine("\nAutor revizije: " + reader["AutorRevizije"].ToString());
                        Console.WriteLine("Datum i vreme kreiranja revizije: " + reader["DatumKreiranjaRevizije"].ToString());
                        Console.WriteLine("Redni broj revizije: " + reader["RedniBrojRevizije"].ToString());
                        Console.WriteLine("Jedinstvena oznaka revizije: " + reader["JedinstvenaOznakaRevizije"].ToString());
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
