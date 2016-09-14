﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using NUnit.Framework;
using SiliconStudio.Xenko.Graphics;

namespace SiliconStudio.Xenko.Particles.Tests
{
    class VisualTestSoftEdge : GameTest
    {
        public VisualTestSoftEdge() : base("VisualTestSoftEdge") { }

        [Test]
        public void RunVisualTests10()
        {
            RunGameTest(new GameTest("VisualTestSoftEdge", GraphicsProfile.Level_10_0));
        }

        [Test]
        public void RunVisualTests11()
        {
            RunGameTest(new GameTest("VisualTestSoftEdge", GraphicsProfile.Level_11_0));
        }

    }
}
