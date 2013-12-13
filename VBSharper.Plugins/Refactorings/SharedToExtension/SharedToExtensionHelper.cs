using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resolve.ExtensionMethods;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Impl.Resolve;
using JetBrains.ReSharper.Psi.VB.Tree;

namespace VBSharper.Plugins.Refactorings.SharedToExtension
{
    public class SharedToExtensionHelper
    {
        public void MakeCallExtension(IReference reference) {
            var treeNode = reference.GetTreeNode();
            if (!treeNode.IsVB9Supported()) return;

            var referenceExpression = treeNode as IReferenceExpression;
            if (referenceExpression == null) return;

            IIndexExpression byExpression = IndexExpressionNavigator.GetByExpression(referenceExpression);
            if (byExpression == null) return;

            IVBArgument vbArgument = null;
            int index = 0;
            using (var enumerator = byExpression.ArgumentList.Arguments.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var current = enumerator.Current;
                    var matchingParameter = current.GetMatchingParameter();
                    if (matchingParameter != null && matchingParameter.Element.IndexOf() == 0) {
                        vbArgument = current;
                        break;
                    }
                    
                    ++index;
                }
            }

            if (vbArgument == null) return;

            IVBExpression vbExpression = null;
            var expressionArgument = vbArgument as IExpressionArgument;
            if (expressionArgument != null)
                vbExpression = expressionArgument.Expression;
            if (vbExpression == null) return;
            
            var indexExpression1 = 
                (IIndexExpression) VBElementFactory.GetInstance(treeNode.GetPsiModule())
                    .CreateExpression("$0." + reference.GetName() + "()", new object[] { vbExpression });
            indexExpression1.SetArgumentList(byExpression.ArgumentList);
            indexExpression1.RemoveArgument(indexExpression1.Arguments[index]);
            byExpression.ReplaceBy(indexExpression1);
        }

        public void MakeCallShared(IReference reference) {
            var referenceExpression1 = reference.GetTreeNode() as IReferenceExpression;
            if (referenceExpression1 == null || !reference.Resolve().Result.IsExtensionMethod()) return;

            var byExpression = IndexExpressionNavigator.GetByExpression(referenceExpression1);
            if (byExpression == null) return;

            var substitution = reference.Resolve().Result.Substitution;
            var referenceExpression2 = byExpression.Expression as IReferenceExpression;
            if (referenceExpression2 == null) return;

            var instance = VBElementFactory.GetInstance(referenceExpression1.GetPsiModule());
            var vbExpression = referenceExpression2.QualifierExpression ?? instance.CreateExpression("me", new object[0]);
            byExpression.AddArgumentAfter(instance.CreateArgument(vbExpression), null);
            //byExpression.SetExpression(instance.CreateReferenceExpression(this.Executer.Method.ShortName, new object[0]));
            //((IReferenceExpression)byExpression.Expression).Reference.BindTo((IDeclaredElement)this.Executer.Method);
            //if (((IReferenceExpression)byExpression.Expression).Reference.BindTo((IDeclaredElement)this.Workflow.Method, substitution).CheckResolveResult() == ResolveErrorType.OK) return;

            //this.Driver.AddConflict((IConflict)ReferenceConflict.CreateError(reference, "{0} can not be transformed correctly"));
        }
    }

}
