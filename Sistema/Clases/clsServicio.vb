Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic


Public Class clsServicio

#Region "Variables"
    Dim objSQL As New clsSQLClient()
    Dim dtFolio As New DataTable()
#End Region

#Region "SP SVT"
    Private sp_CargarServicio As String = "Seguridad.sp_CargaTiposServicios"
    Private sp_VerificaNuevoServ As String = "Operaciones.sp_VerificaNuevoServ"
    Private sp_InsertaModificaEliminaTipoServicio As String = "CRUD.sp_InsertaModificaEliminaTipoServicio"
#End Region

#Region "Constructor"

    Public Sub New(usuario As clsUsuarios)
        MyBase.New()
        'obten valores para llevarlos a las funciones
        _strIdTipo = IdTipo
        _strNombre = NombreTipo
        _strCuotaTipo = CuotaTipo
        _strEstatusTipo = EstatusTipo
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Private _strIdTipo As Integer
    Public Property IdTipo() As Integer
        Get
            Return _strIdTipo
        End Get
        Set(ByVal value As Integer)
            _strIdTipo = value
        End Set
    End Property

    Private _strNombre As String
    Public Property NombreTipo() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
        End Set
    End Property

    Private _strCuotaTipo As Double
    Public Property CuotaTipo() As Double
        Get
            Return _strCuotaTipo
        End Get
        Set(ByVal value As Double)
            _strCuotaTipo = value
        End Set
    End Property

    Private _strEstatusTipo As String
    Public Property EstatusTipo() As String
        Get
            Return _strEstatusTipo
        End Get
        Set(ByVal value As String)
            _strEstatusTipo = value
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Function CargarServiciosUsuarios() As DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarServicio, 0)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema Cargar Servicios en Tab: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function CargarServicios() As DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_CargarServicio, 1)
        Catch ex As Exception
            MsgBox("Ocurrio el siguiente problema Cargar Servicios: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Function VerificaNuevaServ(strNomRed As String)
        Dim dtFolio As New DataTable
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_VerificaNuevoServ, strNomRed)
        Catch ex As Exception
            MsgBox("Problema al buscar Servicio existente: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Return dtFolio
    End Function

    Public Sub AgregaModificaEliminaServicio(strTipo As Integer, serv As clsServicio)
        Try
            dtFolio = objSQL.ejecutaProcedimientoTable(sp_InsertaModificaEliminaTipoServicio,
                                                        strTipo,
                                                        serv.IdTipo,
                                                        serv.NombreTipo,
                                                        serv.CuotaTipo,
                                                        serv.EstatusTipo)
        Catch ex As Exception
            MsgBox("Problema ejecutar la Operacion del Servicio: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

#End Region

End Class