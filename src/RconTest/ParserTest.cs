﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using CoreRCON.Parsers.Standard;
using CoreRCON.Parsers.Csgo;

namespace CoreRCON.Tests
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void testRoundEndParser()
        {
            string team = "TERRORIST";
            int tRounds = 0;
            int ctRounds = 16;
            string testString = $"12:00 junk: Team \"{team}\" triggered \"SFUI_Notice_Terrorists_Win\" (CT \"{ctRounds}\") (T \"{tRounds}\")";
            RoundEndScoreParser parser = new RoundEndScoreParser();
            Assert.IsTrue(parser.IsMatch(testString));
            RoundEndScore score = parser.Parse(testString);
            Assert.AreEqual(team, score.WinningTeam);
            Assert.AreEqual(ctRounds, score.CTScore);
            Assert.AreEqual(tRounds, score.TScore);
        }


        [TestMethod]
        public void testDisconnectParser()
        {
            string reason = "test123   (oj)";
            string withReason = $"12:00 junk: \"Xavier<2><BOT><TERRORIST>\" disconnected (reason \"{reason}\")";
            string noREason = $"12:00 junk: \"Xavier<2><BOT><TERRORIST>\" disconnected";
            PlayerDisconnectedParser parser = new PlayerDisconnectedParser();
            Assert.IsTrue(parser.IsMatch(withReason));
            Assert.IsTrue(parser.IsMatch(noREason));
            PlayerDisconnected disconnection = parser.Parse(withReason);
            Assert.AreEqual(reason, disconnection.Reason);
        }

        [TestMethod]
        public void testFrag()
        {
            string weapon = "usp_silencer";
            string headShot = $"L 13:37 spam: \"Prince<12><STEAM_1:1:123338101><CT>\" [2264 19 128] killed \"Bot<11><STEAM_1:0:123371565><TERRORIST>\" [1938 -198 320] with \"{weapon}\" (headshot)";
            string kill = $"L 13:37 spam: \"Prince<12><STEAM_1:1:123338101><CT>\" [2264 19 128] killed \"Bot<11><STEAM_1:0:123371565><TERRORIST>\" [1938 -198 320] with \"{weapon}\"";
            FragParser parser = new FragParser();
            Assert.IsTrue(parser.IsMatch(headShot));
            Assert.IsTrue(parser.IsMatch(kill));
            Frag hsFarg = parser.Parse(headShot);
            Frag frag = parser.Parse(kill);
            Assert.IsTrue(hsFarg.Headshot);
            Assert.IsFalse(frag.Headshot);
            Assert.AreEqual(frag.Weapon, weapon);
        }
    }
}

