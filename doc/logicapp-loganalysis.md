Overview
========

In this lab we will add monitoring mechanism to our Logic App so that we can
track Logic App run status in Log Analysis service.

Add Tracked Properties
======================

In each Logic Action, you can add “Tracked Properties” to it. A tracked property
is a property that holds important information of incoming or outgoing message
such as Order Number or Customer ID…etc. Tracked Properties will be send to
specified diagnostic log storage for future processing and tracking.

-   To add a tracked property, open Logic App Code View of your receiver adapter
    flow

![](media/4943c71d637dc71b3acbca3fedbc2752.png)

-   Add below code to your Send_Message action

"trackedProperties": {

"OrderNumber2":
"\@{first(xpath(xml(decodeBase64(triggerOutputs().body['\$content'])),'//OrderNumber/text()'))}"

},

-   Your Send_Message should look similar below

![](media/c00af9fec5e346c01b2199bed9f81f2c.png)

-   Save it and the Post a new message to APIM endpoint

Monitor Message and Logic App Runs
==================================

-   Go back to Azure Portal, Open the Log Analysis instance you configured Logic
    App to send diagnostic logs to and click Overview

![](media/0effbc9df5a5018a86f8c67409c9ef3d.png)

-   You should see a default dashboard. Now we want to add Logic App solution
    into our dashboard, Click “Add” to add a new solution

![](media/09e4080336b5540f0f89275573eb083b.png)

-   Add Logic Apps Management (Preview)

![](media/92867f790b4342def2a066d265b229d6.png)

-   Once added, wait few minutes for data to populate, you should see some data
    shown in the dashboard.

![](media/f2d2118175bc5758c823e0267a6bd213.png)

-   Click on Logic Apps Management (Preview) tile will bring up a detail
    dashboard

![](media/28b200488de407b5e12bca1ce9b3ba8c.png)

-   Click “Logic App Runs” will show you detail information of each run

![](media/ed581f30e0b581c5165724c1eba40d0c.png)

-   Tracked Properties we added in previous step has been added to the view as
    well.

![](media/7991d256568e75db5b901f127f0ee522.png)

-   Click Analytics

![](media/d0a9407aa90ff1d4d2512dc654746090.png)

-   Start a new Tab and input below query to query window, this will search for
    “demo-receive-adapter” Logic App runs has Tracked Properties OrderNumbers2
    with value equals “2001”

>   AzureDiagnostics

>   \| where Category == "WorkflowRuntime"

>   \| where resource_workflowName_s == "demo-receive-adapter"

>   \| where (trackedProperties_OrderNumber2_s) == "2001"
