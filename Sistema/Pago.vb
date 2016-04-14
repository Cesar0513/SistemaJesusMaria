Imports MySql.Data.MySqlClient
Imports MySql.Data.MySqlClient.MySqlDataReader
Imports System.Data.SqlClient
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Public Class Pago

#Region "VARIABLES GLOBALES"
    Public servicio As String = "server = localhost; user id = root; password= root; database = sistemaagua"
    Public conexion As New MySqlConnection(servicio)
    Public id_usuario As Integer
    Public fec_usuario As String
    Public id_red As Integer
    Public rtn As String
    Public MesPagar As String
    Public mesesaPagar As New ArrayList()
    Public mesesaPagarNum As New ArrayList()
    Public mestotal As Integer = 0
    Public MontoPagar As Double = Nothing
    Public ret_meses, ret_descuento, ret_pronto_p, ret_medio_consu, ret_aport As Integer
    Public ret_Cuota, ret_cant_apor, ret_submonto, ret_medio, ret_monto_total As Double
    Public enero_, febrero_, marzo_, abril_, mayo_, junio_, julio_, agosto_, septiembre_, octubre_, noviembre_, diciembre_ As Integer
    Public Administrador As String
#End Region

#Region "SP TVF"
    Dim sp_ObtieneDatosUsuario As String = "ObtieneDatosUsuario"
    Dim sp_ObtienePagosUsuario As String = "ObtienePagosUsuario"
    Dim sp_ObtirneTarifaSaldo As String = "ObtieneMontoTotal"
    Dim sp_ObtieneMontoPendiente As String = "ObtieneMontoPendiente"
    Dim sp_AgregaNuevoPago As String = "AgregaNuevoPago"
    Dim sp_ObtieneNombreAdministrador As String = "ObtieneNombreAdministrador"
#End Region

#Region "LOAD"

    Private Sub Pago1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim usuario As Integer = Principal.cl
        Dim i As Integer = Now.Year
        For i = Now.Year To i - 15 Step -1
            cmbAnio.Items.Add(i)
        Next

        'ObtieneDatosUsuario(usuario)
        'ObtienePagos(usuario, 0)
        'ObtieneMontoPendiente()
    End Sub

#End Region

