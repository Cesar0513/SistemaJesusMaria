Imports MySql.Data.MySqlClient


Class clsMysqlClient_1

#Region "Atributos"
    Private odcConexion As MySqlConnection = Nothing
#End Region

#Region "Constructor"
    Public Sub clsMysqlClient_1()
        Dim strCadenaConexion As String = ""
        odcConexion = New MySqlConnection(strCadenaConexion)
    End Sub
#End Region

#Region "Métodos Y Funciones"

    Private Sub Conectar()
        Try
            If odcConexion.State = ConnectionState.Open Then
                Desconectar()
            End If
            odcConexion.Open()
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error conectandose a la base de datos" + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Sub

    Private Sub Desconectar()
        Try
            If odcConexion.State = ConnectionState.Closed Then
                Conectar()
            End If
            odcConexion.Close()
            odcConexion.Dispose()
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error desconectando la base de datos" + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Sub

    'FUNCION PARA SOLO REALIZAR UNA INSTRUCCION
    Public Function Consultar(ByVal strSQL As String) As DataTable
        Try
            Conectar()
            Dim odaAdaptador As New MySqlDataAdapter()
            odaAdaptador.SelectCommand = New MySqlCommand()
            odaAdaptador.SelectCommand.CommandText = strSQL
            odaAdaptador.SelectCommand.Connection = odcConexion
            Dim dsDatos As New DataSet()
            odaAdaptador.Fill(dsDatos)
            Return dsDatos.Tables(0)
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strSQL + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function


    'EJEMPLO DE ENVÍO DE PARÁMETROS
    Public Function Consultar(ByVal strSQL As String, ByVal objParametros As Object) As DataTable
        Try
            Conectar()
            Dim odaAdaptador As New MySqlDataAdapter()
            odaAdaptador.SelectCommand = New MySqlCommand()
            odaAdaptador.SelectCommand.CommandText = strSQL

            Dim objParametro As MySqlParameter
            For Each objParametro In objParametros
                odaAdaptador.SelectCommand.Parameters.Add(objParametro)
            Next

            odaAdaptador.SelectCommand.Connection = odcConexion
            Dim dsDatos As New DataSet
            odaAdaptador.Fill(dsDatos)
            Return dsDatos.Tables(0)
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strSQL + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function


    Public Function Consultar_StoredProcedure(ByVal strNombreProcedimAlmac As String, ByVal objParametros As Object)
        Try
            Conectar()
            Dim odaAdaptador As New MySqlDataAdapter(strNombreProcedimAlmac, odcConexion)
            odaAdaptador.SelectCommand.CommandType = CommandType.StoredProcedure

            Dim objParametro As MySqlParameter
            For Each objParametro In objParametros
                odaAdaptador.SelectCommand.Parameters.Add(objParametro)
            Next

            Dim dsDatos As New DataSet()
            odaAdaptador.Fill(dsDatos)
            Return dsDatos.Tables(0)
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strNombreProcedimAlmac + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function

    Public Function EjecutarOperacion(ByVal strSQL As String) As Integer
        Try
            Conectar()
            Dim odcComando As New MySqlCommand(strSQL, odcConexion)
            odcComando.Connection = odcConexion
            Return odcComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strSQL + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function


    Public Function EjecutarOperacion(ByVal strSQL As String, ByVal objParametros As Object) As Integer

        Try
            Conectar()
            Dim odaAdaptador As New MySqlDataAdapter()
            Dim odcComando As New MySqlCommand(strSQL, odcConexion)
            Dim objParametro As MySqlParameter
            For Each objParametro In objParametros
                odaAdaptador.SelectCommand.Parameters.Add(objParametro)
            Next
            odcComando.Connection = odcConexion
            Return odcComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strSQL + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function



    Public Function EjecutarOperacion_StoredProcedure(ByVal strNombreProcedimAlmac As String, ByVal objParametros As Object) As Integer

        Try
            Conectar()
            Dim odaAdaptador As New MySqlDataAdapter()
            Dim odcComando As New MySqlCommand(strNombreProcedimAlmac, odcConexion)
            odcComando.CommandType = CommandType.StoredProcedure
            For Each objParametro In objParametros
                odaAdaptador.SelectCommand.Parameters.Add(objParametro)
            Next
            Return odcComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strNombreProcedimAlmac + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function


    Public Function ObtenerUltimoIdInsertado(ByVal strNombreProcedimAlmac As String) As Integer
        Try
            Conectar()
            Dim odcComando As New MySqlCommand()
            odcComando.Connection = odcConexion
            Return odcComando.LastInsertedId
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strNombreProcedimAlmac + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function

    Public Function EjecutarOperacion_StoredProcedure_1(ByVal strNombreProcedimAlmac As String, ByVal objParametros As Object) As Integer
        Try
            Conectar()
            Dim odcComando As New MySqlCommand(strNombreProcedimAlmac, odcConexion)
            odcComando.CommandType = CommandType.StoredProcedure
            For Each objParametro In objParametros
                odcComando.Parameters.Add(objParametro)
            Next
            Return odcComando.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("Ha ocurrido un error ejecutando el query:\n" + strNombreProcedimAlmac + "\n\nDetalle del Error:\n" + ex.Message)
        End Try
    End Function

#End Region

End Class

