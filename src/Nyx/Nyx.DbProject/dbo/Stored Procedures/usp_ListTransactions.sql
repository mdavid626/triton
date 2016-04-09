create procedure usp_ListTransactions
as
begin
    set nocount on

	select * from dbo.[Transaction]
end