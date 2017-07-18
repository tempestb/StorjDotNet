﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorjDotNet;
using StorjDotNet.Models;
using System.Net.Http;

namespace StorjTests
{
    [TestClass]
    public class StorjTests
    {
        private static IStorj m_libStorj;

        [ClassInitialize]
        public static void TestClassinitialize(TestContext context)
        {
            string bridgeUser = context.Properties.Contains("bridgeUser") ?
                context.Properties["bridgeUser"].ToString() : null;
            string bridgePassword = context.Properties.Contains("bridgePass") ?
                context.Properties["bridgePass"].ToString() : null;

            m_libStorj = new Storj(new BridgeOptions()
            {
                BridgeUrl = "api.storj.io",
                Protocol = BridgeProtocol.HTTPS,
                Password = bridgePassword,
                Username = bridgeUser
            });
        }

        [TestMethod]
        public void ShouldGenerateMnemonic12Words()
        {
            string mnemonic = m_libStorj.GenerateMnemonic(128).Result;
            Assert.IsFalse(string.IsNullOrEmpty(mnemonic), "Mnemonic should not be null or empty");
            Assert.IsTrue(mnemonic.Split(' ').Length == 12, "Mnemonic should be 24 words");
            Console.WriteLine("Mnemonic is \"{0}\"", mnemonic);
        }

        [TestMethod]
        public void ShouldGenerateMnemonic24Words()
        {
            string mnemonic = m_libStorj.GenerateMnemonic(256).Result;
            Assert.IsFalse(string.IsNullOrEmpty(mnemonic), "Mnemonic should not be null or empty");
            Assert.IsTrue(mnemonic.Split(' ').Length == 24, "Mnemonic should be 24 words");
            Console.WriteLine("Mnemonic is \"{0}\"", mnemonic);
        }

        [TestMethod]
        public async Task ShouldGetBridge()
        {
            Bridge bridge = await m_libStorj.GetBridge();
            Assert.IsNotNull(bridge);
            Assert.IsNotNull(bridge.Info);
            Assert.AreEqual(bridge.Info.Title, "Storj Bridge");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException), "Email is already registered")]
        public async Task ShouldGetUserExistsError()
        {
            try
            {
                BridgeUser user = await m_libStorj.BridgeRegister("ssa3512@gmail.com", "password");
                Assert.Fail("Register should error");
            }
            catch(HttpRequestException ex)
            {
                Assert.AreEqual(ex.Message, "Email is already registered");
                throw;
            }
            
        }

        [TestMethod]
        public async Task ShouldGetBuckets()
        {
            var buckets = await m_libStorj.GetBuckets();
            Assert.IsNotNull(buckets);
        }

        [TestMethod]
        public async Task ShouldCreateBucket()
        {
            Bucket createdBucket = await m_libStorj.CreateBucket(null);
            Assert.IsNotNull(createdBucket);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException), "Name already used by another bucket")]
        public async Task ShouldFailBucketExists()
        {
            Bucket newBucket = new Bucket()
            {
                Name = "BucketExists"
            };
            try
            {
                Bucket createdBucket = await m_libStorj.CreateBucket(newBucket);
                Assert.Fail("CreateBucket should error");
            }
            catch(HttpRequestException ex)
            {
                Assert.AreEqual(ex.Message, "Name already used by another bucket");
                throw;
            }
            
            
        }
    }
}
