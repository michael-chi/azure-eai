Overview
========

In this lab we will be creating a Logic App that simulates Order business flow.
We will be addressing how a Logic App flow integrates with backend APIs with
Azure AD authentication and returns result to frontend. In a message pattern,
this usually refer to a Channel, which take messages from incoming adapter,
process and then send to next flow for further processing.

A high-level design looks like this

![](media/e35bc0a8044ca3440825fd822f645508.png)

Create business flow
====================

-   This flow takes messages in the internal Service Bus Queue we created in
    previous lab, so here we will be using Service Bus connector to get those
    messages which we have been through it several times.

    -   Here we use a “auto-complete” trigger, which a message was found in the
        Service Bus Queue, it take that message from the queue and automatically
        complete that message. In this case, if the flow failed for some reason,
        we would have to handle that error and message in an error handler
        process, otherwise that message will be “lost”.

![](media/90aacd829d24cdc6b27fb24cd74cb542.png)

-   Next we want to invoke our backend API to process this message, in this lab
    we have already have our simulation backend REST API deployed to Azure. So
    here we will be creating a HTTP action to communicate with it.

![](media/faeb8640af9e2e288562310759d81183.png)

-   The API allows HTTP POST only, so we choose POST here and specify its URL

![](media/2e453eaa93f2410a0a45eff0b583b1f1.png)

-   We want to modify our Posting body as a (custom-defined) JSON document with
    below format. In Logic App you do this by specifying your own content in
    text with “expressions” when needed.

-   In this case, the message content that will be sending to the backend API is
    the XML message content we retrieved from Service Bus Queue, we just need to
    decorate it with surrounding JSON tags.

{

“content”:”\<MESSAGE CONTENT\>”

}

-   Modify “Body” column as below

![](media/2e453eaa93f2410a0a45eff0b583b1f1.png)

-   Move your cursor to the middle of “double quote”

![](media/c86a8341eb15d68a7e944b72a8fc34f6.png)

-   From right panel, click “Content” from Service Bus Action

![](media/846223b93bb6e7810e671df81e6ac698.png)

-   Your action should look similar to this

![](media/e41bad10d3564f0bd34cf761716d321e.png)

-   Now we want to send the message back to Contoso. Our protocol is to put
    messages to a particular queue so that Sender adapter flow will pick them up
    and send back to Contoso. So here we create a Service Bus – Send Message
    Action

![](media/b9819b5b0a0a5f5cb87b4364d2090644.png)

-   Message Body should be the output of previous HTTP action

![](media/e5742c36ad054701566910487e6e0ab1.png)

-   Now, let’s add an Action to handle errors. Click the “+” icon following HTTP
    action, and add an parallel branch.

![](media/d1da5cc9b4b76d9cead5b2abf527b0f8.png)

-   When an error occurs, in this case, POST to internal API failed, depends on
    your business flow you may want to resubmit it again, or you may want to
    trigger other error handler flow to take care of that message. Here we want
    to send an email to administrators to take care of that particular order

![](media/1d8a3e9c705b7dd872dd9fefce8959f3.png)

-   You will need to sign-in to your Office 365 account in order to send emails
    on your behalf

![](media/4d0b267581241c1531d8eec75b33253e.png)

-   Email Body should be the message content from Service Bus Queue, you can
    modify properties of this email when needed.

![](media/0494f94d4132b8312f694ba488b853ff.png)

-   This Action should only be triggered when Invocation has failed

![](media/f7762b6296ebe7906386dedc2fcefd37.png)

-   This is how your flow should look like

![](media/033a39ee8bfb085be45cc0539f17ff84.png)

-   Save your Logic App

Invoke Azure AD Protected API in Logic App

In this section we will be discussing how a Logic App action invokes backend API
protected by Azure AD with Logic App’s built-in support.

-   Go to HTTP action

-   Click “Show Advanced Options”

![](media/5322aa03bb438555e7711d6bdf22665d.png)

-   Choose Auzre AD Oauth

![](media/085cc515c751bad27eae05364d9b169b.png)

-   Fill in required information

    -   Tenant is the tenant ID of your Azure AD tenant, it can be found in
        Azure Active Directory, Properties, Directory ID

    -   Audience is the application Id of your backend application since after
        authentication succeed, the token audience is our backend application

    -   Client ID is your frontend application’s Azure AD application ID

    -   Credential Type should be Secret

    -   Secret is the Key we generated for your frontend application

![](media/5cc51c1d376b87f7cac8066bfb98d244.png)

-   Click Save to save your changes.
