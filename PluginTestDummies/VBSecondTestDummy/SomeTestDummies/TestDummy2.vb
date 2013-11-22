Public Class TestDummy2

    Private _justAProperty As String

    Public Property JustAProperty As String
        Get
            Return _justAProperty
        End Get
        Set(value As String)
            _justAProperty = value
        End Set
    End Property

    Public Sub New(ByVal myTestParam As Integer)
        Dim justSaving = myTestParam
    End Sub

    Public Shared Function GetSomeStuff(ByVal stuffID As Integer, ByRef moreStuffID As Integer) As String
        Dim testers = New List(Of Integer)
        Dim runners = New List(Of String)

        Dim myBoolean As Boolean
        myBoolean = (testers IsNot Nothing)

        Dim myArray = testers.ToList().ToArray()
        Dim justATest = ControlChars.NewLine

        If (runners.Count > 0 OrElse runners.Count = 1) AndAlso runners.Count < 0 Then
            ' Do something really cool here.
        End If

        If Not (Not testers Is Nothing AndAlso testers IsNot Nothing) OrElse (Not testers Is Nothing) AndAlso Not runners IsNot Nothing Then
            myBoolean = True
        End If

        If CBool(myBoolean) Then Return CStr(String.Empty)

        Return String.Empty
    End Function

    Public Sub JustDoIt(ByVal it As String)

    End Sub

    Public Sub JustDoIt()
        Dim test = GetSomeStuff(5, 6)
    End Sub

End Class
