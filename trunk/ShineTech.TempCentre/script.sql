CREATE TABLE [AlarmConfig] (
    [ID] int PRIMARY KEY NOT NULL,
    [SN] nvarchar(64) NOT NULL,
    [TN] nvarchar(64) NOT NULL,
    [ProductName] nvarchar(64) NOT NULL,
    [AlarmMode] nvarchar(64),
    [AlarmLevel] nvarchar(8),
    [AlarmTemp] nvarchar(16),
    [AlarmType] nvarchar(16),
    [AlarmDelay] nvarchar(16),
    [IsAlarm] int NOT NULL,
    [Remark] nvarchar(256)
);

CREATE TABLE [Device] (
    [ID] int PRIMARY KEY NOT NULL,
    [PID] int NOT NULL UNIQUE,
    [TypeID] int NOT NULL,
    [ProductName] nvarchar(50),
    [SerialNum] nvarchar(50),
    [TripNum] nvarchar(50),
    [Model] nvarchar(50),
    [Battery] numeric(12,2),
    [DESCS] nvarchar(128),
    [Remark] nvarchar(256)
);

CREATE TABLE [LogConfig] (
    [ID] int PRIMARY KEY NOT NULL,
    [SN] Nvarchar(64) NOT NULL,
    [TN] nvarchar(64) NOT NULL,
    [ProductName] nvarchar(64) NOT NULL,
    [StartMode] nvarchar(64),
    [LogInterval] nvarchar(16) NOT NULL,
    [LogCycle] nvarchar(16) NOT NULL,
    [StartDelay] nvarchar(16) NOT NULL,
    [Remark] nvarchar(256) NOT NULL
);

CREATE TABLE [Meanings] (
    [ID] int PRIMARY KEY NOT NULL,
    [Desc] nvarchar(64) NOT NULL,
    [Remark] nvarchar(256)
);

CREATE TABLE [OperationLog] (
    [ID] int PRIMARY KEY NOT NULL,
    [OperateTime] datetime,
    [Action] Nvarchar(256),
    [User] Nvarchar(128)
);

CREATE TABLE [PointInfo] (
    [ID] int PRIMARY KEY NOT NULL,
    [SN] nvarchar(64) NOT NULL,
    [TN] nvarchar(64) NOT NULL,
    [ProductName] nvarchar(64) NOT NULL,
    [PointTime] datetime,
    [TripLength] nvarchar(16),
    [PointTemp] nvarchar(16),
    [Remark] nvarchar(256)
);

CREATE TABLE [Policy] (
    [ID] int PRIMARY KEY NOT NULL,
    [MinPwdSize] int NOT NULL,
    [InactivityTime] int NOT NULL,
    [PwdExpiredDay] int NOT NULL,
    [LockedTimes] int NOT NULL,
    [ProfileFolder] nvarchar(128),
    [Remark] nvarchar(256)
);

CREATE TABLE [ProductType ] (
    [ID] int PRIMARY KEY NOT NULL,
    [Name] Nvarchar(32) NOT NULL,
    [Remark] Nvarchar(256)
);

CREATE TABLE [ReportConfig] (
    [ID] int PRIMARY KEY NOT NULL,
    [ReportTitle] nvarchar(64),
    [CompanyName] nvarchar(128),
    [Adress] nvarchar(128),
    [ContactPhone] nvarchar(80),
    [Fax] nvarchar(64),
    [Email] nvarchar(64),
    [WebSite] nvarchar(50),
    [Logo] blob,
    [Remark] nvarchar(50)
);

CREATE TABLE [RoleInfo] (
    [ID] int PRIMARY KEY NOT NULL,
    [RoleName] nVarchar(64) NOT NULL,
    [Remark] Nvarchar(256)
);

CREATE TABLE [StatusCode] (
    [ID] int PRIMARY KEY NOT NULL,
    [StatusID] int NOT NULL,
    [StatusName] nvarchar(64) NOT NULL,
    [Remark] nvarchar(256)
);

CREATE TABLE [UserInfo] (
    [Userid] int PRIMARY KEY NOT NULL,
    [Account] nvarchar(32) NOT NULL,
    [UserName] nvarchar(128) NOT NULL,
    [FullName] nvarchar(128),
    [Description] nvarchar(64),
    [Pwd] nvarchar(32),
    [ChangePwd] int NOT NULL,
    [Locked] int NOT NULL,
    [RoleId] int NOT NULL,
    [LastPwdChangedTime] datetime NOT NULL,
    [Remark] nvarchar(256)
);

CREATE TABLE [UserMeanRelation] (
    [ID] int PRIMARY KEY NOT NULL,
    [UserName] nvarchar(64) NOT NULL,
    [MeaningsID] int NOT NULL,
    [MeaningDesc] nvarchar(64) NOT NULL,
    [Remark] nvarchar(256)
);
