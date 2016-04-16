CREATE TABLE [dbo].[SchemaVersions] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ScriptName] NVARCHAR (255) NOT NULL,
    [Applied]    DATETIME       NOT NULL,
    CONSTRAINT [PK_SchemaVersions_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

