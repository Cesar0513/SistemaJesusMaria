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

    'SP DE ADMINISTRADORES
    Private sp_CargarAdministradores As String = "Seguridad.CargaAdministradores"
    Private sp_CargarTipoAdministradores As String = "Seguridad.CargarTiposAdministradores"
    Private sp_InsertaActualizaEliminaAdmin As String = "Seguridad.sp_InsertaActualizaEliminaAdmin"
    Private sp_VerificaAdminExistente As String = "Seguridad.VerificaAdministrador"

    'SP DE USUARIOS
    Private sp_CargarUsuarios As String = "Seguridad.CargaUsuarios"
    Private sp_CargarRedes As String = "Seguridad.CargaRedes"
    Private sp_CargarServicio As String = "Seguridad.CargaTipoServicio"
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
        cmbTipoAdmin.Items.Clear()
        Dim dtFolio As New DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarTipoAdministradores, 1)

            If dtFolio.Rows.Count = 0 Then
                MsgBox("No se cargaron los tipos de Administradores", MsgBoxStyle.Information, "Aviso!")
            Else
                For i As Integer = 0 To dtFolio.Rows.Count - 1 Step 1
                    cmbTipoAdmin.Items.Add(dtFolio.Rows(i).Item(0))
                Next
            End If
        Catch ex As Exception
            MsgBox("Problema al Cargar Tipo de Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub CargarAdministradores()
        Dim filtro As String = ""
        Dim dtFolio As New DataTable
        If String.IsNullOrEmpty(txtFiltro.Text) Then
            filtro = ""
        Else
            filtro = Trim(txtFiltro.Text)
            Try
                dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, filtro)

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
        cmbTipoAdmin.Text = ""
        cmbEstatusAdmin.Text = ""
        txtPwd.Text = ""
        txtPwd1.Text = ""
    End Sub

    Public Sub MostrarDatosAdmin(ByVal nombre As String, ByVal apepaterno As String, ByVal apematerno As String, ByVal tipo As String, ByVal perfil As String)
        txtNombre.Text = nombre
        txtPaterno.Text = apepaterno
        txtMaterno.Text = apematerno
        cmbTipoAdmin.SelectedItem = tipo
        cmbEstatusAdmin.SelectedItem = perfil
        txtPwd.Clear()
        txtPwd1.Clear()
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
        ElseIf txtPwd.Text <> txtPwd1.Text Then
            MsgBox("Las Contraseñas no coinciden", MsgBoxStyle.Information, "Error!")
            txtPwd1.Text = ""
            retorna = 0
        Else
            retorna = 1
        End If
        Return retorna
    End Function

    Public Function VerificaAdministrador(ByRef tipo As Integer)
        Dim retorna As Integer = Nothing
        If tipo = 1 Then
                If txtPwd.Text = txtPwd1.Text Then
                    Dim dtFolio As New DataTable
                    Try
                        dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaAdminExistente, Trim(txtPwd.Text))

                        If dtFolio.Rows.Count = 0 Then
                        MsgBox("Sin registros", MsgBoxStyle.Information, "Aviso!")
                        Else
                            If CInt(dtFolio.Rows(0).Item(0)) >= 1 Then
                                retorna = 1
                            Else
                                retorna = 0
                            End If
                        End If
                    Catch ex As Exception
                        MsgBox("Problema al Cargar Tipo de Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
                    End Try
                Else
                    MsgBox("Las contraseñas no coinciden", MsgBoxStyle.Information, "AVISO!")
                    retorna = 0
                End If
        Else
            If String.IsNullOrEmpty(txtPwd.Text) Then
                MsgBox("Ingrese una contraseña para Continuar", MsgBoxStyle.Information, "AVISO!")
            Else
                If txtPwd.Text = txtPwd1.Text Then
                    Dim dtFolio As New DataTable
                    Try
                        dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaAdminExistente, Trim(txtPwd.Text))

                        If dtFolio.Rows.Count = 0 Then
                            MsgBox("No se cargaron los tipos de Administradores", MsgBoxStyle.Information, "Aviso!")
                        Else
                            If CInt(dtFolio.Rows(0).Item(0)) >= 1 Then
                                retorna = 1
                            Else
                                retorna = 0
                            End If
                        End If
                    Catch ex As Exception
                        MsgBox("Problema al Cargar Tipo de Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
                    End Try
                Else
                    MsgBox("Las contraseñas no coinciden", MsgBoxStyle.Information, "AVISO!")
                    retorna = 0
                End If
            End If
        End If
        Return retorna
    End Function

#End Region

#Region "USUARIOS"

    Public Sub CargarRedUsuario()
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarRedes, 1)

            If dtFolio.Rows.Count = 0 Then
                MsgBox("No hay Redes registradas", MsgBoxStyle.Information, "Aviso!")
            Else
                For i As Integer = 0 To dtFolio.Rows.Count - 1 Step 1
                    cmbRedUsu.Items.Add(dtFolio.Rows(i).Item(0))
                Next
            End If
        Catch ex As Exception
            MsgBox("Problema al Cargar Redes en Tab Usuarios: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub CargarServicioUsuario()
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarServicio, 1)

            If dtFolio.Rows.Count = 0 Then
                MsgBox("No hay Redes registradas", MsgBoxStyle.Information, "Aviso!")
            Else
                For i As Integer = 0 To dtFolio.Rows.Count - 1 Step 1
                    cmbTpoServUsu.Items.Add(dtFolio.Rows(i).Item(0))
                Next
            End If
        Catch ex As Exception
            MsgBox("Problema al Cargar Servicios en Tab Usuarios: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub CargarUsuarios()
        Dim filtro As String = ""
        Dim dtFolio As New DataTable
        If String.IsNullOrEmpty(txtFiltroUsu.Text) Then
            filtro = ""
        Else
            filtro = Trim(txtFiltroUsu.Text)
            Try
                dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarUsuarios, filtro)

                If dtFolio.Rows.Count = 0 Then
                    MsgBox("No Usuarios Registrados", MsgBoxStyle.Information, "Aviso!")
                    Me.dtGridUsuarios.DataSource = dtFolio
                Else
                    Me.dtGridUsuarios.DataSource = dtFolio
                    Me.dtGridUsuarios.CurrentRow.Selected = False
                End If
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Public Sub LimpiarCamposUsuarios()
        txtNombreUsu.Text = ""
        txtPaternoUsu.Text = ""
        txtMaternoUsu.Text = ""
        FechaNacUsu.Value = Date.Now
        txtPrecioTomaUsu.Text = ""
        txtRefUsu.Text = ""
    End Sub

    Public Sub MostrarDatosUsuario(ByVal nombre As String, ByVal apepaterno As String, ByVal apematerno As String, ByVal fec_nac As String, ByVal red As String, ByVal serv As String, ByVal ref As String)
        FechaNacUsu.Format = DateTimePickerFormat.Custom
        FechaNacUsu.CustomFormat = "yyyy-MM-dd"
        txtNombreUsu.Text = nombre
        txtPaternoUsu.Text = apepaterno
        txtMaternoUsu.Text = apematerno
        FechaNacUsu.Value = fec_nac
        cmbRedUsu.SelectedItem = red
        cmbTpoServUsu.SelectedItem = serv
        txtPrecioTomaUsu.Text = 0
        txtRefUsu.Text = ref
        btnPagar.Visible = True
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
        'MsgBox("La clave del usuarios es : " & RowIdUsuario, MsgBoxStyle.Information, "AVISO!")
        'Dim frmPagos As New Pago
        'frmPagos.ShowDialog()
    End Sub

