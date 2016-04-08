Module DatosSession

    Dim IdAdmin As Integer = Nothing
    Dim NomAdmin As String = ""
    Dim PerfilAdmin As String = ""

    Public Sub AsignarDatos(ByVal id As Integer, ByVal nombre As String, ByVal perfil As String)
        'IdAdmin = id
        'NomAdmin = nombre
        'PerfilAdmin = perfil
        IdAdmin = 1
        NomAdmin = "Cesar"
        PerfilAdmin = "Administrador"
    End Sub

    Public Function ObtenerValores() As DataTable
        Dim dt As DataTable = Nothing
        Dim celda As DataRow = Nothing
        celda(0) = IdAdmin
        celda(1) = NomAdmin
        celda(1) = PerfilAdmin

        dt.Rows.Add(celda)

        Return dt
    End Function


End Module
