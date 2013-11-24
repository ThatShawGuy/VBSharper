using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB;
using JetBrains.ReSharper.Psi.VB.Parsing;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.Util;

namespace VBSharper.Plugins.QuickFixes.UseImplicitLineContinuation
{
    public static class VBImplicitLineContinuationHelper 
    {
        private static readonly NodeTypeSet OurOperatorSigns = new NodeTypeSet(new NodeType[] {
          VBTokenType.LT,
          VBTokenType.LTLT,
          VBTokenType.LTLTEQ,
          VBTokenType.GT,
          VBTokenType.GTGT,
          VBTokenType.GTGTEQ,
          VBTokenType.NE,
          VBTokenType.LE,
          VBTokenType.GE,
          VBTokenType.AT,
          VBTokenType.AND,
          VBTokenType.ANDEQ,
          VBTokenType.PLUS,
          VBTokenType.PLUSEQ,
          VBTokenType.MINUS,
          VBTokenType.MINUSEQ,
          VBTokenType.BACKSLASH,
          VBTokenType.BACKSLASHEQ,
          VBTokenType.ASTERISK,
          VBTokenType.ASTERISKEQ,
          VBTokenType.DIV,
          VBTokenType.DIVEQ,
          VBTokenType.XOR,
          VBTokenType.XOREQ,
          VBTokenType.IS_KEYWORD,
          VBTokenType.ISNOT_KEYWORD,
          VBTokenType.XOR_KEYWORD,
          VBTokenType.OR_KEYWORD,
          VBTokenType.ORELSE_KEYWORD,
          VBTokenType.AND_KEYWORD,
          VBTokenType.ANDALSO_KEYWORD,
          VBTokenType.LIKE_KEYWORD,
          VBTokenType.MOD_KEYWORD
        });

        public static bool CanUseImplicitLineContinuationBetween([NotNull] ITokenNode leftNode, [NotNull] ITokenNode rightNode) {
            Assertion.Assert(leftNode != null, "leftNode != null");
            Assertion.Assert(rightNode != null, "rightNode != null");
            
            var leftNodeParent = leftNode.Parent;
            Assertion.Assert(leftNodeParent != null, "leftNodeParent != null");
            var leftNodeGrandParent = leftNodeParent.Parent;
            Assertion.Assert(leftNodeGrandParent != null, "leftNodeGrandParent != null");
            var rightNodeParent = rightNode.Parent;
            Assertion.Assert(rightNodeParent != null, "rightNodeParent != null");

            if (!leftNodeParent.IsVB10Supported()) return false;
            
            var leftNodeTokenType = leftNode.GetTokenType();
            var rightNodeTokenType = rightNode.GetTokenType();
      
            return 
                leftNodeTokenType == VBTokenType.COMMA || 
                leftNodeTokenType == VBTokenType.LPARENTH ||
                rightNodeTokenType == VBTokenType.RPARENTH ||
                leftNodeTokenType == VBTokenType.LBRACE ||
                leftNodeTokenType == VBTokenType.RBRACE ||
                rightNodeTokenType == VBTokenType.RBRACE ||
                leftNodeTokenType == VBTokenType.EQ ||
                leftNodeTokenType == VBTokenType.IN_KEYWORD ||
                leftNodeTokenType == VBTokenType.FROM_KEYWORD ||
                leftNodeParent is IQueryOperator ||
                rightNodeParent is IQueryOperator ||
                (leftNodeTokenType == VBTokenType.WITH_KEYWORD && leftNodeParent is IAnonymousObjectInitializer) ||
                (leftNodeParent is IAttributeList && (leftNodeTokenType == VBTokenType.LT || leftNodeTokenType == VBTokenType.GT)) ||
                (leftNodeGrandParent is IAttribute && (leftNodeTokenType == VBTokenType.LPARENTH || leftNodeTokenType == VBTokenType.RPARENTH)) ||
                (leftNodeGrandParent is IPositionalArgument && rightNodeTokenType == VBTokenType.RPARENTH) ||
                leftNodeTokenType == VBTokenType.XML_SCRIPLET_START ||
                leftNodeTokenType == VBTokenType.XML_SCRIPLET_END || 
                rightNodeTokenType == VBTokenType.XML_SCRIPLET_END ||
                (leftNodeParent is IReferenceExpression && leftNodeTokenType == VBTokenType.DOT) || 
                (leftNodeParent is IReferenceName && leftNodeTokenType == VBTokenType.DOT) || 
                (leftNodeParent is IVBXmlMemberAccessExpression && leftNodeTokenType == VBTokenType.DOT && rightNodeTokenType != VBTokenType.DOT) || 
                (leftNodeParent is IVBXmlMemberAccessExpression && leftNodeTokenType == VBTokenType.AT) || 
                (leftNodeParent is IAttributeList && VBFileNavigator.GetByGlobalAttributes(leftNodeParent as IAttributeList) == null) || 
                (OurOperatorSigns[leftNodeTokenType] && (leftNodeParent is IVBExpression || leftNodeParent is IAssignmentStatement));
        } 
    }
}
