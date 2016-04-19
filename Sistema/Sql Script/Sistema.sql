
CREATE DATABASE SistemaAgua;
USE SistemaAgua;

CREATE TABLE Catalogos.Administrador
(
	IdAdministrador int primary key not null Identity,
	NomAdmin nvarchar(50),
	ApePatAdmin nvarchar(50),
	ApeMatAdmin nvarchar(50),
	IdTipoAdmin int,
	PwdAdmin nvarchar(50),
	EstaAdmin int
)
INSERT INTO Catalogos.Administrador VALUES ("César","Orozco","Vilchis",1,"123",1),("Admin","Auxiliar","Sistema",2,"1234",2)


CREATE TABLE Catalogos.TipoAdministrador
(
	IdTipoAdmin int primary key not null Identity,
	NomTipo nvarchar(50),
	DescTipo nvarchar(200),
	EstaTipo int
)
INSERT INTO Catalogos.TipoAdministrador VALUES ("Administrador","Administrador del Sistema",1),("Auxiliar","Usuario que realiza Pagos",1)



CREATE TABLE Catalogos.Usuarios
(
	IdUsuario int primary key not null Identity,
	NomUsuario nvarchar(50),
	ApePatUsuario nvarchar(50),
	ApeMatUsuario nvarchar(50),
	FecNacUsuario date,
	IdRed int,
	IdTipo int,
	PrecTomaUsuario float(5,1),
	RefUsuario nvarchar(1000),
	EstaUsuario int
)
INSERT INTO Catalogos.Usuarios VALUES ("Usuario 1","USUARIO1","usuario","1992-05-13",1,1,0,"Cerca de la Escuela Primaria",1),("Usuario 2","Prueba","Sistema","1992-05-13",2,2,0,"En el Centro",1)


CREATE TABLE Catalogos.Redes
(
	IdRed int primary key not null Identity,
	NomRed nvarchar(50),
	EncarRed nvarchar(100),
	IdTamano int,
	CuotaRed float(5,1)
	RefRed nvarchar(1000),
	EstaRed int
)
INSERT INTO Catalogos.Redes VALUES ("RED 1A","Eloy",1,500,"Del pozo a la Escuela",1),("RED 1B","Eloy Salgado",2,100,"De la Escuela al Pozo",1)



CREATE TABLE Catalogos.RedTamano
(
	IdTamano int primary key not null Identity,
	NomTamano nvarchar(50),
	Tamano nvarchar(50)
	EstaTamano int
)
INSERT INTO Catalogos.RedTamano VALUES ("Normal","4 Pulgadas",1),("Pequeño","2 Pulgadas",1)


CREATE TABLE Catalogos.TipoServicio
(
	IdTipo int primary key not null Identity,
	NomTipo nvarchar(50),
	CuotaTipo float(5,1),
	EstaTipo int
)
INSERT INTO Catalogos.TipoServicio VALUES ("DOMESTICO",57,1),("COMERCIAL",122,1),("ESPECIAL",240,1)



CREATE TABLE Catalogos.Pagousuarios
(
	IdPago int primary key not null Identity,
	IdUsuario int,
	IdAdministrador int,
	FechaPago date,
	HoraPago date,
	Enero int,
	Febrero int,
	Marzo int,
	Abril int,
	Mayo int,
	Junio int,
	Julio int,
	Agosto int,
	Septiembre int,
	Octubre int,
	Noviembre int,
	Diciembre int,
	Anio int,
	SubPago float(5,0),
	DescPago float(5,0)
	MontoPago float(5,0)
)

CREATE TABLE Catalogos.DescPago
(
	IdDescuento int primary key not null Identity,
	IdPago int,
	TipoDesc nvarchar(50),
	Cantdesc float(5,1),
	DescDesc nvarchar(1000)
)

CREATE TABLE Catalogos.CorteCaja
(
	IdCorte int primary key not null Identity,
	Fecha date,
	IdAdmin int,
	AdminCorte int,
	NumPagos int,
	MontoCorte float(5,0) 
)

CREATE TABLE Catalogos.PagosCorte
(
	IdCorPag int primary key not null Identity,
	IdPago int,
	IdCorte int,
	EstaCorPag int
)

CREATE TABLE Catalogos.UsuariosDesconectados
(
	IdDesc int primary key not null Identity,
	AdminDesc int,
	FecDesc date,
	AdminCone int,
	FecCone date
)

CREATE TABLE Catalogos.AportUsuarios
(
	IdAportacion int primary key not null Identity,
	IdUsuario int,
	IdTpoAport int,
	DescAport nvarchar(1000),
	CantAport float(5,1),
	EstaAport int
)

CREATE TABLE Catalogos.TipoAportacion
(
	IdAport int primary key not null Identity,
	NomAport nvarchar(50),
	DescAport nvarchar(250)
)
INSERT INTO Catalogos.TipoAportacion VALUES ("APORTACION","ESTA ES UNA CANTIDAD EXTRA SEGUN ACUERDO"),("SANCION","ES UNA MULTA POR ALGUNA INFRACCION QUE EL USUARIO PUEDA COMETER"),("REPARACION","ES EL COSTO DE LA REPARACION POR LA CUAL SE DEVOLVERA EL SERVICIO")


-- AQUI EMPIEZAN LAS VISTAS DE LAS CONSULTAS PARA LA CARGA DE CADA UNA DE LAS TAB DEL SISTEMA


CREATE VIEW [dbo].[CargarAdministradores]
AS
SELECT     Catalogos.Administrador.IdAdministrador, Catalogos.Administrador.NomAdmin, Catalogos.TipoAdministrador.NomTipo, Catalogos.Administrador.ApePatAdmin, 
                      Catalogos.Administrador.ApeMatAdmin, Catalogos.Administrador.EstaAdmin, Catalogos.TipoAdministrador.EstaTipo, Catalogos.TipoAdministrador.IdTipoAdmin
FROM         Catalogos.Administrador INNER JOIN
                      Catalogos.TipoAdministrador ON Catalogos.Administrador.IdTipoAdmin = Catalogos.TipoAdministrador.IdTipoAdmin

GO