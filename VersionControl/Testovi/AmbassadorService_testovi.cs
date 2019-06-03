using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmbassadorService;
using EventSourcingService;

namespace Testovi
{
    [TestFixture]
    class AmbassadorService_testovi
    {
        private Ambassador ambassadorService;

        [SetUp]
        public void SetUp()
        {
            ambassadorService = new Ambassador();
        }


        #region TestiranjeParametaraMetodeNapraviReviziju
        [Test]
        [TestCase("", "", null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "Maja", null)]
        [TestCase(null, null, null)]
        [TestCase(null, "Maja", null)]
        public void NapraviReviziju_LosiParametri(string putanjaDoRepozitorijuma, string nazivAutora, List<string> fajlovi)
        {
            List<string> fajl = new List<string>();
            Assert.Throws<ArgumentException>(() => { ambassadorService.NapraviReviziju("", "", fajl); });
            Assert.Throws<ArgumentException>(() => { ambassadorService.NapraviReviziju(@"C:\Users\Maja\Downloads\Test", "", fajl); });
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.NapraviReviziju(null, null, null); });
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.NapraviReviziju(null, "Maja", null); });
        }
        #endregion

        #region TestiranjeParametaraMetodeObradiReviziju
        [Test]
        [TestCase("", "", 0, "")]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "bla.txt", -3, @"C:\Users\Maja\Downloads\Test\bla.txt")]
        [TestCase(null, null, 2, null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test", "bla.txt", 1, null)]
        public void ObradiReviziju_LosiParametri(string putanjaDoVC, string nazivIzabranogFajla, int izbor, string putanjaDoOriginalnogFajla)
        {
            Assert.Throws<ArgumentException>(() => { ambassadorService.ObradiReviziju("", "", 0, ""); });
            Assert.Throws<ArgumentException>(() => { ambassadorService.ObradiReviziju(@"C:\Users\Maja\Downloads\Test", "bla.txt", -3, @"C:\Users\Maja\Downloads\Test\bla.txt"); });
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.ObradiReviziju(null, null, 2, null); });
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.ObradiReviziju(@"C:\Users\Maja\Downloads\Test", "bla.txt", 1, null); });
        }
        #endregion

        #region TestiranjeParametaraMetodePrikaziSveDokumente
        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void PrikaziSveDokumente_LosiParametri(string putanjaDoRepozitorijuma)
        {
            Assert.Throws<ArgumentException>(() => { ambassadorService.PrikaziSveDokumente(""); });
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.PrikaziSveDokumente(null); });
        }
        #endregion

        #region TestiranjeParametaraMetodePretraziReviziju
        [Test]
        [TestCase(null, null)]
        [TestCase(@"C:\Users\Maja\Downloads\Test", null)]
        [TestCase("", "")]
        [TestCase("", "C1")]
        public void PretraziReviziju_LosiParametri(string putanjaDoVC, string izbor)
        {
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.PretraziReviziju(null, null); });
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.PretraziReviziju(@"C:\Users\Maja\Downloads\Test", null); });
            Assert.Throws<ArgumentException>(() => { ambassadorService.PretraziReviziju("", ""); });
            Assert.Throws<ArgumentException>(() => { ambassadorService.PretraziReviziju("", "C1"); });
        }
        #endregion

        #region TestiranjeParametaraMetodeVratiSveRevizije
        [Test]
        [TestCase(5, null)]
        [TestCase(-3, @"C:\Users\Maja\Downloads\Test")]
        public void VratiSveRevizije_LosiParametri(int izbor, string putanjaDoRepozitorijuma)
        {
            Assert.Throws<ArgumentNullException>(() => { ambassadorService.VratiSveRevizije(izbor, null); });
            Assert.Throws<ArgumentException>(() => { ambassadorService.VratiSveRevizije(-3, ""); });
        }
        #endregion
    }
}
