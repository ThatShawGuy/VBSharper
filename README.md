VBSharper
=========

VBSharper is a set of ReSharper extensions for those of us who want/need to work in VB.Net for one reason or another.

ReSharper is an outstanding add-on for the Visual Studio IDE. The C# support in ReSharper is excellent. However, during their .Net career many developers end up working with both C# and VB.Net. VB.Net is often thought of and treated as a second-class language and inevitably this bias shows up in ReSharper (Case in point is Structural Search and Replace). These extensions are meant to even out the playing field and give you some much needed VB.Net code refactorings and quick fixes.

Many thanks to the ReSharper team for allowing plugins full access to the ReSharper API.

Bulk Quick Fixes
----------------
<ul>
  <li>Use IsNot Operator - Replace "Not x Is y" with "x IsNot y"</li>
  <li>Remove ByVal Keyword - Explicit use of ByVal keyword is unnecessary</li>
  <li>Use Short-circuit Operators - Replace And with AndAlso & Or with OrElse</li>
  <li>Use Implicit Line Continuation - Remove unnecessary explicit line continuation characters</li>
