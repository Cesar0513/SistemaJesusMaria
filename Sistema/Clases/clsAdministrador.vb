Imports System.Data
Imports System.Data.SqlClient


Public Class clsAdministrador

#Region "Variables"
    Dim objSQL As New clsSQLClient()
    Dim dtFolio As New DataTable()
#End Region

#Region "SP SVT"
    Private sp_CargarAdministradores As String = "Seguridad.sp_CargarAdministradores"
    Private sp_VerificaNuevoAdmin As String = "Operaciones.sp_VerificaNuevoAdmin"
    Private sp_InsertaModificaEliminaAdministrador As String = "CRUD.sp_InsertaModificaEliminaAdministrador"
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

    Public Function CargarAdministradores(strFiltro As String) As DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarAdministradores, strFiltro)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema al cargar Administradores: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function VerificaExistenciaAdmin(strPwd As String)
        Dim dtFolio As New DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaNuevoAdmin, strPwd)
        Catch ex As Exception
            MsgBox("Problema al buscar Administrador existente: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Sub InsertaModificaEliminaAdministrador(strTipo As Integer, admin As clsAdministrador)
        Try
            objSQL.ejecutaProcedimientoTable(sp_InsertaModificaEliminaAdministrador, strTipo, admin.IdAdmin, admin.NombrAdmin, admin.ApePatAdmin, admin.ApeMatAdmin, admin.TipoAdmin, admin.PwdAdmin, admin.EstatusAdmin)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

#End Region

End Class
