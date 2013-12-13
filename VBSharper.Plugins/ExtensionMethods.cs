using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB.Tree;

namespace VBSharper.Plugins
{
    public static class ExtensionMethods
    {       
        public static bool IsPrimaryLanguage<TLanguageType>(this IPsiSourceFile sourceFile) where TLanguageType : PsiLanguageType {
            var psiServices = sourceFile.GetPsiServices();
            IFile psiFile = psiServices.Files.GetDominantPsiFile<TLanguageType>(sourceFile);
            
            return (psiFile != null);
        }

        /// <summary>
        /// Replacement for VBExpressionBase.ReplaceBy that does not insert unnecessary parentheses.
        /// </summary>
        /// <param name="oldExpression"></param>
        /// <param name="newExpression"></param>
        /// <returns></returns>
        public static IVBExpression ReplaceByExtension(this IVBExpression oldExpression, IVBExpression newExpression)
        {
            using (WriteLockCookie.Create(oldExpression.IsPhysical())) {
                if (oldExpression.Contains(newExpression))
                    newExpression = newExpression.Copy(oldExpression.Parent);

                return ModificationUtil.ReplaceChild(oldExpression, newExpression);               
            }
        }
    }
}
