using ScreenLocker.Data.Contracts;
using ScreenLocker.Model;
using ScreenLocker.Model.Contracts;
using System;

namespace ScreenLocker.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IContext context;
        private bool disposed;

        public UnitOfWork(IContext context)
        {
            this.context = context;
        }

        private IRepository<Student> studentRepository;
        public IRepository<Student> StudentRepository => studentRepository ?? (studentRepository = new Repository<Student>(context));

        private IRepository<UsageLog> usageLogRepository;
        public IRepository<UsageLog> UsageLogRepository => usageLogRepository ?? (usageLogRepository = new Repository<UsageLog>(context));

        public void Dispose()
        {
            if (!disposed)
            {
                context.Dispose();
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }

        public int Save()
        {
            int changes = 0;

            if (!disposed && context.ChangeTracker.HasChanges())
            {
                changes = context.SaveChanges();
            }

            return changes;
        }
    }
}