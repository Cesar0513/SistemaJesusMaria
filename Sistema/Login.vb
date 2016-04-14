Public Class Login

#Region "VARIABLES GLOBALES"
    Dim objSQL As clsSQLClient
#End Region

#Region "SP"
    Dim sp_ValidaUsuarioLogin As String = "Seguridad.InicioSesion"
#End Region

#Region "SUB FUNCTION"

    Public Sub ValidaUsuario(ByVal pwd As String)
        Dim dtFolio As DataTable = Nothing
        Dim dtDatos As DataTable = Nothing
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_ValidaUsuarioLogin, pwd)
            If dtFolio.Rows.Count <> 0 Then
                DatosSession.DatosSession(CInt(dtFolio.Rows(0).Item(0).ToString()), CStr(dtFolio.Rows(0).Item(1).ToString()), CStr(dtFolio.Rows(0).Item(2).ToString()))
                dtDatos = ObtenerValores()

                If dtDatos.Rows.Count <> 0 Then
                    Dim frmPrincipal As New Principal
                    frmPrincipal.Show()
                    Me.Hide()
                End If
            Else
                MsgBox("No se encontro usuario con esa contraseña, Intente nuevamente", MsgBoxStyle.Critical, "AVISO!")
                txtPassword.Clear()
            End If
        Catch ex As Exception
            MsgBox("Error al Verificar Usuario", MsgBoxStyle.Information, "AVISO")
        End Try
    End Sub

#End Region

#Region "EVENTOS CONTROLADORES"

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            If String.IsNullOrEmpty(txtPassword.Text) Then
                MsgBox("Por favor ingrese su contraseña y al terminar presione ENTER", MsgBoxStyle.Exclamation, "AVISO")
            Else
                ValidaUsuario(txtPassword.Text)
            End If
        End If
    End Sub

#End Region


End Class
