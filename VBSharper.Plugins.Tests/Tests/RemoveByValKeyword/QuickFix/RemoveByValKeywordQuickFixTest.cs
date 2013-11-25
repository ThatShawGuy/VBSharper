using JetBrains.ReSharper.IntentionsTests;
using NUnit.Framework;
using VBSharper.Plugins.QuickFixes.RemoveByValKeyword;

namespace VBSharper.Plugins.Tests.Tests.RemoveByValKeyword.QuickFix
{
    [TestFixture]
    public class RemoveByValKeywordQuickFixTest : QuickFixTestBase<RemoveByValKeywordQuickFix>
    {
        protected override string RelativeTestDataPath {
            get { return @"..\..\TestData\RemoveByValKeyword\QuickFix"; }
        }

        [Test]
        public void ExecuteTest() {
            DoTestFiles("Test1.vb");
        }
    }
}
