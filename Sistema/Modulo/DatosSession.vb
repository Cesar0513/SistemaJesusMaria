Public Module DatosSession

    Public IdAdmin As Integer = Nothing
    Public NomAdmin As String = ""
    Public PerfilAdmin As String = ""

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
        celda("IdAdmin") = IdAdmin
        celda("NomAdmin") = NomAdmin
        celda("PerfilAdmin") = PerfilAdmin

        dt.Rows.Add(celda)
        Return dt
    End Function
End Module
