using FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Data;
using NServiceBus;
using NServiceBus.Config;

namespace FileImportProcessingSagaNSB5.FileImportInsertionEndpoint
{
    public class EnsureTablesAreCreatedWhenConfiguringEndpoint : IWantToRunWhenConfigurationIsComplete
    {
        public void Run(Configure config)
        {
            using (var context = new FileImportContext())
                context.Database.Initialize(false);
        }
    }
}
