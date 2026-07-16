USE [master]
GO
/****** Object:  Database [SoftOneTasksDb]    Script Date: 07/17/2026 12:54:43 AM ******/
CREATE DATABASE [SoftOneTasksDb]
 CONTAINMENT = NONE

ALTER DATABASE [SoftOneTasksDb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SoftOneTasksDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SoftOneTasksDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SoftOneTasksDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SoftOneTasksDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SoftOneTasksDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SoftOneTasksDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SoftOneTasksDb] SET  MULTI_USER 
GO
ALTER DATABASE [SoftOneTasksDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SoftOneTasksDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SoftOneTasksDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SoftOneTasksDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SoftOneTasksDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SoftOneTasksDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SoftOneTasksDb] SET QUERY_STORE = OFF
GO
USE [SoftOneTasksDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 07/17/2026 12:54:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 07/17/2026 12:54:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[Priority] [int] NOT NULL,
	[DueDate] [datetime2](7) NULL,
	[IsCompleted] [bit] NOT NULL,
	[IsReOpened] [bit] NULL,
	[CreatedUserId] [int] NULL,
	[UpdatedUserId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 07/17/2026 12:54:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](500) NOT NULL,
	[CreatedUserId] [int] NULL,
	[UpdatedUserId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Tasks] ON 
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (1, N'Welcome to SoftOne Tasks', N'Explore the task management API and Angular UI.', 5, CAST(N'2026-07-23T00:00:00.0000000' AS DateTime2), 0, 0, 1, 1, 0, CAST(N'2026-07-16T12:41:01.0383482' AS DateTime2), CAST(N'2026-07-16T19:07:30.6959874' AS DateTime2))
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (2, N'Review assignment requirements', N'Confirm CRUD, auth, sorting and filtering work end-to-end.', 4, CAST(N'2026-07-19T00:00:00.0000000' AS DateTime2), 1, 0, 1, 1, 0, CAST(N'2026-07-16T12:41:01.0383482' AS DateTime2), CAST(N'2026-07-16T15:45:50.2053077' AS DateTime2))
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (3, N'dsf', N'sd', 1, NULL, 0, 0, 1, 1, 1, CAST(N'2026-07-16T14:35:48.7205776' AS DateTime2), CAST(N'2026-07-16T14:38:48.4550006' AS DateTime2))
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (4, N'df', N'fsdfsd', 1, CAST(N'2026-07-17T00:00:00.0000000' AS DateTime2), 0, 0, 1, 1, 1, CAST(N'2026-07-16T14:38:56.0203635' AS DateTime2), CAST(N'2026-07-16T18:03:59.9175526' AS DateTime2))
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (5, N'dsf', N'sdf', 1, CAST(N'2026-07-25T00:00:00.0000000' AS DateTime2), 0, 0, 1, 1, 1, CAST(N'2026-07-16T14:39:04.8889479' AS DateTime2), CAST(N'2026-07-16T18:04:07.4370215' AS DateTime2))
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (6, N'add assignment requirements API', N'add assignment requirements API end points', 1, CAST(N'2026-07-22T00:00:00.0000000' AS DateTime2), 0, 0, 1, 1, 0, CAST(N'2026-07-16T18:04:43.9554268' AS DateTime2), CAST(N'2026-07-16T18:09:44.0147699' AS DateTime2))
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (7, N'add assignment requirements APP', N'add assignment requirements API', 5, CAST(N'2026-07-31T00:00:00.0000000' AS DateTime2), 0, 0, 1, NULL, 0, CAST(N'2026-07-16T18:10:27.7071070' AS DateTime2), NULL)
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (8, N'Welcome to SoftOne Tasks', N'Welcome to SoftOne TasksWelcWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne TasksWelcome to SoftOne Tasks', 4, CAST(N'2026-07-22T00:00:00.0000000' AS DateTime2), 0, 0, 1, NULL, 0, CAST(N'2026-07-16T18:12:48.1364432' AS DateTime2), NULL)
GO
INSERT [dbo].[Tasks] ([Id], [Title], [Description], [Priority], [DueDate], [IsCompleted], [IsReOpened], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (9, N'ccc', N'ssss', 3, CAST(N'2026-07-18T00:00:00.0000000' AS DateTime2), 0, 0, 1, 1, 1, CAST(N'2026-07-16T19:07:47.2760727' AS DateTime2), CAST(N'2026-07-16T19:07:56.2173729' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Tasks] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [Username], [PasswordHash], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (1, N'admin', N'100000.RDOtYBOZ80X+R1GQQAtspg==.521GMqx0FqRHl7CUGMRGDUXQxYrd4Swzr3y5DVW7FbQ=', NULL, NULL, 0, CAST(N'2026-07-16T12:41:00.7368369' AS DateTime2), NULL)
GO
INSERT [dbo].[Users] ([Id], [Username], [PasswordHash], [CreatedUserId], [UpdatedUserId], [IsDeleted], [CreatedDate], [UpdatedDate]) VALUES (2, N'demo', N'100000.PpdLE7ONEXk8+zWNIgRk6g==.k5iC/dsrneERp+8PDF5PbOWbYFMfi/CQJvRJz634L4w=', NULL, NULL, 0, CAST(N'2026-07-16T12:41:00.7368369' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Tasks_IsCompleted]    Script Date: 07/17/2026 12:54:43 AM ******/
CREATE NONCLUSTERED INDEX [IX_Tasks_IsCompleted] ON [dbo].[Tasks]
(
	[IsCompleted] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tasks_Priority]    Script Date: 07/17/2026 12:54:43 AM ******/
CREATE NONCLUSTERED INDEX [IX_Tasks_Priority] ON [dbo].[Tasks]
(
	[Priority] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Username]    Script Date: 07/17/2026 12:54:43 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [SoftOneTasksDb] SET  READ_WRITE 
GO
