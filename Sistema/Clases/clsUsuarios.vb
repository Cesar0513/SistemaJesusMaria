Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic

Public Class clsUsuarios

#Region "Variables"
    Dim objSQL As New clsSQLClient()
    Dim dtFolio As New DataTable()
    Dim dtRetorna As New DataTable()
#End Region

#Region "SP SVT"
    Private sp_CargarUsuarios As String = "Seguridad.sp_CargarUsuarios"
    Private sp_VerificaNuevoUsuario As String = "Operaciones.sp_VerificaNuevoUsuario"
    Private sp_InsertaModificaEliminaUsuario As String = "CRUD.sp_InsertaModificaEliminaUsuario"
#End Region

#Region "Constructor"

    Public Sub New(usuario As clsUsuarios)
        MyBase.New()
        'obten valores para llevarlos a las funciones
        _strIdUsuario = usuario.IdUsuario
        _strNombre = usuario.NombreUsu
        _strApePaterno = usuario.ApePatUsu
        _strApeMaterno = usuario.ApeMatUsu
        _strFecNacUsu = usuario.FecNacUsu
        _strRedUsuario = usuario.RedUsuario
        _strTipoUsuario = usuario.TipoUsuario
        _strPrecUsuario = usuario.Precio
        _strRefUsuario = usuario.RefUsuario
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Private _strIdUsuario As Integer
    Public Property IdUsuario() As Integer
        Get
            Return _strIdUsuario
        End Get
        Set(ByVal value As Integer)
            _strIdUsuario = value
        End Set
    End Property

    Private _strNombre As String
    Public Property NombreUsu() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
        End Set
    End Property

    Private _strApePaterno As String
    Public Property ApePatUsu() As String
        Get
            Return _strApePaterno
        End Get
        Set(ByVal value As String)
            _strApePaterno = value
        End Set
    End Property

    Private _strApeMaterno As String
    Public Property ApeMatUsu() As String
        Get
            Return _strApeMaterno
        End Get
        Set(ByVal value As String)
            _strApeMaterno = value
        End Set
    End Property

    Private _strFecNacUsu As String
    Public Property FecNacUsu() As String
        Get
            Return _strFecNacUsu
        End Get
        Set(ByVal value As String)
            _strFecNacUsu = value
        End Set
    End Property

    Private _strRedUsuario As String
    Public Property RedUsuario() As String
        Get
            Return _strRedUsuario
        End Get
        Set(ByVal value As String)
            _strRedUsuario = value
        End Set
    End Property

    Private _strTipoUsuario As String
    Public Property TipoUsuario() As String
        Get
            Return _strTipoUsuario
        End Get
        Set(ByVal value As String)
            _strTipoUsuario = value
        End Set
    End Property

    Private _strPrecUsuario As Double
    Public Property Precio() As Double
        Get
            Return _strPrecUsuario
        End Get
        Set(ByVal value As Double)
            _strPrecUsuario = value
        End Set
    End Property

    Private _strRefUsuario As String
    Public Property RefUsuario() As String
        Get
            Return _strRefUsuario
        End Get
        Set(ByVal value As String)
            _strRefUsuario = value
        End Set
    End Property

    Private _strEstaUsuario As String
    Public Property EstaUsuario() As String
        Get
            Return _strEstaUsuario
        End Get
        Set(ByVal value As String)
            _strEstaUsuario = value
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Function CargarUsuarios(strTipo As Integer, strFiltro As String) As DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarUsuarios, strTipo, strFiltro)
        Catch ex As Exception
            MsgBox("Ocurrio al Cargar Usuarios: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function VerificaExistenciaUsuario(usuario As clsUsuarios)
        Dim dtFolio As New DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaNuevoUsuario, usuario.NombreUsu, usuario.ApePatUsu, usuario.ApeMatUsu)
        Catch ex As Exception
            MsgBox("Problema al verificar Usuario existente: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Sub AgregaModificaEliminaUsuario(strTipo As Integer, usuario As clsUsuarios)
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_InsertaModificaEliminaUsuario,
                                                        strTipo,
                                                        usuario.IdUsuario,
                                                        usuario.NombreUsu,
                                                        usuario.ApePatUsu,
                                                        usuario.ApeMatUsu,
                                                        usuario.FecNacUsu,
                                                        usuario.RedUsuario,
                                                        usuario.TipoUsuario,
                                                        usuario.Precio,
                                                        usuario.RefUsuario,
                                                        usuario.EstaUsuario)
        Catch ex As Exception
            MsgBox("Problema ejecutar la Operacion del Usuario: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

#End Region

End Class
