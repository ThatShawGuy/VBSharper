using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.Psi;
using NUnit.Framework;
using VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation;

namespace VBSharper.Plugins.Tests.Tests.UseImplicitLineContinuation.QuickFix
{
    [TestFixture]
    public class UseImplicitLineContinuationQuickFixAvailabilityTest : QuickFixAvailabilityTestBase
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IPsiSourceFile psiSourceFile) {
            return highlighting is UseImplicitLineContinuationHighlighting;
        }
        
        protected override string RelativeTestDataPath {
            get { return @"..\..\TestData\UseImplicitLineContinuation\QuickFix"; }
        }

        [Test]
        public void AvailabilityTest() {
            DoTestFiles("Availability1.vb");
        }
    }
}
