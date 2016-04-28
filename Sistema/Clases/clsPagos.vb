Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic


Public Class clsPagos

#Region "Variables"
    Dim objSQL As New clsSQLClient()
    Dim dtFolio As New DataTable()
#End Region

#Region "SP SVT"
    Private sp_CargarDatosUsuarios As String = "Seguridad.sp_CargarDatosUsuarios"
    Private sp_CargarPagosUsuario As String = "Seguridad.sp_CargarPagosUsuarios"
    Private sp_VerificaPagoUsuario As String = "Pagos.sp_VerificaPagoUsuario"
    Private sp_CalcularMontoPagar As String = "Pagos.sp_CalcularMontoPagar"
    Private sp_RealizarPago As String = "Pagos.sp_RealizarPagoUsuario"

#End Region

#Region "Constructor"

    Public Sub New(pago As clsPagos)
        MyBase.New()
        'obten valores para llevarlos a las funciones
        strIdPago = pago.IdPago
        strIdUsuario = pago.IdUsuario
        strIdAdministrador = pago.IdAdministrador
        strFechaPago = pago.FechaPago
        strMeses = pago.Meses
        strEnero = pago.Enero
        strFebrero = pago.Febrero
        strMarzo = pago.Marzo
        strAbril = pago.Abril
        strMayo = pago.Mayo
        strJunio = pago.Junio
        strJulio = pago.Julio
        strAgosto = pago.Agosto
        strSeptiembre = pago.Septiembre
        strOctubre = pago.Octubre
        strNoviembre = pago.Noviembre
        strDiciembre = pago.Diciembre
        strAnio = pago.Anio
        strSubPago = pago.SubPago
        strDescPago = pago.DescPago
        strMontoPago = pago.MontoPago
        strPagoCorte = pago.PagoCorte
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Private strIdPago as Integer
    Public Property IdPago() As Integer
        Get
            Return strIdPago
        End Get
        Set(ByVal value As Integer)
            strIdPago = value
        End Set
    End Property

    Private strIdUsuario As Integer
    Public Property IdUsuario() As Integer
        Get
            Return strIdUsuario
        End Get
        Set(ByVal value As Integer)
            strIdUsuario = value
        End Set
    End Property

    Private strIdAdministrador As Integer
    Public Property IdAdministrador() As Integer
        Get
            Return strIdAdministrador
        End Get
        Set(ByVal value As Integer)
            strIdAdministrador = value
        End Set
    End Property

    Private strFechaPago As Date
    Public Property FechaPago() As Date
        Get
            Return strFechaPago
        End Get
        Set(ByVal value As Date)
            strFechaPago = value
        End Set
    End Property

    Private strMeses As String
    Public Property Meses() As String
        Get
            Return strMeses
        End Get
        Set(ByVal value As String)
            strMeses = value
        End Set
    End Property

    Private strEnero As Integer
    Public Property Enero() As Integer
        Get
            Return strEnero
        End Get
        Set(ByVal value As Integer)
            strEnero = value
        End Set
    End Property

    Private strFebrero As Integer
    Public Property Febrero() As Integer
        Get
            Return strFebrero
        End Get
        Set(ByVal value As Integer)
            strFebrero = value
        End Set
    End Property

    Private strMarzo As Integer
    Public Property Marzo() As Integer
        Get
            Return strMarzo
        End Get
        Set(ByVal value As Integer)
            strMarzo = value
        End Set
    End Property

    Private strAbril As Integer
    Public Property Abril() As Integer
        Get
            Return strAbril
        End Get
        Set(ByVal value As Integer)
            strAbril = value
        End Set
    End Property

    Private strMayo As Integer
    Public Property Mayo() As Integer
        Get
            Return strMayo
        End Get
        Set(ByVal value As Integer)
            strMayo = value
        End Set
    End Property

    Private strJunio As Integer
    Public Property Junio() As Integer
        Get
            Return strJunio
        End Get
        Set(ByVal value As Integer)
            strJunio = value
        End Set
    End Property

    Private strJulio As Integer
    Public Property Julio() As Integer
        Get
            Return strJulio
        End Get
        Set(ByVal value As Integer)
            strJulio = value
        End Set
    End Property

    Private strAgosto As Integer
    Public Property Agosto() As Integer
        Get
            Return strAgosto
        End Get
        Set(ByVal value As Integer)
            strAgosto = value
        End Set
    End Property

    Private strSeptiembre As Integer
    Public Property Septiembre() As Integer
        Get
            Return strSeptiembre
        End Get
        Set(ByVal value As Integer)
            strSeptiembre = value
        End Set
    End Property

    Private strOctubre As Integer
    Public Property Octubre() As Integer
        Get
            Return strOctubre
        End Get
        Set(ByVal value As Integer)
            strOctubre = value
        End Set
    End Property

    Private strNoviembre As Integer
    Public Property Noviembre() As Integer
        Get
            Return strNoviembre
        End Get
        Set(ByVal value As Integer)
            strNoviembre = value
        End Set
    End Property

    Private strDiciembre As Integer
    Public Property Diciembre() As Integer
        Get
            Return strDiciembre
        End Get
        Set(ByVal value As Integer)
            strDiciembre = value
        End Set
    End Property

    Private strAnio As Integer
    Public Property Anio() As Integer
        Get
            Return strAnio
        End Get
        Set(ByVal value As Integer)
            strAnio = value
        End Set
    End Property

    Private strSubPago As Double
    Public Property SubPago() As Double
        Get
            Return strSubPago
        End Get
        Set(ByVal value As Double)
            strSubPago = value
        End Set
    End Property

    Private strDescPago As Double
    Public Property DescPago() As Double
        Get
            Return strDescPago
        End Get
        Set(ByVal value As Double)
            strDescPago = value
        End Set
    End Property

    Private strMontoPago As Double
    Public Property MontoPago() As Double
        Get
            Return strMontoPago
        End Get
        Set(ByVal value As Double)
            strMontoPago = value
        End Set
    End Property

    Private strPagoCorte As Double
    Public Property PagoCorte() As Double
        Get
            Return strPagoCorte
        End Get
        Set(ByVal value As Double)
            strPagoCorte = value
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Function CargarDatosUsuario(strIdUsuario As Integer)
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarDatosUsuarios, strIdUsuario)
        Catch ex As Exception
            MsgBox("Ocurrio al Cargar Pagos: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function CargarPagos(strTipo As Integer, strIdAdministrador As String, fecha1 As String, fecha2 As String) As DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarPagosUsuario, strTipo, strIdAdministrador, fecha1, fecha2)
        Catch ex As Exception
            MsgBox("Ocurrio al Cargar Pagos: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function VerificaPagosPorMes(pago As clsPagos)
        Dim ret As Integer = Nothing
        ret = 0
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaPagoUsuario, pago)
            ret = CInt(dtFolio.Rows(0).Item(0))
        Catch ex As Exception
            MsgBox("Ocurrio al Cargar Pagos: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return ret
    End Function

    Public Sub InsertaNuevoPago(pago As clsPagos, condiciones As clsCondicionesPago)
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_RealizarPago, pago)
        Catch ex As Exception
            MsgBox("Ocurrio al Cargar Pagos: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Function CalculaMonto(pago As clsPagos, condiciones As clsCondicionesPago, aportacion As clsAportaciones)
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CalcularMontoPagar, pago)
        Catch ex As Exception
            MsgBox("Ocurrio al Cargar Pagos: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Return dtFolio
    End Function

#End Region

End Class
