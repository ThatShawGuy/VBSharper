< _
Serializable() _
> _
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

        Dim customer = _
            New With _
            { _
                .Name = "Terry Adams", _
                .Company = "Adventure Works", _
                .Email = "terry@www.adventure-works.com" _
            }

        Dim customerXml = _
            <Customer>
                <Name> This is just a test. ______
                    <%= _
                        customer.Name _
                    %>
                </Name>
                <Email>
                    <%= _
                        customer.Email _
                    %>
                </Email>
            </Customer>

        Dim test = customerXml.Nodes

        Dim mySQLText = _
            "SELECT * FROM Titles JOIN Publishers " & _
            "ON Publishers.PubId = Titles.PubID " & _
            "WHERE Publishers.State = 'CA'"

        Dim fileStream =
            My.Computer.FileSystem. _
            OpenTextFileReader("C:\FakePath")

        Dim memoryInUse =
            My.Computer.Info.TotalPhysicalMemory + _
            My.Computer.Info.TotalVirtualMemory - _
            My.Computer.Info.AvailablePhysicalMemory - _
            My.Computer.Info.AvailableVirtualMemory

        Dim inStream As IO.FileStream
        If TypeOf inStream Is  _
            IO.FileStream AndAlso _
            inStream IsNot _
            Nothing Then
            'ReadFile(inStream)
        End If
    End Sub

    Public Sub AnalyzeMemberQualifierCharacterLineContinuation()
        Dim fileStream = _
            My.Computer.FileSystem. _
            OpenTextFileReader("C:\Test")

        ' Not allowed:
        Dim aType = New With {. _
           PropertyName = "Value"}

        ' Allowed:
        Dim anotherType = New With { _
            .PropertyName = _
            "Value"}

        '-----------------------------------
        Dim log As New EventLog()

        ' Not allowed:
        With log _
            .
            Source = "Application"
        End With

        ' Allowed:
        With log
            .Source = _
                "Application"
        End With
    End Sub

    Public Sub MoreLineContinuationFun()
        Dim customerXml = <Test>My XML</Test>
        Dim customerName = customerXml. _
            <Test>.Value
        Dim customerEmail = customerXml... _
          <Email>.Value

        Dim vsProcesses = From _
                          proc In _
                          Process.GetProcesses _
                          Where _
                          proc.MainWindowTitle.Contains("Visual Studio") _
                          Select _
                          proc.ProcessName, proc.Id, _
                          proc.MainWindowTitle

        For Each _
            p In _
            vsProcesses

            Console.WriteLine("{0}" & vbTab & "{1}" & vbTab & "{2}", _
              p.ProcessName, _
              p.Id, _
              p.MainWindowTitle)
        Next

        Dim days = New List(Of String) From _
            { _
                "Mo", "Tu", "We", "Th", "F", "Sa", "Su" _
            }

    End Sub

End Class
