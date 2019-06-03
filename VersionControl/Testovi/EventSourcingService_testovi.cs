using CQRSService;
using EventSourcingService;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testovi
{
    [TestFixture]
    class EventSourcingService_testovi
    {
        private IEventSourcing eventSourcingService;
        private IEventSourcing eventSourcing;
        private ICQRSWrite cqrsWrite;
        private ICQRSRead cqrsRead;
        private List<string> lista;

        [SetUp]
        public void SetUp()
        {
            eventSourcingService = new EventSourcing();
            cqrsWrite = Substitute.For<ICQRSWrite>();
            cqrsRead = Substitute.For<ICQRSRead>();
            eventSourcing = new EventSourcing(cqrsRead, cqrsWrite);
            lista = new List<string>() { "Repozitorijumi.txt" };
        }

        #region TestiranjeParametaraMetodeFajloviRevizije
        [Test]
        [TestCase(null, null)]
        [TestCase(null, "C1")]
        [TestCase("", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "")]
        public void FajloviRevizije_LosiParametri(string putanjaDoRepozitorijuma, string izbor)
        {
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.FajloviRevizije(null, null); });
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.FajloviRevizije(null, "C1"); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.FajloviRevizije("", ""); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.FajloviRevizije(@"C:\Users\Maja\Downloads\Test", ""); });
        }
        #endregion

        #region TestiranjeParametaraMetodePrimeniRevizije
        [Test]
        [TestCase(null, null, 0, null)]
        [TestCase(null, "bla.txt", 0, @"C:\Users\Maja\Downloads\Test")]
        [TestCase("", "", -3, "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", "bla.txt", 2, "")]
        public void PrimeniRevizije_LosiParametri(string putanjaDoVC, string nazivIzabranogFajla, int izbor, string putanjaDoRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.PrimeniRevizije(null, null, 0, null); });
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.PrimeniRevizije(null, "bla.txt", 0, @"C:\Users\Maja\Downloads\Test"); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.PrimeniRevizije("", "", -3, ""); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.PrimeniRevizije(@"C:\Users\Maja\Downloads\Test\.vc", "bla.txt", 2, ""); });
        }
        #endregion

        #region TestiranjeParametaraMetodeUcitajSadrzajSaRazlikama
        [Test]
        [TestCase(0, null, null, null, null)]
        [TestCase(0, "bla.txt", @"C:\Users\Maja\Downloads\Test", @"C:\Users\Maja\Downloads\Test\.vc", null)]
        [TestCase(-1, "", "", "", null)]
        [TestCase(2, "bla.txt", "", "", null)]
        public void UcitajSadrzajSaRazlikama_LosiParametri(int izbor, string nazivIzabranogFajla, string putanjaDoRepozitorijuma, string putanjaDoVC, string[] nazivFajlaSplitovan)
        {
            string[] temp = new string[] { };
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.UcitajSadrzajSaRazlikama(0, null, null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.UcitajSadrzajSaRazlikama(0, "bla.txt", @"C:\Users\Maja\Downloads\Test", @"C:\Users\Maja\Downloads\Test\.vc", null); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.UcitajSadrzajSaRazlikama(-1, "", "", "", temp); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.UcitajSadrzajSaRazlikama(2, "bla.txt", "", "", temp); });
        }
        #endregion

        #region TestiranjeParametaraMetodeVratiRevizije
        [Test]
        [TestCase(2, null)]
        [TestCase(-2, @"C:\Users\Maja\Downloads\Test")]
        [TestCase(-2, "")]
        public void VratiRevizije_LosiParametri(int izbor, string putanjaDoRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.VratiRevizije(2, null); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.VratiRevizije(-2, @"C:\Users\Maja\Downloads\Test"); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.VratiRevizije(-2, ""); });
        }
        #endregion

        #region TestiranjeParametaraMetodeVratiSveDokumente
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void VratiSveDokumente_LosiParametri(string putanjaDoRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.VratiSveDokumente(null); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.VratiSveDokumente(""); });
        }

        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test")]
        public void VratiSveDokumente_DobriParametri(string putanjaDoRepozitorijuma)
        {
            eventSourcingService.VratiSveDokumente(putanjaDoRepozitorijuma);
        }
        #endregion
        #region TestiranjePovratneVrednostiMetodeVratiSveDokumente
        [Test]
        [TestCaseSource(typeof(EventSourcingService_testovi), "TestCaseVratiSveDokumente")]
        public void VratiSveDokumenteTestPovratneVrednosti(string putanjaDoRepozitorijuma, string[] povratna)
        {
            var test = eventSourcingService.VratiSveDokumente(putanjaDoRepozitorijuma);
            Assert.AreEqual(povratna, test);
        }

        public static IEnumerable<ITestCaseData> TestCaseVratiSveDokumente
        {
            get
            {
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test", new string[] { "\t1. aaa.txt", "\t2. abc.txt", "\t3. bla.txt", "\t4. BlaRazlikeTest.txt", "\t5. Repozitorijumi.txt" });
            }
        }
        #endregion

        #region TestiranjeParametaraMetodeUcitajRazlike
        [Test]
        [TestCase(null, null, null, null, null, null, null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "bla.txt", @"C:\Users\Maja\Downloads\Test\.vc", null, null, null, null)]
        [TestCase("", "", "", "", "", "", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "bla.txt", @"C:\Users\Maja\Downloads\Test\.vc", "", "", "", "")]
        public void UcitajRazlike_LosiParametri(string putanjaDoRepozitorijuma, string nazivOriginalnogFajla, string putanjaDoFajlaSaRazlikama, string pom, string pomocna, string nazivAutora, string k)
        {
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.UcitajRazlike(null, null, null, null, null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.UcitajRazlike(@"C:\Users\Maja\Downloads\Test", "bla.txt", @"C:\Users\Maja\Downloads\Test\.vc", null, null, null, null); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.UcitajRazlike("", "", "", "", "", "", ""); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.UcitajRazlike(@"C:\Users\Maja\Downloads\Test", "bla.txt", @"C:\Users\Maja\Downloads\Test\.vc", "", "", "", ""); });
        }
        #endregion

        #region TestiranjeParametaraMetodePostojiCommit
        [Test]
        [TestCase(null, null)]
        [TestCase("bla.txt", null)]
        [TestCase("", "")]
        [TestCase("", @"C:\Users\Maja\Downloads\Test\.vc\C1")]
        public void PostojiCommit_LosiParametri(string nazivOriginalnogFajla, string putanjaUnutarCommita)
        {
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.PostojiCommit(null, null); });
            Assert.Throws<ArgumentNullException>(() => { eventSourcingService.PostojiCommit("bla.txt", null); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.PostojiCommit("", ""); });
            Assert.Throws<ArgumentException>(() => { eventSourcingService.PostojiCommit("", @"C:\Users\Maja\Downloads\Test\.vc\C1"); });
        }

        [Test]
        [TestCase("bla.txt", @"C:\Users\Maja\Downloads\Test\.vc\C1")]
        public void PostojiCommit_DobriParametri(string nazivOriginalnogFajla, string putanjaUnutarCommita)
        {
            eventSourcingService.PostojiCommit(nazivOriginalnogFajla, putanjaUnutarCommita);
        }
        #endregion

        #region TestiranjePovratneVrednostiMetodePostojiCommit
        [Test]
        [TestCaseSource(typeof(EventSourcingService_testovi), "TestCasePostojiCommit")]
        [TestCaseSource(typeof(EventSourcingService_testovi), "TestCasePostojiCommit")]
        public void PostojiCommitTestPovratneVrednosti(string nazivOriginalnogFajla, string putanjaUnutarCommita, bool povratna)
        {
            var test = eventSourcingService.PostojiCommit(nazivOriginalnogFajla, putanjaUnutarCommita);
            Assert.AreEqual(povratna, test);
            Assert.AreEqual(povratna, test);
        }

        public static IEnumerable<ITestCaseData> TestCasePostojiCommit
        {
            get
            {
                yield return new TestCaseData("bla.txt", @"C:\Users\Maja\Downloads\Test\.vc\C1", true);
                yield return new TestCaseData("pera.txt", @"C:\Users\Maja\Downloads\Test\.vc\C1", false);
            }
        }
        #endregion

        [Test]
        public void NapraviReviziju_test()
        {
            string[] stringRet = cqrsWrite.KreirajFolderZaC1(@"C:\Users\Maja\Downloads\Test2\.vc", lista[0]);
            Assert.AreEqual(new string[0], stringRet);
            eventSourcing.NapraviMeta(cqrsWrite, "Maja", "RepozitorijumMaja", @"C:\Users\Maja\Downloads\Test2\.vc", @"C:\Users\Maja\Downloads\Test2\.vc\C1", "mile");
            cqrsWrite.Received().NapraviPrvuReviziju(@"C:\Users\Maja\Downloads\Test2\.vc", "mile");
        }

        [Test]
        public void ProdjiKrozFoldereSaRazlikama_test()
        {
            bool povratna = eventSourcing.DodavanjeFajlaSaRazlikamaUCommitKojiPostoji(true, "Maja", @"C:\Users\Maja\Downloads\Test", new string[] { "Repozitorijumi", "txt" }, new string[] { @"C:\Users\Maja\Downloads\Test\.vc" });

            Assert.AreEqual(true, povratna);
            Assert.AreNotEqual(false, povratna);
            Assert.AreNotEqual(null, povratna);
        }

        [Test]
        public void PrimeniRevizije_test()
        {
            eventSourcing.PrimeniRevizije(@"C:\Users\Maja\Downloads\Test2\.vc", "Repozitorijumi.txt", 1, @"C:\Users\Maja\Downloads\Test2");
            cqrsRead.Received().CitajCeoFajl(@"C:\Users\Maja\Downloads\Test2\.vc\C1\Repozitorijumi.txt");
        }

        [Test]
        public void FajloviRevizije_test()
        {
            eventSourcing.FajloviRevizije(@"C:\Users\Maja\Downloads\Test2", "C1");
            cqrsRead.Received().VratiFajloveIzRevizije(@"C:\Users\Maja\Downloads\Test2\.vc", "C1");

        }
        [Test]
        public void VratiRevizije_test()
        {
            eventSourcing.VratiRevizije(1, @"C:\Users\Maja\Downloads\Test2");
            cqrsRead.Received().PosaljiRevizije(@"C:\Users\Maja\Downloads\Test2\.vc", "Repozitorijumi.txt");
        }
    }
}

