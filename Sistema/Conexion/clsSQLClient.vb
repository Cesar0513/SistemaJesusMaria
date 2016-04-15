Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic

Public Class clsSQLClient


#Region "VAriables"
    Private _strConfiguracion As String
    Public strConfiguracion As String = ConfigurationManager.AppSettings("strDefaultConfig").ToString
#End Region


#Region "Propiedades"
    Public cnn As New SqlConnection()

    Public Property _cnn() As SqlConnection
        Get
            Return cnn
        End Get
        Set(ByVal value As SqlConnection)
            cnn = value
        End Set
    End Property

    Private _strServer As String
    Public Property strServer() As String
        Get
            Return _strServer
        End Get
        Set(ByVal value As String)
            _strServer = value
        End Set
    End Property

    Private _strLink As String
    Public Property strLink() As String
        Get
            Return _strLink
        End Get
        Set(ByVal value As String)
            _strLink = value
        End Set
    End Property

    Private _strBase As String
    Public Property strBase() As String
        Get
            Return _strBase
        End Get
        Set(ByVal value As String)
            _strBase = value
        End Set
    End Property

    Private _strUid As String
    Public Property strUid() As String
        Get
            Return _strUid
        End Get
        Set(ByVal value As String)
            _strUid = value
        End Set
    End Property

    Private _strPassword As String
    Public Property strPassword() As String
        Get
            Return _strPassword
        End Get
        Set(ByVal value As String)
            _strPassword = value
        End Set
    End Property

    Private _strConexion As String
    Public Property strConexion() As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        MyBase.New()
        'obten valores para llevarlos a las funciones
        _strConfiguracion = strConfiguracion
        _strConexion = strConexion
        _strServer = strServer
        _strLink = strLink
        _strBase = strBase
        _strUid = strUid
        _strPassword = strPassword
        _cnn = cnn

        obtenCadenaConexion()
        obtenValoresConexion()
    End Sub

#End Region

#Region "Métodos"

    Public Sub obtenValoresConexion()

        strServer = ConfigurationManager.AppSettings(ConfigurationManager.AppSettings("strDefaultServer").ToString).ToString
        strBase = ConfigurationManager.AppSettings(ConfigurationManager.AppSettings("strDefaultBase").ToString).ToString
        strUid = ConfigurationManager.AppSettings(ConfigurationManager.AppSettings("strDefaultUid").ToString).ToString
        strPassword = ConfigurationManager.AppSettings(ConfigurationManager.AppSettings("strDefaultPassword").ToString).ToString

    End Sub

    Public Function obtenCadenaConexion() As String
        strConexion = ConfigurationManager.AppSettings(strConfiguracion).ToString()
        Return strConexion
    End Function

    Public Function abreConexion() As Integer
        Try
            cnn = New SqlConnection(strConexion)
            If cnn.State = Data.ConnectionState.Open Then cnn.Close()
            cnn.Open()

        Catch ex As Exception
            If cnn.State = Data.ConnectionState.Open Then cnn.Close()
            Throw (ex)
        End Try

    End Function

    Public Function cierraConexion() As Integer
        Try
            If cnn.State = Data.ConnectionState.Open Then cnn.Close()
            cnn.Dispose()

        Catch ex As Exception
            If cnn.State = Data.ConnectionState.Open Then cnn.Close()
            cnn.Dispose()
            Throw (ex)
        End Try

    End Function

    Public Function creaObjComando(ByVal strComando As String, ByVal objConexion As SqlConnection, ByVal strTipo As String, ByVal ParamArray arrSPParametros() As Object) As SqlCommand
        Dim objComando As SqlCommand
        objComando = New SqlCommand(strComando, objConexion)
        objComando.CommandType = CommandType.StoredProcedure

        Dim parametersNames(0) As String
        'carga parametros de la función
        SqlCommandBuilder.DeriveParameters(objComando)
        Dim i As Integer = 0
        For Each parameter As SqlParameter In objComando.Parameters
            ReDim Preserve parametersNames(i)
            If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                parameter.Value = arrSPParametros(i)
                parametersNames(i) = parameter.ParameterName
                i = i + 1
            End If
        Next

        If strTipo = "TVF" Then
            objComando.CommandText = "SELECT * FROM " & strComando & "(" & String.Join(",", parametersNames) & ")"
            objComando.CommandType = CommandType.Text
        End If


        Return objComando
    End Function

