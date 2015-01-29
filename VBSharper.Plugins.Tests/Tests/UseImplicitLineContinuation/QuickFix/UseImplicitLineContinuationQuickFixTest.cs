using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;
using VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation;

namespace VBSharper.Plugins.Tests.Tests.UseImplicitLineContinuation.QuickFix
{
    [TestFixture]
    public class UseImplicitLineContinuationQuickFixTest : QuickFixTestBase<UseImplicitLineContinuationQuickFix>
    {
        protected override string RelativeTestDataPath {
            get { return @"..\..\TestData\UseImplicitLineContinuation\QuickFix"; }
        }

        [Test]
        public void ExecuteTest() {
            DoTestFiles("Test1.vb");
        }
    }
}
