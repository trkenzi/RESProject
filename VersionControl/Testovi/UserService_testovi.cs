using AmbassadorService;
using EventSourcingService;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService;

namespace Testovi
{
    class UserService_testovi
    {
        private IUser userService;
        private IAmbassador ambasador;
        
        [SetUp]
        public void SetUp()
        {
            userService = new User();
            ambasador = Substitute.For<IAmbassador>();
            

        }

        #region TestiranjeParametaraMetodeSprovediRevizijuNadSvim
        [Test]
        [TestCase(null, null, null)]
        [TestCase(null, "Maja", "RepozitorijumMaja")]
        [TestCase("", "", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "Maja", "")]
        public void SprovediRevizijuNadSvim_LosiParametri(string putanjaDoRepozitorijuma, string nazivAutora, string nazivRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.SprovediRevizijuNadSvim(null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.SprovediRevizijuNadSvim(null, "Maja", "RepozitorijumMaja"); });
            Assert.Throws<ArgumentException>(() => { userService.SprovediRevizijuNadSvim("", "", ""); });
            Assert.Throws<ArgumentException>(() => { userService.SprovediRevizijuNadSvim(@"C:\Users\Maja\Downloads\Test", "Maja", ""); });
        }
        #endregion

        #region TestiranjeParametaraMetodeSprovediRevizijuNadKonkretnim
        [Test]
        [TestCase(null, null, null)]
        [TestCase(null, "Maja", "RepozitorijumMaja")]
        [TestCase("", "", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "Maja", "")]
        public void SprovediRevizijuNadKonkretnim_LosiParametri(string putanjaDoRepozitorijuma, string nazivAutora, string nazivRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.SprovediRevizijuNadSvim(null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.SprovediRevizijuNadSvim(null, "Maja", "RepozitorijumMaja"); });
            Assert.Throws<ArgumentException>(() => { userService.SprovediRevizijuNadSvim("", "", ""); });
            Assert.Throws<ArgumentException>(() => { userService.SprovediRevizijuNadSvim(@"C:\Users\Maja\Downloads\Test", "Maja", ""); });
        }
        #endregion

        #region TestiranjeParametaraMetodeIzaberiFajlZaPrikazNjegovihRevizija
        [Test]
        [TestCase(null, null, null)]
        [TestCase(null, "Maja", "RepozitorijumMaja")]
        [TestCase("", "", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "Maja", "")]
        public void IzaberiFajlZaPrikazNjegovihRevizija_LosiParametri(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string nazivAutora)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.IzaberiFajlZaPrikazNjegovihRevizija(null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.IzaberiFajlZaPrikazNjegovihRevizija(null, "Maja", "RepozitorijumMaja"); });
            Assert.Throws<ArgumentException>(() => { userService.IzaberiFajlZaPrikazNjegovihRevizija("", "", ""); });
            Assert.Throws<ArgumentException>(() => { userService.IzaberiFajlZaPrikazNjegovihRevizija(@"C:\Users\Maja\Downloads\Test", "Maja", ""); });
        }

        /*[Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "RepozitorijumMaja", "Maja")]
        public void IzaberiFajlZaPrikazNjegovihRevizija_DobriParametri(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string nazivAutora)
        {
            
            userService.IzaberiFajlZaPrikazNjegovihRevizija(putanjaDoRepozitorijuma, nazivRepozitorijuma, nazivAutora);
        }*/
        #endregion
        #region TestiranjePovratneVrednostiMetodeIzaberiFajlZaPrikazNjegovihRevizija
        /*[Test]
        [TestCaseSource(typeof(UserService_testovi), "TestCaseIzaberiFajlZaPrikazNjegovihRevizija")]
        public void IzaberiFajlZaPrikazNjegovihRevizijaTestPovratneVrednosti(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string nazivAutora, string povratna)
        {
            string test = userService.IzaberiFajlZaPrikazNjegovihRevizija(putanjaDoRepozitorijuma, nazivRepozitorijuma, nazivAutora);
            Assert.AreEqual(povratna, test);
        }

        public static IEnumerable<ITestCaseData> TestCaseIzaberiFajlZaPrikazNjegovihRevizija
        {
            get
            {
                yield return new TestCaseData(@"C:\Users\Maja\Downloads", "RepozitorijumMaja", "Maja", "Test");
            }
        }*/
        #endregion

        #region TestiranjeParametaraMetodePronadjiRevizijeZaIzabraniFajl
        [Test]
        [TestCase("bla.txt", @"C:\Users\Maja\Downloads\Test\.vc")]
        public void PronadjiRevizijeZaIzabraniFajl_DobriParametri(string nazivIzabranogFajla, string putanjaDoVC)
        {
            userService.PronadjiRevizijeZaIzabraniFajl(nazivIzabranogFajla, putanjaDoVC);
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("bla.txt", null)]
        [TestCase("", "")]
        [TestCase("", @"C:\Users\Maja\Downloads\Test\.vc")]
        public void PronadjiRevizijeZaIzabraniFajl_LosiParametri(string nazivIzabranogFajla, string putanjaDoVC)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.PronadjiRevizijeZaIzabraniFajl(null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.PronadjiRevizijeZaIzabraniFajl("bla.txt", null); });
            Assert.Throws<ArgumentException>(() => { userService.PronadjiRevizijeZaIzabraniFajl("", ""); });
            Assert.Throws<ArgumentException>(() => { userService.PronadjiRevizijeZaIzabraniFajl("", @"C:\Users\Maja\Downloads\Test\.vc"); });
        }

        #endregion
        #region TestiranjePovratneVrednostiMetodePronadjiRevizijeZaIzabraniFajl
        [Test]
        [TestCaseSource(typeof(UserService_testovi), "TestCasePronadjiRevizijeZaIzabraniFajl")]
        public void PronadjiRevizijeZaIzabraniFajlTestPovratneVrednosti(string nazivIzabranogFajla, string putanjaDoVC, List<string> povratna)
        {
            var test = userService.PronadjiRevizijeZaIzabraniFajl(nazivIzabranogFajla, putanjaDoVC);
            Assert.AreEqual(povratna, test);
        }

        public static IEnumerable<ITestCaseData> TestCasePronadjiRevizijeZaIzabraniFajl
        {
            get
            {
                yield return new TestCaseData("bla.txt", @"C:\Users\Maja\Downloads\Test\.vc", new List<string> { "C1", "C2" });
            }
        }
        #endregion

        #region TestiranjeParametaraMetodePrikaziRevizijeZaSveDokumente
        [Test]
        [TestCase(null, null, null)]
        [TestCase(null, "RepozitorijumMaja", "Maja")]
        [TestCase("", "", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", "", "Maja")]
        public void PrikaziRevizijeZaSveDokumente_LosiParametri(string putanjaDoVC, string nazivRepozitorijuma, string nazivAutora)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.PrikaziRevizijeZaSveDokumente(null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.PrikaziRevizijeZaSveDokumente(null, "RepozitorijumMaja", "Maja"); });
            Assert.Throws<ArgumentException>(() => { userService.PrikaziRevizijeZaSveDokumente("", "", ""); });
            Assert.Throws<ArgumentException>(() => { userService.PrikaziRevizijeZaSveDokumente(@"C:\Users\Maja\Downloads\Test\.vc", "", "Maja"); });
        }
        #endregion

        #region TestiranjeParametaraMetodePrikaziRevizijeZaKonkretanDokument
        [Test]
        [TestCase(null, null, null)]
        [TestCase(null, "RepozitorijumMaja", "Maja")]
        [TestCase("", "", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", "", "Maja")]
        public void PrikaziRevizijeZaKonkretanDokument_LosiParametri(string putanjaDoRepozitorijuma, string nazivRepozitorijuma, string NazivAutora)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.PrikaziRevizijeZaKonkretanDokument(null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.PrikaziRevizijeZaKonkretanDokument(null, "RepozitorijumMaja", "Maja"); });
            Assert.Throws<ArgumentException>(() => { userService.PrikaziRevizijeZaKonkretanDokument("", "", ""); });
            Assert.Throws<ArgumentException>(() => { userService.PrikaziRevizijeZaKonkretanDokument(@"C:\Users\Maja\Downloads\Test\.vc", "", "Maja"); });
        }
        #endregion

        #region TestiranjeParametaraMetodeCitajIzLoga


        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CitajIzLoga_LosiParametri(string nazivRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.CitajIzLoga(null); });
            Assert.Throws<ArgumentException>(() => { userService.CitajIzLoga(""); });
        }
        #endregion

        #region TestiranjeParametaraMetodeKreirajRepozitorijum
        [Test]
        [TestCase(null, null)]
        [TestCase(null, "RepozitorijumMaja")]
        [TestCase("", "")]
        [TestCase("", "RepozitorijumMaja")]
        public void KreirajRepozitorijum_LosiParametri(string putanjaDoRepozitorijumiTxt, string nazivRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.KreirajRepozitorijum(null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.KreirajRepozitorijum(null, "RepozitorijumMaja"); });
            Assert.Throws<ArgumentException>(() => { userService.KreirajRepozitorijum("", ""); });
            Assert.Throws<ArgumentException>(() => { userService.KreirajRepozitorijum("", "RepozitorijumMaja"); });
        }
        #endregion

        #region TestiranjeParametaraMetodeReferenciranjeNaPutanju
        [Test]
        [TestCase(null, null)]
        [TestCase(null, "RepozitorijumMaja")]
        [TestCase("", "")]
        [TestCase("", "RepozitorijumMaja")]
        public void ReferenciranjeNaPutanju_LosiParametri(string putanjaDoRepozitorijumiTxt, string nazivRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { userService.ReferenciranjeNaPutanju(null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.ReferenciranjeNaPutanju(null, "RepozitorijumMaja"); });
            Assert.Throws<ArgumentException>(() => { userService.ReferenciranjeNaPutanju("", ""); });
            Assert.Throws<ArgumentException>(() => { userService.ReferenciranjeNaPutanju("", "RepozitorijumMaja"); });
        }
        #endregion

        #region TestiranjeParametaraMetodeUpisiNovuPutanju
        [Test]
        [TestCase(null, null)]
        [TestCase(@"C:\Users\Maja\Downloads", null)]
        [TestCase("", null)]
        public void UpisiNovuPutanju_LosiParametri(string putanjaDoRepozitorijumiTxt, string[] data2)
        {
            string[] temp = new string[] { };
            Assert.Throws<ArgumentNullException>(() => { userService.UpisiNovuPutanju(null, null); });
            Assert.Throws<ArgumentNullException>(() => { userService.UpisiNovuPutanju(@"C:\Users\Maja\Downloads", null); });
            Assert.Throws<ArgumentException>(() => { userService.UpisiNovuPutanju("", temp); });

        }
        #endregion

        #region TestiranjeParametaraMetodeUpisiURepozitorijumiTxt
        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        public void UpisiURepozitorijumiTxt_LosiParametri(string putanjaDoRepozitorijumiTxt, RepozitorijumiInfo repoInfo)
        {
            RepozitorijumiInfo repo = new RepozitorijumiInfo();
            Assert.Throws<ArgumentNullException>(() => { userService.UpisiURepozitorijumiTxt(null, null); });
            Assert.Throws<ArgumentException>(() => { userService.UpisiURepozitorijumiTxt("", repo); });

        }
        #endregion

        #region TestiranjeMetodeNapraviReviziju
        [Test]
        public void NapraviReviziju_test()
        {
            IUser UserService = new User(ambasador);
            
           
            Console.SetIn(new StringReader("RepozitorijumMaja"));
            string repo = UserService.UnesiRepozitorijum();
            Assert.AreEqual("RepozitorijumMaja", repo);
            Assert.AreNotEqual("", repo);
            Assert.AreNotEqual(null, repo);
            Console.SetIn(new StringReader("C:\\Users\\Maja\\Downloads\\Test" + "\\Repozitorijumi.txt"));

            string log = UserService.ProveraIKreiranjeLogFajla("C:\\Users\\Maja\\Downloads\\Test" + "\\Repozitorijumi.txt");
            Assert.AreEqual("C:\\Users\\Maja\\Downloads\\Test\\Repozitorijumi.txt", log);
            Assert.AreNotEqual("", log);
            Assert.AreNotEqual(null, log);

            RepozitorijumiInfo repoInfo = UserService.ReferenciranjeNaPutanju(log, repo);
            string[] data = new string[3] { "Maja", @"C:\Users\Maja\Downloads", "RepozitorijumMaja" };

            Console.SetIn(new StringReader("1"));

            RepozitorijumiInfo citanjePutanje = UserService.CitanjePutanjeDoRepozitorijuma(data, repo, log, new RepozitorijumiInfo()); // ako odabere 1
            Assert.AreEqual("Maja", citanjePutanje.NazivAutora);
            Assert.AreEqual(@"C:\Users\Maja\Downloads", citanjePutanje.PreuzetaPutanja);
            Assert.AreEqual("RepozitorijumMaja", citanjePutanje.NazivRepozitorijuma);
            Assert.AreNotEqual("", citanjePutanje.NazivAutora);
            Assert.AreNotEqual("", citanjePutanje.PreuzetaPutanja);
            Assert.AreNotEqual("", citanjePutanje.NazivRepozitorijuma);
            Assert.AreNotEqual(null, citanjePutanje.NazivAutora);
            Assert.AreNotEqual(null, citanjePutanje.PreuzetaPutanja);
            Assert.AreNotEqual(null, citanjePutanje.NazivRepozitorijuma);
            Assert.AreEqual(repoInfo.NazivAutora, citanjePutanje.NazivAutora);
            Assert.AreEqual(repoInfo.PreuzetaPutanja, citanjePutanje.PreuzetaPutanja);
            Assert.AreEqual(repoInfo.NazivRepozitorijuma, citanjePutanje.NazivRepozitorijuma);
            UserService.SprovediRevizijuNadSvim(citanjePutanje.PreuzetaPutanja, citanjePutanje.NazivAutora, citanjePutanje.NazivRepozitorijuma);

            ambasador.NapraviReviziju(citanjePutanje.PreuzetaPutanja, citanjePutanje.NazivAutora, new List<string>() { "BlaRazlikeTest.txt", "proba2.txt", "Test.txt", "Test2.txt" });
        } 

        [Test]
        public void NapraviReviziju_test2()
        {
            
            IUser UserService = new User(ambasador);

            Console.SetIn(new StringReader("RepozitorijumMaja"));
            string repo = UserService.UnesiRepozitorijum();
            Assert.AreEqual("RepozitorijumMaja", repo);
            Assert.AreNotEqual("", repo);
            Assert.AreNotEqual(null, repo);
            Console.SetIn(new StringReader("C:\\Users\\Maja\\Downloads\\Test" + "\\Repozitorijumi.txt"));

            string log = UserService.ProveraIKreiranjeLogFajla("C:\\Users\\Maja\\Downloads\\Test" + "\\Repozitorijumi.txt");
            Assert.AreEqual("C:\\Users\\Maja\\Downloads\\Test\\Repozitorijumi.txt", log);
            Assert.AreNotEqual("", log);
            Assert.AreNotEqual(null, log);

            RepozitorijumiInfo repoInfo = UserService.ReferenciranjeNaPutanju(log, repo);
            string[] data = new string[3] { "Maja", @"C:\Users\Maja\Downloads", "RepozitorijumMaja" };

            Console.SetIn(new StringReader("1"));

            RepozitorijumiInfo citanjePutanje = UserService.CitanjePutanjeDoRepozitorijuma(data, repo, log, new RepozitorijumiInfo()); 
            Assert.AreEqual("Maja", citanjePutanje.NazivAutora);
            Assert.AreEqual(@"C:\Users\Maja\Downloads", citanjePutanje.PreuzetaPutanja);
            Assert.AreEqual("RepozitorijumMaja", citanjePutanje.NazivRepozitorijuma);
            Assert.AreNotEqual("", citanjePutanje.NazivAutora);
            Assert.AreNotEqual("", citanjePutanje.PreuzetaPutanja);
            Assert.AreNotEqual("", citanjePutanje.NazivRepozitorijuma);
            Assert.AreNotEqual(null, citanjePutanje.NazivAutora);
            Assert.AreNotEqual(null, citanjePutanje.PreuzetaPutanja);
            Assert.AreNotEqual(null, citanjePutanje.NazivRepozitorijuma);
            Assert.AreEqual(repoInfo.NazivAutora, citanjePutanje.NazivAutora);
            Assert.AreEqual(repoInfo.PreuzetaPutanja, citanjePutanje.PreuzetaPutanja);
            Assert.AreEqual(repoInfo.NazivRepozitorijuma, citanjePutanje.NazivRepozitorijuma);

            Console.SetIn(new StringReader("1"));
            UserService.SprovediRevizijuNadKonkretnim(citanjePutanje.PreuzetaPutanja, citanjePutanje.NazivAutora, citanjePutanje.NazivRepozitorijuma);

            ambasador.NapraviReviziju(citanjePutanje.PreuzetaPutanja, citanjePutanje.NazivAutora, new List<string>() { "BlaRazlikeTest.txt" });
        }
        #endregion

        #region TestiranjeMetodeOdaberiReviziju
        [Test]
        public void OdaberiReviziju_test()
        {
            
            IUser UserService = new User(ambasador);
            Console.SetIn(new StringReader("RepozitorijumMaja"));
            string repo = UserService.UnesiRepozitorijum();
            Assert.AreEqual("RepozitorijumMaja", repo);
            Assert.AreNotEqual("", repo);
            Assert.AreNotEqual(null, repo);
            UserService.SetPutanjaDoRepozitorijumiText("C:\\Users\\Maja\\Downloads\\Test\\Repozitorijumi.txt");

            RepozitorijumiInfo log = UserService.CitajIzLoga(repo);
            Assert.AreEqual("Maja", log.NazivAutora);
            Assert.AreNotEqual("", log.NazivAutora);
            Assert.AreNotEqual(null, log.NazivAutora);
            Assert.AreEqual(@"C:\Users\Maja\Downloads", log.PreuzetaPutanja);
            Assert.AreNotEqual("", log.PreuzetaPutanja);
            Assert.AreNotEqual(null, log.PreuzetaPutanja);

            Console.SetIn(new StringReader("1"));
            string imeFajla = UserService.IzaberiFajlZaPrikazNjegovihRevizija("C:\\Users\\Maja\\Downloads\\Test", repo, log.NazivAutora);
            Assert.AreEqual("aaa.txt", imeFajla);
            Assert.AreNotEqual("", imeFajla);
            Assert.AreNotEqual(null, imeFajla);

            List<string> commiti = UserService.PronadjiRevizijeZaIzabraniFajl(imeFajla, "C:\\Users\\Maja\\Downloads\\Test");

            Assert.AreEqual(new List<string>() { "C1", "CD1" }, commiti);
            Assert.AreNotEqual(new List<string>() { "" }, commiti);
            Assert.AreNotEqual(null, commiti);


            Console.SetIn(new StringReader("1"));
            ambasador.ObradiReviziju(log.PreuzetaPutanja, imeFajla, 1, log.PreuzetaPutanja);
        }
        #endregion

        #region TestiranjeMetodeProveraIKreiranjeLogFajla
        [Test]
        public void ProveraIKreiranjeLogFajla_test()
        {

            IUser UserService = new User();
            string povratnaPutanja = UserService.ProveraIKreiranjeLogFajla(@"C:\Users\Maja\Downloads\Test\Repozitorijumi.txt");
            Assert.AreEqual(@"C:\Users\Maja\Downloads\Test\Repozitorijumi.txt", povratnaPutanja);
            Assert.AreNotEqual(null, povratnaPutanja);
            Assert.AreNotEqual("", povratnaPutanja);
        }
        #endregion

        #region TestiranjeMetodePromenaRepozitorijumaAutora
        [Test]
        public void PromenaRepozitorijumaAutora_test()
        {
            IUser userService = new User();
            Console.SetIn(new StringReader(@"C:\Users\Maja\Downloads\Test")); //nova putanja koja je uneta, treba je upisati u repozitorijumi.txt
            RepozitorijumiInfo repoInfo = userService.PromenaRepozitorijumaAutora(new string[] { @"Maja C:\Users\Maja\Downloads RepozitorijumMaja" }, "RepozitorijumMaja", @"C:\Users\Maja\Downloads\Test\Repozitorijumi.txt", new RepozitorijumiInfo());

            Assert.AreEqual(@"C:\Users\Maja\Downloads\Test", repoInfo.PreuzetaPutanja);
            Assert.AreNotEqual("", repoInfo.PreuzetaPutanja);
            Assert.AreNotEqual(null, repoInfo.PreuzetaPutanja);
        }
        #endregion

        #region TestiranjeMetodePrikaziFajlove
        [Test]
        public void PrikaziFajlove_test()
        {
            
            IUser UserService = new User(ambasador);
            
            Console.SetIn(new StringReader("RepozitorijumMaja"));
            string repo = UserService.UnesiRepozitorijum();
            Assert.AreEqual("RepozitorijumMaja", repo);
            Assert.AreNotEqual("", repo);
            Assert.AreNotEqual(null, repo);
            Console.SetIn(new StringReader(@"C:\Users\Maja\Downloads\Test" + "\\Repozitorijumi.txt"));

            string log = UserService.ProveraIKreiranjeLogFajla(@"C:\Users\Maja\Downloads\Test" + "\\Repozitorijumi.txt");
            Assert.AreEqual(@"C:\Users\Maja\Downloads\Test\Repozitorijumi.txt", log);
            Assert.AreNotEqual("", log);
            Assert.AreNotEqual(null, log);

            RepozitorijumiInfo repoInfo = UserService.ReferenciranjeNaPutanju(log, repo);
            string[] data = new string[3] { "Maja", @"C:\Users\Maja\Downloads", "RepozitorijumMaja" };

            Console.SetIn(new StringReader("1"));

            RepozitorijumiInfo citanjePutanje = UserService.CitanjePutanjeDoRepozitorijuma(data, repo, log, new RepozitorijumiInfo()); // ako odabere 1
            Assert.AreEqual("Maja", citanjePutanje.NazivAutora);
            Assert.AreEqual(@"C:\Users\Maja\Downloads", citanjePutanje.PreuzetaPutanja);
            Assert.AreEqual("RepozitorijumMaja", citanjePutanje.NazivRepozitorijuma);
            Assert.AreNotEqual("", citanjePutanje.NazivAutora);
            Assert.AreNotEqual("", citanjePutanje.PreuzetaPutanja);
            Assert.AreNotEqual("", citanjePutanje.NazivRepozitorijuma);
            Assert.AreNotEqual(null, citanjePutanje.NazivAutora);
            Assert.AreNotEqual(null, citanjePutanje.PreuzetaPutanja);
            Assert.AreNotEqual(null, citanjePutanje.NazivRepozitorijuma);
            Assert.AreEqual(repoInfo.NazivAutora, citanjePutanje.NazivAutora);
            Assert.AreEqual(repoInfo.PreuzetaPutanja, citanjePutanje.PreuzetaPutanja);
            Assert.AreEqual(repoInfo.NazivRepozitorijuma, citanjePutanje.NazivRepozitorijuma);


            string[] povratna = ambasador.PrikaziSveDokumente(repoInfo.PreuzetaPutanja);

            Assert.AreEqual(new string[] { }, povratna);
            Assert.AreNotEqual(null, povratna);

        }
        #endregion

        #region TestiranjeMetodePretragaRevizija
        [Test]
        public void PretragaRevizija_test()
        {
            
            IUser userService = new User(ambasador);
            Console.SetIn(new StringReader("RepozitorijumMaja"));
            string repo = userService.UnesiRepozitorijum();
            Assert.AreEqual("RepozitorijumMaja", repo);
            Assert.AreNotEqual("", repo);
            Assert.AreNotEqual(null, repo);
            userService.SetPutanjaDoRepozitorijumiText(@"C:\Users\Maja\Downloads\Test\Repozitorijumi.txt");

            RepozitorijumiInfo log = userService.CitajIzLoga(repo);

            Assert.AreEqual("Maja", log.NazivAutora);
            Assert.AreNotEqual("", log.NazivAutora);
            Assert.AreNotEqual(null, log.NazivAutora);
            Assert.AreEqual(@"C:\Users\Maja\Downloads", log.PreuzetaPutanja);
            Assert.AreNotEqual("", log.PreuzetaPutanja);
            Assert.AreNotEqual(null, log.PreuzetaPutanja);

            ambasador.PretraziReviziju(log.PreuzetaPutanja, "1");
            ambasador.Received().PretraziReviziju(@"C:\Users\Maja\Downloads", "1");
        }
        #endregion
    }
}

