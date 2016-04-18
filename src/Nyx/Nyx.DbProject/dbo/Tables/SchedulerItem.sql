CREATE TABLE [dbo].[SchedulerItem] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (MAX) NULL,
    [Interval]      INT            NOT NULL,
    [Title]         NVARCHAR (MAX) NULL,
    [LastRun]       DATETIME       NULL,
    [State]         NVARCHAR (MAX) NULL,
    [NextRun]       DATETIME       NULL,
    [Locked]        BIT            NOT NULL,
    [LockValidTime] INT            NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.SchedulerItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);

