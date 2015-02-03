using System;

namespace FileImportProcessingSagaNSB5.Messages.InternalMessages
{
    //apparently, when you use Bus.Reply(), the message you reply on the bus cannot be of type IEvent or ICommand. It has to be IMessage
    //http://stackoverflow.com/questions/22973996/nservicebus-wcf-service-with-error-system-invalidoperationexception-reply-is-ne
    public class FileImportSuccesAndFailureCount
    {
        public Guid ImportId { get; set; }
        public int RowsSucceeded { get; set; }
        public int RowsFailed { get; set; }
    }
}
