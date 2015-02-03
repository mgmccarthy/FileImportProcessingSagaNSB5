using System.Linq;
using FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Data;
using FileImportProcessingSagaNSB5.Messages.Commands;
using FileImportProcessingSagaNSB5.Messages.InternalMessages;
using NServiceBus;

namespace FileImportProcessingSagaNSB5.FileImportInsertionEndpoint.Handlers
{
    public class CheckFileImportSuccessAndFailureCountHandler : IHandleMessages<CheckFileImportSuccessAndFailureCount>
    {
        private readonly IBus bus;
        private readonly IDataStore dataStore;

        public CheckFileImportSuccessAndFailureCountHandler(IBus bus, IDataStore dataStore)
        {
            this.bus = bus;
            this.dataStore = dataStore;
        }

        public void Handle(CheckFileImportSuccessAndFailureCount message)
        {
            int rowsSucceeded;
            int rowsFailed;
            using (var session = dataStore.OpenSession())
            {
                rowsSucceeded = session.Query<FileImport>().Where(x => x.ImportId == message.ImportId).Count(x => x.Successfull);
                rowsFailed = session.Query<FileImport>().Where(x => x.ImportId == message.ImportId).Count(x => !x.Successfull);
            }

            //http://stackoverflow.com/questions/22973996/nservicebus-wcf-service-with-error-system-invalidoperationexception-reply-is-ne
            bus.Reply(new FileImportSuccesAndFailureCount { ImportId = message.ImportId, RowsSucceeded = rowsSucceeded, RowsFailed = rowsFailed });
        }
    }
}
