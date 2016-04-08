Imports System.Configuration
Imports MySql.Data
Imports MySql.Data.MySqlClient

Imports Microsoft.VisualBasic

Public Class clsMysqlClient

#Region "Propiedades"
    Dim _strConexion As String
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
        _strConexion = strConexion
        obtenCadenaConexion()
    End Sub
#End Region

#Region "Metodos"
    Public Function obtenCadenaConexion() As String
        strConexion = ConfigurationManager.ConnectionStrings(ConfigurationManager.AppSettings("strDefaultConfigArabelaPorteo").ToString).ConnectionString.ToString
        Return strConexion
    End Function

    Public Function ejecutaNonQuery(ByVal strSQL As String) As Boolean
        Dim objConexion As MySqlConnection = New MySqlConnection(strConexion)

        Try
            If objConexion.State = ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            Dim objComando = New MySqlCommand(strSQL, objConexion)
            objComando.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            If objConexion.State = ConnectionState.Open Then objConexion.Close()
            objConexion.Dispose()
            Throw (ex)
        Finally
            objConexion.Close()
            objConexion.Dispose()
        End Try
    End Function

    Public Function ejecutaDataTable(ByVal strSQL As String) As DataTable
        Dim objConexion As MySqlConnection = New MySqlConnection(strConexion)
        Dim dt As New DataTable
        Try
            If objConexion.State = ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            Dim objComando = New MySqlCommand(strSQL, objConexion)

            Dim da As New MySqlDataAdapter(objComando)
            da.Fill(dt)

        Catch ex As Exception
            If objConexion.State = ConnectionState.Open Then objConexion.Close()
            dt.Dispose()
            objConexion.Dispose()
            Throw (ex)
        Finally
            dt.Dispose()
            objConexion.Close()
            objConexion.Dispose()
        End Try

        Return dt

    End Function

#End Region
End Class

