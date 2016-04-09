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

