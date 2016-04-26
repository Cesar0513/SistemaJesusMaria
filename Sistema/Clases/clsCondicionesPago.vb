Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic


Public Class clsCondicionesPago

#Region "Variables"
    Dim objSQL As New clsSQLClient()
    Dim dtFolio As New DataTable()
#End Region

#Region "SP SVT"
    Private sp_CargarPagosUsuario As String = "Seguridad.sp_CargarPagosUsuarios"

#End Region

#Region "Constructor"

    Public Sub New(descuento As clsCondicionesPago)
        MyBase.New()
        'obten valores para llevarlos a las funciones
        strIdPago = descuento.IdPago
        strDescPronto = descuento.DescPronto
        strDescMedio = descuento.DescMedio
        strDescUsuarioCom = descuento.DescUsuarioCom
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

    Private strDescPronto As Integer
    Public Property DescPronto() As Integer
        Get
            Return strDescPronto
        End Get
        Set(ByVal value As Integer)
            strDescPronto = value
        End Set
    End Property

    Private strDescMedio As Integer
    Public Property DescMedio() As Integer
        Get
            Return strDescMedio
        End Get
        Set(ByVal value As Integer)
            strDescMedio = value
        End Set
    End Property

    Private strDescUsuarioCom As Integer
    Public Property DescUsuarioCom() As Integer
        Get
            Return strDescUsuarioCom
        End Get
        Set(ByVal value As Integer)
            strDescUsuarioCom = value
        End Set
    End Property

#End Region


End Class
