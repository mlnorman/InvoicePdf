# PDF Project
## Application to display a list of invoices with the ability to download a pdf of the invoice from a web api.

Contains the following projects
* Duende Identiy Server project for the application IDP.
* ASP.NET Core Razor Pages project for the application client.  Used Razor Pages since I am not too familiar with it and wanted to try it out.
* ASP.NET Core Web Api for the document api.


### How it works
User is required to log in to view invoices.  Using OIDC the client will authenticate with identity server and be redirected back to the application index page. Depending on the pre-created account used to log in they will have different access based on the roles. 
* admin user has the admin role - User can do everything.
* readonly user has the readonly role - User can view invoices but not download the invoice PDF.
* noaccess user has the noaccess role - User does not have access to view anything.

Invoices and Invoice Documents are stored in 2 separate database.  The razor pages client will pull invoices from the Invoices database.

When the download button id clicked for an invoice an API request is made to the api project and the file is returned from the InvoiceDocuments database.

Assumptions were made that invoice numbers are unique, so an identity was used when inserting into the database.

The InvoiceDocuments table has a trigger that will set a bit on the Invoices table when a new document is inserted into the InvoiceDocuments table.

This bit is used to determine whether or not to show a "Download" button on the invoice in the razor pages client.  If a document does not exist, no reason to show a download button.

When the download button is clicked a file exporer will open so the file can be saved.

Both client and api are protected by Identity Server, so a valid access token is necessary to make a call to the api.

# How to run
**It is recommened to have .Net 7 as well as the latest dotnet EF Core tool installed**

1. Update the DefaultConnection connection string, setting the "Server" value.  Leave the database names the same for seed data to work without modifications.
* ./InvoicePdf/InvoiceClient/appsettings.Development.json
* ./InvoicePdf/InvoiceDocumentApi/appsettings.Development.json

2. Build project.
* `cd InvoicePdf`
* `dotnet build`

3. Run migrations in this order.  Ensured no errors from creating the InvoiceDocuments trigger.
* `cd InvoiceClient`
* `dotnet ef database update`
* `cd ../InvoiceDocumentApi`
* `dotnet ef database update`

4. Run Seed Data.
* `cd InvoicePdf/Data`
* Run the seed_data.sql script in SSMS.  You will need to update the path to the files in the seed_data.sql script.
* This inserts a couple of invoices and test documents

5. Run.
* Easiest way is to open the solution is Visual Studio.  All projects are already set to launch.
* Otherwise dotnet run for each project.
