namespace FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Data
{
    public interface IDataStore
    {
        ISession OpenSession();
    }
}
