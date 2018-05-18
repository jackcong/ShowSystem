using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.DC;

namespace DataAccess
{
    public interface ILoggedEntity
    {
        long Id { get; }

        string LoggedType { get; }


    }
}
