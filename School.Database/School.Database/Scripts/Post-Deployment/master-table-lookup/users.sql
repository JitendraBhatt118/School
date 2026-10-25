EXEC sp_execute_script
    @sql = '
        SET NOCOUNT ON;

        DECLARE @AdministratorRoleId INT =
        (
            SELECT TOP (1) [RoleId]
            FROM [dbo].[Role]
            WHERE [Name] = N''Administrator''
        );

        IF @AdministratorRoleId IS NULL
        BEGIN
            RAISERROR(''Administrator role was not found. Seed Role before Users.'', 16, 1);
            RETURN;
        END

        CREATE TABLE #UsersTemp
        (
            [FirstName] NVARCHAR (50)   NOT NULL,
            [LastName]  NVARCHAR (50)   NOT NULL,
            [Email]     NVARCHAR (300)  NOT NULL,
            [Password]  NVARCHAR (2000) NOT NULL,
            [RoleId]    INT             NOT NULL
        );

        INSERT INTO #UsersTemp
        (
            [FirstName],
            [LastName],
            [Email],
            [Password],
            [RoleId]
        )
        VALUES
        (
            N''Jitendra'',
            N''Bhatt'',
            N''Jitendra.bhatt@injala.com'',
            N''9ea338953f5d4fe4b6686dbd21b639ae4d655e23b69192acd5fd85e48557cbac'',
            @AdministratorRoleId
        );

        MERGE [dbo].[Users] AS u
        USING #UsersTemp AS ut
            ON u.[Email] = ut.[Email]
        WHEN MATCHED THEN
            UPDATE SET
                u.[FirstName] = ut.[FirstName],
                u.[LastName]  = ut.[LastName],
                u.[Password]  = ut.[Password],
                u.[RoleId]    = ut.[RoleId],
                u.[IsDeleted] = 0,
                u.[ModifiedDate] = GETUTCDATE()
        WHEN NOT MATCHED BY TARGET THEN
            INSERT
            (
                [FirstName],
                [LastName],
                [Email],
                [Password],
                [RoleId],
                [CreatedBy],
                [CreatedDate],
                [IsDeleted]
            )
            VALUES
            (
                ut.[FirstName],
                ut.[LastName],
                ut.[Email],
                ut.[Password],
                ut.[RoleId],
                NULL,
                GETUTCDATE(),
                0
            );

        DROP TABLE #UsersTemp;
    ',
    @author = 'School.Database',
    @build  = '2026-09-17';
