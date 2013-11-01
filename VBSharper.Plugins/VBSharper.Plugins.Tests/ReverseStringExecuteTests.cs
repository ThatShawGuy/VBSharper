using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.CSharp.Test;
using JetBrains.ReSharper.Intentions.Extensibility;
using NUnit.Framework;

namespace VBSharper.Plugins.Tests
{
  [TestFixture]
  public class ReverseStringExecuteTests : CSharpContextActionExecuteTestBase
  {
    protected override string ExtraPath
    {
      get { return "ReverseStringAction"; }
    }

    protected override string RelativeTestDataPath
    {
      get { return "ReverseStringAction"; }
    }

    protected override IContextAction CreateContextAction(ICSharpContextActionDataProvider dataProvider)
    {
      //return new ReverseStringAction(dataProvider);
        return null;
    }

    [Test]
    public void ExecuteTest()
    {
      DoTestFiles("execute01.cs");
    }
  }
}
