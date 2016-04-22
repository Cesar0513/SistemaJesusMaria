﻿Imports System.Drawing
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
    Dim RowIdSer As Integer = Nothing
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
        Dim clsAdmin As New clsAdministrador()
        Try
            dtFolio = clsAdmin.CargarAdministradores(0, "q")
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
        Dim AdminOperaciones As New clsAdministrador()
        Dim filtro As String = ""
        Dim dtFolio As New DataTable
        If String.IsNullOrEmpty(txtFiltro.Text) Then
            filtro = ""
        Else
            filtro = Trim(txtFiltro.Text)
            Try
                dtFolio = AdminOperaciones.CargarAdministradores(1, filtro)

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
            retorna = 0
        ElseIf Val(txtPwd.Text) <> Val(txtPwd1.Text) Then
            MsgBox("Los campos de Contraseña NO son Iguales", MsgBoxStyle.Information, "AVISO!")
            retorna = 0
        ElseIf txtPwd.Text = txtPwd1.Text And String.IsNullOrEmpty(txtPwd.Text) Then
            retorna = 0
        ElseIf txtPwd.Text = txtPwd1.Text And Not String.IsNullOrEmpty(txtPwd.Text) Then
            retorna = 1
        End If
        Return retorna
    End Function

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
        ElseIf txtPwd.Text = txtPwd1.Text Then
            retorna = 1
        End If
        Return retorna
    End Function

    Public Function VerificaAdministrador()
        Dim AdminOperacion As New clsAdministrador()
        Dim retorna As Integer = Nothing

            If txtPwd.Text = txtPwd1.Text Then
                Try
                    retorna = AdminOperacion.VerificaExistenciaAdmin(Trim(txtPwd.Text))
                Catch ex As Exception
                MsgBox("Error al Verificar la Existencia de Administrador: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            Else
                MsgBox("Las contraseñas no coinciden", MsgBoxStyle.Information, "AVISO!")
                retorna = 0
            End If
        Return retorna
    End Function

#End Region

#Region "USUARIOS"

    Public Sub CargarRedUsuario()
        Dim RedOperacion As New clsRedes()
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = RedOperacion.CargarRedes(0, "")

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
        Dim ServOperacion As New clsServicio
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = ServOperacion.CargarServiciosUsuarios()

            If dtFolio.Rows.Count = 0 Then
                MsgBox("No hay tipos de Servicios registradas", MsgBoxStyle.Information, "Aviso!")
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
        Dim UsuOperacion As New clsUsuarios()
        Dim filtro As String = ""
        Dim dtFolio As New DataTable
        If String.IsNullOrEmpty(txtFiltroUsu.Text) Then
            filtro = ""
        Else
            filtro = Trim(txtFiltroUsu.Text)
            Try
                dtFolio = UsuOperacion.CargarUsuarios(1, filtro)

                If dtFolio.Rows.Count = 0 Then
                    MsgBox("No hay registros", MsgBoxStyle.Information, "Aviso!")
                    Me.dtGridUsuarios.DataSource = dtFolio
                Else
                    Me.dtGridUsuarios.DataSource = dtFolio
                    Me.dtGridUsuarios.CurrentRow.Selected = False
                    LimpiarCamposUsuarios()
                End If
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
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
            MsgBox("Seleccione la Red del Usuarios", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(cmbTpoServUsu.Text) Then
            MsgBox("Seleccione el tipo de Servicio del Usuarios", MsgBoxStyle.Information, "Error!")
            retorna = 0
        Else
            retorna = 1
        End If
        Return retorna
    End Function

    Public Function VerificaExistenciaUsuarios()
        Dim UsuOperacion As New clsUsuarios()
        Dim ret As Integer = Nothing
        UsuOperacion.IdUsuario = 0
        UsuOperacion.NombreUsu = Trim(txtNombreUsu.Text)
        UsuOperacion.ApePatUsu = Trim(txtPaternoUsu.Text)
        UsuOperacion.ApeMatUsu = Trim(txtMaternoUsu.Text)
        UsuOperacion.FecNacUsu = ""
        UsuOperacion.RedUsuario = ""
        UsuOperacion.TipoUsuario = ""
        UsuOperacion.RefUsuario = ""

        Try
            ret = UsuOperacion.VerificaExistenciaUsuario(UsuOperacion)
        Catch ex As Exception
            MsgBox("Error al Consultar Existencia de Usuario " & ex.Message, MsgBoxStyle.Information, "Error!")
        End Try
        Return ret
    End Function

#End Region

#Region "REDES"

    Public Sub CargarTamanoRed()
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarTamano)

            If dtFolio.Rows.Count = 0 Then
                MsgBox("No hay Redes registradas", MsgBoxStyle.Information, "Aviso!")
            Else
                For i As Integer = 0 To dtFolio.Rows.Count - 1 Step 1
                    cmbTamanoRed.Items.Add(dtFolio.Rows(i).Item(0))
                Next
            End If
        Catch ex As Exception
            MsgBox("Problema al Cargar Servicios en Tab Usuarios: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub CargarRedesTab()
        Dim filtro As String = ""
        Dim dtFolio As New DataTable
        If String.IsNullOrEmpty(txtFiltroRed.Text) Then
            filtro = ""
        Else
            filtro = Trim(txtFiltroRed.Text)
            Try
                dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarRedes, filtro)

                If dtFolio.Rows.Count = 0 Then
                    MsgBox("No hay Redes Registrados", MsgBoxStyle.Information, "Aviso!")
                    Me.dtGridRed.DataSource = dtFolio
                Else
                    Me.dtGridRed.DataSource = dtFolio
                    Me.dtGridRed.CurrentRow.Selected = False
                End If
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Public Sub MostrarDatosRedes(Red As clsRedes)
        txtNumRed.Text = Red.NombreRed
        txtEncargadoRed.Text = Red.Encargado
        cmbTamanoRed.SelectedItem = Red.TamanoRed
        txtCuotaRed.Text = Red.CuotaRed
        txtRefRed.Text = Red.RefRed
    End Sub

    Public Function NuevaRed()
        Dim retorna As Integer = Nothing
        If String.IsNullOrEmpty(txtNumRed.Text) Then
            MsgBox("El Nombre es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtEncargadoRed.Text) Then
            MsgBox("Encargado es campo Obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(cmbTamanoRed.Text) Then
            MsgBox("Seleccione el Tamaño de la Red", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtCuotaRed.Text) Then
            MsgBox("Indique cual es la Cuota, si es Cero Coloquelo", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtRefRed.Text) Then
            MsgBox("Escriba una Referencia de la Ubicación de la Red", MsgBoxStyle.Information, "Error!")
            retorna = 0
        Else
            retorna = 1
        End If
        Return retorna
    End Function

    Public Function VerificaExistenciaRed()
        Dim RedOperacion As New clsRedes()
        Dim ret As Integer = Nothing
        Try
            ret = RedOperacion.VerificaNuevaRed(Trim(txtNumRed.Text))
        Catch ex As Exception
            MsgBox("Error al Consultar Existencia de Usuario " & ex.Message, MsgBoxStyle.Information, "Error!")
        End Try
        Return ret
    End Function

#End Region

#Region "TIPO DE SERVICIO"

    Public Sub CargarTiposServicios()
        Dim ServOperacion As New clsServicio()
        Dim dtFolio As DataTable = Nothing
        Try
            dtFolio = ServOperacion.CargarServicios()

            If dtFolio.Rows.Count = 0 Then
                MsgBox("No hay registros", MsgBoxStyle.Information, "Aviso!")
                Me.dtGridUsuarios.DataSource = dtFolio
            Else
                Me.dtGridServicio.DataSource = dtFolio
                Me.dtGridServicio.CurrentRow.Selected = False
                LimpiarCamposServicios()
            End If
        Catch ex As Exception
            MsgBox("Problema al Cargar Servicios: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub MostrarServicio(Serv As clsServicio)
        txtServ.Text = Serv.NombreTipo
        txtCuotaServ.Text = Serv.CuotaTipo
        cmbEstatusServ.SelectedItem = Serv.EstatusTipo
    End Sub

    Public Function NuevaServicio()
        Dim retorna As Integer = Nothing
        If String.IsNullOrEmpty(txtServ.Text) Then
            MsgBox("El Nombre es obligatorio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(txtCuotaServ.Text) Then
            MsgBox("Indique cual es la Cuota, si es Cero Coloquelo", MsgBoxStyle.Information, "Error!")
            retorna = 0
        ElseIf String.IsNullOrEmpty(cmbEstatusServ.Text) Then
            MsgBox("Seleccione un Estatus del Servicio", MsgBoxStyle.Information, "Error!")
            retorna = 0
        Else
            retorna = 1
        End If
        Return retorna
    End Function

    Public Function VerificaExistenciaServicio()
        Dim ServOperacion As New clsServicio()
        Dim ret As Integer = Nothing
        Try
            ret = ServOperacion.VerificaNuevaServ(Trim(txtServ.Text))
        Catch ex As Exception
            MsgBox("Error al Consultar Existencia de Servicio " & ex.Message, MsgBoxStyle.Information, "Error!")
        End Try
        Return ret
    End Function

#End Region

#Region "RECONEXION"

    Public Sub CargarUsuariosReconexion()
        Dim UsuOperacion As New clsUsuarios()
        Dim filtro As String = ""
        Dim dtFolio As New DataTable
        If String.IsNullOrEmpty(txtFiltroRecon.Text) Then
            filtro = ""
        Else
            filtro = Trim(txtFiltroRecon.Text)
            Try
                dtFolio = UsuOperacion.CargarUsuarios(0, filtro)

                If dtFolio.Rows.Count = 0 Then
                    MsgBox("No hay registros", MsgBoxStyle.Information, "Aviso!")
                    Me.dtGridReconexion.DataSource = dtFolio
                Else
                    Me.dtGridReconexion.DataSource = dtFolio
                    Me.dtGridReconexion.CurrentRow.Selected = False
                    LimpiarCamposUsuarios()
                End If
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Public Sub MostrarUsuarioReconexion(user As clsUsuarios)
        txtNomRec.Text = user.NombreUsu
        txtPaternoRec.Text = user.ApePatUsu
        txtMaternoRec.Text = user.ApeMatUsu
        txtFecRec.Text = user.FecNacUsu
        txtRedRec.Text = user.RedUsuario
        txtRefRec.Text = user.RefUsuario
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
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        BotonesInicio()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim AdminOperacion As New clsAdministrador()
        If MessageBox.Show("Continuar para eliminar al Administrador", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            If String.IsNullOrEmpty(RowIdAdmin) Or RowIdAdmin = 0 Then
                MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
                Exit Sub
            Else
                AdminOperacion.IdAdmin = RowIdAdmin
                AdminOperacion.NombrAdmin = Trim("")
                AdminOperacion.ApePatAdmin = Trim("")
                AdminOperacion.ApeMatAdmin = Trim("")
                AdminOperacion.TipoAdmin = ""
                AdminOperacion.PwdAdmin = Trim("")
                AdminOperacion.EstatusAdmin = ""
                Try

                    AdminOperacion.InsertaModificaEliminaAdministrador(3, AdminOperacion)
                    RowIdAdmin = 0
                    BotonesInicio()
                    CargarAdministradores()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema al Eliminar Administrador: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            End If
        End If
    End Sub

    Private Sub txtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        CargarAdministradores()
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim AdminOperacion As New clsAdministrador()
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
                    Dim existente As Integer = VerificaAdministrador()
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

            AdminOperacion.IdAdmin = RowIdAdmin
            AdminOperacion.NombrAdmin = Trim(txtNombre.Text)
            AdminOperacion.ApePatAdmin = Trim(txtPaterno.Text)
            AdminOperacion.ApeMatAdmin = Trim(txtMaterno.Text)
            AdminOperacion.TipoAdmin = cmbTipoAdmin.SelectedItem
            AdminOperacion.PwdAdmin = Trim(contrasena)
            AdminOperacion.EstatusAdmin = cmbEstatusAdmin.SelectedItem
            Try
                AdminOperacion.InsertaModificaEliminaAdministrador(2, AdminOperacion)
                RowIdAdmin = 0
                BotonesInicio()
                CargarAdministradores()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim AdminOperacion As New clsAdministrador()
        Dim contrasena As String = Nothing

        If String.IsNullOrEmpty(RowIdAdmin) Then
            MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            If VerificaNuevoAdmin() = 0 Then
                Exit Sub
            End If

            Dim existente As Integer = VerificaAdministrador()
            If existente = 1 Then
                MsgBox("La contraseña es incorrecta, ingrese una diferente", MsgBoxStyle.Information, "AVISO")
                Exit Sub
            Else
                AdminOperacion.IdAdmin = 0
                AdminOperacion.NombrAdmin = Trim(txtNombre.Text)
                AdminOperacion.ApePatAdmin = Trim(txtPaterno.Text)
                AdminOperacion.ApeMatAdmin = Trim(txtMaterno.Text)
                AdminOperacion.TipoAdmin = cmbTipoAdmin.SelectedItem
                AdminOperacion.PwdAdmin = Trim(txtPwd.Text)
                AdminOperacion.EstatusAdmin = cmbEstatusAdmin.SelectedItem
                Try
                    AdminOperacion.InsertaModificaEliminaAdministrador(1, AdminOperacion)
                    BotonesInicio()
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
        Dim clave As Integer = Nothing
        RowIdUsuario = 0
        If e.RowIndex >= 0 Then
            BotonesModificaEliminaUsu()
            txtPrecioTomaUsu.Enabled = False
            RowIdUsuario = CInt(dtGridUsuarios.Rows(e.RowIndex).Cells(0).Value)
            MostrarDatosUsuario(CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(1).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(3).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(4).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(5).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(6).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(7).Value), CStr(dtGridUsuarios.Rows(e.RowIndex).Cells(8).Value))
            BotonesModificaEliminaUsu()
        End If
    End Sub

    Private Sub btnAddUsu_Click(sender As Object, e As EventArgs) Handles btnAddUsu.Click
        BotonesNuevoUsu()
        txtPrecioTomaUsu.Enabled = True
    End Sub

    Private Sub btnCancelarUsu_Click(sender As Object, e As EventArgs) Handles btnCancelarUsu.Click
        BotonesInicioUsu()
    End Sub

    Private Sub btnEliminarUsu_Click(sender As Object, e As EventArgs) Handles btnEliminarUsu.Click
        Dim UsuOperacion As New clsUsuarios()
        If MessageBox.Show("Continuar para eliminar al Usuario", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            Dim dtFolio As DataTable = Nothing
            If String.IsNullOrEmpty(RowIdAdmin) Then
                MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
                Exit Sub
            Else
                UsuOperacion.IdUsuario = RowIdUsuario
                UsuOperacion.NombreUsu = Trim(txtNombreUsu.Text)
                UsuOperacion.ApePatUsu = Trim(txtPaternoUsu.Text)
                UsuOperacion.ApeMatUsu = Trim(txtMaternoUsu.Text)
                UsuOperacion.FecNacUsu = Trim(FechaNacUsu.Text)
                UsuOperacion.RedUsuario = Trim(cmbRedUsu.SelectedItem)
                UsuOperacion.TipoUsuario = Trim(cmbTpoServUsu.SelectedItem)
                UsuOperacion.Precio = Trim(txtPrecioTomaUsu.Text)
                UsuOperacion.RefUsuario = Trim(txtRefUsu.Text)
                UsuOperacion.EstaUsuario = "Activo"
                Try
                    BotonesInicioUsu()
                    UsuOperacion.AgregaModificaEliminaUsuario(3, UsuOperacion)
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
        Dim UsuOperacion As New clsUsuarios()
        Dim estatus As Integer = Nothing
        If String.IsNullOrEmpty(RowIdUsuario) Or RowIdUsuario = 0 Then
            MsgBox("No ha selecionado a un Administrador", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            UsuOperacion.IdUsuario = RowIdUsuario
            UsuOperacion.NombreUsu = Trim(txtNombreUsu.Text)
            UsuOperacion.ApePatUsu = Trim(txtPaternoUsu.Text)
            UsuOperacion.ApeMatUsu = Trim(txtMaternoUsu.Text)
            UsuOperacion.FecNacUsu = Trim(FechaNacUsu.Text)
            UsuOperacion.RedUsuario = Trim(cmbRedUsu.SelectedItem)
            UsuOperacion.TipoUsuario = Trim(cmbTpoServUsu.SelectedItem)
            UsuOperacion.Precio = Trim(txtPrecioTomaUsu.Text)
            UsuOperacion.RefUsuario = Trim(txtRefUsu.Text)
            UsuOperacion.EstaUsuario = "Activo"
            Try
                BotonesInicioUsu()
                UsuOperacion.AgregaModificaEliminaUsuario(2, UsuOperacion)
                CargarUsuarios()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Sub btnSaveUsu_ClickGuarda(sender As Object, e As EventArgs) Handles btnSaveUsu.Click
        Dim UsuOperacion As New clsUsuarios()

        If VerificaNuevoUsuario() = 0 Then
            Exit Sub
        End If

        If VerificaExistenciaUsuarios() = 1 Then
            If MessageBox.Show("Ya Existe un Usuarios con el Nombre y Apellidos Iguales, Si es otra toma presione 'Aceptar', si no presione 'Cancelar'", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                UsuOperacion.IdUsuario = 0
                UsuOperacion.NombreUsu = Trim(txtNombreUsu.Text)
                UsuOperacion.ApePatUsu = Trim(txtPaternoUsu.Text)
                UsuOperacion.ApeMatUsu = Trim(txtMaternoUsu.Text)
                UsuOperacion.FecNacUsu = Trim(FechaNacUsu.Text)
                UsuOperacion.RedUsuario = Trim(cmbRedUsu.SelectedItem)
                UsuOperacion.TipoUsuario = Trim(cmbTpoServUsu.SelectedItem)
                UsuOperacion.Precio = Trim(txtPrecioTomaUsu.Text)
                UsuOperacion.RefUsuario = Trim(txtRefUsu.Text)
                UsuOperacion.EstaUsuario = 1
                Try
                    UsuOperacion.AgregaModificaEliminaUsuario(1, UsuOperacion)
                    BotonesInicioUsu()
                    CargarUsuarios()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            Else
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "REDES"

    Private Sub dtGridRed_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dtGridRed.CellEnter
        Dim Red As New clsRedes()
        Dim clave As Integer = Nothing
        RowIdRed = 0
        If e.RowIndex >= 0 Then
            BotonesModificaEliminaRed()
            RowIdRed = CInt(dtGridRed.Rows(e.RowIndex).Cells(0).Value)
            Red.IdRed = CInt(dtGridRed.Rows(e.RowIndex).Cells(0).Value)
            Red.NombreRed = CStr(dtGridRed.Rows(e.RowIndex).Cells(1).Value)
            Red.Encargado = CStr(dtGridRed.Rows(e.RowIndex).Cells(2).Value)
            Red.TamanoRed = CStr(dtGridRed.Rows(e.RowIndex).Cells(3).Value)
            Red.CuotaRed = CDbl(dtGridRed.Rows(e.RowIndex).Cells(4).Value)
            Red.RefRed = CInt(dtGridRed.Rows(e.RowIndex).Cells(5).Value)
            MostrarDatosRedes(Red)
        End If
    End Sub

    Private Sub btnAddRed_Click(sender As Object, e As EventArgs) Handles btnAddRed.Click
        BotonesNuevoRed()
    End Sub

    Private Sub btnCancelarRed_Click(sender As Object, e As EventArgs) Handles btnCancelarRed.Click
        BotonesInicioRed()
    End Sub

    Private Sub txtFiltroRed_TextChanged(sender As Object, e As EventArgs) Handles txtFiltroRed.TextChanged
        CargarRedesTab()
    End Sub

    Private Sub btnEliminarRed_Click(sender As Object, e As EventArgs) Handles btnEliminaRed.Click
        Dim RedOperacion As New clsRedes()
        If MessageBox.Show("Continuar para eliminar la Red", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            Dim dtFolio As DataTable = Nothing
            If String.IsNullOrEmpty(RowIdAdmin) Then
                MsgBox("No ha selecionado ninguna Red", MsgBoxStyle.Information, "AVISO!")
                Exit Sub
            Else
                RedOperacion.IdRed = RowIdRed
                RedOperacion.NombreRed = ""
                RedOperacion.Encargado = ""
                RedOperacion.TamanoRed = ""
                RedOperacion.CuotaRed = 0
                RedOperacion.RefRed = ""
                RedOperacion.EstatusRed = "Activo"
                Try
                    BotonesInicioRed()
                    RedOperacion.AgregaModificaEliminaRed(3, RedOperacion)
                    CargarUsuarios()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            End If
        End If
    End Sub

    Private Sub btnModificarRed_Click(sender As Object, e As EventArgs) Handles btnModificarRed.Click
        Dim RedOperacion As New clsRedes()
        Dim estatus As Integer = Nothing
        If String.IsNullOrEmpty(RowIdUsuario) Or RowIdUsuario = 0 Then
            MsgBox("No ha selecionado ninguna Red", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            RedOperacion.IdRed = RowIdRed
            RedOperacion.NombreRed = Trim(txtNumRed.Text)
            RedOperacion.Encargado = Trim(txtEncargadoRed.Text)
            RedOperacion.TamanoRed = Trim(cmbTamanoRed.SelectedItem)
            RedOperacion.CuotaRed = Trim(txtCuotaRed.Text)
            RedOperacion.RefRed = Trim(txtRefRed.Text)
            RedOperacion.EstatusRed = "Activo"
            Try
                BotonesInicioRed()
                RedOperacion.AgregaModificaEliminaRed(2, RedOperacion)
                CargarRedesTab()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Sub btnSaveRed_Click(sender As Object, e As EventArgs) Handles btnSaveRed.Click
        Dim RedOperacion As New clsRedes()

        If NuevaRed() = 0 Then
            Exit Sub
        End If

        If VerificaExistenciaRed() = 1 Then
            MsgBox("Ya existe una Red con el mismo Nombre, ingerse uno diferente", MsgBoxStyle.Information, "Aviso!")
            Exit Sub
        End If

        RedOperacion.IdRed = RowIdRed
        RedOperacion.NombreRed = Trim(txtNumRed.Text)
        RedOperacion.Encargado = Trim(txtEncargadoRed.Text)
        RedOperacion.TamanoRed = Trim(cmbTamanoRed.SelectedItem)
        RedOperacion.CuotaRed = Trim(txtCuotaRed.Text)
        RedOperacion.RefRed = Trim(txtRefRed.Text)
        RedOperacion.EstatusRed = "Activo"
        Try
            RedOperacion.AgregaModificaEliminaRed(1, RedOperacion)
            BotonesInicioUsu()
            CargarUsuarios()
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub


#End Region

#Region "TIPO SERVICIO"

    Private Sub dtGridServ_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dtGridServicio.CellEnter
        Dim Serv As New clsServicio()
        Dim clave As Integer = Nothing
        RowIdSer = 0
        If e.RowIndex >= 0 Then
            BotonesModificaEliminaRed()
            RowIdSer = CInt(dtGridServicio.Rows(e.RowIndex).Cells(0).Value)
            Serv.IdTipo = CInt(dtGridServicio.Rows(e.RowIndex).Cells(0).Value)
            Serv.NombreTipo = CStr(dtGridServicio.Rows(e.RowIndex).Cells(1).Value)
            Serv.CuotaTipo = CDbl(dtGridServicio.Rows(e.RowIndex).Cells(2).Value)
            Serv.EstatusTipo = CStr(dtGridServicio.Rows(e.RowIndex).Cells(3).Value)
            MostrarServicio(Serv)
        End If
    End Sub

    Private Sub btnAddServ_Click(sender As Object, e As EventArgs) Handles btnAddServ.Click
        BotonesNuevoServ()
    End Sub

    Private Sub btnCancelarServ_Click(sender As Object, e As EventArgs) Handles btnCancelarServ.Click
        BotonesInicioServ()
    End Sub

    Private Sub btnEliminarServ_Click(sender As Object, e As EventArgs) Handles btnEliminaServ.Click
        Dim ServOperacion As New clsServicio()
        If MessageBox.Show("Continuar para eliminar el Servicio", "INFORMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            Dim dtFolio As DataTable = Nothing
            If String.IsNullOrEmpty(RowIdAdmin) Then
                MsgBox("No ha selecionado ninguna Red", MsgBoxStyle.Information, "AVISO!")
                Exit Sub
            Else
                ServOperacion.IdTipo = RowIdSer
                ServOperacion.NombreTipo = Trim(txtServ.Text)
                ServOperacion.CuotaTipo = Trim(txtCuotaServ.Text)
                ServOperacion.EstatusTipo = "Activo"
                Try
                    BotonesInicioServ()
                    ServOperacion.AgregaModificaEliminaServicio(3, ServOperacion)
                    CargarTiposServicios()
                Catch ex As Exception
                    MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
            End If
        End If
    End Sub

    Private Sub btnModificarServ_Click(sender As Object, e As EventArgs) Handles btnModificaServ.Click
        Dim ServOperacion As New clsServicio()
        Dim estatus As Integer = Nothing
        If String.IsNullOrEmpty(RowIdUsuario) Or RowIdUsuario = 0 Then
            MsgBox("No ha selecionado ningun Servicio", MsgBoxStyle.Information, "AVISO!")
            Exit Sub
        Else
            ServOperacion.IdTipo = RowIdSer
            ServOperacion.NombreTipo = Trim(txtServ.Text)
            ServOperacion.CuotaTipo = Trim(txtCuotaServ.Text)
            ServOperacion.EstatusTipo = Trim(cmbEstatusServ.SelectedItem)
            Try
                BotonesInicioServ()
                ServOperacion.AgregaModificaEliminaServicio(2, ServOperacion)
                CargarTiposServicios()
            Catch ex As Exception
                MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Sub btnSaveServ_Click(sender As Object, e As EventArgs) Handles btnSaveServ.Click
        Dim ServOperacion As New clsServicio()

        If NuevaServicio() = 0 Then
            Exit Sub
        End If

        If VerificaExistenciaServicio() = 1 Then
            MsgBox("Ya existe una Servicio con el mismo Nombre, ingerse uno diferente", MsgBoxStyle.Information, "Aviso!")
            Exit Sub
        End If

        ServOperacion.IdTipo = RowIdSer
        ServOperacion.NombreTipo = Trim(txtServ.Text)
        ServOperacion.CuotaTipo = Trim(txtCuotaServ.Text)
        ServOperacion.EstatusTipo = Trim(cmbEstatusServ.SelectedItem)
        Try
            BotonesInicioServ()
            ServOperacion.AgregaModificaEliminaServicio(1, ServOperacion)
            CargarTiposServicios()
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

#End Region

#Region "RECONEXION"


#End Region

#End Region

#Region "SUB LIMPIAR TabPage"

    Public Sub LimpiarTabAdmin()
        txtFiltro.Clear()
        dtGridAdmin.DataSource = Nothing
        txtNombre.Clear()
        txtPaterno.Clear()
        txtMaterno.Clear()
        cmbTipoAdmin.Text = ""
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

    Public Sub LimpiarTabRedes()
        txtFiltroRed.Clear()
        dtGridRed.Rows.Clear()
        txtNumRed.Clear()
        txtEncargadoRed.Clear()
        cmbTamanoRed.Text = ""
        txtCuotaRed.Clear()
        txtRefRed.Clear()
    End Sub

    Public Sub LimpiarTabServicio()
        dtGridServicio.Rows.Clear()
        txtServ.Clear()
        txtCuotaServ.Clear()
        cmbEstatusServ.SelectedIndex = -1
    End Sub

#End Region

#Region "ENTER TAB'S"

    Private Sub TabAdmin_Enter(sender As Object, e As EventArgs) Handles TabAdmin.Enter
        BotonesInicio()
        CargarTiposAdministradores()
        RowIdAdmin = 0
    End Sub

    Private Sub TabUsuarios_Enter(sender As Object, e As EventArgs) Handles TabUsuarios.Enter
        FechaNacUsu.Format = DateTimePickerFormat.Custom
        FechaNacUsu.CustomFormat = "yyyy-MM-dd"
        LimpiarTabUsuarios()
        CargarRedUsuario()
        CargarServicioUsuario()
        RowIdUsuario = 0
    End Sub

    Private Sub TabRedes_Enter(sender As Object, e As EventArgs) Handles TabRedes.Enter
        LimpiarTabUsuarios()
        CargarRedesTab()
        RowIdRed = 0
    End Sub

    Private Sub TabServicio_Enter(sender As Object, e As EventArgs) Handles TabServ.Enter
        LimpiarTabServicio()
        CargarTiposServicios()
        RowIdSer = 0
    End Sub

#End Region

#Region "LOSTFOCUS TAB'S"

    Private Sub TabAdmin_Leave(sender As Object, e As EventArgs) Handles TabAdmin.Leave
        LimpiarTabAdmin()
        cmbTipoAdmin.Items.Clear()
    End Sub

    Private Sub TabUsuarios_LostFocus(sender As Object, e As EventArgs) Handles TabUsuarios.Leave
        LimpiarTabUsuarios()
        cmbRedUsu.Items.Clear()
        cmbTpoServUsu.Items.Clear()
        RowIdUsuario = 0
    End Sub

    Private Sub TabRedes_LostFocus(sender As Object, e As EventArgs) Handles TabRedes.Leave
        LimpiarTabRedes()
        cmbTamanoRed.Items.Clear()
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
        btnPagar.Visible = False
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
        btnPagar.Visible = True
        btnAddUsu.Enabled = True
        btnModUsu.Enabled = True
        btnEliminarUsu.Enabled = True
    End Sub

#End Region

#Region "REDES"
    Public Sub BotonesInicioRed()
        dtGridRed.Enabled = True
        LimpiarCamposRedes()
        btnSaveRed.Visible = False
        btnCancelarRed.Visible = False
        btnAddRed.Visible = True
        btnModificarRed.Visible = True
        btnEliminaRed.Visible = True
    End Sub

    Public Sub BotonesNuevoRed()
        dtGridRed.Enabled = False
        LimpiarCamposRedes()
        btnSaveRed.Visible = True
        btnCancelarRed.Visible = True
        btnAddRed.Visible = False
        btnModificarRed.Visible = False
        btnEliminaRed.Visible = False
    End Sub

    Public Sub BotonesModificaEliminaRed()
        dtGridRed.Enabled = True
        btnSaveRed.Visible = False
        btnCancelarRed.Visible = False
        btnAddRed.Enabled = True
        btnModificarRed.Enabled = True
        btnEliminaRed.Enabled = True
    End Sub

#End Region

#Region "SERVICIO"

    Public Sub BotonesInicioServ()
        dtGridServicio.Enabled = True
        LimpiarCamposServicios()
        btnSaveServ.Visible = False
        btnCancelarServ.Visible = False
        btnAddServ.Visible = True
        btnModificaServ.Visible = True
        btnEliminaServ.Visible = True
    End Sub

    Public Sub BotonesNuevoServ()
        dtGridRed.Enabled = False
        LimpiarCamposServicios()
        btnSaveServ.Visible = True
        btnCancelarServ.Visible = True
        btnAddServ.Visible = False
        btnModificaServ.Visible = False
        btnEliminaServ.Visible = False
    End Sub

    Public Sub BotonesModificaEliminaServ()
        dtGridRed.Enabled = True
        btnSaveServ.Visible = False
        btnCancelarServ.Visible = False
        btnAddServ.Enabled = True
        btnModificaServ.Enabled = True
        btnEliminaServ.Enabled = True
    End Sub

#End Region

#End Region

#Region "LIMPIA CAMPOS"

    Public Sub LimpiarCamposUsuarios()
        txtNombreUsu.Text = ""
        txtPaternoUsu.Text = ""
        txtMaternoUsu.Text = ""
        FechaNacUsu.Value = Date.Now
        cmbTpoServUsu.SelectedIndex = -1
        cmbRedUsu.SelectedIndex = -1
        txtPrecioTomaUsu.Text = ""
        txtRefUsu.Text = ""
        RowIdUsuario = 0
    End Sub

    Public Sub LimpiarCamposRedes()
        txtNombre.Text = ""
        txtPaterno.Text = ""
        txtMaterno.Text = ""
        txtPwd.Text = ""
        txtPwd1.Text = ""
    End Sub

    Public Sub LimpiarCamposAdmin()
        txtNombre.Text = ""
        txtPaterno.Text = ""
        txtMaterno.Text = ""
        cmbTipoAdmin.SelectedIndex = -1
        cmbEstatusAdmin.SelectedIndex = -1
        txtPwd.Text = ""
        txtPwd1.Text = ""
    End Sub

    Public Sub LimpiarCamposServicios()
        txtServ.Text = ""
        txtCuotaServ.Text = ""
        cmbEstatusServ.SelectedIndex = -1
    End Sub

#End Region


End Class