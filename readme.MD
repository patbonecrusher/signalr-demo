
Run the server
Then run the client

The client will connect with the server

In the winform, there is a button.  
Clicking the button does: 
  - sends a message to the signalr server
	- the server sends the message back
	- the hub handler gets called
	- it fires an event
	- the ui receives the event
	- the ui then update a label in the form with the message


