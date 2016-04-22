Imports System.Configuration
Imports System.Data


Public Class Login

#Region "SUB FUNCTION"


    Public Sub ValidaUsuario(ByVal pwd As String)
        pwd = Trim(pwd)
        MsgBox("--" & pwd & "--")
        Dim objSQL As New clsAdministrador()
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = objSQL.VarificaAdminInicioSesion(pwd)
            If dtFolio.Rows.Count <> 0 Then
                DatosSession.DatosSession(CInt(dtFolio.Rows(0).Item(0).ToString()), CStr(dtFolio.Rows(0).Item(1).ToString()), CStr(dtFolio.Rows(0).Item(2).ToString()))
                If DatosSession.IdAdmin <> 0 Then
                    Principal.Show()
                    Me.Close()
                End If
            Else
                MsgBox("No se encontro usuario con esa contraseña, Intente nuevamente", MsgBoxStyle.Critical, "AVISO!")
                txtPassword.Text = ""
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Error al Verificar Usuario: " & ex.Message, MsgBoxStyle.Information, "AVISO")
            txtPassword.Text = ""
        End Try
    End Sub

#End Region

#Region "EVENTOS CONTROLADORES"

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then
            If String.IsNullOrEmpty(txtPassword.Text) Then
                MsgBox("Por favor ingrese su contraseña y al terminar presione ENTER", MsgBoxStyle.Exclamation, "AVISO")
            Else
                Dim pwd As String = Nothing
                pwd = Trim(Val(txtPassword.Text))
                ValidaUsuario(pwd)
            End If
        End If
    End Sub

#End Region

End Class
