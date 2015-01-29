using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using NUnit.Framework;
using VBSharper.Plugins.QuickFixes.RemoveByValKeyword;

namespace VBSharper.Plugins.Tests.Tests.RemoveByValKeyword.DaemonStage
{
    [TestFixture]
    public class RemoveByValKeywordHighlightingTest : VBHighlightingTestBase
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore) {
            return highlighting is RemoveByValKeywordHighlighting;
        }

        protected override string RelativeTestDataPath {
            get { return @"..\..\TestData\RemoveByValKeyword\Daemon"; }
        }

        [Test]
        [TestCase("Case1.vb")]
        public void Test(string testName) {
            DoTestFiles(testName);
        }
    }
}
