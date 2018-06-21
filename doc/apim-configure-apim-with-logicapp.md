Overview
========

An API Management instance is an API gateway that provides throttling control,
access restriction control and management to your APIs. In this lab we will be
managing our APIs with APIM and configure access restriction policy to restrict
access to our APIs.

In this document we will be demonstrating how APIM protects and manages your
backend API. We will configure APIM to allow and check Oauth 2.0 based
authentication token. We will also configure Logic App to allow traffic from
APIM only so that your Logic App is protected by APIM with Oauth 2.0.

Prerequirests
=============

-   Azure Subscription

-   Complete [Backend API Azure AD setup](backendapi-setup-azuread.md) and
    [business flow Logic Apps](create-business-flow.md)

Create an APIM managed Logic App API
====================================

-   Goto Azure Portal, create an empty APIM instance, here I choose Basic tier,
    for development and testing purpose, Developer tier is sufficient.

![](media/721a4f592f9b8b34aaa54320b061751c.png)

-   Once created, open up Essential Blade, copy the VIP address of APIM
    instance, we will need it later.

![](media/117674983663b75be2c02fcd13fd78dd.png)

-   Next, we will be creating a set of “API”, which backed by our Logic App so
    that client (the Trading partner) does not directly interact with Logic App
    but go througth APIM. Hence we have opportunity to manage and monitor usage
    from different partners.

-   To create new API, click API in left panel, Add API then Logic App

![](media/fc7d536a3ac5c15f5aad9741ccd93776.png)

-   Choose the receiver adapter Logic App we created earlier.

    -   API URL suffix will be added to the end of your API URL, if your API
        come with query string then it can make API call not functional properly

    -   Tags can be any text

    -   Product is how you group your APIs into a set of APIs provided to your
        customers. We use default Product “Starter” here, you can create new one
        if you like

    -   Versioning

        -   Check “Vision this API” if you expect to provide different version
            of API

        -   Versioning schema is how your APIs differenciate between different
            API versions

        -   Version identifier will be added to your URL to differenciate
            different versions

![](media/5e34fed1b9b66e03b3e72e949eb335f0.png)

Verify APIM APIs
================

-   Once created, you shoud see screen similar like this. To verify our work,
    click Developer portal to bring up developer portal.

![](media/50d081f8543710468ba2305b2793dad5.png)

-   As we are the administrator of this API, we are automatically signed in.
    Click Products, then click on the Products we added the receiver adapter
    API, in this case, “Starter”.

![](media/12842a13222ab56bcff8c56253142026.png)

-   Click the API we added

![](media/b30de0d09911e69e2a9c2ce7b655be4a.png)

-   Click “Try it”to test the API

![](media/f3741709b7727737bc8a02d6ac135a3d.png)

-   Paste sample request XML and click Send button in the bottom of test page

![](media/dd54ea47b747d842ce3b140d5bb75753.png)

-   If everything works well, you should see below response. Now our Logic App
    is ready to accept order from our trading partners.

![](media/3316086f65048f022b3749d11f4b5fa1.png)

-   If required, you can change the URL schema in APIM. Go to APIM, APIs,
    \<Logic API\>, Choose specific operation, Design then click the “Pen” icon

![](media/2e7af1a14f0bbe1082c2c29d0a34107a.png)

-   Modify URL here

![](media/4ebb6a7ba4e6ea4e80747a2d5feed97f.png)

-   Once Saved, go back to Developer Portal, Refresh the test page and Try Again
    you’ll see the request URL has changed.

![](media/396a4ef38174c3a062cb5dfdac1b6596.png)

Protect Logic App with APIM
===========================

In this section, we will be demonstrating protect our Logic App with APIM.

We will first setup Logic App’s IP restriction that only allows traffic coming
from APIM to Logic App.

Then we will configure Azure AD integration on APIM and verify if required claim
exists in JWT token, only when it exists in the token will the request be routed
to backend Logic App.

>   Note that at this moment, Logic App does not allows “Authorization” Http
>   Header, so we will store Oauth token to another Http Header nstead before
>   request routed to Logic App.

Setup Logic App IP whitelist

In this section we will configure Logic App to only allows traffic coming from
APIM

-   Go to Azure portal, open the receiver adapter Logic App then Workflow
    Settings

![](media/e938eb6c0fd745e8d894e951cead898c.png)

-   Add APIM’s IP address here then Save

![](media/17220e788a39459d920f9be7a1cfbde0.png)

-   Now if you try to POST message to Logic App URL, you will get below error
    message

![](media/796bc16eb6ef36931360a7c5e49d733f.png)

Configure APIM with OAuth authentication
