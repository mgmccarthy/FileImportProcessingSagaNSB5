using System;
using FileImportProcessingSagaNSB5.Messages.Commands;
using FileImportProcessingSagaNSB5.Messages.Events;
using FileImportProcessingSagaNSB5.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

namespace FileImportProcessingSagaNSB5.SagaEndpoint
{
    public class ProcessImportFileSaga : Saga<ProcessImportFileSaga.SagaData>,
        IAmStartedByMessages<FileImportInitiated>,
        IHandleMessages<FileImportSuccesAndFailureCount>,
        IHandleTimeouts<ProcessImportFileSaga.TimeoutState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessImportFileSaga));

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<FileImportInitiated>(msg => msg.ImportId).ToSaga(data => data.ImportId);
        }

        public void Handle(FileImportInitiated message)
        {
            Log.Warn("Starting ProcessImportFileSaga via FileImportInitiated");

            Data.ImportId = message.ImportId;
            Data.TotalNumberOfFilesInImport = message.TotalNumberOfFilesInImport;

            SendCheckFileImportSuccessAndFailureCount(message.ImportId);
        }

        public void Handle(FileImportSuccesAndFailureCount message)
        {
            Log.Warn("handling FileImportSuccesAndFailureCount");
            Log.Warn(string.Format("RowsSucceeded: {0}, RowsFailed: {1}", message.RowsSucceeded, message.RowsFailed));

            if (message.RowsSucceeded + message.RowsFailed == Data.TotalNumberOfFilesInImport)
            {
                Bus.Publish(new FileImportCompleted { ImportId = message.ImportId });
                Log.Warn("Saga Complete");
                MarkAsComplete();
            }
            else
            {
                RequestTimeout<TimeoutState>(TimeSpan.FromSeconds(5));
            }
        }

        public void Timeout(TimeoutState state)
        {
            Log.Warn("Sending CheckFileImportSuccessAndFailureCount.");
            SendCheckFileImportSuccessAndFailureCount(Data.ImportId);
        }

        private void SendCheckFileImportSuccessAndFailureCount(Guid importId)
        {
            Bus.Send(new CheckFileImportSuccessAndFailureCount { ImportId = importId });
        }

        public class SagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }

            [Unique]
            public Guid ImportId { get; set; }
            public int TotalNumberOfFilesInImport { get; set; }
        }

        public class TimeoutState { }
    }
}
