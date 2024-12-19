USE erp_neogenis_13122024

/****** Object:  Table [dbo].[tblCargaProductoCsv]    Script Date: 17/12/2024 08:58:36 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCargaProductoCsv](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[archivo] [varchar](50) NULL,
	[fecha] [varchar](50) NULL,
	[registros] [int] NULL,
	[registros_error] [int] NULL,
	[registros_correctos] [int] NULL
) ON [PRIMARY]
GO


-- 
CREATE TABLE [dbo].[tblCargaProductoCsvDetalle](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cargaid] [int] NULL,
	[codigo] [varchar](50) NULL,
	[upc] [varchar](100) NULL,
	[descripcion] [varchar](400) NULL,
	[descripcion_corta] [varchar](300) NULL,
	[marca] [varchar](50) NULL,
	[temporada] [varchar](50) NULL,
	[unitario1] [money] NULL,
	[unitario2] [money] NULL,
	[unitario3] [money] NULL,
	[unitario4] [money] NULL,
	[modelo_estilo] [varchar](100) NULL,
	[plataforma] [varchar](100) NULL,
	[genero] [varchar](100) NULL,
	[tallaUSA] [varchar](100) NULL,
	[tallaMX] [varchar](100) NULL,
	[color] [varchar](100) NULL,
	[colorMX] [varchar](100) NULL,
	[material] [varchar](100) NULL,
	[peso] [varchar](100) NULL,
	[empaque_alto] [varchar](100) NULL,
	[empaque_largo] [varchar](100) NULL,
	[empaque_ancho] [varchar](100) NULL,
	[unidadMedida] [nvarchar](255) NULL,
	[claveProdServ] [varchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE tblCargaProductoCsvDetalle ADD moneda varchar(50)
ALTER TABLE tblCargaProductoCsvDetalle ADD tasa varchar(50)
ALTER TABLE tblCargaProductoCsvDetalle ADD claveSat varchar(50)

--estos campos se llenan bajo las aguas
ALTER TABLE tblCargaProductoCsvDetalle ADD marcaId int 
ALTER TABLE tblCargaProductoCsvDetalle ADD coleccionId int 
ALTER TABLE tblCargaProductoCsvDetalle ADD proyectoId int 
ALTER TABLE tblCargaProductoCsvDetalle ADD generoId int 
ALTER TABLE tblCargaProductoCsvDetalle ADD tasaId int 
ALTER TABLE tblCargaProductoCsvDetalle ADD monedaId int 
ALTER TABLE tblCargaProductoCsvDetalle ADD objImpId int 

--marketplaces
ALTER TABLE tblCargaProductoCsvDetalle ADD marketPlaceLiverpool varchar(2)  
ALTER TABLE tblCargaProductoCsvDetalle ADD marketPlaceShopify varchar(2)  
ALTER TABLE tblCargaProductoCsvDetalle ADD marketPlaceAcctivity varchar(2)  
--ALTER TABLE tblCargaProductoCsvDetalle ADD marketPlaceDeporprive bit
--ALTER TABLE tblCargaProductoCsvDetalle ADD marketPlaceMarti bit

--
CREATE TABLE [dbo].[tblCargaProductoCsvDetalleError](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cargaid] [int] NULL,
	[codigo] [varchar](50) NULL,
	[upc] [varchar](100) NULL,
	[descripcion] [varchar](400) NULL,
	[descripcion_corta] [varchar](300) NULL,
	[marca] [varchar](50) NULL,
	[temporada] [varchar](50) NULL,
	[unitario1] [money] NULL,
	[unitario2] [money] NULL,
	[unitario3] [money] NULL,
	[unitario4] [money] NULL,
	[modelo_estilo] [varchar](100) NULL,
	[plataforma] [varchar](100) NULL,
	[genero] [varchar](100) NULL,
	[tallaUSA] [varchar](100) NULL,
	[tallaMX] [varchar](100) NULL,
	[color] [varchar](100) NULL,
	[colorMX] [varchar](100) NULL,
	[material] [varchar](100) NULL,
	[peso] [varchar](100) NULL,
	[empaque_alto] [varchar](100) NULL,
	[empaque_largo] [varchar](100) NULL,
	[empaque_ancho] [varchar](100) NULL,
	[unidadMedida] [nvarchar](255) NULL,
	[claveProdServ] [varchar](50) NULL,
	[error] [varchar](max) NULL
) ON [PRIMARY]
GO


ALTER TABLE tblCargaProductoCsvDetalleError ADD moneda varchar(50)
ALTER TABLE tblCargaProductoCsvDetalleError ADD tasa varchar(50)
ALTER TABLE tblCargaProductoCsvDetalleError ADD claveSat varchar(50)

ALTER TABLE tblCargaProductoCsvDetalleError ADD marcaId int 
ALTER TABLE tblCargaProductoCsvDetalleError ADD coleccionId int 
ALTER TABLE tblCargaProductoCsvDetalleError ADD proyectoId int 
ALTER TABLE tblCargaProductoCsvDetalleError ADD generoId int 
ALTER TABLE tblCargaProductoCsvDetalleError ADD tasaId int 
ALTER TABLE tblCargaProductoCsvDetalleError ADD monedaId int 
ALTER TABLE tblCargaProductoCsvDetalleError ADD objImpId int 


ALTER TABLE tblCargaProductoCsvDetalleError ADD marketPlaceLiverpool varchar(2)  
ALTER TABLE tblCargaProductoCsvDetalleError ADD marketPlaceShopify varchar(2)  
ALTER TABLE tblCargaProductoCsvDetalleError ADD marketPlaceAcctivity varchar(2) 
--ALTER TABLE tblCargaProductoCsvDetalleError ADD marketPlaceDeporprive bit
--ALTER TABLE tblCargaProductoCsvDetalleError ADD marketPlaceMarti bit

--rollback por si acaso
--DROP TABLE tblCargaProductoCsvDetalle
--DROP TABLE tblCargaProductoCsvDetalleError
--DROP TABLE tblCargaProductoCsv