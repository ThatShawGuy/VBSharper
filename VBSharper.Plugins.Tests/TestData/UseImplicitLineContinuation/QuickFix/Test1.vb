﻿< _
Serializable() _
{caret}> _
< _
Obsolete() _
> _
Public Class ImplicitLineContinuationDummy

Public Function GetUsername(ByVal username As String, _
                            ByVal delimiter As Char, _
                            ByVal position As Integer _
                            ) As String

    Return username.Split(delimiter)(position)
End Function

Public Sub UsernameTest( _
                       )
    Dim username = GetUsername( _
        Security.Principal.WindowsIdentity.GetCurrent().Name, _
        CChar("\"), _
        1 _
    )
End Sub

End Class