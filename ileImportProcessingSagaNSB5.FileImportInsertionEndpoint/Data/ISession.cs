using System;
using System.Linq;

namespace FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Data
{
    public interface ISession : IDisposable
    {
        void Add<T>(T entity) where T : class;
        void AddOrUpdate<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        IQueryable<T> Query<T>() where T : class;
        void SaveChanges();
    }
}
