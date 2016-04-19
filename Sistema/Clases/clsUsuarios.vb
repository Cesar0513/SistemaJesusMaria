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
    Private sp_VerificaAdminExistente As String = "Seguridad.sp_sp_VerificaUsuarioExistente"
    Private sp_CargarUsuarios As String = "Seguridad.sp_CargarUsuarios"
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

#End Region

#Region "Métodos"

    Public Function CargarUsuarios(strFiltro As String) As DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarUsuarios, strFiltro)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Sub VerificaExistenciaUsuario(usuario As clsUsuarios)
        Dim dtFolio As New DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaAdminExistente)

            If dtFolio.Rows.Count = 0 Then
                MsgBox("Sin resultados", MsgBoxStyle.Information, "Aviso!")
            Else
                'If CInt(dtFolio.Rows(0).Item(0)) >= 1 Then

                'Else

                'End If
            End If
        Catch ex As Exception
            MsgBox("Problema al buscar Usuario existente: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub AgregaNuevoUsuario(usuario As clsUsuarios)

    End Sub

#End Region

End Class
