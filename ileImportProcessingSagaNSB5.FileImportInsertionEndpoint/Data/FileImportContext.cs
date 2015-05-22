using System.Data.Entity;

namespace FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Data
{
    public class FileImportContext : DbContext
    {
        public FileImportContext()
        {
            Database.SetInitializer(new FileImportContextInitializer());
        }

        public DbSet<FileImport> FileImports { get; set; }
    }

    public class FileImportContextInitializer : DropCreateDatabaseIfModelChanges<FileImportContext>
    {
    }
}
