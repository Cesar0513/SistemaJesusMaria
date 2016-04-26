Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic

Public Class clsRedes

#Region "Variables"
    Dim objSQL As New clsSQLClient()
    Dim dtFolio As New DataTable()
#End Region

#Region "SP SVT"
    Private sp_CargarRedes As String = "Seguridad.sp_CargarRedes"
    Private sp_VerificaNuevoRed As String = "Operaciones.sp_VerificaNuevoRed"
    Private sp_InsertaModificaEliminaRedes As String = "CRUD.sp_InsertaModificaEliminaRedes"
    Private sp_CargaTamanoRed As String = "Seguridad.sp_CargaTamanoRed"
#End Region

#Region "Constructor"

    Public Sub New(usuario As clsUsuarios)
        MyBase.New()
        'obten valores para llevarlos a las funciones
        _strIdRed = IdRed
        _strNombre = NombreRed
        _strEncargado = Encargado
        _strTamano = TamanoRed
        _strCuotaRed = CuotaRed
        _strRefRed = RefRed
        _strEstatusRed = EstatusRed
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    	 Private _strIdRed As Integer
    Public Property IdRed() As Integer
        Get
            Return _strIdRed
        End Get
        Set(ByVal value As Integer)
            _strIdRed = value
        End Set
    End Property

    Private _strNombre As String
    Public Property NombreRed() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
        End Set
    End Property

    Private _strEncargado As String
    Public Property Encargado() As String
        Get
            Return _strEncargado
        End Get
        Set(ByVal value As String)
            _strEncargado = value
        End Set
    End Property

    Private _strTamano As String
    Public Property TamanoRed() As String
        Get
            Return _strTamano
        End Get
        Set(ByVal value As String)
            _strTamano = value
        End Set
    End Property

    Private _strCuotaRed As Double
    Public Property CuotaRed() As Double
        Get
            Return _strCuotaRed
        End Get
        Set(ByVal value As Double)
            _strCuotaRed = value
        End Set
    End Property

    Private _strRefRed As String
    Public Property RefRed() As String
        Get
            Return _strRefRed
        End Get
        Set(ByVal value As String)
            _strRefRed = value
        End Set
    End Property

    Private _strEstatusRed As String
    Public Property EstatusRed() As String
        Get
            Return _strEstatusRed
        End Get
        Set(ByVal value As String)
            _strEstatusRed = value
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Function CargarRedes(strTipo As Integer, strFiltro As String) As DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarRedes, strTipo, strFiltro)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function VerificaNuevaRed(strNomRed As String)
        Dim dtFolio As New DataTable
        Dim folio As Integer = 0
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaNuevoRed, strNomRed)
            folio = CInt(dtFolio.Rows(0).Item(0))
        Catch ex As Exception
            MsgBox("Problema al buscar Usuario existente: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return folio
    End Function

    Public Sub AgregaModificaEliminaRed(strTipo As Integer, red As clsRedes)
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_InsertaModificaEliminaRedes,
                                                        strTipo,
                                                        red.IdRed,
                                                        red.NombreRed,
                                                        red.Encargado,
                                                        red.TamanoRed,
                                                        red.CuotaRed,
                                                        red.RefRed,
                                                        red.EstatusRed)
        Catch ex As Exception
            MsgBox("Problema ejecutar la Operacion del Usuario: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Function CargarTamanoRed()
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargaTamanoRed)
        Catch ex As Exception
            MsgBox("Problema ejecutar la Operacion del Usuario: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

#End Region

End Class

