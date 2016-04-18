Imports Microsoft.VisualBasic

Namespace Touchmark

    Public Class CustomButton : Inherits System.Web.UI.WebControls.Button

        '1) IsDisable will add an identical, display:none, disabled button. If there's no
        '   confirm message, it will append javascript so that validators will work. This
        '   way the original button DOES post. So on client click, dummy button will show
        '   as disabled, original button is hidden. Acts like original button is disabled,
        '   but it is not, and page accepts the post event.
        '3) ConfirmMessage just adds a return confirm to client click.

        Private DivName As String = "Div_"
        Private _IsDisable As Boolean = False

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

            If _IsDisable = True Then
                writer.Write("<input type=""button"" id=""" & DivName & Me.ID & """ disabled=disabled class=""" & Me.CssClass & """ style=""display:none;"" value=""" & Me.Text & """ />")
            End If

            MyBase.Render(writer)
        End Sub

        Public Overrides Property OnClientClick() As String
            Get

                Dim NewValue As String = String.Empty
                Dim sb As New StringBuilder
                Dim DivString As String = "var buttondiv=document.getElementById('" & DivName & Me.ID & "'); if (buttondiv) {buttondiv.style.display=''; this.style.display='none';}"

                sb.Append("if (typeof(Page_Validators)=='undefined') {")
                sb.Append(DivString)
                sb.Append("} else {")
                sb.Append("if (typeof(Page_ClientValidate)=='function') {")

                sb.Append("if (Page_ClientValidate()) {")
                sb.Append(DivString)
                sb.Append("}}};")

                If _IsDisable = True Then
                    NewValue = "javascript:" & MyBase.OnClientClick & sb.ToString()
                Else
                    NewValue = "javascript:" & sb.ToString()
                End If

                If Not NewValue = "" Then Me.OnClientClick = NewValue

                Return MyBase.OnClientClick
            End Get
            Set(ByVal value As String)
                MyBase.OnClientClick = value
            End Set
        End Property

        Public Property IsDisable() As Boolean
            Get
                Return _IsDisable
            End Get
            Set(ByVal value As Boolean)
                _IsDisable = value
            End Set
        End Property

    End Class

End Namespace