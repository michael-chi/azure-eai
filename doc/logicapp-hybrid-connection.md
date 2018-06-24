Overview
========

In this lab we will be creating a Logic App that connect to an on-prem Web
Service via an on-prem data gateway.

Install on-prem data gateway on local computer
==============================================

-   Download and install data gateway on a local computer that is not the Domain
    Controller and has access to the on-prem resource, in this case, a web
    service.

-   Download and Install Data gateway in a computer that has access to the
    resources we want to access from Azure.

![](media/d6b7d95dd396136bba2a570b86c5c07d.png)

-   Provide a work or school email, this email has to be managed by Azure AD and
    has to be the same one you use to login Azure Portal

![](media/9e09a0c1e17d5c0a79f38bbd63d0127c.png)

-   Provide required information, since this is our first time creating a Data
    gateway, we are not joining existing cluster.

    To change region of this gateway, click “Change Region”, default region is
    West Central US.

![](media/5d00c8dfc9d47a7f6d1328522b738ff6.png)

-   Once configured completed you will be prompt a summary

![](media/212fd6e0a42680c4a3d2667c106917d6.png)

Register data gateway in the Cloud
==================================

-   Now we are to associate our cloud infrastructure with installed on-prem data
    gateway. To do this, create a new on-prem data gateway in Azure Portal.

![](media/f6151fdea696a7e1050a3a375e0daf78.png)

-   Fill in required information. Note that you need to select a region that
    matches the region your date gateway was installed

![](media/5f3331865c6efb763e3517257c254ff4.png)

-   Once created, create a new Custom Connector

![](media/c47cdc4f9e02e19f44596e573a9a7565.png)

-   We want to create a WSDL wrapper hence here we upload WSDL we retrieved from
    Web Service

![](media/b509e6f7d260ef551b11a22cc571c35d.png)

-   Once successfully loaded WSDL, information should be automatically
    populated. Verify if the hostname is valid and Check the “Connect via
    on-prem data gateway” checkbox

![](media/95361b539171822adadd6d5ced3d6fa9.png)

-   Proceed to update connector

Invoking on-prem web service in Logic App
=========================================

-   Now that we have successfully created a on-prem Gateway connection. We are
    good to use it in Logic App.

    To do this, create a new Logic App, add an Action with customer connector
    type

![](media/9113502359dd597a4033e6e19dd0a246.png)

-   Choose the Web Method we want to invoke

![](media/54945ffe61e028243a3a3d5ff17a7f8a.png)

-   Choose Gateway from the dropdown list

![](media/f3b5132e8b6506bb7eddc3a5a24d394e.png)

-   Fill in parameters

![](media/27ab1aa49f87621558685a24aaa71adb.png)

-   Save and run and you should see the result of on-prem web service

![](media/063a3e2498cdf922fbddc52c3ab45550.png)