#Region "ADMINISTRADORES"

    Private Sub dtGridAdmin_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtGridAdmin.CellContentClick
        Dim clave As Integer = Nothing
        RowIdAdmin = 0
        If e.RowIndex >= 0 Then
            BotonesModificaElimina()
            RowIdAdmin = CInt(dtGridAdmin.Rows(e.RowIndex).Cells(0).Value)
            MostrarDatosAdmin(CStr(dtGridAdmin.Rows(e.RowIndex).Cells(1).Value), CStr(dtGridAdmin.Rows(e.RowIndex).Cells(3).Value), CStr(dtGridAdmin.Rows(e.RowIndex).Cells(4).Value), CStr(dtGridAdmin.Rows(e.RowIndex).Cells(5).Value), CStr(dtGridAdmin.Rows(e.RowIndex).Cells(6).Value))
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
            If String.IsNullOrEmpty(RowIdAdmin) Or RowIdAdmin = 0 Then
                MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
                Exit Sub
            Else
                Try
                    dtFolio = objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaAdmin, 3, RowIdAdmin, "nombre", "apepaterno", "apematerno", "perfil", "contrasena", 1)
                    RowIdAdmin = 0
                    BotonesInicio()
                    CargarTiposAdministradores()
                    CargarAdministradores()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            End If
        End If
    End Sub

    Private Sub txtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        CargarAdministradores()
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim contrasena As String = Nothing
        Dim estatus As Integer = Nothing
        If String.IsNullOrEmpty(RowIdAdmin) Or RowIdAdmin = 0 Then
            MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            If cmbEstatusAdmin.SelectedItem = "Activo" Then
                estatus = 1
            Else
                estatus = 0
            End If
            If Not String.IsNullOrEmpty(txtPwd.Text) And Not String.IsNullOrEmpty(txtPwd1.Text) Then
                contrasena = VerificaCambioContraseña()
                If contrasena = 1 Then
                    Dim existente As Integer = VerificaAdministrador(1)
                    If existente = 1 Then
                        MsgBox("La contraseña es incorrecta, ingrese una diferente", MsgBoxStyle.Information, "AVISO")
                        Exit Sub
                    End If
                    If Trim(txtPwd.Text) = Trim(txtPwd1.Text) Then
                        contrasena = Trim(txtPwd.Text)
                    End If
                Else
                    contrasena = ""
                End If
            Else
                contrasena = "null"
            End If
            Try
                objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaAdmin, 2, RowIdAdmin, Trim(txtNombre.Text), Trim(txtPaterno.Text), Trim(txtMaterno.Text), Trim(cmbTipoAdmin.Text), Trim(contrasena), estatus)
                RowIdAdmin = 0
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
            Dim existente As Integer = VerificaAdministrador(0)
            If existente = 1 Then
                MsgBox("La contraseña es incorrecta, ingrese una diferente", MsgBoxStyle.Information, "AVISO")
                Exit Sub
            End If
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

    Private Sub dtGridUsuarios_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dtGridUsuarios.CellEnter
        FechaNacUsu.Format = DateTimePickerFormat.Custom
        FechaNacUsu.CustomFormat = "yyyy-MM-dd"
        Dim clave As Integer = Nothing
        RowIdUsuario = 0
        If e.RowIndex >= 0 Then
            BotonesModificaEliminaUsu()
            txtPrecioTomaUsu.Enabled = False
            RowIdUsuario = CInt(dtGridUsuarios.Rows(e.RowIndex).Cells(0).Value)
            MostrarDatosUsuario(CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(1).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(3).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(4).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(5).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(6).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(7).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(8).Value))
            If RowIdUsuario = 0 Then
                btnPagar.Visible = False
            Else
                btnPagar.Visible = True
            End If
        End If
    End Sub

    Private Sub btnAddUsu_Click(sender As Object, e As EventArgs) Handles btnAddUsu.Click
        BotonesNuevoUsu()
        CargarRedUsuario()
        txtPrecioTomaUsu.Enabled = True
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

    Private Sub txtFiltroUsu_TextChanged(sender As Object, e As EventArgs) Handles txtFiltroUsu.TextChanged
        CargarUsuarios()
    End Sub

    Private Sub btnModificarUsu_Click(sender As Object, e As EventArgs) Handles btnModUsu.Click
        Dim estatus As Integer = Nothing
        If String.IsNullOrEmpty(RowIdUsuario) Or RowIdUsuario = 0 Then
            MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            Try
                Dim fec As String = Nothing
                'FechaNacUsu.Text
                objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaUsuario, 2, RowIdUsuario, Trim(txtNombreUsu.Text), Trim(txtPaternoUsu.Text), Trim(txtMaternoUsu.Text), FechaNacUsu.Text, Trim(cmbRedUsu.SelectedItem), Trim(cmbTpoServUsu.SelectedItem), Trim(txtPrecioTomaUsu.Text), Trim(txtRefUsu.Text), 1)
                BotonesInicioUsu()
                CargarUsuarios()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Sub btnSaveUsu_Click(sender As Object, e As EventArgs) Handles btnSaveUsu.Click
        If String.IsNullOrEmpty(RowIdUsuario) Or RowIdUsuario = 0 Then
            MsgBox("No ha selecionado a un Usuario", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            Try
                objSQL.ejecutaProcedimientoTable(sp_InsertaActualizaEliminaUsuario, 1, 0, Trim(txtNombreUsu.Text), Trim(txtPaternoUsu.Text), Trim(txtMaternoUsu.Text), Trim(FechaNacUsu.Text), Trim(cmbRedUsu.SelectedItem), Trim(cmbTpoServUsu.SelectedItem), Trim(txtPrecioTomaUsu.Text), Trim(txtRefUsu.Text), 1)
                BotonesInicioUsu()
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
        CargarRedUsuario()
        CargarServicioUsuario()
        RowIdAdmin = 0
    End Sub

    Private Sub TabUsuarios_Enter(sender As Object, e As EventArgs) Handles TabUsuarios.Enter, TabUsuarios.LostFocus
        LimpiarTabUsuarios()
        RowIdUsuario = 0
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
        btnPagar.Visible = True
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
        btnPagar.Visible = False
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