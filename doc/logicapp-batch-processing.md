Overview
========

Batch Processing is a common scenario while doing EAI solution, batch processing usually involves system integration. System A sending batch messages to our EAI solution, our EAI solution may need to correlate all these messages into one bigger message for further processing.
In this document, I will be demonstrating how to leverage Logic App's batch feature to help you collect messages and process them as a batch.

High level architecture
=======================
A high level architecture would be like below.
External system send files to pre-configured storage account, each file names start with a batch Id, in this case, 001, 002...etc. Our batch sender process with correlate all 3 files with same batch Id into one big message and send to batch receiver process.


<img src="media/batch-001.png" width="600">

Design Batch Receiver flow
==================================

This flow receives batch messages from Batch Sender flow. It waits until all three files were recevied before it process further. As a demo I will not implemenet actual process here but will simply print out message detail in the flow. In real case you may need to iterate all three files, retrieve their content and aggregate and process them.

You may also want to delete or move received files to another location in order to prevent duplicate processing, this is also NOT convered in this document.

-   Batch Receiver flow will stay idle until all three files are received. We use a Batch trigger to wait files sent by Batch Sender flow.

<img src="media/batch-receiver-001.png" width="600">

-   Specify required information
    -   Batch Name is the unique identifier of this batch flow, batch sender sends different batches from different systems to different flow by this name.
    -   Release Criteria is how Batch release batch messages for furture processing, in this case, Batch are released only when we have three files.

<img src="media/batch-receiver-2.png" width="600">

-   Add a Compose action, specify "Message Content" in "Input" field. Logic App Desinger automatically wrap Componse action with a "For Each" loop as Batch Trigger return an array.

<img src="media/batch-receiver-3.png" width="600">

-   Your flow shoud look like this

<img src="media/batch-receiver-4.png" width="600">

Design Batch Sender flow
==================================

In this demo, batch files are named with below pattern.
```
{BATCH-ID}-{FILE-Type}.txt
```
    For example, 001-OrderDetail.txt

-   We need a workflow to check the storage account periodically for incoming files. Once we have new files, the process will then extract file name and file content. In order to check storage account, first we want to create a Storage trigger.

<img src="media/batch-002.png" width="600">

-   Specify Storage account and container name where files are sent to and save configuration.

<img src="media/batch-003.png" width="600">

-   Add a new List Blob action (under Storage Blob catalog), specify the Storage and container where files are sent.

<img src="media/batch-004.png" width="600">

-   Add a "Initialize Variable" action and create a String variable called "sessionId". This is the session Id Logic App used to identify different batches. In our case, this is the first part of batch files.

<img src="media/batch-005.png" width="600">

-   Add a For Each loop

<img src="media/batch-006.png" width="600">

-   We will iterate each item returned from "List Blobs" action

<img src="media/batch-007.png" width="600">

-   Add a Set Variable action to change SessionID variable we created above

<img src="media/batch-008.png" width="600">

-   Specify "Current Item" to Value field

<img src="media/batch-009.png" width="600">

-   Switch to "Code View", modify action script as below

<img src="media/batch-10.png" width="600">

-   Add "Get Blob Content using Path" action, specify "Path" as its path

<img src="media/batch-11.png" width="600">

-   Add a "Batch" action, select "Batch Receiver" flow we created above

<img src="media/batch-12.png" width="600">

-   Batch Name set to "demo" which is exact same with the batch name we specify in "Batch Receiver" flow

<img src="media/batch-13.png" width="600">

-   Contruct you own message in "Message Content" field, in this case I am creating a Json object contains all information and file content. Also set "Partition Name"to the SessionID variable we assigned above.

<img src="media/batch-15.png" width="600">

-   Save

-   To verify result, upload 3 files with names start with 001 (ex, 001-A.txt, 002-B.txt, 003-C.txt), you will see you Batch Sender flow receives 3 files and send information to Batch Receiver.

<img src="media/batch-16.png" width="600">