create table DatabaseInfo
(
    DatabaseId uniqueidentifier not null
)

GO

insert into DatabaseInfo (DatabaseId)
values (newid())
