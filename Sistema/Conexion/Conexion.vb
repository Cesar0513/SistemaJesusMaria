Imports MySql.Data
Imports MySql.Data.MySqlClient
Public Class Conexion

    Private Comm As New MySqlCommand
    Dim conString As String = "server=Miservidor; user id= myusuario ; password=myContraseña"
    Public con As New MySqlConnection(conString)

    Public Sub AbrirConexion()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If

        Try
            con.Open()
        Catch ex As Exception
            MsgBox("Error al abrir la conexión")
        End Try
    End Sub


    Public Function EjecutaStore(ByVal NombreStore As String, ByVal ParamArray parametros() As Object) As DataTable
        Dim dt As New DataTable
        Try
            Dim da As New MySqlDataAdapter
            Dim comm As New MySqlCommand(NombreStore, con)
            comm.CommandType = CommandType.StoredProcedure
            AbrirConexion()

            'carga parametros del store procedure
            MySqlCommandBuilder.DeriveParameters(comm)
            Dim i As Integer = 0
            For Each parameter As MySqlParameter In comm.Parameters
                If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                    parameter.Value = parametros(i)
                    i = i + 1
                    'para obtener el nombre del parametro  
                    'parameter.ParameterName
                End If
            Next

            'comm.Parameters.AddWithValue("@" & nombre & "", nombre)
            'comm.Parameters.AddWithValue("@" & valor & "", valor)

            da.SelectCommand = comm
            da.SelectCommand.Connection = con

            Dim ds As MySqlDataAdapter = New MySqlDataAdapter(comm)
            da.Fill(dt)
            con.Close()
            Return dt
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
            Dim ds As DataSet = Nothing
            Return dt
        End Try
    End Function


    Public Function ejecutaProcedimiento(ByVal strSPNombre As String, ByVal ParamArray arrSPParametros() As Object) As DataTable
        Dim objConexion As MySqlConnection
        Dim objComando As MySqlCommand

        Dim dt As New DataTable
        Try


            objConexion = New MySqlConnection(conString)
            If objConexion.State = Data.ConnectionState.Open Then objConexion.Close()
            objConexion.Open()
            objComando = New MySqlCommand(strSPNombre, objConexion)
            objComando.CommandType = CommandType.StoredProcedure
            objComando.CommandTimeout = 0

            'carga parametros del store procedure
            MySqlCommandBuilder.DeriveParameters(objComando)
            Dim i As Integer = 0
            For Each parameter As MySqlParameter In objComando.Parameters
                If parameter.Direction = ParameterDirection.Input Or parameter.Direction = ParameterDirection.InputOutput Then
                    parameter.Value = arrSPParametros(i)
                    i = i + 1
                    'para obtener el nombre del parametro  
                    'parameter.ParameterName
                End If
            Next

            Dim da As MySqlDataAdapter = New MySqlDataAdapter(objComando)
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

End Class
