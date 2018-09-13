Overview
========

Batch Processing is a common scenario while doing EAI solution, batch processing usually involves system integration. System A sending batch messages to our EAI solution, our EAI solution may need to correlate all these messages into one bigger message for further processing.
In this document, I will be demonstrating how to leverage Logic App's batch feature to help you collect messages and process them as a batch.

High level architecture
=======================
A high level architecture would be like below.
External system send files to pre-configured storage account, each file names start with a batch Id, in this case, 001, 002...etc. Our batch sender process with correlate all 3 files with same batch Id into one big message and send to batch receiver process.

![](media/batch-001.png)

Design Batch Receiver flow
==================================

This flow receives batch messages from Batch Sender flow. It waits until all three files were recevied before it process further. As a demo I will not implemenet actual process here but will simply print out message detail in the flow. In real case you may need to iterate all three files, retrieve their content and aggregate and process them.

You may also want to delete or move received files to another location in order to prevent duplicate processing, this is also NOT convered in this document.

-   Batch Receiver flow will stay idle until all three files are received. We use a Batch trigger to wait files sent by Batch Sender flow.

![](media/batch-receiver-001.png | width=100)

-   Specify required information
    -   Batch Name is the unique identifier of this batch flow, batch sender sends different batches from different systems to different flow by this name.
    -   Release Criteria is how Batch release batch messages for furture processing, in this case, Batch are released only when we have three files.

![](media/batch-receiver-2.png)

-   Add a Compose action, specify "Message Content" in "Input" field. Logic App Desinger automatically wrap Componse action with a "For Each" loop as Batch Trigger return an array.

![](media/batch-receiver-3.png)

-   Your flow shoud look like this

![](media/batch-receiver-4.png)

Design Batch Sender flow
==================================

-   We need a workflow to check the storage account periodically for incoming files. Once we have new files, the process will then extract file name and file content. In order to check storage account, first we want to create a Storage trigger.

![](media/batch-002.png)

-   Specify Storage account and container name where files are sent to and save configuration.

![](media/batch-003.png)

-   Fill in required information. Note that you need to select a region that
    matches the region your date gateway was installed
