
Public Class Form1
    
    Dim imgIndices As New List(Of Integer) From {0, 0, 1, 1, 2, 2}
    Dim firstClicked As Button = Nothing
    Dim secondClicked As Button = Nothing
    Dim timeLeft As Integer = 30 

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGame()
    End Sub

    
    Private Sub SetupGame()
        Dim rnd As New Random()
        
        imgIndices = imgIndices.OrderBy(Function(x) rnd.Next()).ToList()

        
        Dim buttons = {Button1, Button2, Button3, Button4, Button5, Button6}

        For i As Integer = 0 To buttons.Length - 1
            buttons(i).Image = Nothing 
            buttons(i).Tag = imgIndices(i) 
            buttons(i).Enabled = True
            buttons(i).BackColor = Color.WhiteSmoke
            
            buttons(i).Text = (i + 1).ToString
        Next

        timeLeft = 30 ' 
        Label1.Text = "الوقت: " & timeLeft
        Timer_countdown.Start() 
    End Sub

    
    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click
        Dim clickedButton = DirectCast(sender, Button)

        
        If Timer_Hide.Enabled OrElse clickedButton.Image IsNot Nothing Then Return

        
        clickedButton.Text = ""

        Dim imgIndex As Integer = CInt(clickedButton.Tag)
        clickedButton.Image = ImageList1.Images(imgIndex)

        If firstClicked Is Nothing Then
            
            firstClicked = clickedButton
        Else
            
            secondClicked = clickedButton
            CheckForMatch()
        End If
    End Sub

    
    Private Sub CheckForMatch()
        
        If firstClicked.Tag.ToString = secondClicked.Tag.ToString Then
        
            firstClicked.BackColor = Color.LightGreen
            secondClicked.BackColor = Color.LightGreen
            firstClicked = Nothing
            secondClicked = Nothing
            CheckWinner()
        Else
            
            Timer_Hide.Start()
        End If
    End Sub

    ' 
    Private Sub Timer_Hide_Tick(sender As Object, e As EventArgs) Handles Timer_Hide.Tick
        Timer_Hide.Stop()
    
        firstClicked.Image = Nothing
        secondClicked.Image = Nothing

        

        firstClicked = Nothing
        secondClicked = Nothing
    End Sub

    Private Sub Timer_Countdown_Tick(sender As Object, e As EventArgs) Handles Timer_countdown.Tick
        timeLeft -= 1
        Label1.Text = "الوقت: " & timeLeft

        If timeLeft <= 0 Then
            Timer_countdown.Stop()
            MsgBox("للأسف انتهى الوقت! حاول مجدداً.", MsgBoxStyle.Critical)
            SetupGame() 
        End If
    End Sub

    
    Private Sub CheckWinner()
        Dim buttons = {Button1, Button2, Button3, Button4, Button5, Button6}
        
        If buttons.All(Function(b) b.Image IsNot Nothing) Then
            Timer_countdown.Stop()
            MsgBox("رائع! لقد تذكرت جميع الصور بنجاح.", MsgBoxStyle.Information)
        End If
    End Sub
End Class
