using ScreenLocker.Model;
using System;

namespace ScreenLocker.Data.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Student> StudentRepository { get; }
        IRepository<UsageLog> UsageLogRepository { get; }

        int Save();
    }
}