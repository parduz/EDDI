﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using EDDIVAPlugin;

namespace Tests
{
    [TestClass]
    public class VoiceAttackPluginTests
    {
        [TestMethod]
        public void TestVAPluginHumanize1()
        {
            Assert.AreEqual("on the way to 12 and a half thousand", VoiceAttackPlugin.humanize(12345));
        }

        [TestMethod]
        public void TestVAPluginHumanize2()
        {
            Assert.AreEqual(null, VoiceAttackPlugin.humanize(null));
        }

        [TestMethod]
        public void TestVAPluginHumanize3()
        {
            Assert.AreEqual("0", VoiceAttackPlugin.humanize(0));
        }
    }
}
