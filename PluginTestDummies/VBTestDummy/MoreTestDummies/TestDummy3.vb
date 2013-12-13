Imports Microsoft.VisualBasic.Collection
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic

Public Class TestDummy3

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
        myBoolean = (Not testers Is Nothing)

        Dim myArray As Array = testers.ToList().ToArray()
        Dim justATest = ControlChars.NewLine

        If (runners.Count > 0 Or runners.Count = 1) And runners.Count < 0 Then
            ' Do something really cool here.
        End If

        If Not (Not testers Is Nothing AndAlso Not testers Is Nothing).ToString() Is Nothing OrElse (testers IsNot Nothing) And Not runners IsNot Nothing Then
            myBoolean = True
        End If

        Dim anotherTest As Short = anotherTest And &HFF

        If CBool(myBoolean) Then Return CStr(String.Empty)

        Return String.Empty
    End Function

    Public Shared Sub JustDoIt(ByVal it As String)
        Dim anotherVariable = "myVar"
        Dim myInteger = 8000
        Dim anotherTest = GetSomeStuff(myInteger, 500)
    End Sub

    Public Shared Sub JustDoIt(ByVal justATest As Double)
        Dim myInteger = 9
        Dim test = GetSomeStuff(myInteger, 5)
        Dim myString = "My Test"
        JustDoIt(myString)
    End Sub

End Class
