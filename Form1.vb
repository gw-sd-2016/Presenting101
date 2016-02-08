Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "tutorialspont.com"


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim frmAbout As New Form()
        Me.Opacity = 50
        'frmAbout.ShowDialog(Me)
        Timer1.Start()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ProgressBar1.Increment(1)
        If ProgressBar1.Value = ProgressBar1.Maximum Then
            TabControl1.SelectTab(1)
            Timer1.Stop()
            Timer2.Start()
        End If

    End Sub


    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Tester.Click

    End Sub



    

   

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        ProgressBar2.Increment(5)
        If ProgressBar2.Value = ProgressBar2.Maximum Then

        End If
    End Sub
End Class
