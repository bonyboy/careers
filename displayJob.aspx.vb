Imports System.Data
Imports System.Data.SqlClient
Imports Touchmark
Imports System.IO
Imports System.Net.Mail

Partial Class displayJob
    Inherits System.Web.UI.Page

    Dim connString As String = JobUtils.connString
    Dim BaseURL As String = JobUtils.BaseURL
    Dim ApplyFilePath As String = JobUtils.ApplyFilePath
    Dim TempFilePath As String = JobUtils.TempFilePath

    Dim ApplicantName As String = String.Empty
    Dim ApplicantEmail As String = String.Empty
    Dim ApplicantPhone As String = String.Empty
    Dim ApplicantMessage As String = String.Empty
    Dim Filename1 As String = String.Empty
    Dim Filename2 As String = String.Empty

    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Dim QueryString As String = String.Empty
        If Not Request.ServerVariables("QUERY_STRING") Is Nothing AndAlso Not Request.ServerVariables("QUERY_STRING") = "" Then
            QueryString = "?" & Request.ServerVariables("QUERY_STRING")
        End If

        If Request.ServerVariables("SERVER_PORT") = 80 Then
            Dim strSecureURL As String = "https://"
            strSecureURL += Request.ServerVariables("SERVER_NAME")
            strSecureURL += Request.ServerVariables("URL")
            strSecureURL += QueryString
            Response.Redirect(strSecureURL)
        End If

        If Not Page.IsPostBack = True Then

            Dim UserIP As String = System.Web.HttpContext.Current.Request.UserHostAddress

            If DatabaseUtilsG5.IsIP(UserIP) = True Then
                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site IP Error", "displayjob.aspx", "<br><br>Banned from RFI<br><br>" & UserIP)
                DatabaseUtilsG5.UpdateLastHit(UserIP)
                SetError()
                Exit Sub
            End If

            If DatabaseUtilsG5.BanByCode(UserIP, "IN") = True Then
                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site IP Error", "displayjob.aspx", "<br><br>Banned from RFI<br><br>IN IP:" & UserIP)
                SetError()
                Exit Sub
            End If

            If DatabaseUtilsG5.BanByCode(UserIP, "RU") = True Then
                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site IP Error", "displayjob.aspx", "<br><br>Banned from RFI<br><br>RU IP:" & UserIP)
                SetError()
                Exit Sub
            End If


            If Not Request.QueryString("jid") = "" Then

                Dim s As String = ComUtilsG5.DecryptText(Request.QueryString("jid"))
                Dim i As Integer

                If Int32.TryParse(s, i) = True Then

                    If LoadJob(i) = False Then
                        Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "displayjob.aspx", "job not found: " & s)
                        SetError()
                    Else
                        DivJobNotFound.Visible = False
                        DivMain.Visible = True
                        DivMain2.Visible = True
                        HiddenFieldJobId.Value = i

                        HiddenFieldFileNames.Value = ""
                        HiddenFieldUserIP.Value = UserIP

                    End If

                Else
                    Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "displayjob.aspx", "jid did not resolve: " & s)
                    SetError()
                End If

            Else

                SetError()

            End If

        End If

    End Sub

    Protected Function GetEntityId() As Integer

        Dim i As Integer
        If Int32.TryParse(HiddenFieldEntityId.Value, i) = False Then
            Return 0
        Else
            Return i
        End If

    End Function

    Protected Function LoadJob(ByVal JobId As Integer) As Boolean

        Dim sqlconn As New SqlConnection(connString)

        Dim strSQL As String = "select top(1) j.description, j.requirement,"

        strSQL += " case when j.jobtitleid > 0 then"
        strSQL += " isnull((select jlt.jobtitle from touchmark.dbo.joblookuptitle jlt where jlt.id=j.jobtitleid),'') else j.position end jobtitle,"

        strSQL += "isnull((select case when j.entityid=24 then jlb.fullcatext" 'teab
        strSQL += " when j.isparttimebenefit=1 then jlb.partustext"
        strSQL += " else jlb.fullustext end from touchmark.dbo.joblookupbenefit jlb),'') benefit,"

        strSQL += "e.city + ' ' + e.state citystate, e.id entityid, e.entityabbr, e.entitylong, e.url,"
        strSQL += "e.street1 + '<br />' + e.city + ', ' + e.state + ' ' + e.postalcode address"

        strSQL += " from touchmark.dbo.jobs j left join touchmark.dbo.entity e on j.entityid=e.id"
        strSQL += " where e.active=1 and j.display=1 and j.expiredate > getdate()"
        strSQL += " and j.id=@jid"

        Dim ReturnValue As Boolean = False

        Try
            Dim SqlComm As New SqlCommand(strSQL, sqlconn)
            sqlconn.Open()

            With SqlComm.Parameters
                .Add(New SqlParameter("@jid", SqlDbType.Int)).Value = JobId
            End With

            Dim SqlSelect As SqlDataReader
            SqlSelect = SqlComm.ExecuteReader()

            If SqlSelect.HasRows Then

                SqlSelect.Read()

                LiteralPosition.Text = ComUtilsG5.FixNull(SqlSelect.Item("jobtitle"))
                'LiteralEntityLong.Text = ComUtilsG5.FixNull(SqlSelect.Item("entitylong"))
                'LiteralCityState.Text = ComUtilsG5.FixNull(SqlSelect.Item("citystate"))
                LiteralEntityLink.Text = "<a href=""http://" & ComUtilsG5.FixNull(SqlSelect.Item("url")) & """>" & ComUtilsG5.FixNull(SqlSelect.Item("entitylong")) & "&nbsp;-&nbsp;" & ComUtilsG5.FixNull(SqlSelect.Item("citystate")) & "</a>"
                LiteralDescription.Text = ComUtilsG5.FixNull(SqlSelect.Item("description"))
                LiteralRequirement.Text = ComUtilsG5.FixNull(SqlSelect.Item("requirement"))
                LiteralBenefit.Text = ComUtilsG5.FixNull(SqlSelect.Item("benefit"))
                LiteralAddress.Text = ComUtilsG5.FixNull(SqlSelect.Item("address"))
                HiddenFieldEntityId.Value = SqlSelect.Item("entityid")

                ReturnValue = True

            End If

            SqlSelect.Close()
            SqlComm.Dispose()

        Catch ex As Exception

            Dim temp As Boolean = False
            temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString & "<br><br>" & strSQL)

        Finally
            If Not (sqlconn Is Nothing) Then sqlconn.Close()
        End Try

        Return ReturnValue

    End Function

    Protected Sub SetError()

        DivJobNotFound.Visible = True
        DivMain.Visible = False
        DivMain2.Visible = False

    End Sub




    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''OLD IFRAME CODE'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




    Protected Function GetJobId() As Integer

        Dim i As Integer
        If Int32.TryParse(HiddenFieldJobId.Value, i) = False Then
            Return 0
        Else
            Return i
        End If

    End Function

    Protected Sub AlertJS(ByVal JSMessage As String)
        Dim theScript As String = "setTimeout('alert(\'" & JSMessage & "\');',100);"
        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "alert", theScript, True)
    End Sub

    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not Page.IsValid Then Exit Sub

        If IsJobOpen(GetJobId()) = False Then
            AlertJS("We\\\'re sorry, this job is now closed or unavailable.")
            Exit Sub
        End If

        If TempFileUpload() = False Then Exit Sub

        SaveJobAndFiles(GetJobId())

        Dim EntityAbbr As String = String.Empty
        Dim SendTo As String = String.Empty
        Dim EmailBody As String = String.Empty
        Dim EntityURL As String = String.Empty
        Dim PreHireURL As String = String.Empty
        Dim IncludeSurvey As Integer = 0

        If DoublePostEmail(GetJobId()) = False Then

            BuildEmailBody(GetJobId(), EntityAbbr, SendTo, EmailBody, IncludeSurvey)
            SendEmail(SendTo, EntityAbbr, EmailBody)

            If IncludeSurvey = 1 Then

                PreHireURL = GetPreHireURLByJobId(GetJobId())

                'for testing remove later
                'ApplicantName = ComUtilsG5.AllowedChars(ComUtilsG5.StripSQLKeyWords(Left(TextBoxName.Text.Trim(), 50)))
                'ApplicantEmail = ComUtilsG5.AllowedChars(ComUtilsG5.StripSQLKeyWords(Left(TextBoxEmail.Text.Trim(), 50)))

                If Not PreHireURL = "" Then

                    Dim AutoResponseText As String = JobUtils.BuildResponseHTML(ApplicantName, PreHireURL)
                    JobUtils.SendAutoResponseEmail(AutoResponseText, ApplicantEmail)

                    PreHireURL = "?url2=" & ComUtilsG5.URLEncode(ComUtilsG5.EncryptText(PreHireURL))
                End If

            End If

        End If

        'EntityURL = GetEntityURLByJobId(GetJobId())

        'If Not EntityURL = "" Then
        '    EntityURL = ComUtilsG5.URLEncode(ComUtilsG5.EncryptText(EntityURL))
        'End If

        'chrome will not work using Response.Redirect from iframe.
        'Response.Redirect(BaseURL & "/cus/displayjobsubmitmessage.aspx")

        'ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "redirect", "location.href='displayjobsubmitmessage.aspx?url=" & EntityURL & PreHireURL & "';", True)
        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "redirect", "location.href='displayjobmessage.aspx" & PreHireURL & "';", True)

    End Sub

    Protected Function TempFileUpload() As Boolean

        Dim ReturnValue As Boolean = False
        If Not Page.IsValid Then Return ReturnValue

        Dim UF As HttpFileCollection = Request.Files

        Dim TempFileName As String = JobUtils.CheckExistingFilesTemp(UF)
        Dim ExistingFileName As String = JobUtils.CheckExistingFiles(UF)
        Dim TotalFileLength As Integer = JobUtils.CheckFileLength(UF)

        If TotalFileLength > 0 Then
            AlertJS("Total file size exceeds 20 megs. (approx. " & TotalFileLength & ")")
        ElseIf Not TempFileName = "" Then
            AlertJS(TempFileName & " is an invalid filename. Please rename file.") 'already exists
        ElseIf Not ExistingFileName = "" Then
            AlertJS(ExistingFileName & " is an invalid filename. Please rename file.") 'already exists
        ElseIf JobUtils.CheckDuplicateFile(UF) = True Then
            AlertJS("You have 2 or more files with the same name. Please rename file.")
        ElseIf JobUtils.CheckExtensions(UF) = True Then
            AlertJS("Cannot upload .exe or executable files.")
        ElseIf JobUtils.CheckForBadChars(UF) = True Then
            AlertJS("Filename(s) contain invalid characters. Please rename file.")
        ElseIf JobUtils.CheckFileNameLength(UF) = True Then
            AlertJS("Filename(s) length too long (50 chars max). Please rename file.")
        Else

            JobUtils.CleanUp()
            UploadFiles()
            ReturnValue = True

        End If

        Return ReturnValue

    End Function

    Protected Sub UploadFiles()

        Dim UploadedFiles As HttpFileCollection = Request.Files
        Dim i As Integer = 0
        Dim FileDisplayName As String = String.Empty
        Dim FileName As String = String.Empty
        Dim FileExtensionName As String = String.Empty
        Dim ExtPos As Integer = 0

        Do Until i = UploadedFiles.Count

            Dim UserPostedFile As HttpPostedFile = UploadedFiles(i)

            Try
                If (UserPostedFile.ContentLength > 0) Then

                    FileName = Path.GetFileName(UserPostedFile.FileName)
                    FileDisplayName = FileName
                    ExtPos = FileName.LastIndexOf(".")

                    If ExtPos = -1 Then
                        FileExtensionName = ".noext"
                        FileName = FileName & "_" & Date.Now.ToString("MMddyyyyhmmss") & FileExtensionName
                    Else
                        FileExtensionName = FileName.Remove(0, ExtPos)
                        FileName = FileName.Remove(ExtPos, Len(FileExtensionName)) & "_" & Date.Now.ToString("MMddyyyyhmmss") & FileExtensionName
                    End If

                    UserPostedFile.SaveAs(TempFilePath & FileName)
                    HiddenFieldFileNames.Value += FileName & "###" & FileDisplayName & "|||"

                End If

            Catch ex As Exception

                Dim temp As Boolean = False
                temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString)

            End Try

            i += 1

        Loop

    End Sub

    Protected Sub SaveJobAndFiles(ByVal JobId As Integer)

        Dim sqlconn As New SqlConnection(connString)
        Dim strSQL As String = String.Empty

        ApplicantName = ComUtilsG5.AllowedChars(ComUtilsG5.StripSQLKeyWords(Left(TextBoxName.Text.Trim(), 50)))
        ApplicantEmail = ComUtilsG5.AllowedChars(ComUtilsG5.StripSQLKeyWords(Left(TextBoxEmail.Text.Trim(), 50)))
        ApplicantPhone = ComUtilsG5.AllowedChars(ComUtilsG5.StripSQLKeyWords(Left(TextBoxPhone.Text.Trim(), 50)))
        ApplicantMessage = ComUtilsG5.InsertSpace(ComUtilsG5.AllowedChars(ComUtilsG5.StripSQLKeyWords(Left(TextBoxMessage.Text.Trim(), 1900))), 40)

        strSQL = "insert into touchmark.dbo.jobapplicant"
        strSQL += "(jobId, entityId, position, description, requirement, lastUpdate, author, lastUpdateUserId,"
        strSQL += "applicantName, applicantEmail, applicantPhone, applicantMessage, fileName1, fileName2, userip)"

        strSQL += "select top (1) j.id, e.id,"

        strSQL += "case when j.jobtitleid > 0 then "
        strSQL += " isnull((select jlt.jobtitle from touchmark.dbo.joblookuptitle jlt where jlt.id=j.jobtitleid),'') else j.position end jobtitle,"

        strSQL += "j.description, j.requirement, j.lastUpdate, j.author, j.lastUpdateUserId,"
        strSQL += "@applicantname, @applicantemail, @applicantphone, @applicantmessage, @filename1, @filename2, @userip"

        strSQL += " from touchmark.dbo.jobs j left join touchmark.dbo.entity e on j.entityid=e.id"
        strSQL += " where e.active=1 and j.display=1 and j.expiredate > getdate()"
        strSQL += " and j.id=@jid"

        'may stop double post
        strSQL += " and not exists("
        strSQL += " select ja.id from touchmark.dbo.jobapplicant ja where ja.jobid=j.id"
        strSQL += " and ja.entityid=e.id"

        strSQL += " and lower(rtrim(ltrim(ja.position)))=case when j.jobtitleid > 0 then "
        strSQL += " (select lower(rtrim(ltrim(jlt.jobtitle))) from touchmark.dbo.joblookuptitle jlt where jlt.id=j.jobtitleid) else lower(rtrim(ltrim(j.position))) end"

        strSQL += " and lower(rtrim(ltrim(ja.applicantname)))=lower(rtrim(ltrim(@applicantname)))"
        strSQL += ")"
        strSQL += " SET @nextid = isnull(SCOPE_IDENTITY(),0)"

        Try
            sqlconn.Open()
            Dim SqlComm As SqlCommand = sqlconn.CreateCommand()
            SqlComm.Connection = sqlconn

            Dim temp As String() = Split(HiddenFieldFileNames.Value, "|||")
            Dim temp2 As String() = Nothing

            If temp.Length = 2 Then '1 file

                temp2 = Split(temp(0), "###")
                Filename1 = temp2(0)

            ElseIf temp.Length = 3 Then '2 files

                temp2 = Split(temp(0), "###")
                Filename1 = temp2(0)
                temp2 = Split(temp(1), "###")
                Filename2 = temp2(0)

            End If

            SqlComm.CommandText = strSQL

            With SqlComm.Parameters
                .Add(New SqlParameter("@jid", SqlDbType.Int)).Value = JobId
                .Add(New SqlParameter("@applicantname", SqlDbType.VarChar)).Value = ApplicantName
                .Add(New SqlParameter("@applicantemail", SqlDbType.VarChar)).Value = ApplicantEmail
                .Add(New SqlParameter("@applicantphone", SqlDbType.VarChar)).Value = ApplicantPhone
                .Add(New SqlParameter("@applicantmessage", SqlDbType.VarChar)).Value = ApplicantMessage
                .Add(New SqlParameter("@filename1", SqlDbType.VarChar)).Value = Filename1
                .Add(New SqlParameter("@filename2", SqlDbType.VarChar)).Value = Filename2
                .Add(New SqlParameter("@userip", SqlDbType.VarChar)).Value = HiddenFieldUserIP.Value
            End With

            Dim NextId As SqlParameter = New SqlParameter("@nextid", SqlDbType.Int)
            NextId.Direction = ParameterDirection.Output
            SqlComm.Parameters.Add(NextId)

            SqlComm.ExecuteNonQuery()
            Dim AddNew As Integer = NextId.Value
            SqlComm.Dispose()


            If AddNew > 0 Then

                If File.Exists(TempFilePath & Filename1) = True AndAlso Not File.Exists(ApplyFilePath & Filename1) Then
                    File.Move(TempFilePath & Filename1, ApplyFilePath & Filename1)
                End If

                If File.Exists(TempFilePath & Filename2) = True AndAlso Not File.Exists(ApplyFilePath & Filename2) Then
                    File.Move(TempFilePath & Filename2, ApplyFilePath & Filename2)
                End If

            End If


        Catch ex As Exception

            Dim temp As Boolean = False
            temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString & "<br><br>" & strSQL)

        Finally
            If Not (sqlconn Is Nothing) Then sqlconn.Close()
        End Try

    End Sub

    Protected Sub BuildEmailBody(ByVal JobId As Integer, ByRef EntityAbbr As String, _
                                 ByRef SendTo As String, ByRef EmailBody As String, ByRef IncludeSurvey As Integer)

        Dim sqlconn As New SqlConnection(connString)
        Dim strSQL As String = String.Empty
        Dim JobTitle As String = String.Empty
        Dim ReturnValue As String = ""

        strSQL = "select case when j.jobtitleid > 0 then "
        strSQL += " isnull((select jlt.jobtitle from touchmark.dbo.joblookuptitle jlt where jlt.id=j.jobtitleid),'') else j.position end jobtitle,"
        strSQL += "case when j.includesurvey=1 then 1 else 0 end includesurvey, e.entityabbr,"
        strSQL += "isnull((select ec.contactvalue from touchmark.dbo.entitycontact ec where ec.entityid=e.id and ec.isemail=1),'') email"
        strSQL += " from touchmark.dbo.jobs j left join touchmark.dbo.entity e on j.entityid=e.id where j.id=@jid"

        Try
            Dim SqlComm As New SqlCommand(strSQL, sqlconn)

            With SqlComm.Parameters
                .Add(New SqlParameter("@jid", SqlDbType.Int)).Value = JobId
            End With

            sqlconn.Open()

            Dim SqlSelect As SqlDataReader
            SqlSelect = SqlComm.ExecuteReader()

            If SqlSelect.HasRows Then

                SqlSelect.Read()

                JobTitle = ComUtilsG5.FixNull(SqlSelect.Item(0))
                IncludeSurvey = ComUtilsG5.FixNull(SqlSelect.Item(1))
                EntityAbbr = ComUtilsG5.FixNull(SqlSelect.Item(2))
                SendTo = ComUtilsG5.FixNull(SqlSelect.Item(3))

            End If

            SqlSelect.Close()
            SqlComm.Dispose()

        Catch ex As Exception

            Dim temp As Boolean = False
            temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString & "<br><br>" & strSQL)

        Finally
            If Not (sqlconn Is Nothing) Then sqlconn.Close()
        End Try


        EmailBody = "<b>Touchmark Job Application</b><br><br>"
        EmailBody += "<b>" & EntityAbbr & "</b><br><br>"

        EmailBody += "<table class=""ebody"" cellpadding=""4"" cellspacing=""0"" border=""0"">"

        EmailBody += "<tr><td><b>Job Title:</b></td><td>" & JobTitle & "</td></tr>"
        EmailBody += "<tr><td><b>Name:</b></td><td>" & ApplicantName & "</td></tr>"
        EmailBody += "<tr><td><b>Email:</b></td><td>" & ApplicantEmail & "</td></tr>"
        EmailBody += "<tr><td><b>Phone:</b></td><td>" & ApplicantPhone & "</td></tr>"
        EmailBody += "<tr><td valign=""top""><b>Message:</b></td><td valign=""top"">" & FormatText(ApplicantMessage) & "</td></tr>"
        EmailBody += "<tr><td colspan=""2""><br /><br /></td></tr>"

        If Not Filename1 = "" Then
            EmailBody += "<tr><td colspan=""2""><b>Attached File:</b></td></tr>"
            EmailBody += "<tr><td colspan=""2""><a href=" & BaseURL.Replace("/com", "/employee") & "/jobs/jobdownloadfile.aspx?fn=" & ComUtilsG5.URLEncode(ComUtilsG5.EncryptText(Filename1)) & ">" & Filename1 & "</a></td></tr>"
        End If

        If Not Filename2 = "" Then
            EmailBody += "<tr><td colspan=""2""><b>Attached File:</b></td></tr>"
            EmailBody += "<tr><td colspan=""2""><a href=" & BaseURL.Replace("/com", "/employee") & "/jobs/jobdownloadfile.aspx?fn=" & ComUtilsG5.URLEncode(ComUtilsG5.EncryptText(Filename2)) & ">" & Filename2 & "</a></td></tr>"
        End If

        EmailBody += "</table>"

    End Sub

    Protected Sub SendEmail(ByVal SendTo As String, ByVal CommunityName As String, ByVal EmailMessage As String)

        Try

            Dim EmailBody As New StringBuilder
            Dim mail As New MailMessage()
            mail.From = New MailAddress("Touchmark Auto Message <noreply@touchmark.com>")

            If ConfigurationManager.AppSettings("DeveloperBox") = "0" Then
                If ComUtilsG5.ValidateEmailAddress(SendTo) = True Then
                    mail.To.Add(SendTo)
                End If
            End If

            EmailBody.Append("<html><head></head>")
            EmailBody.Append("<style>.ebody{font-family:verdana,Sans-Serif,Serif; font-size:13px; text-align:left;} .divpad{margin-left:4px;}</style>")
            EmailBody.Append("<body><div class=""ebody"">")
            EmailBody.Append(EmailMessage)
            EmailBody.Append("</div>")
            EmailBody.Append("<br><br><br><br>Re:&nbsp;" & SendTo)
            EmailBody.Append("</body></html>")

            mail.Bcc.Add(ConfigurationManager.AppSettings("DeveloperEmail"))
            mail.Subject = CommunityName.ToUpper() & " - Job Application - " & ApplicantName
            mail.Body = EmailBody.ToString()
            mail.IsBodyHtml = True

            Dim smtp As New SmtpClient(ConfigurationManager.AppSettings("EmailServer"))
            smtp.Send(mail)
            smtp.Dispose()
            smtp = Nothing
            mail = Nothing

        Catch ex As Exception

            Dim ErrorMessage As String = (New System.Diagnostics.StackFrame()).GetMethod().Name & "<br><br>" & ex.Message & "<br><br>" & ex.ToString()
            Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "SendEmail Error", ErrorMessage)

        End Try

    End Sub

    Protected Function IsJobOpen(ByVal JobId As Integer) As Boolean

        Dim sqlconn As New SqlConnection(connString)

        Dim strSQL As String = "select case when isnull((select j.id from touchmark.dbo.jobs j"
        strSQL += " where j.display=1 and j.expiredate > getdate() and j.id=@jid),0)=0 then 0 else 1 end"

        Dim ReturnValue As Boolean = False

        Try
            Dim SqlComm As New SqlCommand(strSQL, sqlconn)
            sqlconn.Open()

            With SqlComm.Parameters
                .Add(New SqlParameter("@jid", SqlDbType.Int)).Value = JobId
            End With

            ReturnValue = Convert.ToBoolean(SqlComm.ExecuteScalar())
            SqlComm.Dispose()

        Catch ex As Exception

            Dim temp As Boolean = False
            temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString & "<br><br>" & strSQL)

        Finally
            If Not (sqlconn Is Nothing) Then sqlconn.Close()
        End Try

        Return ReturnValue

    End Function

    'Protected Function GetEntityURLByJobId(ByVal JobId As Integer) As String

    '    Dim sqlconn As New SqlConnection(connString)

    '    Dim strSQL As String = "select top (1) e.url"
    '    strSQL += " from touchmark.dbo.jobs j left join touchmark.dbo.entity e on j.entityid=e.id"
    '    strSQL += " where e.active=1 and e.phonedirectory=1 and j.display=1 and j.expiredate > getdate()"
    '    strSQL += " and j.id=@jid"

    '    Dim ReturnValue As String = ""

    '    Try
    '        Dim SqlComm As New SqlCommand(strSQL, sqlconn)
    '        sqlconn.Open()

    '        With SqlComm.Parameters
    '            .Add(New SqlParameter("@jid", SqlDbType.Int)).Value = JobId
    '        End With

    '        ReturnValue = SqlComm.ExecuteScalar().ToString()
    '        SqlComm.Dispose()

    '    Catch ex As Exception

    '        Dim temp As Boolean = False
    '        temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString & "<br><br>" & strSQL)

    '    Finally
    '        If Not (sqlconn Is Nothing) Then sqlconn.Close()
    '    End Try

    '    Return ReturnValue

    'End Function

    Protected Function GetPreHireURLByJobId(ByVal JobId As Integer) As String

        Dim sqlconn As New SqlConnection(connString)

        Dim strSQL As String = "select isnull((select top (1) jph.prehireurl"
        strSQL += " from touchmark.dbo.jobs j left join touchmark.dbo.joblookupprehireurl jph"
        strSQL += " on j.prehiretypeid=jph.prehiretypeid"
        strSQL += " where j.display=1 and j.[expiredate] > getdate() and j.id=@jid"
        strSQL += " and jph.entityid=case"
        strSQL += " when j.entityid=58 then 12" 'tafvhh
        strSQL += " when j.entityid=61 then 12" 'tafvhfc
        strSQL += " when j.entityid=16 then 9" 'tmlvhh
        strSQL += " when j.entityid=90 then 13" 'tfndhfc
        strSQL += " when j.entityid=25 then 19" 'tshhh
        strSQL += " when j.entityid=91 then 19" 'tshhfc
        strSQL += " when j.entityid=69 then 21" 'tbndhfc
        strSQL += " when j.entityid=70 then 22" 'tborhc
        strSQL += " when j.entityid=89 then 24" 'teabhsn
        strSQL += " else j.entityid end),'')"

        Dim ReturnValue As String = ""

        Try
            Dim SqlComm As New SqlCommand(strSQL, sqlconn)
            sqlconn.Open()

            With SqlComm.Parameters
                .Add(New SqlParameter("@jid", SqlDbType.Int)).Value = JobId
            End With

            ReturnValue = SqlComm.ExecuteScalar().ToString()
            SqlComm.Dispose()

        Catch ex As Exception

            Dim temp As Boolean = False
            temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString & "<br><br>" & strSQL)

        Finally
            If Not (sqlconn Is Nothing) Then sqlconn.Close()
        End Try

        Return ReturnValue

    End Function

    Protected Function DoublePostEmail(ByVal JobId As Integer) As Boolean

        '10 minute wait, double post or back/forward button
        Dim sqlconn As New SqlConnection(connString)

        Dim strSQL As String = "select case when isnull((select top (1) id from touchmark.dbo.JobApplicant"
        strSQL += " where jobid=@jid and applicantname=@applicantname and applicantEmail=@applicantemail"
        strSQL += " and userip=@userip and datediff(ss,createDate,getdate()) between 1 and 600"
        strSQL += " ),0)=0 then 0 else 1 end"

        Dim ReturnValue As Boolean = False

        Try
            Dim SqlComm As New SqlCommand(strSQL, sqlconn)
            sqlconn.Open()

            With SqlComm.Parameters
                .Add(New SqlParameter("@jid", SqlDbType.Int)).Value = JobId
                .Add(New SqlParameter("@applicantname", SqlDbType.VarChar)).Value = ApplicantName
                .Add(New SqlParameter("@applicantemail", SqlDbType.VarChar)).Value = ApplicantEmail
                .Add(New SqlParameter("@userip", SqlDbType.VarChar)).Value = HiddenFieldUserIP.Value
            End With

            ReturnValue = Convert.ToBoolean(SqlComm.ExecuteScalar())
            SqlComm.Dispose()

        Catch ex As Exception

            Dim temp As Boolean = False
            temp = ComUtilsG5.SendMailToWebmaster("COM Site Error", Request.ServerVariables("SCRIPT_NAME"), ex.Message & "<br><br>" & ex.ToString & "<br><br>" & strSQL)

        Finally
            If Not (sqlconn Is Nothing) Then sqlconn.Close()
        End Try

        Return ReturnValue

    End Function

    Protected Function FormatText(ByVal value As Object) As String
        If ComUtilsG5.FixNull(value) = "" Then
            Return ""
        Else
            Return value.ToString.Replace(Chr(13), "<br />")
        End If
    End Function

End Class
