CREATE TABLE [dbo].[__MigrationLog] (
    [SqlHash]       BINARY (64)    NOT NULL,
    [StartTime]     DATETIME2 (7)  NOT NULL,
    [EndTime]       DATETIME2 (7)  NULL,
    [SqlText]       NVARCHAR (MAX) NOT NULL,
    [WasSuccessful] BIT            DEFAULT ((0)) NOT NULL,
    [Error]         NVARCHAR (MAX) NULL,
    [Author]        NVARCHAR (250) NULL,
    [Build]         NVARCHAR (50)  NULL,
    CONSTRAINT [PK_MigrationLog] PRIMARY KEY CLUSTERED ([SqlHash] ASC)
);
