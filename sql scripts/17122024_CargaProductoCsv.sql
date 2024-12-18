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
