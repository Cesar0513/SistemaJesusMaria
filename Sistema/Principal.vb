Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Resources
Imports System.Configuration

Public Class Principal

#Region "VARIABLES GLOBALES"
    Dim objSQL As New clsSQLClient
    Dim RowIdAdmin As Integer = Nothing
    Dim RowIdUsuario As Integer = Nothing
    Dim RowIdRed As Integer = Nothing
    Dim AdminSistema As Integer = Nothing
#End Region

#Region "SP"

    Private sp_CargarAdministradores As String = "Seguridad.sp_CargarAdministradores"
    Private sp_InsertaActualizaEliminaAdmin As String = "Seguridad.sp_InsertaActualizaEliminaAdmin"
    Private sp_CargarUsuarios As String = "Seguridad.sp_CargarUsuarios"
    Private sp_InsertaActualizaEliminaUsuario As String = "Seguridad.sp_InsertaActualizaEliminaUsuario"

#End Region

#Region "LOAD CLOSING FORMULARIO"

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
        cmbTipoAdmin.Items.Add("Administrador")
        cmbTipoAdmin.Items.Add("Cobrador")
        'Dim dtFolio As DataTable = Nothing
        'Try
        '    dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, 1, "")

        '    If dtFolio.Rows.Count = 0 Then
        '        MsgBox("No se cargaron los tipos de Administradores", MsgBoxStyle.Information, "Aviso!")
        '    Else
        '        For i As Integer = 0 To dtFolio.Rows.Count Step 1
        '            cmbTipoAdmin.Controls.Add(dtFolio.Rows(i).Item(0))
        '        Next
        '    End If
        'Catch ex As Exception
        '    MsgBox("Problema al Cargar Tipo de Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
        'End Try
    End Sub

    Public Sub CargarAdministradores()
        Dim dtFolio As New DataTable

        dtFolio.Columns.AddRange(New DataColumn() {New DataColumn("IDADMIN", GetType(Integer)), _
                                                   New DataColumn("NOMBRE", GetType(String)), _
                                                   New DataColumn("APELLIDOS", GetType(String)), _
                                                   New DataColumn("APEPATERNO", GetType(String)), _
                                                   New DataColumn("APEMATERNO", GetType(String)), _
                                                   New DataColumn("IDPERFIL", GetType(Integer)), _
                                                   New DataColumn("PERFIL", GetType(String)), _
                                                   New DataColumn("ESTATUS", GetType(String))})

        dtFolio.Rows.Add(1, "Pepe", "Martinez Osorio", "Martinez", "Osorio", 1, "Administrador", "Activo")

        Me.dtGridAdmin.DataSource = dtFolio
        Me.dtGridAdmin.CurrentRow.Selected = False


        'Dim filtro As String = ""
        'Dim dtFolio As DataTable = Nothing
        'If String.IsNullOrEmpty(txtFiltro.Text) Then
        '    filtro = ""
        'Else
        '    Try
        '        dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, 0, filtro)

        '        If dtFolio.Rows.Count = 0 Then
        '            MsgBox("No hay registros", MsgBoxStyle.Information, "Aviso!")
        '            Me.dtGridAdmin.DataSource = dtFolio
        '        Else
        '            Me.dtGridAdmin.DataSource = dtFolio
        '            Me.dtGridAdmin.CurrentRow.Selected = False
        '        End If
        '    Catch ex As Exception
        '        MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        '    End Try
        'End If
    End Sub

    Public Function VerificaCambioContraseña()
        Dim retorna As Integer = Nothing

        If String.IsNullOrEmpty(txtPwd.Text) And String.IsNullOrEmpty(txtPwd1.Text) Then
            retorna = 1
        ElseIf String.IsNullOrEmpty(txtPwd.Text) Or String.IsNullOrEmpty(txtPwd1.Text) Then
            MsgBox("Los campos de Contraseña NO son Iguales", MsgBoxStyle.Information, "AVISO!")
            retorna = 0
        ElseIf txtPwd.Text = txtPwd1.Text Then
            retorna = 1
        End If
        Return retorna
    End Function

    Public Sub LimpiarCamposAdmin()
        txtNombre.Text = ""
        txtPaterno.Text = ""
        txtMaterno.Text = ""
        txtPwd.Text = ""
        txtPwd1.Text = ""
    End Sub

    Public Sub MostrarDatosAdmin(ByVal nombre As String, ByVal apepaterno As String, ByVal apematerno As String, ByVal perfil As String)
        txtNombre.Text = nombre
        txtPaterno.Text = apepaterno
        txtMaterno.Text = apematerno
        cmbTipoAdmin.SelectedValue = perfil
    End Sub

    Public Function VerificaNuevoAdmin()
        Dim retorna As Integer = Nothing
        If String.IsNullOrEmpty(txtNombre.Text) Then
            MsgBox("El Nombre es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtPaterno.Text) Then
            MsgBox("El Apellido Paterno es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtMaterno.Text) Then
            MsgBox("El Apellido Materno es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(cmbTipoAdmin.Text) Then
            MsgBox("Seleccione el tipo de Administrador", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtPwd.Text) Then
            MsgBox("Por favor introduzca una contraseña", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf txtPwd.Text = txtPwd1.Text Then
            MsgBox("Las Contraseñas no coinciden", MsgBoxStyle.Information, "Error!")
            txtPwd1.Text = ""
            retorna = 0
        Else
            retorna = 1
        End If
        Return retorna
    End Function

#End Region

#Region "USUARIOS"

    Public Sub CargarRedUsuario()
        cmbTipoAdmin.Items.Add("RED 1A")
        cmbTipoAdmin.Items.Add("RED 1B")
        'Dim dtFolio As DataTable = Nothing
        'Try
        '    dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, 1, "")

        '    If dtFolio.Rows.Count = 0 Then
        '        MsgBox("No se cargaron los tipos de Administradores", MsgBoxStyle.Information, "Aviso!")
        '    Else
        '        For i As Integer = 0 To dtFolio.Rows.Count Step 1
        '            cmbTipoAdmin.Controls.Add(dtFolio.Rows(i).Item(0))
        '        Next
        '    End If
        'Catch ex As Exception
        '    MsgBox("Problema al Cargar Tipo de Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
        'End Try
    End Sub

    Public Sub CargarUsuarios()
        Dim dtFolio As New DataTable

        'dtFolio.Columns.AddRange(New DataColumn() {New DataColumn("IDADMIN", GetType(Integer)), _
        '                                           New DataColumn("NOMBRE", GetType(String)), _
        '                                           New DataColumn("APELLIDOS", GetType(String)), _
        '                                           New DataColumn("APEPATERNO", GetType(String)), _
        '                                           New DataColumn("APEMATERNO", GetType(String)), _
        '                                           New DataColumn("IDPERFIL", GetType(Integer)), _
        '                                           New DataColumn("PERFIL", GetType(String)), _
        '                                           New DataColumn("ESTATUS", GetType(String))})

        'dtFolio.Rows.Add(1, "Pepe", "Martinez Osorio", "Martinez", "Osorio", 1, "Administrador", "Activo")

        'Me.dtGridAdmin.DataSource = dtFolio
        'Me.dtGridAdmin.CurrentRow.Selected = False


        'Dim filtro As String = ""
        'Dim dtFolio As DataTable = Nothing
        'If String.IsNullOrEmpty(txtFiltro.Text) Then
        '    filtro = ""
        'Else
        '    Try
        '        dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, 0, filtro)

        '        If dtFolio.Rows.Count = 0 Then
        '            MsgBox("No hay registros", MsgBoxStyle.Information, "Aviso!")
        '            Me.dtGridAdmin.DataSource = dtFolio
        '        Else
        '            Me.dtGridAdmin.DataSource = dtFolio
        '            Me.dtGridAdmin.CurrentRow.Selected = False
        '        End If
        '    Catch ex As Exception
        '        MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        '    End Try
        'End If
    End Sub

    Public Sub LimpiarCamposUsuarios()
        txtNombreUsu.Text = ""
        txtPaternoUsu.Text = ""
        txtMaternoUsu.Text = ""
        FechaNacUsu.Value = Date.Now
        txtPrecioTomaUsu.Text = ""
        txtRefUsu.Text = ""
    End Sub

    Public Sub MostrarDatosUsuario(ByVal nombre As String, ByVal apepaterno As String, ByVal apematerno As String, ByVal perfil As String)
        txtNombreUsu.Text = nombre
        txtPaternoUsu.Text = apepaterno
        txtMaternoUsu.Text = apematerno
    End Sub

    Public Function VerificaNuevoUsuario()
        Dim retorna As Integer = Nothing
        If String.IsNullOrEmpty(txtNombreUsu.Text) Then
            MsgBox("El Nombre es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtPaternoUsu.Text) Then
            MsgBox("El Apellido Paterno es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtMaternoUsu.Text) Then
            MsgBox("El Apellido Materno es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(cmbRedUsu.Text) Then
            MsgBox("Seleccione el tipo de Administrador", MsgBoxStyle.Information, "Error!")
            retorna = 0
        Else
            retorna = 1
        End If
        Return retorna
    End Function

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
        Dim frmPagos As New Pago
        frmPagos.ShowDialog()
    End Sub

#Region "ADMINISTRADORES"

    Private Sub dtGridAdmin_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtGridAdmin.CellContentClick
        Dim clave As Integer = Nothing
        If e.RowIndex >= 0 Then
            BotonesModificaElimina()
            clave = CInt(dtGridAdmin.Rows(e.RowIndex).Cells(0).Value)
            MostrarDatosAdmin(CStr(dtGridAdmin.Rows(e.RowIndex).Cells(1).Value), CStr(dtGridAdmin.Rows(e.RowIndex).Cells(3).Value), CStr(dtGridAdmin.Rows(e.RowIndex).Cells(4).Value), CStr(dtGridAdmin.Rows(e.RowIndex).Cells(6).Value))
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        BotonesNuevo()
        CargarTiposAdministradores()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        BotonesInicio()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If MessageBox.Show("Continuar para eliminar al Administrador", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            Dim dtFolio As DataTable = Nothing
            If String.IsNullOrEmpty(RowIdAdmin) Then
                MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
                Exit Sub
            Else
                Try
                    dtFolio = objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaAdmin, 3, RowIdAdmin, "nombre", "apepaterno", "apematerno", "perfil", "contrasena")
                    BotonesInicio()
                    CargarTiposAdministradores()
                    CargarAdministradores()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            End If
        End If
    End Sub

    Private Sub txtFiltro_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFiltro.KeyDown
        CargarAdministradores()
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim contrasena As String = Nothing
        Dim estatus As Integer = Nothing
        If String.IsNullOrEmpty(RowIdAdmin) Then
            MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            If cmbEstatusAdmin.SelectedText = "ACTIVO" Then
                estatus = 1
            Else
                estatus = 0
            End If
            contrasena = VerificaCambioContraseña()
            If contrasena = 1 Then
                contrasena = Trim(txtPwd.Text)
            Else
                contrasena = ""
            End If
            Try

                objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaAdmin, 2, RowIdAdmin, Trim(txtNombre.Text), Trim(txtPaterno.Text), Trim(txtMaterno.Text), Trim(cmbTipoAdmin.Text), Trim(contrasena), estatus)
                BotonesInicio()
                CargarTiposAdministradores()
                CargarAdministradores()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim contrasena As String = Nothing

        If String.IsNullOrEmpty(RowIdAdmin) Then
            MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            contrasena = VerificaNuevoAdmin()
            If contrasena = 1 Then
                contrasena = txtPwd.Text
                Try
                    objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaAdmin, 1, 0, Trim(txtNombre.Text), Trim(txtPaterno.Text), Trim(txtMaterno.Text), Trim(cmbTipoAdmin.Text), Trim(contrasena), 1)
                    BotonesInicio()
                    CargarTiposAdministradores()
                    CargarAdministradores()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            End If
        End If
    End Sub

#End Region

#Region "USUARIOS"

    Private Sub dtGridUsuarios_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtGridUsuarios.CellContentClick
        Dim clave As Integer = Nothing
        If e.RowIndex >= 0 Then
            BotonesModificaElimina()
            clave = CInt(dtGridUsuarios.Rows(e.RowIndex).Cells(0).Value)
            MostrarDatosUsuario(CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(1).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(3).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(4).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(6).Value))
        End If
    End Sub

    Private Sub btnAddUsu_Click(sender As Object, e As EventArgs) Handles btnAddUsu.Click
        BotonesNuevoUsu()
        CargarRedUsuario()
    End Sub

    Private Sub btnCancelarUsu_Click(sender As Object, e As EventArgs) Handles btnCancelarUsu.Click
        BotonesInicioUsu()
    End Sub

    Private Sub btnEliminarUsu_Click(sender As Object, e As EventArgs) Handles btnEliminarUsu.Click
        If MessageBox.Show("Continuar para eliminar al Usuario", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            Dim dtFolio As DataTable = Nothing
            If String.IsNullOrEmpty(RowIdAdmin) Then
                MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
                Exit Sub
            Else
                Try
                    dtFolio = objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaUsuario, 3, RowIdUsuario, "nombre", "apepaterno", "apematerno", "perfil", "contrasena")
                    BotonesInicio()
                    CargarRedUsuario()
                    CargarUsuarios()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            End If
        End If
    End Sub

    Private Sub txtFiltroUsu_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFiltroUsu.KeyDown
        CargarUsuarios()
    End Sub

    Private Sub btnModificarUsu_Click(sender As Object, e As EventArgs) Handles btnModUsu.Click
        Dim estatus As Integer = Nothing
        If String.IsNullOrEmpty(RowIdAdmin) Then
            MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            Try

                objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaAdmin, 2, RowIdAdmin, Trim(txtNombre.Text), Trim(txtPaterno.Text), Trim(txtMaterno.Text), Trim(cmbTipoAdmin.Text), estatus)
                BotonesInicioUsu()
                CargarRedUsuario()
                CargarUsuarios()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Sub btnSaveUsu_Click(sender As Object, e As EventArgs) Handles btnSaveUsu.Click
        If String.IsNullOrEmpty(RowIdAdmin) Then
            MsgBox("No ha selecionado a un Usuario", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            Try
                objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaUsuario, 1, 0, Trim(txtNombre.Text), Trim(txtPaterno.Text), Trim(txtMaterno.Text), Trim(cmbTipoAdmin.Text), 1)
                BotonesInicioUsu()
                CargarRedUsuario()
                CargarUsuarios()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
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
        cmbTipoAdmin.Items.Clear()
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
        BotonesInicio()
        CargarTiposAdministradores()
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

#Region "MANEJO DE BOTONES"

#Region "ADMINISTRADORES"

    Public Sub BotonesInicio()
        dtGridAdmin.Enabled = True
        LimpiarCamposAdmin()
        btnNuevo.Visible = False
        btnCancelar.Visible = False
        btnAdd.Visible = True
        btnModificar.Visible = True
        btnEliminar.Visible = True
    End Sub

    Public Sub BotonesNuevo()
        dtGridAdmin.Enabled = False
        LimpiarCamposAdmin()
        btnNuevo.Visible = True
        btnCancelar.Visible = True
        btnAdd.Visible = False
        btnModificar.Visible = False
        btnEliminar.Visible = False
    End Sub

    Public Sub BotonesModificaElimina()
        dtGridAdmin.Enabled = True
        btnNuevo.Visible = False
        btnCancelar.Visible = False
        btnAdd.Enabled = True
        btnModificar.Enabled = True
        btnEliminar.Enabled = True
    End Sub

#End Region

#Region "USUARIOS"

    Public Sub BotonesInicioUsu()
        dtGridUsuarios.Enabled = True
        LimpiarCamposUsuarios()
        btnSaveUsu.Visible = False
        btnCancelarUsu.Visible = False
        btnAddUsu.Visible = True
        btnModUsu.Visible = True
        btnEliminarUsu.Visible = True
    End Sub

    Public Sub BotonesNuevoUsu()
        dtGridUsuarios.Enabled = False
        LimpiarCamposUsuarios()
        btnSaveUsu.Visible = True
        btnCancelarUsu.Visible = True
        btnAddUsu.Visible = False
        btnModUsu.Visible = False
        btnEliminarUsu.Visible = False
    End Sub

    Public Sub BotonesModificaEliminaUsu()
        dtGridUsuarios.Enabled = True
        btnSaveUsu.Visible = False
        btnCancelarUsu.Visible = False
        btnAddUsu.Enabled = True
        btnModUsu.Enabled = True
        btnEliminarUsu.Enabled = True
    End Sub

#End Region

#End Region
End Class