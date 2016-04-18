Imports System.Data
Imports System.Data.SqlClient
Imports Touchmark

Partial Class jobsmain
    Inherits System.Web.UI.Page

    Dim connString As String = JobUtils.connString
    Dim BaseURL As String = JobUtils.BaseURL

    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack = True Then
            LoadLinks()
        End If

    End Sub

    Protected Sub SqlDataSource_Init(ByVal sender As Object, ByVal e As System.EventArgs)
        sender.ConnectionString = connString
    End Sub

    Protected Sub LoadLinks()

        Dim strSQL As String = "select e.id,e.city,e.entitylong,e.url,"
        strSQL += "isnull((select lus.[state] from sales.dbo.lookupstate lus where lower(rtrim(ltrim(lus.stateabbr)))=lower(rtrim(ltrim(e.[state])))),'') 'state'"
        strSQL += " from touchmark.dbo.entity e where ((e.active=1 and e.isretirementcommunity=1) or (e.id=1))"
        strSQL += " and not e.id in(49,57,72,76) order by case when e.id=24 then 'z' else e.[state] end, e.city"
        '(TPAZ) (TPOR) (TGT) (TH)

        SqlDataSource3.SelectCommand = strSQL
        Repeater3.DataBind()
    End Sub

    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ids As String = String.Empty

        For i As Integer = 0 To Repeater3.Items.Count - 1
            Dim CheckBoxEntity As CheckBox = Repeater3.Items(i).FindControl("CheckBoxEntity")
            Dim HiddenFieldEntityId As HiddenField = Repeater3.Items(i).FindControl("HiddenFieldEntityId")

            If CheckBoxEntity.Checked = True Then
                ids += HiddenFieldEntityId.Value & ","
            End If
        Next

        If ids = "" Then
            Exit Sub
        Else
            ids = Left(ids, ids.Length - 1)
        End If

        Dim s As String = ""

        s += "var f=document.createElement('form');"
        s += "f.setAttribute('method', 'post');"
        s += "f.setAttribute('action', 'jobs.aspx');"

        s += "var hf=document.createElement('input');"
        s += "hf.setAttribute('type', 'hidden');"
        s += "hf.setAttribute('name', 'ids');"
        s += "hf.setAttribute('value', '" & ids & "');"
        s += "f.appendChild(hf);"

        s += "document.body.appendChild(f);"
        s += "f.submit();"

        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "post", s, True)
    End Sub

End Class