#Region "SUB FUNCTION"

    Public Sub ObtieneDatosUsuario(ByVal usuario As Integer)
        Try
            rtn = sp_ObtieneDatosUsuario
            Dim DR As MySqlDataReader
            conexion.Open()
            Dim cmd = New MySqlCommand(rtn, conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@usuario", usuario)


            DR = cmd.ExecuteReader()
            If DR.HasRows >= 1 Then
                MsgBox("No Existen Registros de Búsqueda")
            Else
                While DR.Read
                    id_usuario = DR.GetInt16(0)
                    lblNombre.Text = DR.GetString(1)
                    lblFecha.Text = DR.GetString(2)
                    fec_usuario = DR.GetString(2)
                    id_red = DR.GetInt16(3)
                    lblRed.Text = DR.GetString(4)
                End While
                conexion.Close()
            End If
        Catch EX As Exception
            MsgBox("ERROR AL CARGAR DATOS DE USUARIO: " & EX.Message)
        End Try
    End Sub

    Public Sub ObtienePagos(ByVal usuario As Integer, ByVal anio As Integer)
        Dim ano As Integer = Nothing
        Try
            rtn = sp_ObtienePagosUsuario
            Dim DR As MySqlDataReader
            conexion.Open()
            Dim cmd = New MySqlCommand(rtn, conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@usuario", usuario)
            cmd.Parameters.AddWithValue("@anio", anio)

            DR = cmd.ExecuteReader()
            If DR.HasRows() = False Then
                MsgBox("No hay Pagos en este Año")
                LimpiarCampos()
            Else
                While DR.Read
                    If DR.IsDBNull(0) Then
                        Enero.Checked = False
                        Enero.Enabled = False
                    Else
                        Enero.Checked = DR.GetInt16(0)
                        If DR.GetInt16(0) = 1 Then
                            Enero.Enabled = False
                        Else
                            Enero.Enabled = True
                        End If
                    End If

                    If DR.IsDBNull(1) Then
                        Febrero.Checked = False
                        Febrero.Enabled = False
                    Else
                        Febrero.Checked = DR.GetInt16(1)
                        If DR.GetInt16(1) = 1 Then
                            Febrero.Enabled = False
                        Else
                            Febrero.Enabled = True
                        End If
                    End If

                    If DR.IsDBNull(2) Then
                        Marzo.Checked = False
                        Marzo.Enabled = False
                    Else
                        Marzo.Checked = DR.GetInt16(2)
                        If DR.GetInt16(2) = 1 Then
                            Marzo.Enabled = False
                        Else
                            Marzo.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(3) Then
                        Abril.Checked = False
                        Abril.Enabled = True
                    Else
                        Abril.Checked = DR.GetInt16(3)
                        If DR.GetInt16(3) = 1 Then
                            Abril.Enabled = False
                        Else
                            Abril.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(4) Then
                        Mayo.Checked = False
                        Mayo.Enabled = True
                    Else
                        Mayo.Checked = DR.GetInt16(4)
                        If DR.GetInt16(4) = 1 Then
                            Mayo.Enabled = False
                        Else
                            Mayo.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(5) Then
                        Junio.Checked = False
                        Junio.Enabled = True
                    Else
                        Junio.Checked = DR.GetInt16(5)
                        If DR.GetInt16(5) = 1 Then
                            Junio.Enabled = False
                        Else
                            Junio.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(6) Then
                        Julio.Checked = False
                        Julio.Enabled = True
                    Else
                        Julio.Checked = DR.GetInt16(6)
                        If DR.GetInt16(6) = 1 Then
                            Julio.Enabled = False
                        Else
                            Julio.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(7) Then
                        Agosto.Checked = False
                        Agosto.Enabled = True
                    Else
                        Agosto.Checked = DR.GetInt16(7)
                        If DR.GetInt16(7) = 1 Then
                            Agosto.Enabled = False
                        Else
                            Agosto.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(8) Then
                        Septiembre.Checked = False
                        Septiembre.Enabled = True
                    Else
                        Septiembre.Checked = DR.GetInt16(8)
                        If DR.GetInt16(8) = 1 Then
                            Septiembre.Enabled = False
                        Else
                            Septiembre.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(9) Then
                        Octubre.Checked = False
                        Octubre.Enabled = True
                    Else
                        Octubre.Checked = DR.GetInt16(9)
                        If DR.GetInt16(9) = 1 Then
                            Octubre.Enabled = False
                        Else
                            Octubre.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(10) Then
                        Noviembre.Checked = False
                        Noviembre.Enabled = True
                    Else
                        Noviembre.Checked = DR.GetInt16(10)
                        If DR.GetInt16(10) = 1 Then
                            Noviembre.Enabled = False
                        Else
                            Noviembre.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(11) Then
                        Diciembre.Checked = False
                        Diciembre.Enabled = True
                    Else
                        Diciembre.Checked = DR.GetInt16(11)
                        If DR.GetInt16(11) = 1 Then
                            Diciembre.Enabled = False
                        Else
                            Diciembre.Enabled = True
                        End If
                    End If
                    If DR.IsDBNull(12) Then
                        ano = DR.GetString(12)
                    Else
                        ano = DR.GetString(12)
                    End If
                End While
                cmbAnio.SelectedItem = ano
            End If
            conexion.Close()
            CompruebaCambioAnio()
        Catch EX As Exception
            MsgBox("No se encontro el registro " & EX.Message)
        End Try
    End Sub

    Public Sub ObtenerTotales(ByVal mes As String)

    End Sub

    Public Sub ObtieneMontoPendiente()
        Dim aportaciones As Integer = Nothing
        Try
            rtn = sp_ObtieneMontoPendiente
            Dim DR As MySqlDataReader
            conexion.Open()
            Dim cmd = New MySqlCommand(rtn, conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@usuario", id_usuario)

            DR = cmd.ExecuteReader()
            If DR.HasRows >= 1 Then
                MsgBox("No se pudo realizar la operación")
            Else
                While DR.Read
                    aportaciones = DR.GetUInt32(0)
                End While
                If aportaciones >= 1 Then
                    PnlAportacion.Visible = True
                Else
                    PnlAportacion.Visible = False
                End If
                conexion.Close()
            End If
        Catch EX As Exception
            MsgBox("ERROR AL REALIZAR LA OPERACION: " & EX.Message)
        End Try
    End Sub

    Public Sub AjustarMedioConsumo()

    End Sub

    Public Sub PagodeUsuarioComite()
        If UsuarioComite.Checked = True Then
            If MedioConsumo.Checked = True Then
                MedioConsumo.Checked = False
            End If
            If CooperacionExtra.Checked = True Then
                CooperacionExtra.Checked = False
                AgregarAportacion()
            End If
        Else
            MedioConsumo.Checked = False
            CooperacionExtra.Checked = False
            AgregarAportacion()
        End If
        SaldoTotal()
    End Sub

    Public Sub AgregarAportacion()
        If CooperacionExtra.Checked = True Then
            PnlAportacion.Visible = True
            txtCantidadAportacion.Text = Nothing
            txtTpoAportacion.Text = Nothing
            txtDescripcionAportacion.Text = Nothing
        Else
            PnlAportacion.Visible = False
            txtCantidadAportacion.Text = Nothing
            txtTpoAportacion.Text = Nothing
            txtDescripcionAportacion.Text = Nothing
        End If

    End Sub

    Public Sub RealizarPago()
        Dim NombreArchivo As String = Nothing
        Dim folio As Integer = Nothing
        Dim nombre As String = Nothing
        Dim apellidos As String = Nothing
        Dim fec_nac As String = Nothing
        Dim tipo_serv As String = Nothing
        Dim red As String = Nothing
        Dim cuota_recibo As Double = Nothing
        Dim anio As Integer = Nothing
        Dim estatus As Integer = Nothing
        Dim mensaje As String = Nothing



        Dim cadenaMeses As String = Nothing
        anio = cmbAnio.SelectedItem
        ObtenerMeses()
        Dim obj As [Object]
        For Each obj In mesesaPagar
            cadenaMeses += obj + ","
        Next obj
        cadenaMeses = Len(cadenaMeses)
        Try
            rtn = sp_AgregaNuevoPago
            Dim DR As MySqlDataReader
            If conexion.State = ConnectionState.Open Then
                conexion.Close()
            End If
            conexion.Open()
            Dim cmd = New MySqlCommand(rtn, conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@id_usuario", id_usuario)
            cmd.Parameters.AddWithValue("@anio", anio)
            cmd.Parameters.AddWithValue("@cadena_meses", cadenaMeses)
            cmd.Parameters.AddWithValue("@ret_meses", ret_meses)
            cmd.Parameters.AddWithValue("@ret_Cuota", ret_Cuota)
            cmd.Parameters.AddWithValue("@ret_descuento", ret_pronto_p)
            cmd.Parameters.AddWithValue("@ret_pronto_p", ret_pronto_p)
            cmd.Parameters.AddWithValue("@ret_medio_consu", ret_medio_consu)
            cmd.Parameters.AddWithValue("@ret_aport", ret_aport)
            cmd.Parameters.AddWithValue("@tipo_aportacion", txtTpoAportacion.Text)
            cmd.Parameters.AddWithValue("@des_apo", txtDescripcionAportacion.Text)
            cmd.Parameters.AddWithValue("@ret_cant_apor", ret_cant_apor)
            cmd.Parameters.AddWithValue("@ret_submonto", ret_submonto)
            cmd.Parameters.AddWithValue("@ret_medio", ret_medio)
            cmd.Parameters.AddWithValue("@ret_monto_total", ret_monto_total)
            cmd.Parameters.AddWithValue("@in_enero", enero_)
            cmd.Parameters.AddWithValue("in_febrero", febrero_)
            cmd.Parameters.AddWithValue("in_marzo", marzo_)
            cmd.Parameters.AddWithValue("in_abril", abril_)
            cmd.Parameters.AddWithValue("in_mayo", mayo_)
            cmd.Parameters.AddWithValue("in_junio", junio_)
            cmd.Parameters.AddWithValue("in_julio", julio_)
            cmd.Parameters.AddWithValue("in_agosto", agosto_)
            cmd.Parameters.AddWithValue("in_septiembre", septiembre_)
            cmd.Parameters.AddWithValue("in_octubre", octubre_)
            cmd.Parameters.AddWithValue("in_noviembre", noviembre_)
            cmd.Parameters.AddWithValue("in_diciembre", diciembre_)

            DR = cmd.ExecuteReader()
            If DR.HasRows >= 1 Then
                MsgBox("No se pudo realizar la operación")
            Else
                While DR.Read
                    folio = DR.GetInt16(0)
                    nombre = DR.GetString(1)
                    apellidos = DR.GetString(2) & " " & DR.GetString(3)
                    fec_nac = DR.GetString(4)
                    tipo_serv = DR.GetString(6)
                    red = DR.GetString(7)
                    cuota_recibo = DR.GetDouble(8)
                    estatus = DR.GetInt16(9)
                    mensaje = DR.GetString(10)
                    NombreArchivo = folio & "-" & nombre & "-" & apellidos & "-" & DR.GetString(5)
                End While
                conexion.Close()
                If estatus = 1 Then
                    Try
                        CrearPDF(NombreArchivo, folio, nombre, apellidos, fec_nac, tipo_serv, red, cuota_recibo, cadenaMeses, anio, estatus, mensaje)
                        MsgBox(mensaje, MsgBoxStyle.Information, AcceptButton)
                        Me.Close()
                        CargarPdf(NombreArchivo)
                    Catch ex As Exception
                        MsgBox("El pago se registro con exito pero el recibo no se pudo crear ", MsgBoxStyle.Information, AcceptButton)
                    End Try
                Else
                    MsgBox(mensaje, MsgBoxStyle.Information, AcceptButton)
                End If
            End If
        Catch EX As Exception
            MsgBox("ERROR AL REALIZAR LA OPERACION: " & EX.Message)
        End Try
    End Sub

    Public Function DeclaracionMeses(ByVal mes As Integer)
        Dim meses As ArrayList = Nothing
        meses.Add("Enero")
        meses.Add("Febrero")
        meses.Add("Marzo")
        meses.Add("Abril")
        meses.Add("Mayo")
        meses.Add("Junio")
        meses.Add("Julio")
        meses.Add("Agosto")
        meses.Add("Septiembre")
        meses.Add("Octubre")
        meses.Add("Noviembre")
        meses.Add("Diciembre")

        Return meses(mes)
    End Function

    Public Sub SaldoTotal()
        Dim meses As Integer = mestotal
        Dim medio_consumo As Integer = Nothing
        Dim aportacion As Double = Nothing
        Dim usuario_com As Integer

        If meses <= 0 Then
            Exit Sub
        End If

        If MedioConsumo.Checked = True Then
            medio_consumo = 1
        Else
            medio_consumo = 0
        End If

        If UsuarioComite.Checked = True Then
            usuario_com = 1
        Else
            usuario_com = 0
        End If


        If CooperacionExtra.Checked = True Then
            If txtTpoAportacion.Text = "" Then
                MsgBox("Complete el Tipo de Aportación")
                Exit Sub
            ElseIf txtCantidadAportacion.Text = "" Then
                MsgBox("Ingrese la Cantidad de la Aportación")
                Exit Sub
            ElseIf txtDescripcionAportacion.Text = "" Then
                MsgBox("Ingrese la Descripción de la Aportación")
                Exit Sub
            End If
            aportacion = txtCantidadAportacion.Text
        Else
            aportacion = 0
            txtTpoAportacion.Text = Nothing
            txtCantidadAportacion.Text = Nothing
            txtDescripcionAportacion.Text = Nothing
        End If

        Try
            rtn = sp_ObtirneTarifaSaldo
            Dim DR As MySqlDataReader
            conexion.Open()
            Dim cmd = New MySqlCommand(rtn, conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@usuario", id_usuario)
            cmd.Parameters.AddWithValue("@meses", meses)
            cmd.Parameters.AddWithValue("@medio_con", medio_consumo)
            cmd.Parameters.AddWithValue("@aportacion", aportacion)
            cmd.Parameters.AddWithValue("@usuario_com", usuario_com)

            DR = cmd.ExecuteReader()
            If DR.HasRows >= 1 Then
                MsgBox("No se pudo realizar la operación")
            Else
                While DR.Read
                    ret_meses = DR.GetUInt32(0)
                    ret_Cuota = DR.GetFloat(1)
                    ret_descuento = DR.GetUInt32(2)
                    ret_pronto_p = DR.GetUInt32(3)
                    ret_medio_consu = DR.GetUInt32(4)
                    ret_aport = DR.GetUInt32(5)
                    ret_cant_apor = DR.GetFloat(6)
                    ret_submonto = DR.GetFloat(7)
                    ret_medio = DR.GetFloat(8)
                    ret_monto_total = DR.GetFloat(9)
                End While
                conexion.Close()
                lblAportacion.Text = ret_cant_apor
                lblDescuento.Text = ret_medio
                lblSubMonto.Text = ret_submonto
                lblMontoTotal.Text = ret_monto_total
                btnPago.Enabled = True
                If ret_descuento = 1 Then
                    If ret_pronto_p = 1 Then
                        lblTipoDescuento.Text = "Descuento Pronto Pago"
                    End If
                ElseIf ret_medio_consu = 1 Then
                    lblTipoDescuento.Text = "Descuento Medio Consumo"
                Else
                    lblTipoDescuento.Text = "No se aplico Descuento"
                End If
            End If
        Catch EX As Exception
            MsgBox("ERROR AL REALIZAR LA OPERACION: " & EX.Message)
        End Try
    End Sub

    Public Sub CompruebaCambioAnio()
        If Enero.Checked = True And Febrero.Checked = True And Marzo.Checked = True And Abril.Checked = True And Mayo.Checked = True And Junio.Checked = True And Julio.Checked = True And Agosto.Checked = True And Septiembre.Checked = True And Octubre.Checked = True And Noviembre.Checked = True And Diciembre.Checked = True Then
            cmbAnio.Enabled = True
            lblAnioPagado.Visible = True
            cmbAnio.Enabled = True
        Else
            cmbAnio.Enabled = False
            lblAnioPagado.Visible = False
        End If
    End Sub

    Public Sub LimpiarCampos()
        Enero.Checked = False
        Enero.Enabled = True
        Febrero.Checked = False
        Febrero.Enabled = True
        Marzo.Checked = False
        Marzo.Enabled = True
        Abril.Checked = False
        Abril.Enabled = True
        Mayo.Checked = False
        Mayo.Enabled = True
        Junio.Checked = False
        Junio.Enabled = True
        Julio.Checked = False
        Julio.Enabled = True
        Agosto.Checked = False
        Agosto.Enabled = True
        Septiembre.Checked = False
        Septiembre.Enabled = True
        Octubre.Checked = False
        Octubre.Enabled = True
        Noviembre.Checked = False
        Noviembre.Enabled = True
        Diciembre.Checked = False
        Diciembre.Enabled = True
    End Sub

    Public Sub ObtenerMeses()
        If Enero.Checked = True And Enero.Enabled = True Then
            enero_ = 1
        Else
            enero_ = 0
        End If
        If Febrero.Checked = True And Febrero.Enabled = True Then
            febrero_ = 1
        Else
            febrero_ = 0
        End If
        If Marzo.Checked = True And Marzo.Enabled = True Then
            marzo_ = 1
        Else
            marzo_ = 0
        End If
        If Abril.Checked = True And Abril.Enabled = True Then
            abril_ = 1
        Else
            abril_ = 0
        End If
        If Mayo.Checked = True And Mayo.Enabled = True Then
            mayo_ = 1
        Else
            mayo_ = 0
        End If
        If Junio.Checked = True And Junio.Enabled = True Then
            junio_ = 1
        Else
            junio_ = 0
        End If
        If Julio.Checked = True And Julio.Enabled = True Then
            julio_ = 1
        Else
            julio_ = 0
        End If
        If Agosto.Checked = True And Agosto.Enabled = True Then
            agosto_ = 1
        Else
            agosto_ = 0
        End If
        If Septiembre.Checked = True And Septiembre.Enabled = True Then
            septiembre_ = 1
        Else
            septiembre_ = 0
        End If
        If Octubre.Checked = True And Octubre.Enabled = True Then
            octubre_ = 1
        Else
            octubre_ = 0
        End If
        If Noviembre.Checked = True And Noviembre.Enabled = True Then
            noviembre_ = 1
        Else
            noviembre_ = 0
        End If
        If Diciembre.Checked = True And Diciembre.Enabled = True Then
            diciembre_ = 1
        Else
            diciembre_ = 0
        End If

    End Sub

    Public Sub CrearPDF(ByVal NombreArchivo As String, ByVal folio As Integer, ByVal nombre As String, ByVal apellidos As String, ByVal fec_nac As String, ByVal tipo_serv As String, ByVal red As String, ByVal cuota_recibo As Double, ByVal cadenaMeses As String, ByVal anio As Integer, ByVal estatus As Integer, ByVal mensaje As String)
        Dim aportacion As String = Nothing
        Dim cantidad As Double = Nothing
        Dim des_pronto As String = Nothing
        Dim des_medio As String = Nothing

        Administrador = ObtenerAdministrador()


        If ret_descuento = 1 Then
            If ret_pronto_p = 1 Then
                des_pronto = "SI"
                des_medio = "NO"
            Else
                des_pronto = "NO"
            End If
        ElseIf ret_medio_consu = 1 Then
            des_medio = "SI"
            des_pronto = "NO"
        Else
            des_medio = "NO"
        End If



        If ret_aport = 1 Then
            aportacion = txtTpoAportacion.Text & " " & txtDescripcionAportacion.Text
            cantidad = ret_cant_apor
        Else
            aportacion = "NO"
            cantidad = 0
        End If

        Try
            Dim datos As String = NombreArchivo
            Dim doc = New Document(iTextSharp.text.PageSize.LETTER)   'Definimos al autor y el tipo de palabras claves al igual que los margenes
            doc.AddAuthor("César Iván Orozco Vilchis")
            doc.AddKeywords("pdf, PdfWriter; Documento; iTextSharp")
            doc.SetMargins(30.0F, 30.0F, 30.0F, 30.0F)                'Aquí ocupamos una variable de tipo string con la ruta y nombde del archivo
            Dim rut As String = "C:\Backup\recibos\" & datos & ".pdf"   'Procedemos a crear el documento en la ruta antes establecida
            Dim wri = PdfWriter.GetInstance(doc, New FileStream(rut, FileMode.Create))   'Abrimos el documento a ocupar
            doc.Open()
            'doc.Add(New Paragraph(Date.Today & "                                                                                                                                Folio: " & folio))
            'Podemos agregar imágenes a nuestro documento con la siguiente instruccion
            'Dim imgpfd = iTextSharp.text.Image.GetInstance("I:\Backup\Extras\logo.jpg") 'Dirreccion a la imagen que se hace referencia
            'imgpfd.SetAbsolutePosition(50, 650) '//Posicion en el eje carteciano de X y Y
            'imgpfd.ScaleAbsolute(500, 100)   '//Ancho y altura de la imagen
            'doc.Add(imgpfd)   ' // Agrega la imagen al documento

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))



            Dim paragraph = New Paragraph()
            paragraph.Alignment = Element.ALIGN_LEFT
            paragraph.Font = FontFactory.GetFont("Arial", 24)
            paragraph.Add("---------------------------------------------------------------------")
            doc.Add(paragraph)

            doc.Add(New Paragraph(" "))

            Dim table = New PdfPTable(3)
            table.SetWidths({4, 4, 4})
            Dim cell = New PdfPCell()
            cell.Border = 0
            cell.VerticalAlignment = 1
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

            cell.Phrase = New Phrase("NOMBRE")
            table.AddCell(cell)
            cell.Phrase = New Phrase("APELLIDOS")
            table.AddCell(cell)
            cell.Phrase = New Phrase("FECHA NACIMIENTO")
            table.AddCell(cell)

            cell.Phrase = New Phrase(nombre, New Font(Font.Bold, 12))
            table.AddCell(cell)
            cell.Phrase = New Phrase(apellidos, New Font(Font.Bold, 12))
            table.AddCell(cell)
            cell.Phrase = New Phrase(fec_nac, New Font(Font.Bold, 12))
            table.AddCell(cell)

            doc.Add(table)


            doc.Add(New Paragraph(" "))


            Dim table1 = New PdfPTable(3)
            table1.SetWidths({4, 4, 4})
            Dim cell1 = New PdfPCell()
            cell1.Border = 0
            cell1.VerticalAlignment = 1
            cell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            cell1.Phrase = New Phrase("TIPO SERVICIO")
            table1.AddCell(cell1)
            cell1.Phrase = New Phrase("NUMERO DE RED")
            table1.AddCell(cell1)
            cell1.Phrase = New Phrase("CUOTA")
            table1.AddCell(cell1)

            cell1.Phrase = New Phrase(tipo_serv, New Font(Font.Bold, 12))
            table1.AddCell(cell1)
            cell1.Phrase = New Phrase(red, New Font(Font.Bold, 12))
            table1.AddCell(cell1)
            cell1.Phrase = New Phrase(cuota_recibo, New Font(Font.Bold, 12))
            table1.AddCell(cell1)

            doc.Add(table1)


            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))

            Dim table2 = New PdfPTable(4)
            table2.SetWidths({5, 1, 1, 2})
            Dim cell2 = New PdfPCell()
            'cell.Border = 1
            cell2.VerticalAlignment = 1
            cell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            cell2.Phrase = New Phrase("MES(S) A PAGAR")
            table2.AddCell(cell2)
            cell2.Phrase = New Phrase("AÑO A PAGAR")
            table2.AddCell(cell2)
            cell2.Phrase = New Phrase("CUOTA SERVICIO")
            table2.AddCell(cell2)
            cell2.Phrase = New Phrase("TOTAL")
            table2.AddCell(cell2)

            cell2.Phrase = New Phrase(cadenaMeses, New Font(Font.Bold, 12))
            table2.AddCell(cell2)
            cell2.Phrase = New Phrase(anio, New Font(Font.Bold, 12))
            table2.AddCell(cell2)
            cell2.Phrase = New Phrase(cuota_recibo, New Font(Font.Bold, 12))
            table2.AddCell(cell2)
            cell2.Phrase = New Phrase(ret_monto_total, New Font(Font.Bold, 12))
            table2.AddCell(cell2)

            doc.Add(table2)

            doc.Add(New Paragraph(" "))

            Dim table4 = New PdfPTable(2)
            table4.SetWidths({6, 2})
            Dim cell4 = New PdfPCell()
            'cell.Border = 1
            cell4.VerticalAlignment = 1
            cell4.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            cell4.Phrase = New Phrase("APORTACION ORDINARIA")
            table4.AddCell(cell4)
            cell4.Phrase = New Phrase("CANTIDAD")
            table4.AddCell(cell4)

            cell4.Phrase = New Phrase(aportacion, New Font(Font.Bold, 12))
            table4.AddCell(cell4)
            cell4.Phrase = New Phrase(cantidad, New Font(Font.Bold, 12))
            table4.AddCell(cell4)

            'cell4.Phrase = New Phrase(TextBox8.Text, New Font(Font.Bold, 12))
            'table4.AddCell(cell4)
            'cell4.Phrase = New Phrase(TextBox6.Text, New Font(Font.Bold, 12))
            'table4.AddCell(cell4)

            doc.Add(table4)

            doc.Add(New Paragraph(" "))

            Dim table3 = New PdfPTable(2)
            table3.SetWidths({6, 2})
            Dim cell3 = New PdfPCell()
            'cell.Border = 1
            cell3.VerticalAlignment = 1
            cell3.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            cell3.Phrase = New Phrase("DESCRIPCION DE DESCUENTO")
            table3.AddCell(cell3)
            cell3.Phrase = New Phrase("SI/NO")
            table3.AddCell(cell3)

            cell3.Phrase = New Phrase("Descuento por se Mayor de Edad", New Font(Font.Bold, 12))
            table3.AddCell(cell3)
            cell3.Phrase = New Phrase(des_pronto, New Font(Font.Bold, 12))
            table3.AddCell(cell3)

            cell3.Phrase = New Phrase("Descuento de Pronto Pago", New Font(Font.Bold, 12))
            table3.AddCell(cell3)
            cell3.Phrase = New Phrase(des_pronto, New Font(Font.Bold, 12))
            table3.AddCell(cell3)

            cell3.Phrase = New Phrase("Descuento de Medio Consumo", New Font(Font.Bold, 12))
            table3.AddCell(cell3)
            cell3.Phrase = New Phrase(des_medio, New Font(Font.Bold, 12))
            table3.AddCell(cell3)

            doc.Add(table3)

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))

            Dim table5 = New PdfPTable(3)
            table5.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            table5.SetWidths({2, 2, 2})
            Dim cell5 = New PdfPCell()
            cell5.Border = 1
            cell5.VerticalAlignment = 1
            cell5.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            cell5.Phrase = New Phrase("SUBTOTAL")
            table5.AddCell(cell5)
            cell5.Phrase = New Phrase("TOTAL DESCUENTO")
            table5.AddCell(cell5)
            cell5.Phrase = New Phrase("TOTAL A PAGAR")
            table5.AddCell(cell5)

            cell5.Phrase = New Phrase(ret_submonto, New Font(Font.Bold, 12))
            table5.AddCell(cell5)
            cell5.Phrase = New Phrase(ret_medio, New Font(Font.Bold, 12))
            table5.AddCell(cell5)
            cell5.Phrase = New Phrase(ret_monto_total, New Font(Font.Bold, 12))
            table5.AddCell(cell5)
            doc.Add(table5)

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(" "))

            Dim table6 = New PdfPTable(3)
            table6.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            table6.SetWidths({4, 4, 4})
            Dim cell6 = New PdfPCell()
            cell6.Border = 8
            cell6.VerticalAlignment = 1
            cell6.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            cell6.Phrase = New Phrase("QUIEN REGISTRO EL PAGO")
            table6.AddCell(cell6)
            cell6.Phrase = New Phrase("FIRMA")
            table6.AddCell(cell6)
            cell6.Phrase = New Phrase("SELLO")
            table6.AddCell(cell6)

            cell6.Phrase = New Phrase(Administrador, New Font(Font.Bold, 12))
            table6.AddCell(cell6)
            cell6.Phrase = New Phrase("", New Font(Font.Bold, 12))
            table6.AddCell(cell6)
            cell6.Phrase = New Phrase("", New Font(Font.Bold, 12))
            table6.AddCell(cell6)
            doc.Add(table6)
            doc.Close()
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, AcceptButton)
        End Try
    End Sub

    Public Function ObtenerAdministrador()
        Dim id As DataTable
        id = ObtenerValores()
        Try
            rtn = sp_ObtieneNombreAdministrador
            Dim DR As MySqlDataReader
            conexion.Open()
            Dim cmd = New MySqlCommand(rtn, conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@admin", CInt(id.Rows(0).Item(0)))

            DR = cmd.ExecuteReader()
            If DR.HasRows >= 1 Then
                MsgBox("No se pudo realizar la operación")
            Else
                While DR.Read
                    Administrador = DR.GetString(0)
                End While
                conexion.Close()
            End If
        Catch ex As Exception
            MsgBox("Error al obtener el Administrador: " & ex.Message)
        End Try
        Return Administrador
    End Function

    Public Sub CargarPdf(ByVal NombreArchivo As String)
        Try
            Process.Start("C:\Backup\recibos\" & NombreArchivo & ".pdf")
        Catch ex As Exception
            MsgBox("Error al cargar el PDF, por favor abralo manualmente")
        End Try
    End Sub

#End Region

#Region "EVENTOS CONTROLADORES"
    Private Sub Meses_Click(sender As Object, e As EventArgs) Handles Enero.Click, Febrero.Click, Septiembre.Click, Octubre.Click, Noviembre.Click, Mayo.Click, Marzo.Click, Junio.Click, Julio.Click, Diciembre.Click, Agosto.Click, Abril.Click
        Dim check As CheckBox = sender
        Try
            If check.Checked = True And check.Enabled = True Then
                mesesaPagar.Add(check.Name)
                mestotal = mestotal + 1
            Else
                mesesaPagar.Remove(check.Name)
                mestotal = mestotal - 1
            End If
            SaldoTotal()
        Catch ex As Exception
            MsgBox("ERROR AL CARGAR LOS MESES " & ex.Message)
        End Try
    End Sub

    Private Sub CheckBox15_CheckedChanged(sender As Object, e As EventArgs) Handles MedioConsumo.CheckedChanged
        SaldoTotal()
    End Sub

    Private Sub CheckBox12_CheckedChanged(sender As Object, e As EventArgs) Handles UsuarioComite.CheckedChanged
        PagodeUsuarioComite()
    End Sub

    Private Sub CheckBox13_CheckedChanged(sender As Object, e As EventArgs) Handles CooperacionExtra.CheckedChanged
        AgregarAportacion()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnPago.Click
        RealizarPago()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub cmbAnio_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbAnio.SelectionChangeCommitted
        ObtienePagos(id_usuario, cmbAnio.SelectedItem)
    End Sub

    Private Sub txtCantidadAportacion_TextChanged(sender As Object, e As EventArgs) Handles txtCantidadAportacion.TextChanged
        If txtCantidadAportacion.Text = "" Then
            txtCantidadAportacion.Text = Nothing
        End If
        SaldoTotal()
    End Sub

#End Region

End Class