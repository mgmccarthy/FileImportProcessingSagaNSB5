using System;
using FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Data;
using FileImportProcessingSagaNSB5.Messages.Commands;
using FileImportProcessingSagaNSB5.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Handlers
{
    public class ProcessImportFileRowHandler : IHandleMessages<ProcessImportFileRow>
    {
        private readonly IDataStore dataStore;
        private readonly IBus bus;

        public ProcessImportFileRowHandler(IDataStore dataStore, IBus bus)
        {
            this.dataStore = dataStore;
            this.bus = bus;
        }

        public void Handle(ProcessImportFileRow message)
        {
            if (message.FirstImportRowForThisImport)
                bus.Publish(new FileImportInitiated { ImportId = message.ImportId, TotalNumberOfFilesInImport = message.TotalTotalNumberOfFilesInImport });

            //check/validate import data. In the real world, there would be rules run here, db queries, etc... to determine if it's succesful or not
            var success = new Random().Next(100) % 2 == 0;
            LogManager.GetLogger(typeof(ProcessImportFileRowHandler)).Warn(string.Format("Handling ProcessImportFileRow for Customer: {0}", message.CustomerId));
            using (var session = dataStore.OpenSession())
            {
                session.Add(new FileImport { Id = Guid.NewGuid(), ImportId = message.ImportId, CustomerId = message.CustomerId, CustomerName = message.CustomerName, Successfull = success });
                session.SaveChanges();
            }
        }
    }
}
