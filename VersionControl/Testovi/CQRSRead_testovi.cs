using CQRSService;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Testovi
{
    [TestFixture]
    class CQRSRead_testovi
    {
        private CQRSRead cqrsRead;

        [SetUp]
        public void SetUp()
        {
            cqrsRead = new CQRSRead();
        }

        #region TestiranjeParametaraMetodeCitajCeoFajl
        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test\aaa.txt")]
        public void CitajCeoFajl_DobriParametri(string putanja)
        {
            cqrsRead.CitajCeoFajl(putanja);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("nepostojeca putanja")]
        public void CitajCeoFajl_LosiParametri(string putanja)
        {
            Assert.Throws<ArgumentException>(() => { cqrsRead.CitajCeoFajl(""); });
            Assert.Throws<ArgumentNullException>(() => { cqrsRead.CitajCeoFajl(null); });
            Assert.Throws<ArgumentException>(() => { cqrsRead.CitajCeoFajl("nepostojeca putanja"); });
        }
        #endregion
        #region TestiranjePovratneVrednostiMetodeCitajCeoFajl
        [Test]
        [TestCaseSource(typeof(CQRSRead_testovi), "TestCaseCitajCeoFajl")]
        public void CitajCeoFajlTestPovratneVrednosti(string putanja, string povratna)
        {
            var test = cqrsRead.CitajCeoFajl(putanja);
            Assert.AreEqual(povratna, test);
        }

        public static IEnumerable<ITestCaseData> TestCaseCitajCeoFajl
        {
            get
            {
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\CD1\abc.txt", "Da li radi bog te jebo?");
            }
        }
        #endregion

        #region TestiranjeParametaraMetodeCitajRazlike
        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test\BlaRazlikeTest.txt")]
        public void CitajRazlike_DobriParametri(string putanja)
        {
            cqrsRead.CitajRazlike(putanja);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("nepostojeca putanja")]
        public void CitajRazlike_LosiParametri(string putanja)
        {
            Assert.Throws<ArgumentException>(() => { cqrsRead.CitajRazlike(""); });
            Assert.Throws<ArgumentException>(() => { cqrsRead.CitajRazlike("nepostojeca putanja"); });
            Assert.Throws<ArgumentNullException>(() => { cqrsRead.CitajRazlike(null); });
        }
        #endregion
        #region TestiranjePovratneVrednostiMetodeCitajRazlike
        [Test]
        [TestCaseSource(typeof(CQRSRead_testovi), "TestCaseCitajRazlike")]
        public void CitajRazlikeTestPovratneVrednosti(string putanjaDoFajlaSaRazlikama, XElement povratna)
        {
            XElement test = cqrsRead.CitajRazlike(putanjaDoFajlaSaRazlikama);
            Assert.IsTrue(XNode.DeepEquals(povratna, test));
        }

        public static IEnumerable<TestCaseData> TestCaseCitajRazlike
        {
            get
            {
                XElement temp = new XElement("Razlike",
                                    new XElement("Red"),
                                    new XElement("Red"),
                                    new XElement("Red"),
                                    new XElement("Red"),
                                    new XElement("Red"),
                                    new XElement("Red"),
                                    new XElement("Red"));

                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\.vc\C1\BlaRazlikeTest.txt", temp);
                //yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test1", "abc.txt", new List<string> { "Nema" });
                //yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\", new List<string> {  });
            }
        }
        #endregion

        #region TestiranjeParametaraMetodePosaljiRevizije
        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", "bla.txt")]
        public void PosaljiRevizije_DobriParametri(string putanjaDoVC, string nazivFajla)
        {
            cqrsRead.PosaljiRevizije(putanjaDoVC, nazivFajla);
        }

        [Test]
        [TestCase("", "")]
        [TestCase(null, null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", null)]
        [TestCase("", "aaa.txt")]
        public void PosaljiRevizije_LosiParametri(string putanjaDoVC, string nazivFajla)
        {
            Assert.Throws<ArgumentException>(() => { cqrsRead.PosaljiRevizije("", ""); });
            Assert.Throws<ArgumentNullException>(() => { cqrsRead.PosaljiRevizije(null, null); });
            Assert.Throws<ArgumentNullException>(() => { cqrsRead.PosaljiRevizije(@"C:\Users\Maja\Downloads\Test\.vc", null); });
            Assert.Throws<ArgumentException>(() => { cqrsRead.PosaljiRevizije("", "aaa.txt"); });
        }
        #endregion
        #region TestiranjePovratneVrednostiMetodePosaljiRevizije
        [Test]
        [TestCaseSource(typeof(CQRSRead_testovi), "TestCasePosaljiRevizije")]
        public void PosaljiRevizijeTestPovratneVrednosti(string preuzetaPutanja, string nazivFajla, List<string> povratna)
        {
            List<string> test = cqrsRead.PosaljiRevizije(preuzetaPutanja, nazivFajla);
            CollectionAssert.AreEqual(povratna, test);
        }


        public static IEnumerable<TestCaseData> TestCasePosaljiRevizije
        {
            get
            {
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test", "abcc.txt", new List<string> { });
                //yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test1", "abc.txt", new List<string> { "Nema" });
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test", "abc.txt", new List<string> { "CD1" });
            }
        }
        #endregion

        #region TestiranjeParametaraMetodePosaljiSveRevizije
        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc")]
        public void PosaljiSveRevizije_DobriParametri(string putanjaDoVC)
        {
            cqrsRead.PosaljiSveRevizije(putanjaDoVC);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void PosaljiSveRevizije_LosiParametri(string putanjaDoVC)
        {
            Assert.Throws<ArgumentException>(() => { cqrsRead.PosaljiSveRevizije(""); });
            Assert.Throws<ArgumentNullException>(() => { cqrsRead.PosaljiSveRevizije(null); });
        }
        #endregion
        #region TestiranjePovratneVrednostiMetodePosaljiSveRevizije
        [Test]
        [TestCaseSource(typeof(CQRSRead_testovi), "TestCasePosaljiSveRevizije")]
        public void PosaljiSveRevizijeTestPovratneVrednosti(string preuzetaPutanja, List<string> povratna)
        {
            List<string> test = cqrsRead.PosaljiSveRevizije(preuzetaPutanja);
            CollectionAssert.AreEqual(povratna, test);
        }
        public static IEnumerable<TestCaseData> TestCasePosaljiSveRevizije
        {
            get
            {
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\.vc", new List<string> { "C1", "C2" });
                //yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test1", "abc.txt", new List<string> { "Nema" });
                //yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\", new List<string> {  });
            }
        }
        #endregion

        #region TestiranjeParametaraMetodeFajloviIzRevizije
        [Test]
        [TestCase("", "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", "")]
        [TestCase(null, null)]
        [TestCase(null, "C1")]
        public void FajloviIzRevizije_LosiParametri(string putanjaDoVC, string izabraniFajl)
        {
            Assert.Throws<ArgumentException>(() => { cqrsRead.VratiFajloveIzRevizije("", ""); });
            Assert.Throws<ArgumentException>(() => { cqrsRead.VratiFajloveIzRevizije(@"C:\Users\Maja\Downloads\Test\.vc", ""); });
            Assert.Throws<ArgumentNullException>(() => { cqrsRead.VratiFajloveIzRevizije(null, null); });
            Assert.Throws<ArgumentNullException>(() => { cqrsRead.VratiFajloveIzRevizije(null, "C1"); });
        }

        [Test]
        [TestCase(@"C:\Users\Maja\Downloads\Test\.vc", "C1")]
        public void FajloviIzRevizije_DobriParametri(string putanjaDoVC, string izabraniFajl)
        {
            cqrsRead.VratiFajloveIzRevizije(putanjaDoVC, izabraniFajl);
        }

        #endregion
        #region TestiranjePovratneVrednostiMetodeFajloviIzRevizije
        [Test]
        [TestCaseSource(typeof(CQRSRead_testovi), "TestCaseVratiFajloveIzRevizije")]
        public void FajloviIzRevizijeTestPovratneVrednosti(string preuzetaPutanja, string folder, string[] povratna)
        {
            string[] test = cqrsRead.VratiFajloveIzRevizije(preuzetaPutanja, folder);
            CollectionAssert.AreEqual(povratna, test);
        }
        public static IEnumerable<TestCaseData> TestCaseVratiFajloveIzRevizije
        {
            get
            {
                yield return new TestCaseData(@"C:\Users\Maja\Downloads\Test\.vc", "C1", new string[] { "bla.txt", "BlaRazlikeTest.txt" });
            }
        }
        #endregion

    }
}
