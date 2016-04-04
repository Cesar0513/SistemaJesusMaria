Imports MySql.Data
Imports MySql.Data.MySqlClient

Public Class Login

#Region "VARIABLES GLOBALES"
    Dim clsMysql As New Conexion
#End Region

#Region "SP"
    Dim sp_ValidaUsuarioLogin As String = "Seguridad.sp_ValidaUsuarioLogin"
#End Region

#Region "SUB FUNCTION"

    Public Sub ValidaUsuario(ByVal pwd As String)
        Me.Visible = False
        Principal.Show()
        'Try
        '    Dim dt As DataSet = clsMysql.EjecutaStore(sp_ValidaUsuarioLogin, pwd)
        'Catch ex As Exception
        '    MsgBox("Error al Verificar Usuario", MsgBoxStyle.Information, "AVISO")
        'End Try
    End Sub

#End Region

#Region "EVENTOS CONTROLADORES"

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            If String.IsNullOrEmpty(txtPassword.Text) Then
                MsgBox("Por favor ingrese su contraseña y presione ENTER", MsgBoxStyle.Exclamation, "AVISO")
            Else
                ValidaUsuario(txtPassword.Text)
            End If
        End If
    End Sub

#End Region
End Class
