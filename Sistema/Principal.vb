Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Resources
Imports System.Configuration

Public Class Principal

#Region "VARIABLES GLOBALES"
    Dim objSQL As clsSQLClient
    Dim RowIdAdmin As Integer = Nothing
    Dim RowIdUsuario As Integer = Nothing
    Dim RowIdRed As Integer = Nothing
    Dim AdminSistema As Integer = Nothing
#End Region

#Region "SP"

    Private sp_CargarAdministradores As String = "Seguridad.sp_CargarAdministradores"
    Private sp_CargarUsuarios As String = "Seguridad.sp_CargarUsuarios"

#End Region

#Region "LOAD CLOING FORMULARIO"

    Private Sub Principal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ObtenerParametrosSession()
        IniciaServiciosXampp()
    End Sub

    Private Sub Principal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DatosSession.DatosSession(0, "", "")
    End Sub

#End Region

#Region "SUB FUNCTION"

    Public Sub ObtenerParametrosSession()
        If DatosSession.IdAdmin = 0 Then
            Login.Show()
            Me.Close()
        End If
        AdminSistema = DatosSession.IdAdmin
        lblUsuario.Text = DatosSession.NomAdmin
        lblPerfil.Text = DatosSession.PerfilAdmin
    End Sub

    Public Sub IniciaServiciosXampp()
        'Try
        '    Process.Start(ConfigurationManager.AppSettings(ConfigurationManager.AppSettings("strRutaXampp").ToString).ToString)
        'Catch ex As Exception
        '    MsgBox("Error al abrir xampp")
        'End Try
    End Sub

    Public Sub CrearRespaldoDeBase()
        Dim nombre As String
        Dim datos As String
        Dim hora As String
        Dim datof As String
        nombre = "SistemaAgua"
        datos = (Date.Today.Year.ToString & "-" & Date.Today.Month.ToString & "-" & Date.Today.Day.ToString)
        hora = Date.Now.Hour.ToString & "-" & Date.Now.Minute.ToString
        datof = nombre & datos & "--" & hora
        Try
            Process.Start("C:\Program Files (x86)\MySQL\MySQL Server 5.1\bin\mysqldump.exe", " -u root -p root sistemaagua -r ""I:\Backup\respaldos\" & datof & ".sql""")
            MsgBox("El Respaldo creado con Exito" & datos)
        Catch ex As Exception
            MsgBox("Error no se pudo crear el Respaldo")
        End Try
    End Sub

#Region "ADMINISTRADORES"

    Public Sub CargarTiposAdministradores()
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, 1, "")

            If dtFolio.Rows.Count = 0 Then
                MsgBox("No se cargaron los tipos de Administradores", MsgBoxStyle.Information, "Aviso!")
            Else
                For i As Integer = 0 To dtFolio.Rows.Count Step 1
                    cmbTipoAdmin.Controls.Add(dtFolio.Rows(i).Item(0))
                Next
            End If
        Catch ex As Exception
            MsgBox("Problema al Cargar Tipo de Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub CargarAdministradores()
        Dim filtro As String = ""
        Dim dtFolio As DataTable = Nothing
        If String.IsNullOrEmpty(txtFiltro.Text) Then
            filtro = ""
        Else
            Try
                dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, 0, filtro)

                If dtFolio.Rows.Count = 0 Then
                    MsgBox("No hay registros", MsgBoxStyle.Information, "Aviso!")
                    Me.dtGridAdmin.DataSource = dtFolio
                Else
                    Me.dtGridAdmin.DataSource = dtFolio
                    Me.dtGridAdmin.CurrentRow.Selected = False
                End If
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Public Sub LimpiarCamposAdmin()
        txtNombre.Text = ""
        txtPaterno.Text = ""
        txtMaterno.Text = ""
        txtPwd.Text = ""
        txtPwd1.Text = ""
    End Sub

#End Region

#Region "USUARIOS"

    Public Sub CargarUsuarios()

    End Sub

    Public Sub LimpiarCamposUsuarios()
        txtNombre.Text = ""
        txtPaterno.Text = ""
        txtMaterno.Text = ""
        txtPwd.Text = ""
        txtPwd1.Text = ""
    End Sub

#End Region

#Region "REDES"

    Public Sub LimpiarCamposRedes()
        txtNombre.Text = ""
        txtPaterno.Text = ""
        txtMaterno.Text = ""
        txtPwd.Text = ""
        txtPwd1.Text = ""
    End Sub

#End Region


#End Region

#Region "EVENTOS CONTROLADORES"

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        CrearRespaldoDeBase()
    End Sub

    Private Sub btnPagar_Click(sender As Object, e As EventArgs) Handles btnPagar.Click
        Dim frmPagos As New Pago1
        frmPagos.ShowDialog()
    End Sub

#Region "ADMINISTRADORES"

    Private Sub dtGridAdmin_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtGridAdmin.CellContentClick
        Dim clave As Integer = Nothing
        If e.ColumnIndex = 0 Then
            clave = CInt(dtGridAdmin.Rows(e.RowIndex).Cells(1).Value)
            RowIdAdmin = CInt(clave)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        LimpiarCamposAdmin()
        dtGridAdmin.Enabled = False
        btnEliminar.Visible = False
        btnModificar.Visible = False
        btnAdd.Visible = False
        btnNuevo.Visible = True
        btnCancelar.Visible = True
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarCamposAdmin()
        dtGridAdmin.Enabled = True
        btnEliminar.Visible = True
        btnModificar.Visible = True
        btnAdd.Visible = True
        btnNuevo.Visible = False
        btnCancelar.Visible = False
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If MessageBox.Show("Continuar para eliminar al Administrador", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then

        End If
    End Sub

    Private Sub txtFiltro_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFiltro.KeyDown
        CargarTiposAdministradores()
        CargarAdministradores()
    End Sub
#End Region

#Region "USUARIOS"

    Private Sub btnAddUsu_Click(sender As Object, e As EventArgs) Handles btnAddUsu.Click
        LimpiarCamposUsuarios()
        dtGridUsuarios.Enabled = False
        btnEliminarUsu.Visible = False
        btnModUsu.Visible = False
        btnAddUsu.Visible = False
        btnAddUsu.Visible = True
        btnCancelarUsu.Visible = True
    End Sub

    Private Sub dtGridUsuarios_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtGridUsuarios.CellContentClick
        Dim clave As Integer = Nothing
        If e.ColumnIndex = 0 Then
            clave = CInt(dtGridAdmin.Rows(e.RowIndex).Cells(1).Value)
            IdUsuario = CInt(clave)
        End If
    End Sub

    Private Sub btnCancelarUsu_Click(sender As Object, e As EventArgs) Handles btnCancelarUsu.Click
        LimpiarCamposAdmin()
        dtGridUsuarios.Enabled = True
        btnEliminarUsu.Visible = True
        btnModUsu.Visible = True
        btnAddUsu.Visible = True
        btnSaveUsu.Visible = True
        btnCancelarUsu.Visible = False
    End Sub

    Private Sub btnEliminarUsu_Click(sender As Object, e As EventArgs) Handles btnEliminarUsu.Click
        If MessageBox.Show("Continuar para eliminar al usuario", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then

        End If
    End Sub

    Private Sub txtFiltroUsu_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFiltroUsu.KeyDown
        CargarUsuarios()
    End Sub

#End Region

#Region "REDES"

#End Region

#End Region

#Region "SUB LIMPIAR TabPage"

    Public Sub LimpiarTabAdmin()
        txtFiltro.Clear()
        dtGridAdmin.DataSource = Nothing
        txtNombre.Clear()
        txtPaterno.Clear()
        txtMaterno.Clear()
        cmbTipoAdmin.SelectedItem = -1
        txtPwd.Clear()
        txtPwd1.Clear()
    End Sub

    Public Sub LimpiarTabUsuarios()
        txtFiltroUsu.Clear()
        dtGridUsuarios.DataSource = Nothing
        txtNombreUsu.Clear()
        txtPaternoUsu.Clear()
        txtMaternoUsu.Clear()
        FechaNacUsu.Value = Now.Date
        cmbRedUsu.SelectedItem = -1
        txtPrecioTomaUsu.Clear()
        txtRefUsu.Clear()
    End Sub

#End Region

#Region "ENTER TAB'S"

    Private Sub TabAdmin_Enter(sender As Object, e As EventArgs) Handles TabAdmin.Enter
        CargarTiposAdministradores()
        LimpiarTabAdmin()
        RowIdAdmin = 0
    End Sub

    Private Sub TabUsuarios_Enter(sender As Object, e As EventArgs) Handles TabUsuarios.Enter, TabUsuarios.LostFocus
        LimpiarTabUsuarios()
        RowIdAdmin = 0
    End Sub

#End Region

#Region "LOSTFOCUS TAB'S"

    Private Sub TabAdmin_LostFocus(sender As Object, e As EventArgs) Handles TabAdmin.LostFocus
        LimpiarTabAdmin()
    End Sub

#End Region

    
End Class