my take (using NSB 5.x) on Sam Martindale's NSBCon 2014 presentation: Building a Highly Scalable File Processing Platform with NServiceBus. (http://fast.wistia.net/embed/iframe/np7c60nlm5?popover=true). My change to this was instead of making a database call directly in the Saga, I use pub/sub to send a message out of the Saga, have a handler check for me, and then return the results to the Saga.

things I don't like about this solution
---------------------------------------
- each mesage sent to import a row should not have to carry the import id on it or have the knowledge if it's the first import row for a given import

potential solutions
-------------------
- do a compare on the SagaID and/or unique corrlation Id's in order to tell if a saga is running for a given import.
	- check SiteAdjustmentAlertSaga:
		if (Data.AdjustmentId == message.AdjustmentInformation.AdjustmentId)
		return;
	- if the Data.ImportId == message.ImportId then return
		- this thing that's stupid about this is the client has to send more than one message... one to start the saga, then a message per import row
			- what would be optimal is the client sends ONE message to start the import.
- IF we send one message to start the import, we're then in a transaction of a handler, which means for every command sent from that handler to process an import row, we need to wait until ALL commands are sent to call the transation successfull
	- b/c of this, those import row messages will not start to be proccessed until they are all sent, and that defeats the scalability of the import in the first place
	- RESEARCH: is it possible to send multiple commands from one handler and have thosse commands poicked up by the receiving handler asynchoruslly w/out waiting for the transaction to complete?

- Write an NSB Saga solution that checks the saga for completeness on EVERY row process import and see how that compares to this solution

- Could we chunk messages being sent from a transactional endpoint so we send out 100 messages at a time insetead of the handler waiting for ALL messages???

