﻿Public Class Case1

    Private _myProperty As String

    Public Property MyProperty As String
        Get
            Return _myProperty
        End Get
        Set(ByVal{caret} value As String)
            _myProperty = value
        End Set
    End Property

    Public Sub New(ByVal myTestParam As Integer)

    End Sub

    Public Shared Sub DoSomeStuff(ByVal stuffID As Integer, ByRef anotherID As Integer, ByVal moreStuffID As Integer)

    End Sub

End Class