Imports Microsoft.VisualBasic
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient

Namespace Touchmark

    Public Class DatabaseUtilsG5

        Private Shared connString As String = Convert.ToString(ConfigurationManager.ConnectionStrings("Touchmark").ConnectionString)

        Public Shared Function IsIP(ByVal IP As String) As Boolean

            Dim sqlconn As New SqlConnection(connString)
            Dim strSQL As String = String.Empty
            Dim ReturnValue As Boolean = False

            strSQL = "select case when (select count(*) from workonly.dbo.bannedip where ip=@ip) > 0 then 1 else 0 end 'IsIP'"

            Try
                Dim SqlComm As New SqlCommand(strSQL, sqlconn)

                With SqlComm.Parameters
                    .Add(New SqlParameter("@ip", SqlDbType.VarChar)).Value = IP.Trim()
                End With

                sqlconn.Open()
                ReturnValue = Convert.ToBoolean(SqlComm.ExecuteScalar())
                SqlComm.Dispose()

            Catch ex As Exception

                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "Error from IP check", "IP: " & IP)

            Finally
                If Not (sqlconn Is Nothing) Then sqlconn.Close()
            End Try

            Return ReturnValue
        End Function

        Public Shared Sub UpdateLastHit(ByVal IP As String)

            Dim sqlconn As New SqlConnection(connString)
            Dim strSQL As String = String.Empty

            strSQL = "update workonly.dbo.bannedip set lastupdate=getdate() where ip=@ip"

            Try
                Dim SqlComm As New SqlCommand(strSQL, sqlconn)

                With SqlComm.Parameters
                    .Add(New SqlParameter("@ip", SqlDbType.VarChar)).Value = IP.Trim()
                End With

                sqlconn.Open()
                SqlComm.ExecuteScalar()
                SqlComm.Dispose()

            Catch ex As Exception

                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "Error from IP Update last Hit", "IP: " & IP)

            Finally
                If Not (sqlconn Is Nothing) Then sqlconn.Close()
            End Try

        End Sub

        Public Shared Function BanByCode(ByVal IP As String, ByVal CountryCode As String) As Boolean

            Dim sqlconn As New SqlConnection(connString)
            Dim ReturnValue As Boolean = False

            Try
                Dim SqlComm As New SqlCommand("workonly.dbo.isblock", sqlconn)
                SqlComm.CommandType = CommandType.StoredProcedure
                SqlComm.Parameters.Add(New SqlParameter("@ip", IP))
                SqlComm.Parameters.Add(New SqlParameter("@blockcode", CountryCode))

                Dim RV As SqlParameter = New SqlParameter("@returnvalue", SqlDbType.Int)
                RV.Direction = ParameterDirection.Output
                SqlComm.Parameters.Add(RV)

                sqlconn.Open()
                SqlComm.ExecuteNonQuery()

                ReturnValue = Convert.ToBoolean(RV.Value)
                SqlComm.Dispose()

            Catch ex As Exception

                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", (New System.Diagnostics.StackFrame()).GetMethod().Name, "IP: " & IP & vbCrLf & ex.Message & "<br><br>" & ex.ToString())
                Return ReturnValue

            Finally
                If Not (sqlconn Is Nothing) Then sqlconn.Close()
            End Try

            Return ReturnValue
        End Function

        Public Shared Function GetJobLastUpdate() As Boolean

            'return true if last update > 6 months ago
            Dim sqlconn As New SqlConnection(connString)
            Dim strSQL As String = String.Empty
            Dim ReturnValue As Boolean = False

            strSQL = "select case when datediff(mm, (select top(1) lastupdate from touchmark.dbo.JobFileCleanUpLastDate order by 1 desc),getdate())"
            strSQL += " > 0 then 1 else 0 end"

            Try
                Dim SqlComm As New SqlCommand(strSQL, sqlconn)
                sqlconn.Open()
                ReturnValue = Convert.ToBoolean(SqlComm.ExecuteScalar())
                SqlComm.Dispose()

            Catch ex As Exception

                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "Error from GetJobLastUpdate", strSQL)

            Finally
                If Not (sqlconn Is Nothing) Then sqlconn.Close()
            End Try

            Return ReturnValue
        End Function

        Public Shared Sub SetJobLastUpdate()

            Dim sqlconn As New SqlConnection(connString)
            Dim strSQL As String = String.Empty

            strSQL = "update touchmark.dbo.JobFileCleanUpLastDate set lastupdate=convert(varchar(2),month(dateadd(mm,1,getdate()))) + '/01/' + convert(varchar(4),year(dateadd(mm,1,getdate())))"

            Try
                Dim SqlComm As New SqlCommand(strSQL, sqlconn)
                sqlconn.Open()
                SqlComm.ExecuteScalar()
                SqlComm.Dispose()

            Catch ex As Exception

                Dim temp As Boolean = ComUtilsG5.SendMailToWebmaster("COM Site Error", "Error from SetJobLastUpdate", strSQL)

            Finally
                If Not (sqlconn Is Nothing) Then sqlconn.Close()
            End Try

        End Sub
        
    End Class

End Namespace