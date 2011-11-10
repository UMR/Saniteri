/****** Object:  Table [dbo].[can_inventory]    Script Date: 11/02/2011 08:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[can_inventory](
	[can_id] [bigint] IDENTITY(1,1) NOT NULL,
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
	[can_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[can_eventcodes]    Script Date: 11/02/2011 08:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_eventcodes](
	[event_type] [int] IDENTITY(1,1) NOT NULL,
	[description] [text] NULL,
 CONSTRAINT [PK_CAN_EventCodes] PRIMARY KEY CLUSTERED 
(
	[event_type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_statuscode]    Script Date: 11/02/2011 08:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_statuscode](
	[status_type] [int] IDENTITY(1,1) NOT NULL,
	[description] [text] NULL,
 CONSTRAINT [PK_can_StatusCode] PRIMARY KEY CLUSTERED 
(
	[status_type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_users]    Script Date: 11/02/2011 08:50:50 ******/
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
/****** Object:  Table [dbo].[can_transaction_log]    Script Date: 11/02/2011 08:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[can_transaction_log](
	[seqno] [uniqueidentifier] NOT NULL,
	[can_id] [bigint] NOT NULL,
	[event_time_stamp] [datetime] NOT NULL,
	[event_type] [int] NULL,
	[event_description] [varchar](50) NULL,
	[event_data] [nvarchar](max) NULL,
	[tag_id] [bigint] NULL,
	[name] [varchar](50) NULL,
 CONSTRAINT [PK_can_Transaction_Log_1] PRIMARY KEY CLUSTERED 
(
	[seqno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[can_status]    Script Date: 11/02/2011 08:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_status](
	[seqno] [uniqueidentifier] NOT NULL,
	[can_id] [bigint] NOT NULL,
	[status_type] [int] NULL,
	[edate] [datetime] NULL,
	[status_description] [text] NULL,
 CONSTRAINT [PK_can_Status] PRIMARY KEY CLUSTERED 
(
	[seqno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_maintenance]    Script Date: 11/02/2011 08:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_maintenance](
	[can_id] [bigint] NOT NULL,
	[service_date] [date] NOT NULL,
	[service_performed] [nvarchar](max) NULL,
 CONSTRAINT [PK_can_maintenance] PRIMARY KEY CLUSTERED 
(
	[can_id] ASC,
	[service_date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_command]    Script Date: 11/02/2011 08:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_command](
	[command_id] [uniqueidentifier] NOT NULL,
	[can_id] [bigint] NOT NULL,
	[can_lid_status] [tinyint] NOT NULL,
	[command_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_can_command] PRIMARY KEY CLUSTERED 
(
	[command_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_can_EventsLog_seqno]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_transaction_log] ADD  CONSTRAINT [DF_can_EventsLog_seqno]  DEFAULT (newid()) FOR [seqno]
GO
/****** Object:  Default [DF_can_Status_seqno]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_status] ADD  CONSTRAINT [DF_can_Status_seqno]  DEFAULT (newid()) FOR [seqno]
GO
/****** Object:  Default [DF_can_Command_command_id]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_command] ADD  CONSTRAINT [DF_can_Command_command_id]  DEFAULT (newid()) FOR [command_id]
GO
/****** Object:  ForeignKey [FK_can_EventsLog_can_EventCodes]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_transaction_log]  WITH CHECK ADD  CONSTRAINT [FK_can_EventsLog_can_EventCodes] FOREIGN KEY([event_type])
REFERENCES [dbo].[can_eventcodes] ([event_type])
GO
ALTER TABLE [dbo].[can_transaction_log] CHECK CONSTRAINT [FK_can_EventsLog_can_EventCodes]
GO
/****** Object:  ForeignKey [FK_can_transaction_log_can_inventory]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_transaction_log]  WITH CHECK ADD  CONSTRAINT [FK_can_transaction_log_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_transaction_log] CHECK CONSTRAINT [FK_can_transaction_log_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_status_can_inventory]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_status]  WITH CHECK ADD  CONSTRAINT [FK_can_status_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_status] CHECK CONSTRAINT [FK_can_status_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_Status_can_StatusCode]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_status]  WITH CHECK ADD  CONSTRAINT [FK_can_Status_can_StatusCode] FOREIGN KEY([status_type])
REFERENCES [dbo].[can_statuscode] ([status_type])
GO
ALTER TABLE [dbo].[can_status] CHECK CONSTRAINT [FK_can_Status_can_StatusCode]
GO
/****** Object:  ForeignKey [FK_can_maintenance_can_inventory]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_maintenance]  WITH CHECK ADD  CONSTRAINT [FK_can_maintenance_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_maintenance] CHECK CONSTRAINT [FK_can_maintenance_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_command_can_inventory]    Script Date: 11/02/2011 08:50:50 ******/
ALTER TABLE [dbo].[can_command]  WITH CHECK ADD  CONSTRAINT [FK_can_command_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_command] CHECK CONSTRAINT [FK_can_command_can_inventory]
GO