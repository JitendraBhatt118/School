EXEC sp_execute_script
    @sql = '
        SET NOCOUNT ON;

        CREATE TABLE #RoleTemp
        (
            [RoleId] INT           NOT NULL,
            [Name]   NVARCHAR (50) NOT NULL
        );

        INSERT INTO #RoleTemp
        (
            [RoleId],
            [Name]
        )
        VALUES
             (1, N''Administrator'')
            ,(2, N''Accountant'')
            ,(3, N''User'');

        SET IDENTITY_INSERT [dbo].[Role] ON;

        MERGE [dbo].[Role] AS r
        USING #RoleTemp AS rt
            ON r.[RoleId] = rt.[RoleId]
        WHEN MATCHED AND r.[Name] <> rt.[Name] THEN
            UPDATE SET
                r.[Name] = rt.[Name]
        WHEN NOT MATCHED BY TARGET THEN
            INSERT ([RoleId], [Name])
            VALUES (rt.[RoleId], rt.[Name]);

        SET IDENTITY_INSERT [dbo].[Role] OFF;

        DROP TABLE #RoleTemp;
    ',
    @author = 'School.Database',
    @build  = '2026-09-17';
