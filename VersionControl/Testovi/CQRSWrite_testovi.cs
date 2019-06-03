using CQRSService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testovi
{
    [TestFixture]
    class CQRSWrite_testovi
    {
        private CQRSWrite cqrsWrite;

        [SetUp]
        public void SetUp()
        {
            cqrsWrite = new CQRSWrite();
        }

        #region TestiranjeParametaraMetodeKreirajFolderZaC1
        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "aaa.txt")]
        public void KreirajFolderZaC1_DobriParametri(string preuzetaPutanja, string nazivOriginalnogFajla)
        {
            cqrsWrite.KreirajFolderZaC1(preuzetaPutanja, nazivOriginalnogFajla);
        }

        [Test]
        [TestCase(null, "aaa.txt")]
        [TestCase(null, null)]
        [TestCase(@"C\Users\Maja\Downloads\Test", null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "")]
        [TestCase("", "")]
        [TestCase("", "aaa.txt")]
        public void KreirajFolderZaC1_LosiParametri(string preuzetaPutanja, string nazivOriginalnogFajla)
        {
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.KreirajFolderZaC1(null, "aaa.txt"); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.KreirajFolderZaC1(null, null); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.KreirajFolderZaC1(@"C:\Users\Maja\Downloads\Test", null); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.KreirajFolderZaC1(@"C:\Users\Maja\Downloads\Test", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.KreirajFolderZaC1("", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.KreirajFolderZaC1("", "aaa.txt"); });
        }
        #endregion
        #region TestiranjePovratneVrednostiMetodeKreirajFolderZaC1
        [Test]
        [TestCaseSource(typeof(CQRSWrite_testovi), "TestCaseKreirajFolderZaC1")]
        public void KreirajFolderZaC1TestPovratneVrednosti(string preuzetaPutanja, string nazivOriginalnogFajla, string[] povratna)
        {
            var test = cqrsWrite.KreirajFolderZaC1(preuzetaPutanja, nazivOriginalnogFajla);
            CollectionAssert.AreEqual(povratna, test);
            CollectionAssert.AreEqual(povratna, test);
        }

        public static IEnumerable<TestCaseData> TestCaseKreirajFolderZaC1
        {
            get
            {
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test", "bla.txt", new string[] { @"C:\Users\Maja\Downloads\Test\.vc\C1\bla.txt", @"C:\Users\Maja\Downloads\Test\.vc\C1", @"C:\Users\Maja\Downloads\Test\bla.txt", @"C:\Users\Maja\Downloads\Test\.vc", "" });
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\CD1", "bla.txt", new string[] { @"C:\Users\Maja\Downloads\Test\CD1\.vc\C1\bla.txt", @"C:\Users\Maja\Downloads\Test\CD1\.vc\C1", @"C:\Users\Maja\Downloads\Test\CD1\bla.txt", @"C:\Users\Maja\Downloads\Test\CD1\.vc", "ima" });
            }
        }
        #endregion

        #region TestiranjeParametaraMetodeUpisiReviziju
        [TestCase(null, null, null)]
        [TestCase(null, null, "cao")]
        [TestCase(@"C\Users\Maja\Downloads\Test", null, null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "", "")]
        [TestCase("", "", "")]
        [TestCase("", "", "cao")]
        public void UpisiReviziju_LosiParametri(string putanjaDoRazlika, string staro, string novo)
        {
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.UpisiReviziju(null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.UpisiReviziju(null, null, "cao"); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.UpisiReviziju(@"C\Users\Maja\Downloads\Test", null, null); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.UpisiReviziju(@"C:\Users\Maja\Downloads\Test", "", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.UpisiReviziju("", "", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.UpisiReviziju("", "", "cao"); });
        }


        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc\C1\bla.txt", "pozdrav", "cao")]
        public void UpisiReviziju_DobriParametri(string putanjaDoRazlika, string staro, string novo)
        {
            cqrsWrite.UpisiReviziju(putanjaDoRazlika, staro, novo);
        }
        #endregion

        #region TestiranjeParametaraMetodeNapraviPrvuReviziju
        [Test]
        [TestCase(null, @"C\Users\Maja\Downloads\Test")]
        [TestCase(null, null)]
        [TestCase(@"C\Users\Maja\Downloads\Test", null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "")]
        [TestCase("", "")]
        [TestCase("", @"C:\Users\Maja\Downloads\Test")]
        public void NapraviPrvuReviziju_LosiParametri(string izvor, string odrediste)
        {
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.NapraviPrvuReviziju(null, @"C\Users\Maja\Downloads\Test"); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.NapraviPrvuReviziju(null, null); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.NapraviPrvuReviziju(@"C:\Users\Maja\Downloads\Test", null); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.NapraviPrvuReviziju(@"C:\Users\Maja\Downloads\Test", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.NapraviPrvuReviziju("", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.NapraviPrvuReviziju("", @"C:\Users\Maja\Downloads\Test"); });
        }

        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test\aaa.txt", @"C:\Users\Maja\Downloads\Test\CD1\aaa2.txt")]
        public void NapraviPrvuReviziju_DobriParametri(string izvor, string odrediste)
        {
            cqrsWrite.NapraviPrvuReviziju(izvor, odrediste);
        }
        #endregion

        #region TestiranjeParametaraMetodeNapraviMetaPodatke
        [Test]
        [TestCase(null, null, null, 5)]
        [TestCase(@"C:\Users\Maja\Downloads\Test\aaa.txt", "Maja", null, 1)]
        [TestCase("", "", "", 0)]
        [TestCase(@"C:\Users\Maja\Downloads\Test\aaa.txt", "Maja", "C1", -3)]
        public void napraviMetaPodatke_LosiParametri(string putanjaDoCommitaSaRazlikama, string nazivAutora, string oznakaRevizije, int redniBrojRevizije)
        {
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.napraviMetaPodatke(null, null, DateTime.Now, null, 5); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.napraviMetaPodatke(@"C:\Users\Maja\Downloads\Test\aaa.txt", "Maja", DateTime.Now, null, 1); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.napraviMetaPodatke("", "", DateTime.Now, "", 0); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.napraviMetaPodatke(@"C:\Users\Maja\Downloads\Test\aaa.txt", "Maja", DateTime.Now, "C1", -3); });
        }

        [Test]

        [TestCase(@"C:\Users\Maja\Downloads\Test\aaa.txt", "Maja", "C1", 1)]
        public void napraviMetaPodatke_DobriParametri(string putanjaDoCommitaSaRazlikama, string nazivAutora, string oznakaRevizije, int redniBrojRevizije)
        {
            cqrsWrite.napraviMetaPodatke(@"C:\Users\Maja\Downloads\Test\aaa.txt", "Maja", DateTime.Now, "C1", 1);
        }
        #endregion


        #region TestiranjeParametaraMetodeVratiPovratnePutanje
        [Test]
        [TestCase(null, null, null, null)]
        [TestCase(null, @"C:\Users\Maja\Downloads\Test", @"C:\Users\Maja\Downloads\Test\.vc", @"C:\Users\Maja\Downloads")]
        [TestCase("", "", "", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test", @"C:\Users\Maja\Downloads\Test\.vc", @"C:\Users\Maja\Downloads", "")]
        public void VratiPovratnePutanje_LosiParametri(string putanjaZaFolderSvakeRevizije, string putanjaDoRazlika, string pomocnaReferencaNaVC, string putanjaDoOriginalnogFajla)
        {
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.VratiPovratnePutanje(null, null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { cqrsWrite.VratiPovratnePutanje(null, @"C:\Users\Maja\Downloads\Test", @"C:\Users\Maja\Downloads\Test\.vc", @"C:\Users\Maja\Downloads"); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.VratiPovratnePutanje("", "", "", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsWrite.VratiPovratnePutanje(@"C:\Users\Maja\Downloads\Test", @"C:\Users\Maja\Downloads\Test\.vc", @"C:\Users\Maja\Downloads", ""); });
        }

        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", @"C:\Users\Maja\Downloads\Test\.vc\C1\bla.txt", @"C:\Users\Maja\Downloads\Test\.vc", @"C:\Users\Maja\Downloads\bla.txt")]
        public void VratiPovratnePutanje_DobriParametri(string putanjaZaFolderSvakeRevizije, string putanjaDoRazlika, string pomocnaReferencaNaVC, string putanjaDoOriginalnogFajla)
        {
            cqrsWrite.VratiPovratnePutanje(putanjaZaFolderSvakeRevizije, putanjaDoRazlika, pomocnaReferencaNaVC, putanjaDoOriginalnogFajla);
        }
        #endregion
        #region TestiranjePovratneVrednostMetodeVratiPovratnePutanje


        [Test]
        [TestCaseSource(typeof(CQRSWrite_testovi), "TestCaseVratiPovratnePutanjePovratnaVrednost")]
        public void VratiPovratnePutanjeTestPovratneVrednosti(string putanjaZaFolderSvakeRevizije, string putanjaDoRazlika, string pomocnaReferencaNaVC, string putanjaDoOriginalnogFajla, string[] povratna)
        {
            string[] test = cqrsWrite.VratiPovratnePutanje(putanjaZaFolderSvakeRevizije, putanjaDoRazlika, pomocnaReferencaNaVC, putanjaDoOriginalnogFajla);
            CollectionAssert.AreEqual(povratna, test);
        }
        public static IEnumerable<TestCaseData> TestCaseVratiPovratnePutanjePovratnaVrednost
        {
            get
            {
                string putanjaDoRazlika = @"C:\Users\Maja\Downloads\Test\.vc\C1\bla.txt";
                string putanjaZaFolderSvakeRevizije = @"C:\Users\Maja\Downloads\Test\.vc";
                string putanjaDoOriginalnogFajla = @"C:\Users\Maja\Downloads\bla.txt";
                string pomocnaReferencaNaVC = @"C:\Users\Maja\Downloads\Test\.vc";

                string[] povratnaVrednost = new string[5];
                povratnaVrednost[0] = putanjaDoRazlika;
                povratnaVrednost[1] = putanjaZaFolderSvakeRevizije;
                povratnaVrednost[2] = putanjaDoOriginalnogFajla;
                povratnaVrednost[3] = pomocnaReferencaNaVC;
                povratnaVrednost[4] = "";

                yield return new TestCaseData(putanjaZaFolderSvakeRevizije, putanjaDoRazlika, pomocnaReferencaNaVC, putanjaDoOriginalnogFajla, povratnaVrednost);
                //yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test1", "abc.txt", new List<string> { "Nema" });
                //yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\", new List<string> {  });
            }
        }
        #endregion

    }
}
