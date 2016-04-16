namespace Cadmus.DbUp.Interfaces
{
    public interface IApplicationArguments
    {
        bool Create { get; set; }

        bool Drop { get; set; }

        bool Help { get; set; }

        bool Silent { get; set; }

        int Timeout { get; set; }

        TransactionOption TransactionOption { get; set; }

        bool Upgrade { get; set; }

        bool Version { get; set; }
    }
}