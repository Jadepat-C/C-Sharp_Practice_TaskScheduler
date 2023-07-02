DROP DATABASE IF EXISTS [TaskScheduler];
CREATE DATABASE [TaskScheduler];
USE [TaskScheduler]
GO

/****** Object:  Table [dbo].[task]    Script Date: 7/2/2023 6:53:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[task](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[type] [varchar](50) NOT NULL,
	[due] [datetime] NULL,
	[status] [varchar](50) NULL,
	[notes] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

