Imports Microsoft.VisualBasic
Imports System.Web.Configuration
Imports System.Net.Mail
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Namespace Touchmark

    Public Class JobUtils

        Public Shared connString As String = ConfigurationManager.ConnectionStrings("Touchmark").ConnectionString
        Public Shared BaseURL As String = ConfigurationManager.AppSettings("BaseURL").ToString()
        Public Shared JobFilePath As String = ConfigurationManager.AppSettings("JobFilePath").ToString()
        Public Shared TempFilePath As String = JobFilePath & "temp\"
        Public Shared ApplyFilePath As String = JobFilePath & "apply\"

        Public Shared Function CheckExistingFilesTemp(ByVal UploadedFiles As HttpFileCollection) As String

            Dim i As Integer = 0
            Dim ReturnValue As String = ""

            Do Until i = UploadedFiles.Count

                Dim userPostedFile As HttpPostedFile = UploadedFiles(i)

                If (userPostedFile.ContentLength > 0) Then

                    If File.Exists(TempFilePath & System.IO.Path.GetFileName(userPostedFile.FileName)) = True Then
                        ReturnValue = Path.GetFileName(userPostedFile.FileName)
                        Exit Do
                    End If

                End If
                i += 1
            Loop

            Return ReturnValue
        End Function
    
        Public Shared Function CheckExistingFiles(ByVal UploadedFiles As HttpFileCollection) As String

            Dim i As Integer = 0
            Dim ReturnValue As String = ""

            Do Until i = UploadedFiles.Count

                Dim userPostedFile As HttpPostedFile = UploadedFiles(i)

                If (userPostedFile.ContentLength > 0) Then

                    If File.Exists(ApplyFilePath & System.IO.Path.GetFileName(userPostedFile.FileName)) = True Then
                        ReturnValue = Path.GetFileName(userPostedFile.FileName)
                        Exit Do
                    End If

                End If
                i += 1
            Loop

            Return ReturnValue
        End Function
    
        Public Shared Function CheckFileLength(ByVal UploadedFiles As HttpFileCollection) As Integer

            Dim i As Integer = 0
            Dim Counter As Integer = 0
            Dim ReturnValue As Integer = 0

            Do Until i = UploadedFiles.Count

                Dim userPostedFile As HttpPostedFile = UploadedFiles(i)

                If (userPostedFile.ContentLength > 0) Then

                    Counter += userPostedFile.ContentLength
                    If Counter / 1024 / 1024 > 20 Then
                        ReturnValue = Counter / 1024 / 1024
                        Exit Do
                    End If

                End If
                i += 1
            Loop

            Return ReturnValue
        End Function
    
        Public Shared Function CheckDuplicateFile(ByVal UploadedFiles As HttpFileCollection) As Boolean

            Dim i As Integer = 0
            Dim FileArray As New ArrayList
            Dim ReturnValue As Boolean = False

            Do Until i = UploadedFiles.Count

                Dim userPostedFile As HttpPostedFile = UploadedFiles(i)

                If (userPostedFile.ContentLength > 0) Then
                    FileArray.Sort()
                    Dim IsExist As Integer = FileArray.BinarySearch(userPostedFile.FileName.ToLower())
                    If IsExist < 0 Then
                        FileArray.Add(userPostedFile.FileName.ToLower())
                    Else
                        ReturnValue = True
                        Exit Do
                    End If

                End If
                i += 1
            Loop

            Return ReturnValue
        End Function
    
    Public Shared Function CheckExtensions(ByVal UploadedFiles As HttpFileCollection) As Boolean
        
        Dim i As Integer = 0
        Dim ReturnValue As Boolean = False

        Do Until i = UploadedFiles.Count
			
            Dim userPostedFile As HttpPostedFile = UploadedFiles(i)
			
            If (userPostedFile.ContentLength > 0) Then
					
                Select Case Path.GetExtension(userPostedFile.FileName).ToLower()
                    Case ".exe", ".cmd", ".bat"
                        ReturnValue = True
                        Exit Do
                    Case Else
                        ReturnValue = False
                End Select

            End If
            i += 1
        Loop
        
        Return ReturnValue
    End Function
    
        Public Shared Function CheckForBadChars(ByVal UploadedFiles As HttpFileCollection) As Boolean

            Dim i As Integer = 0
            Dim BadChars As String() = New String() {"#", "\", "/", ":", "*", "?", """", ">", "<", "|", "'"}
            Dim ReturnValue As Boolean = False

            Do Until i = UploadedFiles.Count

                Dim userPostedFile As HttpPostedFile = UploadedFiles(i)

                If (userPostedFile.ContentLength > 0) Then

                    Dim FileName As String = Path.GetFileName(userPostedFile.FileName)
                    Dim BadChar As String = ""
                    For Each BadChar In BadChars
                        If FileName.IndexOf(BadChar, 0) >= 0 Then
                            'Response.Write(BadChar & "--" & FileName) : Response.End()
                            ReturnValue = True
                            Exit For
                            Exit Do
                        End If
                    Next

                End If
                i += 1
            Loop

            Return ReturnValue
        End Function
    
        Public Shared Function CheckFileNameLength(ByVal UploadedFiles As HttpFileCollection) As Boolean

            Dim i As Integer = 0
            Const MaxLength As Integer = 50
            Dim ReturnValue As Boolean = False

            Do Until i = UploadedFiles.Count

                Dim userPostedFile As HttpPostedFile = UploadedFiles(i)

                If (userPostedFile.ContentLength > 0) Then

                    Dim FileName As String = Path.GetFileName(userPostedFile.FileName)

                    If FileName.Length > MaxLength Then
                        ReturnValue = True
                        Exit Do
                    End If

                End If
                i += 1
            Loop

            Return ReturnValue
        End Function
    
        Public Shared Function GetUploadedFileCount(ByVal UploadedFiles As HttpFileCollection) As Integer

            Dim i As Integer = 0
            Dim ReturnValue As Integer = 0

            Do Until i = UploadedFiles.Count
                Dim UserPostedFile As HttpPostedFile = UploadedFiles(i)

                If (UserPostedFile.ContentLength > 0) Then
                    ReturnValue += 1
                End If
                i += 1
            Loop

            Return ReturnValue
        End Function

        Public Shared Sub CleanUp()

            If DatabaseUtilsG5.GetJobLastUpdate() = True Then
                DatabaseUtilsG5.SetJobLastUpdate()
                RemoveOldFiles()
            End If

            CleanTemp()

        End Sub

        Private Shared Sub CleanTemp()

            Dim sFolder As String = TempFilePath
            Dim fis As New List(Of System.IO.FileInfo)
            fis.AddRange(New System.IO.DirectoryInfo(sFolder).GetFiles())
            For Each fi As System.IO.FileInfo In fis
                If fi.CreationTime < DateAdd(DateInterval.Minute, -5, Date.Now()) Then
                    DeleteFile(sFolder, fi)
                End If
            Next

        End Sub

        Private Shared Sub RemoveOldFiles()

            Dim sFolder As String = ApplyFilePath
            Dim fis2 As New List(Of System.IO.FileInfo)
            fis2.AddRange(New System.IO.DirectoryInfo(sFolder).GetFiles())
            For Each fi2 As System.IO.FileInfo In fis2
                If fi2.CreationTime < DateAdd(DateInterval.Month, -12, Date.Now()) Then
                    DeleteFile(sFolder, fi2)
                End If
            Next

        End Sub
   
        Private Shared Sub DeleteFile(ByVal sFolder As String, ByVal fi As System.IO.FileInfo)
            If System.IO.File.Exists(sFolder & fi.Name) = True Then
                System.IO.File.Delete(sFolder & fi.Name)
            End If
        End Sub
    
        Private Shared Function GetJobAutoResponseFile() As String

            Dim FileFolderPath As String = ConfigurationManager.AppSettings("AutoResponseFilePath").ToString()
            Dim ReturnValue As New StringBuilder

            Try
                Dim objStreamReader As New StreamReader(FileFolderPath & "all.html")

                While objStreamReader.Peek <> -1
                    ReturnValue.Append(objStreamReader.ReadLine())
                End While

                objStreamReader.Close()

            Catch ex As Exception
                Return ""
            End Try

            Return ReturnValue.ToString()
        End Function
    
        Public Shared Function BuildResponseHTML(ByVal FullName As String, ByVal PreHireURL As String) As String

            Dim FileString As String = GetJobAutoResponseFile()

            FileString = FileString.Replace("|||name|||", FullName).Replace("|||link|||", PreHireURL)

            Return FileString
        End Function
    
        Public Shared Sub SendAutoResponseEmail(ByVal AutoResponseText As String, ByVal SenderEmail As String)

            If ComUtilsG5.FixNull(AutoResponseText) = "" OrElse _
            ComUtilsG5.ValidateEmailAddress(ComUtilsG5.FixNull(SenderEmail)) = False Then Exit Sub

            Try

                Dim EmailSubject As String = "Touchmark resume submission — Next Step"

                Dim mail As New MailMessage()
                mail.From = New MailAddress("Touchmark.com <noreply@touchmark.com>")
                mail.To.Add(SenderEmail)

                mail.Bcc.Add(ConfigurationManager.AppSettings("DeveloperEmail"))
                mail.Subject = EmailSubject
                mail.Body = AutoResponseText & "<br /><br />To: " & SenderEmail
                mail.IsBodyHtml = True

                Dim smtp As New SmtpClient(ConfigurationManager.AppSettings("EmailServer"))
                smtp.Send(mail)
                smtp = Nothing
                mail = Nothing

            Catch ex As Exception

                Dim ErrorMessage As String = (New System.Diagnostics.StackFrame()).GetMethod().Name & "<br><br>" & ex.Message & "<br><br>" & ex.ToString()
                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "SendAutoResponseEmail [Job] Error", ErrorMessage)

            End Try

        End Sub

    End Class

End Namespace