/*

CREATE TABLE[dbo].[AppUser](

   [UserId][bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL,

  [FileID] nvarchar(1000) null,
	[NameEn] [nvarchar](200) NOT NULL,
	[NameAr] [nvarchar](200) NULL,
	[Password] [nvarchar](max)NULL,
	[StaffID] int NULL,
	[QID] int NULL,
	[VehicleID] int NULL,
	[IsActive] [bit] NOT NULL DEFAULT (1),
	[IsDeleted] [bit] NOT NULL DEFAULT (0),
	[CreatedDate] [datetime] NOT NULL  DEFAULT getdate(),
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	FOREIGN KEY(FileID) REFERENCES AppUserImage(FileID),
	FOREIGN KEY(VehicleID) REFERENCES VehicleType(VehicleID),
	CONSTRAINT UC_QID UNIQUE (QID, StaffID)
);

go

CREATE TABLE AppUserImage (
 [FileID] nvarchar(1000) not null PRIMARY KEY,
 [ImgContent] varbinary(max) not null,
 [IsActive] [bit] NOT NULL DEFAULT (1),
[IsDeleted] [bit] NOT NULL DEFAULT (0),
[CreatedDate] [datetime] NOT NULL  DEFAULT getdate(),
[CreatedBy] [bigint] NOT NULL,
[UpdatedDate] [datetime] NULL,
[UpdatedBy] [bigint] NULL,
);

go


CREATE TABLE VehicleType(
 VehicleID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
 Vehicle nvarchar(100) not null,
 [IsActive] [bit] NOT NULL DEFAULT (1),
[IsDeleted] [bit] NOT NULL DEFAULT (0),
[CreatedDate] [datetime] NOT NULL  DEFAULT getdate(),
[CreatedBy] [bigint] NOT NULL,
[UpdatedDate] [datetime] NULL,
[UpdatedBy] [bigint] NULL
);

go



CREATE TABLE SystemSetting(
 SystemSettingID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
 BgKey nvarchar(100) not null,
 [Value] nvarchar(1000) not null,
 [IsActive] [bit] NOT NULL DEFAULT (1),
[IsDeleted] [bit] NOT NULL DEFAULT (0),
[CreatedDate] [datetime] NOT NULL  DEFAULT getdate(),
[CreatedBy] [bigint] NOT NULL,
[UpdatedDate] [datetime] NULL,
[UpdatedBy] [bigint] NULL
);

go

ALTER TABLE SystemSetting
ADD Description nvarchar(500);

go

CREATE TABLE DriverEod(
 DriverEodID bigint PRIMARY KEY IDENTITY(1,1) NOT NULL,
 StaffId int not null,
 UserId bigint null,
 [FileID] nvarchar(1000) null,
 TotalDelivery int null,
 Delivered int null,
 FailedDelivery int null,
 Drops int null,
 AdditionalDelivery int null,
 Remarks nvarchar(1000) null,
 [IsActive] [bit] NOT NULL DEFAULT (1),
[IsDeleted] [bit] NOT NULL DEFAULT (0),
[CreatedDate] [datetime] NOT NULL  DEFAULT getdate(),
[CreatedBy] [bigint] NOT NULL,
[UpdatedDate] [datetime] NULL,
[UpdatedBy] [bigint] NULL,
FOREIGN KEY(FileID) REFERENCES AppUserSignature(FileID),
FOREIGN KEY(UserId) REFERENCES AppUser(UserId)
);

CREATE TABLE AppUserSignature (
 [FileID] nvarchar(1000) not null PRIMARY KEY,
 [ImgContent] varbinary(max) not null,
 [IsActive] [bit] NOT NULL DEFAULT (1),
[IsDeleted] [bit] NOT NULL DEFAULT (0),
[CreatedDate] [datetime] NOT NULL  DEFAULT getdate(),
[CreatedBy] [bigint] NOT NULL,
[UpdatedDate] [datetime] NULL,
[UpdatedBy] [bigint] NULL,
);


go

ALTER TABLE [AppUser]
ADD CONSTRAINT UC_QID UNIQUE (QID);



< --drievr delivery status sql updated--->

CREATE TABLE [DriverDeliveryStatus](
	[StaffId][int] NOT NULL,

   [Name] [nvarchar](200) NULL,
	[Total] [int] NULL,
	[Delivered] [int] NULL,
	[Failed] [int] NULL,
	[Drops] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[DutyStatus] [varchar](50) NULL,
 CONSTRAINT[PK_DriverDeliveryStatus] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO



CREATE VIEW ViewDriverDeliveryStatus AS
SELECT StaffId,[Name], Total, Delivered, Failed, Drops, CreatedDate, DutyStatus
FROM DriverDeliveryStatus

*/
