using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;

namespace Cadmus.DbUp
{
    public enum ReturnCode
    {
        Success = 0,
        ArgumentsError = -1,
        Failure = -2,
        Exception = -3,
    }

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var parser = new ApplicationArgumentsParser();
                var result = parser.Parse(args);

                if (result.HasErrors)
                {
                    ApplicationArgumentsParser.ShowHelp();
                    return (int)ReturnCode.ArgumentsError;
                }

                var router = new Router(parser.Arguments);
                var routerResult = router.Route();
                return routerResult ? (int)ReturnCode.Success : (int)ReturnCode.Failure;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return (int) ReturnCode.Exception;
            }
        }
    }
}
