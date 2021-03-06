﻿CREATE TABLE [dbo].[SchemaVersions] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ScriptName] NVARCHAR (255) NOT NULL,
    [Applied]    DATETIME       NOT NULL,
    CONSTRAINT [PK_SchemaVersions_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Transaction]...';


GO
CREATE TABLE [dbo].[Transaction] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Amount]      DECIMAL (18, 2) NOT NULL,
    [From]        NVARCHAR (MAX)  NULL,
    [To]          NVARCHAR (MAX)  NULL,
    [Purpose]     NVARCHAR (MAX)  NULL,
    [Category]    NVARCHAR (MAX)  NULL,
    [SubCategory] NVARCHAR (MAX)  NULL,
    [Note]        NVARCHAR (MAX)  NULL,
    [Created]     DATETIME        NOT NULL,
    CONSTRAINT [PK_dbo.Transaction] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[usp_ListTransactions]...';


GO
create procedure usp_ListTransactions
as
begin
    set nocount on

	select * from dbo.[Transaction]
end
GO
PRINT N'Update complete.';


GO
