# Upload file to onedrive

In this example we use: 
 - MSAL
 - Microsoft Graph
 - Microsoft Azure 
 - Microsoft 365
 - Onedrive


# Prerequisites:

1. You need Microsoft 365 Office Business license.

This license allows to use Microsoft Graph API to access Onedrive.
Microsoft Graph API is a part of cloud services accessible through Azure Portal
You need to register license owner as Azure user. 
If you register for trial access to Microsoft 365, you will get access to Azure Portal 
and license automatically.

2. You need to register your application in Azure.

In Azure portal go to App Registrations and register your application.
You will need to give the delegated permissions to Microsoft Graph API for your application.
You can find permissions for Onedrive under Files (Files.Read, Files.ReadWrite, etc.)

3. You need to create client secret.
It will be used to access Onedrive without standard login popup window.

4. You need to copy Onedrive owner Id into app configuration.
This is GUID id of Azure user, that has access to Onedrive.


# Configuration settings:

set the variables in appsettings.json:
 - appId: 		Guid value, Client Id, or Application Id from App Registrations
 - clientSecret: 	Guid value, Client secret from App Registrations 
 - tenantId:		Guid value, Id of your Azure tenant, can be seen in App Registrations
 - userId:		Guid value, Id of Azure user for delegated access to Onedrive