#Region "Conexion a SQL Directa"

    Public Function ejecutaReader(ByVal objConexion As SqlConnection, ByVal objComando As SqlCommand, _
                             ByVal strSQL As String) As SqlDataReader
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New SqlCommand(strSQL, objConexion)

            Dim objReader As SqlDataReader = objComando.ExecuteReader(CommandBehavior.CloseConnection)

            Return objReader

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        End Try

    End Function

    Public Sub ejecutaNonQuery(ByVal objConexion As SqlConnection, ByVal objComando As SqlCommand, ByVal strSQL As String)
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New SqlCommand(strSQL, objConexion)
            objComando.ExecuteNonQuery()

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        Finally
            objConexion.Close()
            objConexion.Dispose()
        End Try
    End Sub

#End Region

#Region "Conexión a SQL por Stored Procedures"

    Public Function ejecutaProcedimientoNonQuery(ByVal strSPNombre As String, ByVal ParamArray arrSPParametros() As Object) As Integer
        Dim objConexion As SqlConnection
        Dim objComando As SqlCommand
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New SqlCommand(strSPNombre, objConexion)
            objComando.CommandType = CommandType.StoredProcedure

            'carga parametros del store procedure
            SqlCommandBuilder.DeriveParameters(objComando)
            Dim i As Integer = 0
            For Each parameter As SqlParameter In objComando.Parameters
                If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                    parameter.Value = arrSPParametros(i)
                    i = i + 1
                End If
            Next

            objComando.ExecuteNonQuery()

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        Finally
            objComando.Dispose()
            objConexion.Close()
            objConexion.Dispose()
        End Try

    End Function


    Public Function ejecutaProcedimientoNonQueryDatatable(ByVal strSPNombre As String, ByVal ParamArray arrSPParametros() As Object) As Integer
        Dim objConexion As SqlConnection
        Dim objComando As SqlCommand
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New SqlCommand(strSPNombre, objConexion)
            objComando.CommandType = CommandType.StoredProcedure

            'carga parametros del store procedure
            SqlCommandBuilder.DeriveParameters(objComando)

            '-------------------------------------
            Dim nameParameter As String
            Dim iParameter As Integer
            '-------------------------------------
            Dim i As Integer = 0
            For Each parameter As SqlParameter In objComando.Parameters
                If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                    parameter.Value = arrSPParametros(i)
                    i = i + 1
                    'Para obtener el nombre del parámetro y verifcar si tiene esquema
                    '------------------------------------------------------------------
                    nameParameter = parameter.TypeName
                    iParameter = nameParameter.IndexOf(".")
                    If iParameter = -1 Then
                        Continue For
                    End If
                    nameParameter = nameParameter.Substring(iParameter + 1)
                    If nameParameter.Contains(".") Then
                        parameter.TypeName = nameParameter
                    End If
                    '------------------------------------------------------------------
                End If
            Next

            objComando.ExecuteNonQuery()

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        Finally
            objComando.Dispose()
            objConexion.Close()
            objConexion.Dispose()
        End Try

    End Function



    Public Function ejecutaProcedimientoReader(ByVal objConexion As SqlConnection, ByVal objComando As SqlCommand, _
                                               ByVal strSPNombre As String, ByVal ParamArray arrSPParametros() As Object) As SqlDataReader
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New SqlCommand(strSPNombre, objConexion)
            objComando.CommandType = CommandType.StoredProcedure

            'carga parametros del store procedure
            SqlCommandBuilder.DeriveParameters(objComando)
            Dim i As Integer = 0
            For Each parameter As SqlParameter In objComando.Parameters
                If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                    parameter.Value = arrSPParametros(i)
                    i = i + 1
                End If
            Next
            'For i As Integer = 0 To arrSPParametros.GetUpperBound(0)
            '    objComando.Parameters(i + 1).Value = arrSPParametros(i)
            '    'para obtener el nombre del parametro i 
            '    'objComando.Parameters.Item(i).ParameterName
            'Next

            Dim objReader As SqlDataReader = objComando.ExecuteReader(CommandBehavior.CloseConnection)

            Return objReader

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        End Try

    End Function

    Public Function ejecutaProcedimientoTable(ByVal strSPNombre As String, ByVal ParamArray arrSPParametros() As Object) As DataTable
        Dim objConexion As SqlConnection
        Dim objComando As SqlCommand

        Dim dt As New DataTable
        Try


            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New SqlCommand(strSPNombre, objConexion)
            objComando.CommandType = CommandType.StoredProcedure
            objComando.CommandTimeout = 0

            'carga parametros del store procedure
            SqlCommandBuilder.DeriveParameters(objComando)
            Dim i As Integer = 0
            For Each parameter As SqlParameter In objComando.Parameters
                If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                    parameter.Value = arrSPParametros(i)
                    i = i + 1
                    'para obtener el nombre del parametro  
                    'parameter.ParameterName
                End If
            Next

            Dim da As SqlDataAdapter = New SqlDataAdapter(objComando)
            da.Fill(dt)

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        Finally
            dt.Dispose()
            objComando.Dispose()
            objConexion.Close()
            objConexion.Dispose()
        End Try
        Return dt

    End Function

    Public Function ejecutaProcedimientoTableDataTable(ByVal strSPNombre As String, ByVal ParamArray arrSPParametros() As Object) As DataTable
        Dim objConexion As SqlConnection
        Dim objComando As SqlCommand

        Dim dt As New DataTable
        Try


            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New SqlCommand(strSPNombre, objConexion)
            objComando.CommandType = CommandType.StoredProcedure
            objComando.CommandTimeout = 0

            'carga parametros del store procedure
            SqlCommandBuilder.DeriveParameters(objComando)
            Dim i As Integer = 0
            '----------
            Dim nameParameter As String
            Dim iParameter As Integer
            '----------
            For Each parameter As SqlParameter In objComando.Parameters
                If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                    parameter.Value = arrSPParametros(i)
                    i = i + 1
                    'para obtener el nombre del parametro  
                    'parameter.ParameterName
                    '-----------------------------------------------
                    nameParameter = parameter.TypeName
                    iParameter = nameParameter.IndexOf(".")
                    If iParameter = -1 Then
                        Continue For
                    End If
                    nameParameter = nameParameter.Substring(iParameter + 1)
                    If nameParameter.Contains(".") Then
                        parameter.TypeName = nameParameter
                    End If
                    '-----------------------------------------------
                End If
            Next

            Dim da As SqlDataAdapter = New SqlDataAdapter(objComando)
            da.Fill(dt)

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        Finally
            dt.Dispose()
            objComando.Dispose()
            objConexion.Close()
            objConexion.Dispose()
        End Try
        Return dt

    End Function

    Public Function ejecutaProcedimientoScalar(ByVal strSPNombre As String, _
                                               ByVal ParamArray arrSPParametros() As Object) As Object
        Dim objConexion As SqlConnection
        Dim objComando As SqlCommand
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = creaObjComando(strSPNombre, objConexion, "SP", arrSPParametros)

            'el parametro objComando.Parameters(0) es "@RETURN_VALUE"
            objComando.Parameters(0).Direction = ParameterDirection.ReturnValue
            objComando.ExecuteScalar()

            Dim valSalida As Object = objComando.Parameters(0).Value

            Return valSalida
        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        Finally
            objComando.Dispose()
            objConexion.Close()
            objConexion.Dispose()
        End Try

    End Function

