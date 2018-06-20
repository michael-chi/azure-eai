Overview
========

In this document, we will be creating a receiver adapter logic app to receive
messages sending to our organization from an external trading partner, in this
case, Contoso.

We will be using a pre-defined Xml schema as incoming message format, validate
incoming message with this schema and transform it to internal message format
and send transformed message to a Service Bus queue for further processing.
Incoming message will be coming through a HTTP REST endpoint provided by this
flow.

A high-level overview of the architecture illustrated below.

![](media/7007d4d2ca64ca6a8de208ea445c5f30.png)

Prerequisites
=============

-   Azure Subscription

# Create an Integration Account
=============================

An Integration Account is a container that manages B2B artifacts used in your
flow.

-   Goto Azure Portal, create new Integration Account

![](media/9a4897ebfd4273d0f61c2520337fdff8.png)

-   Give this account a name, which better matches to the trading partner’s
    name. In our case we can use
    [Basic](https://azure.microsoft.com/en-us/pricing/details/logic-apps/)
    pricing tier.

![](media/1c0fa3b4b08533b1c865af23315760ff.png)

-   Once created, you should see console like below

![](media/94e08666c6cab6b5b7cb6bb0fa4d4079.png)

-   Now we will be creating Schemas and Maps and add to this Integration Account
    so that our Logic App can access these artifacts in logic app flow.

-   For simplicity, we’ve provided pre-defined schema and maps
    [here](../source/LogicApps/Artifacts), you can create your own when needed.

-   Click the “Schemas” block, then Add a new Schema

![](media/d1877fa9ed26d9e32831f648fb97e7a8.png)

-   Repeat above steps to add Order.xsd and Order-Sap.xsd.

    -   Order.xsd is the schema of incoming messages from Contoso

    -   Order-Sap.xsd represents the schema of internal SAP message format

-   Click Maps block and add a new Map

    -   Order-Map-To-SAP map is the mapping between Order and internal SAP
        message format.

![](media/b9cb6272cfc82c53ee4d2148188634eb.png)

# Create Service Bus Queue
========================

In our lab we are using a Service Bus queue as an integration point between our
backend and logic app flow. In real case you can use other technology as it
provide reliability delivery.

-   Go to Azure Portal, create a new Service Bus namespace.

![](media/7c6c982b41ff72e0e1e06bea08aeeafe.png)

In our case a Basic tier service bus is sufficient.

-   Once created, add a queue

    -   Name of the queue should be easy to identify its purpose, for example,
        Contoso-To-SAP, so that administrators or developers can easily
        understand what the purpose of this queue is.

>   Your company may also have naming convention policy you want to follow.

-   In real life case, you may want to enable dead lettering so that your
    application has opportunities to handle expired messages.

![](media/01bd80692f62469e1b1873a6352a632b.png)

-   We need below three queues in this lab

![](media/8004f128bdb755b5a170d4a813be66fd.png)

-   Adapter-receivefrom-contoso

>   As name suggested, this queue receives message from Contoso sending to our
>   organization

-   Adapter-sendto-contoso

>   Our organization will be sending messages to Contoso through this queue

-   Queue-contoso-to-sap

>   This is an internal queue for Contoso orders to be sent to SAP

# Create Receiver Adapter
=======================

In this section, we will be creating a receiver adapter flow to allow Contoso
submit their PO to our system.

-   Create an empty Logic App

    -   The name of the logic app should be human readable and understandable.
        For example in this case you can name your flow as
        “adapter-receiveFrom-Contoso” which as the name suggested, are to
        receive messages coming from Contoso

-   Once created, go to Logic App console, we need to associate this logic app
    with the Contoso Integration Account we created above

![](media/c3cbf98116350cc22e6f5e5699cf2d05.png)

-   You should see below screen once created your Logic app. Click big cross to
    create a new blank Logic App

![](media/4f8cf9072cb715e132af9bc76614040d.png)

-   Since in this flow we are to receive messages coming from Contoso through
    HTTP REST endpoint, here we choose Request as our Logic App trigger.

![](media/9c9607fc08bc8c0b82e846693937d476.png)

-   When a HTTP Request is received, we will trigger this process.

![](media/9536d42592946151284b1c560b1c9971.png)

-   By default, Logic App Http trigger accepts Json format request body. If you
    do accept JSON document, here you can specify a sample JSON document so the
    Logic App generates schema for you to easily access its elements.

>   Here we are to accept XML body.

>   Click “Show advanced options”

![](media/3fe460449e05518fdfd7a80063cba961.png)

-   We want to accept HTTP POST only

![](media/03916626d4f7361e0976613e5505e70b.png)

-   You can find our HTTP Endpoint here.

![](media/7e00fa32ad9662c7ce756cd7c7802e04.png)

-   Click “New Step” to add a new step then add an Action

![](media/9466e40f8136f700a37066d55328f2d8.png)

-   We want to validate incoming message is a valid XML

![](media/0b241f79f42ce6697907ad90180c1697.png)

-   Here we define from which part of the request this action will be retrieve
    incoming Xml document from Http Trigger, and what is the schema name to
    valid the incoming message.

>   We assume the Xml is coming from HTTP POST body and we use pre-created Order
>   schema to validate incoming message

![](media/8a58e39e432f5430bfe5668aa3d3e3f8.png)

-   Once finished, create next action reply error message back to Contoso when
    Xml validation failed

![](media/66f6b36ae941d5acd125325c3b45437f.png)

-   In this lab, we just return a Xml message that state the incoming message
    failed processing. In real case you may want to contain more detailed
    information to Contoso so they can handle it properly.

-   Fill in an error xml message in “Body” section then Click the “…” menu in
    the upper right corner then “Configure run after”

![](media/8f848fc8fd03b435f9d20fa95781d1a9.png)

-   This action is to send error message back to trading partner (Contoso) so we
    only need to run this action when Xml validation failed

![](media/523fbac84ec695ba41a7834bd9ab3457.png)

-   Your Logic App should look like below

![](media/e90737de42c32f19f5757f29ecc74342.png)

-   Next, we want to add an Action to transform incoming message to internal SAP
    acceptable message. This will be a parallel action that kicks-off only when
    Xml validation succeed.

![](media/0f9c67e40a4b5109b306de5af6fa9976.png)

-   Add a xml transform action

![](media/fb766615fbd92408a58b263fa0cbb95f.png)

-   Specify Request body as Xml content and choose the map we created
    (Order-To-SAP) above as Map.

![](media/2072ee841710f71284b26dfc83752363.png)

-   Your Action should look like below

![](media/c164a5264dce33b5e8b95207446f2a85.png)

-   Also, we want this Action to be triggered when Xml validation succeed

![](media/4793b426fbf08f5be6d3c1472eba4958.png)

-   At this point, your Logic App should looks like below

![](media/1a7d23696e9777cd41605cb28127c5e2.png)

-   Following Transform Xml action, we want to track orders coming from Contoso.
    We add a “Compose” action and specify items we want to track in this action

![](media/4c84bc1a79783b1f9eed558444736bf7.png)

-   Next we want to send transformed order to the service bus queue we created.

![](media/114507524e47cde8096d338a4566343e.png)

-   Choose Send Message

![](media/2b2011b6e30b8625086a22cd6db894ce.png)

-   Configure action as below.

    -   The Xml content is set to Transformed Xml so that transformed SAP
        message will be sent.

    -   The queue name is set to queue-contoso-to-sap so that message will be
        placed in the queue we used to communicate between SAP and our flow
        system.

    -   Content Type is set to application/xml to specify that message is in xml
        format. Details of content types in Logic App are described here:
        <https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-content-type>

![](media/3d005e3529bd0dbc3f1e18b273378a7d.png)

-   This Action should only be triggered when Transform Xml succeed

![](media/5bda06cc5e29722470f8565db702a80c.png)

-   Now your Logic App should look like this

![](media/dcbc16da8f0aeb64c80df7f518c66aae.png)

-   Next, we want to send response message to Contoso stating that we have
    successfully received order and successfully validate incoming message.

>   An order can take several days to proceed, so here we are not returning
>   actual order result to our trading partner, but only a notification of
>   acceptance.

-   Add a Response Action

![](media/4a20105645850b5e6ca4c585b38eff44.png)

-   Here we just notify Contoso that we have accepted incoming order

![](media/5be49a819bf9c9381053980f66f5ba46.png)

-   Save your Logic App
