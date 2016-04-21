Imports System.Data
Imports System.Data.SqlClient


Public Class clsAdministrador

#Region "Variables"
    Dim objSQL As New clsSQLClient()
#End Region

#Region "SP SVT"
    Private sp_CargarAdministradores As String = "Seguridad.sp_CargarAdministradores"
    Private sp_VerificaNuevoAdmin As String = "Operaciones.sp_VerificaNuevoAdmin"
    Private sp_InsertaModificaEliminaAdministrador As String = "CRUD.sp_InsertaModificaEliminaAdministrador"
    Private sp_InicioSessionAdmin As String = "Seguridad.sp_InicioSessionAdmin"
#End Region

#Region "Constructor"

    Public Sub New(admin As clsAdministrador)
        MyBase.New()
        'obten valores para llevarlos a las funciones
        _strIdAdmin = admin.IdAdmin
        _strNombre = admin.NombrAdmin
        _strApePaterno = admin.ApePatAdmin
        _strApeMaterno = admin.ApeMatAdmin
        _strTipoAdmin = admin.TipoAdmin
        _strPwdAdmin = admin.PwdAdmin
        _strEstatusAdmin = admin.EstatusAdmin
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Private _strIdAdmin As Integer
    Public Property IdAdmin() As Integer
        Get
            Return _strIdAdmin
        End Get
        Set(ByVal value As Integer)
            _strIdAdmin = value
        End Set
    End Property

    Private _strNombre As String
    Public Property NombrAdmin() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
        End Set
    End Property

    Private _strApePaterno As String
    Public Property ApePatAdmin() As String
        Get
            Return _strApePaterno
        End Get
        Set(ByVal value As String)
            _strApePaterno = value
        End Set
    End Property

    Private _strApeMaterno As String
    Public Property ApeMatAdmin() As String
        Get
            Return _strApeMaterno
        End Get
        Set(ByVal value As String)
            _strApeMaterno = value
        End Set
    End Property

    Private _strTipoAdmin As String
    Public Property TipoAdmin() As String
        Get
            Return _strTipoAdmin
        End Get
        Set(ByVal value As String)
            _strTipoAdmin = value
        End Set
    End Property

    Private _strPwdAdmin As String
    Public Property PwdAdmin() As String
        Get
            Return _strPwdAdmin
        End Get
        Set(ByVal value As String)
            _strPwdAdmin = value
        End Set
    End Property

    Private _strEstatusAdmin As String
    Public Property EstatusAdmin() As String
        Get
            Return _strEstatusAdmin
        End Get
        Set(ByVal value As String)
            _strEstatusAdmin = value
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Function CargarAdministradores(strTipo As Integer, strFiltro As String) As DataTable
        Dim dtFolio As New DataTable()
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, strTipo, strFiltro)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema al cargar Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function CargarTiposAdministradores(strTipo As Integer, strFiltro As String) As DataTable
        Dim dtFolio As New DataTable()
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, strTipo, strFiltro)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema al cargar Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function VerificaExistenciaAdmin(strPwd As String)
        Dim count As New Integer
        Dim dtFolio As New DataTable()
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaNuevoAdmin, strPwd)
            count = CInt(dtFolio.Rows(0).Item(0))
        Catch ex As Exception
            MsgBox("Problema al buscar Administrador existente: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return count
    End Function

    Public Sub InsertaModificaEliminaAdministrador(strTipo As Integer, AdminOperaciones As clsAdministrador)
        Try
            objSQL.ejecutaProcedimientoTable(sp_InsertaModificaEliminaAdministrador,
                                                strTipo,
                                                AdminOperaciones.IdAdmin,
                                                AdminOperaciones.NombrAdmin,
                                                AdminOperaciones.ApePatAdmin,
                                                AdminOperaciones.ApeMatAdmin,
                                                AdminOperaciones.TipoAdmin,
                                                AdminOperaciones.PwdAdmin,
                                                AdminOperaciones.EstatusAdmin)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Function VarificaAdminInicioSesion(strPwd As String)
        Dim dtFolio As New DataTable()
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_InicioSessionAdmin, strPwd)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

#End Region

End Class
