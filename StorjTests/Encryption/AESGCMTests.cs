using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorjDotNet;
using StorjDotNet.Encryption;

namespace StorjTests.Encryption
{
    [TestClass]
    public class AESGCMTests
    {
        private readonly byte[] _key = new byte[32] {215,99,0,133,172,219,64,35,54,53,171,23,146,160,
            81,126,137,21,253,171,48,217,184,188,8,137,3,4,83,50,30,251};
        private readonly byte[] _iv = new byte[32] {70,219,247,135,162,7,93,193,44,123,188,234,203,115,129,
            82,70,219,247,135,162,7,93,193,44,123,188,234,203,115,129,82};
        private readonly string _secretMessage = "Secret";

        private readonly string _encryptedMessage =
            "mjWjwoR2YRcSeHf+OqV/uUbb94eiB13BLHu86stzgVJG2/eHogddwSx7vOrLc4FSjQTgqktamjWjwoR2YRcSeHf+OqV/uQ==";

        [TestMethod]
        public void EncryptMessage()
        {
            string encrypted = AESGCM.SimpleEncrypt(_secretMessage, _key, _iv); 
            Console.WriteLine("Encrypted message is {0}", encrypted);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted), "Encrypted message should not be null or empty.");
            Assert.AreNotEqual(_secretMessage, encrypted, "Encrypted message should not equal cleartext secret");
        }
        [TestMethod]
        public void DecryptsMessage()
        {
            string clearText = AESGCM.SimpleDecrypt(_encryptedMessage, _key);
            Console.WriteLine("Decrypted message is {0}", clearText);
            Assert.AreEqual(_secretMessage, clearText);
        }
    }
}
