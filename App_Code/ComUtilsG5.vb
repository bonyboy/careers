Imports Microsoft.VisualBasic
Imports System.Web.Configuration
Imports System.Net.Mail
' for encryption functions
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography

Namespace Touchmark

    Public Class ComUtilsG5

        Public Shared connString As String = Convert.ToString(ConfigurationManager.ConnectionStrings("Touchmark").ConnectionString)
        Public Shared BaseURL As String = Convert.ToString(ConfigurationManager.AppSettings("BaseURL"))

        Public Shared PageName As String = "g5form.aspx"
        Public Shared MaxLengthStandard As Integer = 50
        Public Shared MaxLengthMessage As Integer = 255
        Public Shared MaxLengthResidentMessage As Integer = 5000

        Public Shared Function AllowedChars(ByVal s As String) As String

            'metacharacters
            '\ | ( ) [ { ^ $ * + ? . < >
            Dim regex As Regex = New Regex("[^a-zA-Z0-9\,\.\?\/\:\;\'\-\=\\\~\!\@\#\$\%\*\(\)\\_\+\|\{\[\}\]\s]")
            s = regex.Replace(s, "")

            Return s
        End Function

        Public Shared Function StripSQLKeyWords(ByVal s As String) As String

            If s = "" Then Return ""

            Dim TempString As String = ""

            Dim str1 As String = "insert"
            Dim str2 As String = "union"
            Dim str3 As String = "delete"
            Dim str4 As String = "drop"
            Dim str5 As String = "alter"
            Dim str6 As String = "update"
            Dim str7 As String = "administrator"

            TempString = s.Replace(str1, "").Replace(str2, "").Replace(str3, "").Replace(str4, "").Replace(str5, "").Replace(str6, "").Replace(str7, "")

            Return TempString
        End Function

        Public Shared Function InsertSpace(ByVal OrigString As String, ByVal HowMany As Integer) As String

            Dim intLength As Integer = OrigString.Length()
            Dim intPosition As Integer = 1
            Dim CollectString As New StringBuilder
            Dim TempHolder As String = String.Empty
            Dim intLoop As Integer = intLength \ HowMany

            If intLoop > 0 Then

                For i As Integer = 0 To intLoop

                    TempHolder = Mid(OrigString, intPosition, HowMany)
                    CollectString.Append(TempHolder)

                    If TempHolder.IndexOf(Chr(10) & Chr(13)) = -1 And TempHolder.IndexOf(" ") = -1 Then
                        CollectString.Append(" ")
                    End If

                    intPosition += HowMany

                Next
                Return CollectString.ToString()
            Else
                Return OrigString
            End If
        End Function

        Public Shared Function SendMailToWebmaster(ByVal Subject As String, ByVal Pagename As String, ByVal ErrorMessage As String) As Boolean
            Try
                Dim mail As New MailMessage()
                mail.From = New MailAddress("Web Site <noreply@touchmark.com>")
                mail.To.Add(ConfigurationManager.AppSettings("DeveloperEmail"))
                mail.Subject = Subject
                mail.Body = "Message: " & Pagename & vbCrLf & vbCrLf & ErrorMessage
                mail.IsBodyHtml = True

                Dim smtp As New SmtpClient(ConfigurationManager.AppSettings("EmailServer"))
                smtp.Send(mail)
                smtp = Nothing
                mail = Nothing
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function FixNull(ByVal dbvalue As Object) As String
            If dbvalue Is DBNull.Value Then
                Return ""
            Else
                'NOTE: This will cast value to string if it isn't a string.
                Return dbvalue.ToString
            End If
        End Function

        Public Shared Function ValidateEmailAddress(ByVal EmailAddress As Object) As Boolean
            Dim AddressExpression As New Regex("\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
            Dim Email As String = FixNull(EmailAddress)
            Dim i As Integer = Email.Length - Email.Replace("@", "").Length
            If AddressExpression.IsMatch(Email) = True AndAlso i = 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function EncryptText(ByVal strText As String) As String
            Return Encrypt(strText, "&%#@?,:*")
        End Function

        Public Shared Function DecryptText(ByVal strText As String) As String
            Return Decrypt(strText, "&%#@?,:*")
        End Function

        Private Shared Function Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String
            Dim byKey() As Byte = {}
            Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

            Try
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))

                Dim des As New DESCryptoServiceProvider()
                Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
                Dim ms As New MemoryStream()
                Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
                cs.Write(inputByteArray, 0, inputByteArray.Length)
                cs.FlushFinalBlock()
                Return Convert.ToBase64String(ms.ToArray())

            Catch ex As Exception
                Return ex.Message
            End Try

        End Function

        Private Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
            Dim byKey() As Byte = {}
            Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
            Dim inputByteArray(strText.Length) As Byte

            Try
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
                Dim des As New DESCryptoServiceProvider()
                inputByteArray = Convert.FromBase64String(strText)
                Dim ms As New MemoryStream()
                Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

                cs.Write(inputByteArray, 0, inputByteArray.Length)
                cs.FlushFinalBlock()
                Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

                Return encoding.GetString(ms.ToArray())

            Catch ex As Exception
                Return ex.Message
            End Try

        End Function

        Public Shared Function URLDecode(ByVal str As String) As String
            Dim sT As String = String.Empty
            Dim sR As String = String.Empty

            For i As Integer = 1 To Len(str)
                sT = Mid(str, i, 1)
                If sT = "%" Then
                    If i + 2 <= Len(str) Then
                        sR += Chr(CLng("&H" & Mid(str, i + 1, 2)))
                        i += 2
                    End If
                Else
                    sR += sT
                End If
            Next

            Return sR
        End Function

        Public Shared Function URLEncode(ByVal str As String) As String

            Dim sRtn As String = String.Empty
            Dim sTmp As String = String.Empty
            Const sValidChars As String = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

            If Len(str) > 0 Then

                For iLoop As Integer = 1 To Len(str)

                    sTmp = Mid(str, iLoop, 1)

                    If InStr(1, sValidChars, sTmp, CompareMethod.Binary) = 0 Then
                        'if not ValidChar, convert to HEX and prefix with %
                        sTmp = Hex(Asc(sTmp))

                        If Len(sTmp) = 1 Then
                            sTmp = "%0" & sTmp.ToLower()
                        Else
                            sTmp = "%" & sTmp.ToLower()
                        End If
                    End If

                    sRtn += sTmp

                Next

                Return sRtn
            Else
                Return ""
            End If
        End Function

    End Class

End Namespace