USE [SaniteriMain]
GO
/****** Object:  Table [dbo].[can_users]    Script Date: 03/06/2012 12:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[can_users](
	[user_id] [varchar](50) NOT NULL,
	[first_name] [nvarchar](50) NULL,
	[last_name] [nvarchar](50) NULL,
	[title] [nvarchar](50) NULL,
	[options] [int] NULL,
 CONSTRAINT [PK_can_users] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[can_statuscode]    Script Date: 03/06/2012 12:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_statuscode](
	[status_type] [int] NOT NULL,
	[description] [text] NULL,
 CONSTRAINT [PK_can_StatusCode] PRIMARY KEY CLUSTERED 
(
	[status_type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[can_statuscode] ([status_type], [description]) VALUES (1, N'Can Lid is Open')
INSERT [dbo].[can_statuscode] ([status_type], [description]) VALUES (2, N'Can Door is Open')
INSERT [dbo].[can_statuscode] ([status_type], [description]) VALUES (3, N'Can Foot Sensor is Tripped')
INSERT [dbo].[can_statuscode] ([status_type], [description]) VALUES (4, N'Can Lid is Closed')
INSERT [dbo].[can_statuscode] ([status_type], [description]) VALUES (5, N'Can Door is Locked')
INSERT [dbo].[can_statuscode] ([status_type], [description]) VALUES (6, N'Unknown')
/****** Object:  Table [dbo].[can_inventory]    Script Date: 03/06/2012 12:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[can_inventory](
	[can_id] [bigint] NOT NULL,
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
/****** Object:  Table [dbo].[can_eventcodes]    Script Date: 03/06/2012 12:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_eventcodes](
	[event_type] [int] NOT NULL,
	[description] [text] NULL,
 CONSTRAINT [PK_CAN_EventCodes] PRIMARY KEY CLUSTERED 
(
	[event_type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (0, N'Monitored Passage')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (1, N'Refuse Access')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (2, N'Maintenance Access')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (3, N'Refuse Bag Full')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (4, N'Lid Still Open')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (5, N'Lid Not Closed')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (6, N'No Bag Tag')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (7, N'New Bag Inserted')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (8, N'Door not Closed')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (9, N'Can Weight')
INSERT [dbo].[can_eventcodes] ([event_type], [description]) VALUES (10, N'Tag Exit Area')
/****** Object:  Table [dbo].[can_command]    Script Date: 03/06/2012 12:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_command](
	[seqno] [int] IDENTITY(1,1) NOT NULL,
	[command_id] [int] NULL,
	[can_id] [bigint] NOT NULL,
 CONSTRAINT [PK_can_command] PRIMARY KEY CLUSTERED 
(
	[seqno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_status]    Script Date: 03/06/2012 12:22:26 ******/
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
/****** Object:  Table [dbo].[can_maintenance]    Script Date: 03/06/2012 12:22:26 ******/
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
/****** Object:  Table [dbo].[can_livestatus]    Script Date: 03/06/2012 12:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[can_livestatus](
	[can_id] [bigint] NOT NULL,
	[need_service] [bit] NULL,
	[lid_open] [bit] NULL,
	[door_open] [bit] NULL,
	[fault] [nvarchar](120) NULL,
	[weight] [float] NULL,
	[bag_info] [nvarchar](50) NULL,
	[power_status] [tinyint] NULL,
	[comm_status] [tinyint] NULL,
 CONSTRAINT [PK_can_livestatus] PRIMARY KEY CLUSTERED 
(
	[can_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[can_transaction_log]    Script Date: 03/06/2012 12:22:26 ******/
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
	[tag_id] [varchar](50) NULL,
	[name] [varchar](50) NULL,
 CONSTRAINT [PK_can_Transaction_Log_1] PRIMARY KEY CLUSTERED 
(
	[seqno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Trigger [tr_Insert_can_status]    Script Date: 03/06/2012 12:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[tr_Insert_can_status] ON [dbo].[can_status]
FOR INSERT AS
	DECLARE @SeqNo uniqueidentifier,
			@Status_Type int,
			@Description varchar(120),
			@Count int
    SELECT @Count = count(*) from inserted
	IF @Count = 1 
		BEGIN
			SELECT @Status_Type = inserted.status_type from inserted
			SELECT @SeqNo = inserted.seqno from inserted
			SELECT @Description = description from can_statuscode where status_type = @Status_Type
			Update can_status set status_description = @Description where seqno = @SeqNo
		END
GO
/****** Object:  Default [DF_can_EventsLog_seqno]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_transaction_log] ADD  CONSTRAINT [DF_can_EventsLog_seqno]  DEFAULT (newid()) FOR [seqno]
GO
/****** Object:  Default [DF_can_Status_seqno]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_status] ADD  CONSTRAINT [DF_can_Status_seqno]  DEFAULT (newid()) FOR [seqno]
GO
/****** Object:  ForeignKey [FK_can_EventsLog_can_EventCodes]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_transaction_log]  WITH CHECK ADD  CONSTRAINT [FK_can_EventsLog_can_EventCodes] FOREIGN KEY([event_type])
REFERENCES [dbo].[can_eventcodes] ([event_type])
GO
ALTER TABLE [dbo].[can_transaction_log] CHECK CONSTRAINT [FK_can_EventsLog_can_EventCodes]
GO
/****** Object:  ForeignKey [FK_can_transaction_log_can_inventory]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_transaction_log]  WITH CHECK ADD  CONSTRAINT [FK_can_transaction_log_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_transaction_log] CHECK CONSTRAINT [FK_can_transaction_log_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_status_can_inventory]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_status]  WITH CHECK ADD  CONSTRAINT [FK_can_status_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_status] CHECK CONSTRAINT [FK_can_status_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_Status_can_StatusCode]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_status]  WITH CHECK ADD  CONSTRAINT [FK_can_Status_can_StatusCode] FOREIGN KEY([status_type])
REFERENCES [dbo].[can_statuscode] ([status_type])
GO
ALTER TABLE [dbo].[can_status] CHECK CONSTRAINT [FK_can_Status_can_StatusCode]
GO
/****** Object:  ForeignKey [FK_can_maintenance_can_inventory]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_maintenance]  WITH CHECK ADD  CONSTRAINT [FK_can_maintenance_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_maintenance] CHECK CONSTRAINT [FK_can_maintenance_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_livestatus_can_inventory]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_livestatus]  WITH CHECK ADD  CONSTRAINT [FK_can_livestatus_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_livestatus] CHECK CONSTRAINT [FK_can_livestatus_can_inventory]
GO
/****** Object:  ForeignKey [FK_can_command_can_inventory]    Script Date: 03/06/2012 12:22:26 ******/
ALTER TABLE [dbo].[can_command]  WITH CHECK ADD  CONSTRAINT [FK_can_command_can_inventory] FOREIGN KEY([can_id])
REFERENCES [dbo].[can_inventory] ([can_id])
GO
ALTER TABLE [dbo].[can_command] CHECK CONSTRAINT [FK_can_command_can_inventory]
GO