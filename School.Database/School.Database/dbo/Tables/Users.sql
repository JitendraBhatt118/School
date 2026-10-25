CREATE TABLE [dbo].[Users] (
    [UserId]       BIGINT          IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (50)   NOT NULL,
    [LastName]     NVARCHAR (50)   NOT NULL,
    [Email]        NVARCHAR (300)  NOT NULL,
    [Password]     NVARCHAR (2000) NOT NULL,
    [RoleId]       INT             NOT NULL,
    [CreatedBy]    BIGINT          NULL,
    [CreatedDate]  DATETIME        NOT NULL,
    [ModifiedBy]   BIGINT          NULL,
    [ModifiedDate] DATETIME        NULL,
    [IsDeleted]    BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UX_Users_Email] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [FK_Users_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([RoleId]),
    CONSTRAINT [FK_Users_CreatedBy_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_Users_ModifiedBy_Users] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
);
