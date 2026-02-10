
Public Class Form1
    ' مصفوفة لتخزين أرقام الصور (0, 1, 2) مكررة مرتين
    Dim imgIndices As New List(Of Integer) From {0, 0, 1, 1, 2, 2}
    Dim firstClicked As Button = Nothing
    Dim secondClicked As Button = Nothing
    Dim timeLeft As Integer = 30 ' وقت اللعبة بالثواني

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGame()
    End Sub

    ' إعداد اللعبة وتوزيع الصور عشوائياً
    Private Sub SetupGame()
        Dim rnd As New Random()
        ' ترتيب الأرقام عشوائياً في كل مرة تبدأ اللعبة
        imgIndices = imgIndices.OrderBy(Function(x) rnd.Next()).ToList()

        ' مصفوفة تضم الأزرار الستة
        Dim buttons = {Button1, Button2, Button3, Button4, Button5, Button6}

        For i As Integer = 0 To buttons.Length - 1
            buttons(i).Image = Nothing ' إخفاء الصورة في البداية
            buttons(i).Tag = imgIndices(i) ' تخزين رقم الفهرس الخاص بالصورة
            buttons(i).Enabled = True
            buttons(i).BackColor = Color.WhiteSmoke
            ' لإعادة الأرقام في بداية كل لعبة جديدة (اختياري)
            buttons(i).Text = (i + 1).ToString
        Next

        timeLeft = 30 ' إعادة ضبط الوقت
        Label1.Text = "الوقت: " & timeLeft
        Timer_countdown.Start() ' تشغيل مؤقت الوقت الكلي
    End Sub

    ' حدث الضغط على أي زر من الأزرار الستة
    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click
        Dim clickedButton = DirectCast(sender, Button)

        ' منع الضغط إذا كان التيمر يعمل أو الزر تم كشفه بالفعل
        If Timer_Hide.Enabled OrElse clickedButton.Image IsNot Nothing Then Return

        ' --- إخفاء الرقم فوراً عند ظهور الصورة ---
        clickedButton.Text = ""

        ' إظهار الصورة من الـ ImageList بناءً على الرقم المخزن في الـ Tag
        Dim imgIndex As Integer = CInt(clickedButton.Tag)
        clickedButton.Image = ImageList1.Images(imgIndex)

        If firstClicked Is Nothing Then
            ' حفظ الزر الأول الذي تم النقر عليه
            firstClicked = clickedButton
        Else
            ' حفظ الزر الثاني والتحقق من التطابق
            secondClicked = clickedButton
            CheckForMatch()
        End If
    End Sub

    ' التحقق من تطابق الصورتين
    Private Sub CheckForMatch()
        ' مقارنة الـ Tag (رقم فهرس الصورة)
        If firstClicked.Tag.ToString = secondClicked.Tag.ToString Then
            ' حالة التطابق: تمييز الأزرار باللون الأخضر
            firstClicked.BackColor = Color.LightGreen
            secondClicked.BackColor = Color.LightGreen
            firstClicked = Nothing
            secondClicked = Nothing
            CheckWinner()
        Else
            ' حالة عدم التطابق: تشغيل تيمر الإخفاء
            Timer_Hide.Start()
        End If
    End Sub

    ' تيمر إخفاء الصور (Interval = 750)
    Private Sub Timer_Hide_Tick(sender As Object, e As EventArgs) Handles Timer_Hide.Tick
        Timer_Hide.Stop()
        ' إعادة إخفاء الصور ومسح الاختيارات
        firstClicked.Image = Nothing
        secondClicked.Image = Nothing

        ' إعادة إظهار الأرقام إذا أردت بعد فشل التطابق
        ' firstClicked.Text = "؟" 
        ' secondClicked.Text = "؟"

        firstClicked = Nothing
        secondClicked = Nothing
    End Sub

    ' تيمر الوقت التنازلي (Interval = 1000)
    Private Sub Timer_Countdown_Tick(sender As Object, e As EventArgs) Handles Timer_countdown.Tick
        timeLeft -= 1
        Label1.Text = "الوقت: " & timeLeft

        If timeLeft <= 0 Then
            Timer_countdown.Stop()
            MsgBox("للأسف انتهى الوقت! حاول مجدداً.", MsgBoxStyle.Critical)
            SetupGame() ' إعادة تشغيل اللعبة تلقائياً
        End If
    End Sub

    ' التحقق مما إذا فاز اللاعب
    Private Sub CheckWinner()
        Dim buttons = {Button1, Button2, Button3, Button4, Button5, Button6}
        ' الفوز يتحقق عندما تظهر جميع الصور في الأزرار الستة
        If buttons.All(Function(b) b.Image IsNot Nothing) Then
            Timer_countdown.Stop()
            MsgBox("رائع! لقد تذكرت جميع الصور بنجاح.", MsgBoxStyle.Information)
        End If
    End Sub
End Class