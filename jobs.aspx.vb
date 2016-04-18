Imports System.Data
Imports System.Data.SqlClient
Imports Touchmark

Partial Class jobs
    Inherits System.Web.UI.Page

    Dim connString As String = JobUtils.connString
    Dim BaseURL As String = JobUtils.BaseURL

    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
		
        If Not Page.IsPostBack = True Then

            Dim ids As String = String.Empty

            For Each Item As Object In Request.Form
                If Item.ToString().ToLower() = "ids" Then
                    ids = Request.Form(Item).ToString()
                    LoadForm(ids)
                End If
            Next

            If ids = "" Then
                GoBack()
            End If

        End If

    End Sub

    Protected Sub LoadForm(ByVal ids As String)

        Dim DT As DataTable = LoadJobLinks(ids)
        Dim EntityId As Integer = 0
        Dim EntityLong As String = String.Empty
        Dim URL As String = String.Empty
        Dim City As String = String.Empty
        Dim State As String = String.Empty
        Dim RowNum As Integer = 0
        Dim JID As Integer = 0
        Dim Position As String = String.Empty

        Dim DupeEntityId As Integer = 0
        Dim temp As String = String.Empty

        If IsNothing(DT) = False AndAlso DT.Rows.Count > 0 Then

            For k As Integer = 0 To DT.Rows.Count - 1

                'entityid, city, [state], entitylong, url, rownum, jid, position
                EntityId = Convert.ToInt32(DT.Rows(k).Item(0).ToString())
                EntityLong = DT.Rows(k).Item(3).ToString()
                URL = DT.Rows(k).Item(4).ToString()
                City = DT.Rows(k).Item(1).ToString()
                State = DT.Rows(k).Item(2).ToString()
                RowNum = Convert.ToInt32(DT.Rows(k).Item(5).ToString())
                JID = Convert.ToInt32(DT.Rows(k).Item(6).ToString())
                Position = DT.Rows(k).Item(7).ToString()

                If Not EntityId = DupeEntityId Then

                    If Not k = 0 Then 'not first pass
                        temp += "</ul>"
                    End If

                    temp += "<h3><a href=""http://" & URL & """>" & EntityLong & " " & City & ", " & State & "</a>"

                    If JID = 0 Then
                        temp += "</h3>"
                    Else
                        temp += "&nbsp;&mdash;&nbsp;Current job opportunities listed below.</h3><p>Click on the job title for details.</p>"
                    End If

                    temp += "<ul>"
                End If

                '" & RowNum & ".&nbsp;
                If JID = 0 Then 'position returns no job message
                    temp += "<li>" & Position & "</li>"
                Else
                    temp += "<li><a href =""" & BaseURL & "/displayjob.aspx?jid=" & ComUtilsG5.URLEncode(ComUtilsG5.EncryptText(JID)) & """ target=""_blank"">" & Position & "</a></li>"
                End If

                DupeEntityId = EntityId

                If k = DT.Rows.Count - 1 Then 'last row
                    temp += "</ul>"
                End If

            Next

        End If

        LiteralOut.Text = temp

    End Sub

    Protected Function LoadJobLinks(ByVal s As String) As DataTable

        Dim sqlconn As New SqlConnection(connString)
        Dim ReturnValue As DataTable = Nothing

        Try
            Dim SqlComm As New SqlCommand("touchmark.dbo.joblisting", sqlconn)
            SqlComm.CommandType = CommandType.StoredProcedure

            With SqlComm.Parameters
                .Add(New SqlParameter("@ids", SqlDbType.VarChar)).Value = s
            End With

            sqlconn.Open()

            Dim SDA As New SqlDataAdapter(SqlComm)
            Dim DT As New DataTable()

            SDA.Fill(DT)

            SqlComm.Dispose()
            sqlconn.Close()

            ReturnValue = DT

        Catch ex As Exception

            Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster(Request.ServerVariables("SCRIPT_NAME"), ex.Message, "<br><br>" & ex.ToString())

        Finally
            If Not (sqlconn Is Nothing) Then sqlconn.Close()
        End Try

        Return ReturnValue
    End Function

    Protected Sub ButtonBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GoBack()
    End Sub

    Protected Sub GoBack()
        Response.Redirect("jobsmain.aspx")
    End Sub

End Class
