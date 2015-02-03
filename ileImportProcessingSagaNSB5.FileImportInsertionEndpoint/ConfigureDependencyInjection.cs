using FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Data;
using NServiceBus;

namespace FileImportProcessingSagaNSB5.FileImportInsertionEndpoint
{
    public class ConfigureDependencyInjection : INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.RegisterComponents(reg => reg.ConfigureComponent<DataStore>(DependencyLifecycle.InstancePerCall));
        }
    }
}
