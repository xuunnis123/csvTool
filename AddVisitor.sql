-- Create a new stored procedure called 'AddVisitor' in schema 'dbo'
-- Drop the stored procedure if it already exists
IF EXISTS (
SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
WHERE SPECIFIC_SCHEMA = N'dbo'
    AND SPECIFIC_NAME = N'AddVisitor'
    AND ROUTINE_TYPE = N'PROCEDURE'
)
DROP PROCEDURE dbo.AddVisitor
GO
-- Create the stored procedure in the specified schema
CREATE PROCEDURE dbo.AddVisitor
    @id int,
    @visitor_phone int = 0,
    @visitor_name VARCHAR(255) =''

-- add more stored procedure parameters here
AS
BEGIN

    -- body of the stored procedure
    INSERT INTO dbo.visitor (id,visitor_phone,visitor_name) VALUES(@id,@visitor_phone,@visitor_name)
END
GO
-- example to execute the stored procedure we just created
EXECUTE dbo.AddVisitor 1 /*value_for_param1*/, 2 /*value_for_param2*/
GO

