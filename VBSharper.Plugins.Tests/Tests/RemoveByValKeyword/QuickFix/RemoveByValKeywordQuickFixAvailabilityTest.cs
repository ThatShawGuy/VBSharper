using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.Test;
using JetBrains.ReSharper.Psi;
using NUnit.Framework;
using VBSharper.Plugins.QuickFixes.RemoveByValKeyword;

namespace VBSharper.Plugins.Tests.Tests.RemoveByValKeyword.QuickFix
{
    [TestFixture]
    public class RemoveByValKeywordQuickFixAvailabilityTest : QuickFixAvailabilityTestBase
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IPsiSourceFile psiSourceFile) {
            return highlighting is RemoveByValKeywordHighlighting;
        }
        
        protected override string RelativeTestDataPath {
            get { return @"..\..\TestData\RemoveByValKeyword\QuickFix"; }
        }

        [Test]
        public void AvailabilityTest() {
            DoTestFiles("Availability1.vb");
        }
    }
}
