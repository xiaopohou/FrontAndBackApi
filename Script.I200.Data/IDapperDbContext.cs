using System;
using System.Data;

namespace Script.I200.Data
{
    public interface IDapperDbContext : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
