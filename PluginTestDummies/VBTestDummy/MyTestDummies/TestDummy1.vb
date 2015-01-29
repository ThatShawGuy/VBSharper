Imports Microsoft.VisualBasic.Collection
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic


<Serializable()>
Public Class TestDummy1

    Private _justAProperty As String

    Public Property JustAProperty As String
        Get
            Return _justAProperty
        End Get
        Set(ByVal value As String)
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

        Dim myLongString =
            "Just a test " _
            & "keep testing " & _
            "and we're done."

        Dim myArray As Array = testers.ToList().ToArray()
        Dim justATest = ControlChars.NewLine

        If (runners.Count > 0 OrElse runners.Count = 1) AndAlso runners.Count < 0 Then
            ' Do something really cool here.
        End If

        If Not (Not testers Is Nothing And Not testers Is Nothing) Or (testers IsNot Nothing) AndAlso Not runners IsNot Nothing Then
            myBoolean = True
        End If

        If CBool(myBoolean) Then Return CStr(String.Empty)

        JustDoIt("test")

        Return String.Empty
    End Function

    Public Shared Sub JustDoIt(ByVal it As String)

    End Sub

    Public Shared Sub JustDoIt()
        Dim myInteger = 1
        Dim test = GetSomeStuff(myInteger, 2)
    End Sub

End Class
