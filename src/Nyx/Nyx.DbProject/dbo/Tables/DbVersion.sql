CREATE TABLE [dbo].[DbVersion] (
    [Version] VARCHAR (50)  NOT NULL,
    [Note]    VARCHAR (255) NULL,
    [Date]    DATETIME      NOT NULL,
    CONSTRAINT [PK_DbVersion] PRIMARY KEY CLUSTERED ([Version] ASC)
);

