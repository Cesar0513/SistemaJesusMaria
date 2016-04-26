Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic

Public Class clsAportaciones

#Region "Variables"
    Dim objSQL As New clsSQLClient()
    Dim dtFolio As New DataTable()
#End Region

#Region "SP SVT"
    'Private sp_CargarPagosUsuario As String = "Seguridad.sp_CargarPagosUsuarios"

#End Region

#Region "Constructor"

    Public Sub New(Aportacion As clsAportaciones)
        MyBase.New()
        'obten valores para llevarlos a las funciones
        strIdPago = Aportacion.IdPago
        strAportacion = Aportacion.Aportacion
        strDescAportacion = Aportacion.DescAportacion
        strCantidadAportacion = Aportacion.CantAportacion
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Private strIdPago As Integer
    Public Property IdPago() As Integer
        Get
            Return strIdPago
        End Get
        Set(ByVal value As Integer)
            strIdPago = value
        End Set
    End Property

    Private strAportacion As Integer
    Public Property Aportacion() As Integer
        Get
            Return strAportacion
        End Get
        Set(ByVal value As Integer)
            strAportacion = value
        End Set
    End Property

    Private strDescAportacion As String
    Public Property DescAportacion() As String
        Get
            Return strDescAportacion
        End Get
        Set(ByVal value As String)
            strDescAportacion = value
        End Set
    End Property

    Private strCantidadAportacion As Integer
    Public Property CantAportacion() As Integer
        Get
            Return strCantidadAportacion
        End Get
        Set(ByVal value As Integer)
            strCantidadAportacion = value
        End Set
    End Property

#End Region

End Class
