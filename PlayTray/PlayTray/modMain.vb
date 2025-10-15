Module modMain
    <STAThread()>
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Start the application with tray form
        Application.Run(New frmMain())
    End Sub
End Module