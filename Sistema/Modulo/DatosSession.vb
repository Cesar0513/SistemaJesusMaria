Public Module DatosSession

    Private _IdAdmin As Integer
    Public Property IdAdmin() As Integer
        Get
            Return _IdAdmin
        End Get
        Set(ByVal value As Integer)
            _IdAdmin = value
        End Set
    End Property

    Private _NomAdmin As String
    Public Property NomAdmin() As String
        Get
            Return _NomAdmin
        End Get
        Set(ByVal value As String)
            _NomAdmin = value
        End Set
    End Property

    Private _PerfilAdmin As String
    Public Property PerfilAdmin() As String
        Get
            Return _PerfilAdmin
        End Get
        Set(ByVal value As String)
            _PerfilAdmin = value
        End Set
    End Property

    Public Sub DatosSession()

    End Sub


    Public Sub DatosSession(ByVal id As Integer, ByVal nombre As String, ByVal perfil As String)
        _IdAdmin = id
        _NomAdmin = nombre
        _PerfilAdmin = perfil
    End Sub
End Module
