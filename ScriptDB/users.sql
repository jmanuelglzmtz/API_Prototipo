/*
Primero Crear una base de datos llamada  TestDb
Despues ejecutar este script
*/
USE [TestDb]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/11/2018 05:37:22 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Username] [varchar](50) NULL,
	[PasswordHash] [binary](64) NULL,
	[PasswordSalt] [binary](128) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
