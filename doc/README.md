Overview
========

In this hands-on lab, we will be demonstrating a typical EAI message endpoint
pattern implementation with Azure Logic App and APIM.

In real world, each trading partner can use different protocols such as FTP,
HTTP and Queue…etc; Message pattern in EAI is to separate message sending and
receiving process with message channel, so that each part knows only small
pieces of the message processing process.

A high-level overview of the architecture illustrated below.

-   Trading Partner represents the organization that will be sending or
    receiving documents (sales orders for example).

-   Adapter Flow represents a sub-flow that handles incoming and outgoing
    messages, including transform and validation.

-   Transaction Flow is the flow to process this message, including enrich
    messages.

![](media/52ab7b0f55ed239aff8e31b614aa0716.png)

We will use this hands-on lab to create above flow. Below function and services
will be covered in this document.

-   Logic App

-   API Management

-   Azure AD

-   Azure Functions

-   Monitoring your flow

Prerequisites
=============

-   Azure Subscription

-   Visual Studio Code

-   Visual Studio Community + (For creating frontend and backend API)

Table of Content
================

-   Preparation

    -   [Create a Backend API App](#create-a-backend-api-app)

    -   [Set up Azure AD Authentication for Backend API](backendapi-setup-azuread.md#configure-azure-ad-authentication-for-your-backend-api)

    -   [Verify Azure AD with Frontend App](backendapi-setup-azuread.md#register-frontend-application)

-   Create you flow

    -   [Integration
        Account](create-receiver-adapter.md#create-an-integration-account)

    -   [Service Bus Queue](create-receiver-adapter.md#create-service-bus-queue)

    -   [Receiver Adapter](create-receiver-adapter.md)

    -   [Send Adapter](create-send-adapter.md)

    -   [Business Flow](create-business-flow.md) and Handle API with Azure AD
        Authentication

-   Manage your API

    -   APIM integration with Logic App

    -   Azure AD integration with your backend API

    -   Create a sample frontend app

-   Monitor your business flow with built-in services

Create a Backend API App
========================

-   For simplicity sake, we’ve prepare a backend [REST
    API](../source/EAIBackendAPI). It only echoes back fixed string in this lab,
    you can modify it to fit your requirement.

-   Open the solution file with Visual Studio and deploy to you Azure
    Subscription

-   Make sure you see below results.

![](media/e86d951fba3e07b81576ffbf59970cd9.png)

Configure Backend API with AAD Authentication
=============================================

-   Enable Backend API’s AAD authentication

![](media/5f0cba92c024501b3773d155f852f06f.png)

APIM
====

-   Create from Swagger File

![](media/ff5a4b04db199b9fd742c4153d25f398.png)

-   Upload Swagger file and specify an API URL suffix

![](media/16c1f9d607ee675a47d6ae9beee7c74a.png)

-   Configure Azure AD OAuth Integration

![](media/ca8785473800378d6698498c098f093f.png)

-   First part

![](media/e12ff17ff26ccfaf9a2cc8c02a4bc19b.png)

-   Get Endpoints from Azure AD console

![](media/c61456094658f3d1e9fa4f96a812137c.png)

-   Second part

![](media/c72cabbdf8f2a7bdc4ec5d3653420d3e.png)

-   Grant Type (For OAuth)

![](media/4e703d1d42d85f5cd6ab1e37058d138e.png)

-   Get Backend Application Id from Azure AD

![](media/89ca258ef3e8239712ab5e492bdb53cc.png)

-   Paste it here

![](media/dc48274c9b81fde3efc78bc6f513783e.png)

-   Specify Client Credential, Retrieve Client ID and Secret from Azure AD app
    page

![](media/9e48cfc6e78072990fb21f3f420dee29.png)

-   Paste it here

![](media/6ae5c2db2950d4e4568522486c472995.png)

-   Note down Reply URL

![](media/71aa4036148d246361b4f51dc658557e.png)

-   Complete

![](media/7098c8d2977b5570936955b769248fe5.png)

-   Got back to API, Settings

![](media/7198d86fc4067d4cea2e20af453869b8.png)

-   Choose OAuth Authentication we just created

![](media/9429bb532a9a6c4624cf6f3068e0b1f9.png)

-   Get back to APIM, Create or Add our API to a product

![](media/4b51f8ee39aacbbdb4b0359552330fb6.png)

-   Go to Azure AD, we need to update Frontend App’s Reply URL

![](media/0c2bea27ed2ce6868150f4cedc2099b4.png)
