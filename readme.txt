my take (using NSB 5.x) on Sam Martindale's NSBCon 2014 presentation: Building a Highly Scalable File Processing Platform with NServiceBus. (http://fast.wistia.net/embed/iframe/np7c60nlm5?popover=true). My change to this was instead of making a database call directly in the Saga, I use pub/sub to send a message out of the Saga, have a handler check for me, and then return the results to the Saga.

How to Run
==========
- This projects assumes you have copy of SQL Server LocalDb 2012 installed. If you don't have sql server installed, the EF code to store your import rows and check counts will fail
- make sure thee following projects are set for startup:
	- FileImportProcessingSaga.ClassClient
	- FileImportProcessingSaga.FileImportInsertionEndpoint
	- FileImportProcessingSaga.SagaEndpoint
- when prompted by the .ClassClient console window (make sure the other two endpoints are spun up), hit enter and watch as the Saga sends messagse to another handler to check the status of succeeded and failed counts for the import every 5 seconds
- note that the .FileInsertionEndpoint contains the CheckFileImportSuccessAndFailureCountHandler, which handles a command sent from the Saga every 5 seconds to do a query against the table into which we're inserting our row imports
- also note the use of Bus.Reply() (the message sent on Reply is of type :IMessage, not :ICommand or IEvent) to communicate back to the Saga the amount failed and succeeded, and when that message is recieved, the Saga figures out if it's done or not