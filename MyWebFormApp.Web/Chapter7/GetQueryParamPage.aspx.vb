Public Class GetQueryParamPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblKetrangan.Text = "Param: " & Request.QueryString("id").ToString
    End Sub

End Class