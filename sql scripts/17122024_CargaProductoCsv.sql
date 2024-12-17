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

CREATE TABLE [dbo].[tblCargaProductoCsvDetalle](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cargaid] [int] NULL,
	[codigo] [varchar](50) NULL,
	[upc] [varchar](100) NULL,
	[claveSat] [varchar](50) NULL,
	[unidad] [varchar](50) NULL,
	[descripcion] [varchar](150) NULL,
	[marcaId] [int] NULL,
	[coleccionId] [int] NULL,
	[precioUnit1] [money] NULL,
	[precioUnit2] [money] NULL,
	[precioUnit3] [money] NULL,
	[precioUnit4] [money] NULL,
	[precioUnit5] [money] NULL,
	[precioUnit6] [money] NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblCargaProductoCsvDetalleError](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cargaid] [int] NULL,
	[codigo] [varchar](50) NULL,
	[claveSat] [varchar](50) NULL,
	[upc] [varchar](100) NULL,
	[unidad] [varchar](50) NULL,
	[descripcion] [varchar](150) NULL,
	[marcaId] [int] NULL,
	[coleccionId] [int] NULL,
	[precioUnit1] [money] NULL,
	[precioUnit2] [money] NULL,
	[precioUnit3] [money] NULL,
	[precioUnit4] [money] NULL,
	[precioUnit5] [money] NULL,
	[precioUnit6] [money] NULL,
	[error] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


--COLUMNAS DE BROOKS tblCargaProductoCsvDetalle
ALTER TABLE tblCargaProductoCsvDetalle ADD sku varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD claveprodserv varchar(10) null
ALTER TABLE tblCargaProductoCsvDetalle ADD claveunidad varchar(10) null
ALTER TABLE tblCargaProductoCsvDetalle ADD tasa varchar(20) null
ALTER TABLE tblCargaProductoCsvDetalle ADD costo_estandar varchar(50) null
ALTER TABLE tblCargaProductoCsvDetalle ADD costo_promedio varchar(50) null
ALTER TABLE tblCargaProductoCsvDetalle ADD moneda varchar(30) null
ALTER TABLE tblCargaProductoCsvDetalle ADD peso float
ALTER TABLE tblCargaProductoCsvDetalle ADD modeloEstilo varchar(100) NULL

ALTER TABLE tblCargaProductoCsvDetalle ADD plataforma varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD genero varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD generoid int
ALTER TABLE tblCargaProductoCsvDetalle ADD tallaUSA varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD tallaMX varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD color varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD material varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD pesoKg varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD empaqueAlto varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD empaqueLargo varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD empaqueAncho varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD codigoproductoid int
ALTER TABLE tblCargaProductoCsvDetalle ADD colorMX varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalle ADD descripcion_corta varchar(200) null
ALTER TABLE tblCargaProductoCsvDetalle ADD marca varchar(200) null
ALTER TABLE tblCargaProductoCsvDetalle ADD temporada varchar(200) null
ALTER TABLE tblCargaProductoCsvDetalle ADD matriz numeric(10)
ALTER TABLE tblCargaProductoCsvDetalle ADD en_proceso numeric(10)
ALTER TABLE tblCargaProductoCsvDetalle ADD en_consignacion numeric(10)

--COLUMNAS DE BROOKS tblCargaProductoCsvDetalleError
ALTER TABLE tblCargaProductoCsvDetalleError ADD sku varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD claveprodserv varchar(10) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD claveunidad varchar(10) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD tasa varchar(20) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD costo_estandar varchar(50) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD costo_promedio varchar(50) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD moneda varchar(30) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD peso float
ALTER TABLE tblCargaProductoCsvDetalleError ADD modeloEstilo varchar(100) NULL

ALTER TABLE tblCargaProductoCsvDetalleError ADD plataforma varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD genero varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD generoid int
ALTER TABLE tblCargaProductoCsvDetalleError ADD tallaUSA varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD tallaMX varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD color varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD material varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD pesoKg varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD empaqueAlto varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD empaqueLargo varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD empaqueAncho varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD codigoproductoid int
ALTER TABLE tblCargaProductoCsvDetalleError ADD colorMX varchar(100) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD descripcion_corta varchar(200) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD marca varchar(200) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD temporada varchar(200) null
ALTER TABLE tblCargaProductoCsvDetalleError ADD matriz numeric(10)
ALTER TABLE tblCargaProductoCsvDetalleError ADD en_proceso numeric(10)
ALTER TABLE tblCargaProductoCsvDetalleError ADD en_consignacion numeric(10)