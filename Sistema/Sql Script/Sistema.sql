

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
INSERT INTO Catalogos.Administrador VALUES ('César','Orozco','Vilchis',1,'123',1)
INSERT INTO Catalogos.Administrador VALUES('Admin','Auxiliar','Sistema',2,'1234',2)


CREATE TABLE Catalogos.TipoAdministrador
(
	IdTipoAdmin int primary key not null Identity,
	NomTipo nvarchar(50),
	DescTipo nvarchar(200),
	EstaTipo int
)
INSERT INTO Catalogos.TipoAdministrador VALUES ('Administrador','Administrador del Sistema',1)
INSERT INTO Catalogos.TipoAdministrador VALUES ('Auxiliar','Usuario que realiza Pagos',1)



CREATE TABLE Catalogos.Usuarios
(
	IdUsuario int primary key not null Identity,
	NomUsuario nvarchar(50),
	ApePatUsuario nvarchar(50),
	ApeMatUsuario nvarchar(50),
	FecNacUsuario DateTime,
	IdRed int,
	IdTipo int,
	PrecTomaUsuario float,
	RefUsuario nvarchar(1000),
	EstaUsuario int
)
INSERT INTO Catalogos.Usuarios VALUES ('Usuario 1','USUARIO1','usuario',1992-05-13,1,1,0,'Cerca de la Escuela Primaria',1)
INSERT INTO Catalogos.Usuarios VALUES ('Usuario 2','Prueba','Sistema',1992-05-13,2,2,0,'En el Centro',1)


CREATE TABLE Catalogos.Redes
(
	IdRed int primary key not null Identity,
	NomRed nvarchar(50),
	EncarRed nvarchar(100),
	IdTamano int,
	CuotaRed float,
	RefRed nvarchar(1000),
	EstaRed int
)
INSERT INTO Catalogos.Redes VALUES ('RED 1A','Eloy',1,500,'Del pozo a la Escuela',1)
INSERT INTO Catalogos.Redes VALUES('RED 1B','Eloy Salgado',2,100,'De la Escuela al Pozo',1)



CREATE TABLE Catalogos.RedTamano
(
	IdTamano int primary key not null Identity,
	NomTamano nvarchar(50),
	Tamano nvarchar(50),
	EstaTamano int
)
INSERT INTO Catalogos.RedTamano VALUES ('Normal','4 Pulgadas',1)
INSERT INTO Catalogos.RedTamano VALUES ('Pequeño','2 Pulgadas',1)


CREATE TABLE Catalogos.TipoServicio
(
	IdTipo int primary key not null Identity,
	NomTipo nvarchar(50),
	CuotaTipo float,
	EstaTipo int
)
INSERT INTO Catalogos.TipoServicio VALUES ('DOMESTICO',57,1)
INSERT INTO Catalogos.TipoServicio VALUES ('COMERCIAL',122,1)
INSERT INTO Catalogos.TipoServicio VALUES ('ESPECIAL',240,1)



CREATE TABLE Catalogos.Pagousuarios
(
	IdPago int primary key not null Identity,
	IdUsuario int,
	IdAdministrador int,
	FechaPago DateTime,
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
	SubPago float,
	DescPago float,
	MontoPago float
)

CREATE TABLE Catalogos.DescPago
(
	IdDescuento int primary key not null Identity,
	IdPago int,
	TipoDesc nvarchar(50),
	Cantdesc float,
	DescDesc nvarchar(1000)
)

CREATE TABLE Catalogos.CorteCaja
(
	IdCorte int primary key not null Identity,
	Fecha DateTime,
	IdAdmin int,
	AdminCorte int,
	NumPagos int,
	MontoCorte float 
)

CREATE TABLE Catalogos.PagosCorte
(
	IdCorPag int primary key not null Identity,
	IdPago int,
	IdCorte int,
	EstaCorPag int
)


CREATE TABLE Catalogos.AportUsuarios
(
	IdAportacion int primary key not null Identity,
	IdUsuario int,
	FecAport DateTime,
	IdTpoAport int,
	DescAport nvarchar(1000),
	CantAport float,
	EstaAport int
)

CREATE TABLE Catalogos.TipoAportacion
(
	IdAport int primary key not null Identity,
	NomAport nvarchar(50),
	DescAport nvarchar(250)
)
INSERT INTO Catalogos.TipoAportacion VALUES ('APORTACION','ESTA ES UNA CANTIDAD EXTRA SEGUN ACUERDO')
INSERT INTO Catalogos.TipoAportacion VALUES ('SANCION','ES UNA MULTA POR ALGUNA INFRACCION QUE EL USUARIO PUEDA COMETER')
INSERT INTO Catalogos.TipoAportacion VALUES ('REPARACION','ES EL COSTO DE LA REPARACION POR LA CUAL SE DEVOLVERA EL SERVICIO')


