USE [saniteri_main]
GO
/****** Object:  Table [dbo].[can_inventory]    Script Date: 10/24/2011 19:50:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[can_inventory](
	[id] [uniqueidentifier] NOT NULL,
	[production_date] [date] NULL,
	[in_service_date] [date] NULL,
	[street] [varchar](50) NULL,
	[additional] [varchar](50) NULL,
	[city] [varchar](50) NULL,
	[state] [varchar](50) NULL,
	[zip] [varchar](50) NULL,
	[floor] [varchar](50) NULL,
	[room] [varchar](50) NULL,
	[custom_1] [varchar](50) NULL,
	[custom_2] [varchar](50) NULL,
	[custom_3] [varchar](50) NULL,
	[ip_address] [nvarchar](50) NULL,
 CONSTRAINT [PK_can_inventory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[can_users]    Script Date: 10/24/2011 19:50:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_users](
	[id] [uniqueidentifier] NOT NULL,
	[user_id] [nvarchar](50) NOT NULL,
	[first_name] [nvarchar](50) NULL,
	[last_name] [nvarchar](50) NULL,
	[title] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_transaction_log]    Script Date: 10/24/2011 19:50:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[can_transaction_log](
	[can_id] [uniqueidentifier] NOT NULL,
	[event_time_stamp] [datetime] NOT NULL,
	[event_type] [smallint] NULL,
	[event_description] [varchar](50) NULL,
	[event_data] [nvarchar](max) NULL,
 CONSTRAINT [PK_can_transaction_log] PRIMARY KEY CLUSTERED 
(
	[can_id] ASC,
	[event_time_stamp] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[can_maintenance]    Script Date: 10/24/2011 19:50:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_maintenance](
	[can_id] [uniqueidentifier] NOT NULL,
	[service_date] [date] NOT NULL,
	[service_performed] [nvarchar](max) NULL,
 CONSTRAINT [PK_can_maintenance] PRIMARY KEY CLUSTERED 
(
	[can_id] ASC,
	[service_date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_command]    Script Date: 10/24/2011 19:50:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_command](
	[id] [uniqueidentifier] NOT NULL,
	[can_id] [uniqueidentifier] NOT NULL,
	[can_lid_status] [tinyint] NOT NULL,
	[command_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_can_command] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_can_inventory_id]    Script Date: 10/24/2011 19:50:29 ******/
ALTER TABLE [dbo].[can_inventory] ADD  CONSTRAINT [DF_can_inventory_id]  DEFAULT (newid()) FOR [id]
GO
/****** Object:  ForeignKey [FK_can_transaction_log_can_inventory]    Script Date: 10/24/2011 19:50:29 ******/
ALTER TABLE [dbo].[can_transaction_log]  WITH CHECK ADD  CONSTRAINT [FK_can_transaction_log_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([id])
GO
ALTER TABLE [dbo].[can_transaction_log] CHECK CONSTRAINT [FK_can_transaction_log_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_maintenance_can_inventory]    Script Date: 10/24/2011 19:50:29 ******/
ALTER TABLE [dbo].[can_maintenance]  WITH CHECK ADD  CONSTRAINT [FK_can_maintenance_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([id])
GO
ALTER TABLE [dbo].[can_maintenance] CHECK CONSTRAINT [FK_can_maintenance_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_command_can_inventory]    Script Date: 10/24/2011 19:50:29 ******/
ALTER TABLE [dbo].[can_command]  WITH CHECK ADD  CONSTRAINT [FK_can_command_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([id])
GO
ALTER TABLE [dbo].[can_command] CHECK CONSTRAINT [FK_can_command_can_inventory]
GO