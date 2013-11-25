using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.DaemonTests.VB;
using NUnit.Framework;
using VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation;

namespace VBSharper.Plugins.Tests.Tests.UseImplicitLineContinuation.DaemonStage
{
    [TestFixture]
    public class UseImplicitLineContinuationHighlightingTest : VBHighlightingTestBase
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore) {
            return highlighting is UseImplicitLineContinuationHighlighting;
        }

        protected override string RelativeTestDataPath {
            get { return @"..\..\TestData\UseImplicitLineContinuation\Daemon"; }
        }

        [Test]
        [TestCase("Case1.vb")]
        public void Test(string testName) {
            DoTestFiles(testName);
        }
    }
}
