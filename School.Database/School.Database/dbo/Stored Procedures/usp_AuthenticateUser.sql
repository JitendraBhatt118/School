CREATE PROCEDURE [dbo].[usp_AuthenticateUser]
    @Email NVARCHAR (300)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.[UserId],
        u.[FirstName],
        u.[LastName],
        u.[Email],
        u.[Password],
        u.[RoleId],
        r.[Name] AS [RoleName]
    FROM [dbo].[Users] AS u
    INNER JOIN [dbo].[Role] AS r
        ON r.[RoleId] = u.[RoleId]
    WHERE u.[Email] = @Email
        AND u.[IsDeleted] = 0;
END