-- AQUI VISTAS


CREATE VIEW [dbo].[CargarAdministradores]
AS
SELECT     Catalogos.Administrador.IdAdministrador, Catalogos.Administrador.NomAdmin, Catalogos.TipoAdministrador.NomTipo, Catalogos.Administrador.ApePatAdmin, 
                      Catalogos.Administrador.ApeMatAdmin, Catalogos.Administrador.EstaAdmin, Catalogos.TipoAdministrador.EstaTipo, Catalogos.TipoAdministrador.IdTipoAdmin
FROM         Catalogos.Administrador INNER JOIN
                      Catalogos.TipoAdministrador ON Catalogos.Administrador.IdTipoAdmin = Catalogos.TipoAdministrador.IdTipoAdmin
GO


CREATE VIEW [dbo].[CargarUsuarios]
AS
SELECT     Catalogos.Usuarios.IdUsuario, Catalogos.Usuarios.NomUsuario, Catalogos.Usuarios.ApePatUsuario, Catalogos.Usuarios.ApeMatUsuario, Catalogos.Usuarios.FecNacUsuario, 
                      Catalogos.Usuarios.PrecTomaUsuario, Catalogos.Usuarios.RefUsuario, Catalogos.Usuarios.EstaUsuario, Catalogos.TipoServicio.IdTipo, Catalogos.TipoServicio.NomTipo, 
                      Catalogos.TipoServicio.CuotaTipo, Catalogos.Redes.IdRed, Catalogos.Redes.NomRed, Catalogos.RedTamano.NomTamano
FROM         Catalogos.Usuarios INNER JOIN
                      Catalogos.TipoServicio ON Catalogos.Usuarios.IdTipo = Catalogos.TipoServicio.IdTipo INNER JOIN
                      Catalogos.Redes ON Catalogos.Usuarios.IdRed = Catalogos.Redes.IdRed INNER JOIN
                      Catalogos.RedTamano ON Catalogos.Redes.IdTamano = Catalogos.RedTamano.IdTamano
GO

CREATE VIEW [dbo].[CargaRedes]
AS
SELECT     Catalogos.Redes.IdRed, Catalogos.Redes.NomRed, Catalogos.Redes.EncarRed, Catalogos.Redes.CuotaRed, Catalogos.Redes.RefRed, Catalogos.RedTamano.NomTamano, 
                      Catalogos.RedTamano.Tamano
FROM         Catalogos.Redes INNER JOIN
                      Catalogos.RedTamano ON Catalogos.Redes.IdTamano = Catalogos.RedTamano.IdTamano
GO



-- AQUI STORE PROCEDURES


CREATE PROCEDURE Seguridad.sp_InicioSessionAdmin 
	@pwd nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Top 1
		IdAdministrador as IdAdmin,
		NomAdmin + ' ' + ApePatAdmin as NomAdmin,
		NomTipo as PerfilAdmin
	FROM dbo.CargarAdministradores
	WHERE PwdAdmin = 123--@pwd
END
GO


-- =============================================
CREATE PROCEDURE Seguridad.sp_CargarAdministradores 
	@filtro nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	IF @filtro = '*'
		BEGIN
			SELECT 
				IdAdministrador as IdAdmin,
				NomAdmin as NomAdmin,
				ApePatAdmin + ' ' + ApeMatAdmin as ApeAdmin,
				ApePatAdmin as ApePatAdmin,
				ApeMatAdmin as ApeMatAdmin,
				NomTipo as PerfilAdmin,
				CASE EstaAdmin WHEN 1 THEN 'Activo' WHEN 0 THEN 'Inactivo' ELSE 'Verificar' END as Estatus
			FROM dbo.CargarAdministradores
			--WHERE PwdAdmin = 123--@pwd
		END
	ELSE
		SELECT 
				IdAdministrador as IdAdmin,
				NomAdmin as NomAdmin,
				ApePatAdmin + ' ' + ApeMatAdmin as ApeAdmin,
				ApePatAdmin as ApePatAdmin,
				ApeMatAdmin as ApeMatAdmin,
				NomTipo as PerfilAdmin,
				CASE EstaAdmin WHEN 1 THEN 'Activo' WHEN 0 THEN 'Inactivo' ELSE 'Verificar' END as Estatus
			FROM dbo.CargarAdministradores
			WHERE NomAdmin LIKE '%'+ @filtro +'%'
END
GO