#End Region

#Region "Conexión a SQL por Table-valued Functions"

    Public Function ejecutaTVFDataReader(ByVal strTVNombre As String, ByVal ParamArray arrSPParametros() As Object) As SqlDataReader
        Dim objConexion As SqlConnection
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            Dim objComando As SqlCommand = creaObjComando(strTVNombre, objConexion, "TVF", arrSPParametros)

            Dim objReader As SqlDataReader = objComando.ExecuteReader(CommandBehavior.CloseConnection)

            Return objReader

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        End Try

    End Function

    Public Function ejecutaTVFDataTable(ByVal strTVNombre As String, ByVal ParamArray arrSPParametros() As Object) As DataTable
        Dim objConexion As SqlConnection
        Dim dt As New DataTable
        Try

            objConexion = New SqlConnection(strConexion)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            Dim objComando As SqlCommand = creaObjComando(strTVNombre, objConexion, "TVF", arrSPParametros)
            objComando.CommandTimeout = 0

            Dim da As New SqlDataAdapter(objComando)
            da.Fill(dt)

        Catch ex As Exception
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            Throw (ex)
        Finally
            dt.Dispose()
            objConexion.Close()
            objConexion.Dispose()
        End Try

        Return dt

    End Function

#End Region

#End Region

End Class
