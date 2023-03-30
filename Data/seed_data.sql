 BEGIN TRY
   	BEGIN TRANSACTION
   		
		DECLARE @CreatedDate DATETIME = DATEADD(DAY, -20, CURRENT_TIMESTAMP);
		INSERT INTO Invoices.dbo.Invoices(CreatedDate, InvoicePeriodStart, InvoicePeriodEnd, DueDate, Description, DocumentExists)
		   VALUES ( @CreatedDate, '2023-02-01', '2023-02-19', '2023-03-01', 'Walmart paper sale.', 0);

		INSERT INTO InvoiceDocuments.dbo.InvoiceDocuments(InvoiceId, CreatedDate, pdfDocument) 
		   SELECT 1, @CreatedDate, BulkColumn
		   FROM OPENROWSET(BULK N'C:\Users\...\PDFProject\Data\invoice_sample_1.pdf', SINGLE_BLOB) AS VARBINARY;
   
	
		SET @CreatedDate  = DATEADD(Month, -10, CURRENT_TIMESTAMP);
		INSERT INTO Invoices.dbo.Invoices(CreatedDate, InvoicePeriodStart, InvoicePeriodEnd, DueDate, Description, DocumentExists)
		   VALUES ( @CreatedDate, '2022-04-19', '2022-04-22', '2022-05-07', 'Invoice for taking scrap removal.', 0);

		INSERT INTO InvoiceDocuments.dbo.InvoiceDocuments(InvoiceId, CreatedDate, pdfDocument) 
		   SELECT 2, @CreatedDate, BulkColumn
		   FROM OPENROWSET(BULK N'C:\Users\...\PDFProject\Data\invoice_sample_2.pdf', SINGLE_BLOB) AS VARBINARY;
   
		
		SET @CreatedDate  = DATEADD(DAY, 15,DATEADD(year, -2, CURRENT_TIMESTAMP));

		INSERT INTO Invoices.dbo.Invoices(CreatedDate, InvoicePeriodStart, InvoicePeriodEnd, DueDate, Description, DocumentExists)
		   VALUES ( @CreatedDate, '2021-04-01', '2021-04-13', '2021-04-25', 'Invoice for taking in paper rolls.', 0);

		INSERT INTO InvoiceDocuments.dbo.InvoiceDocuments(InvoiceId, CreatedDate, pdfDocument) 
		   SELECT 3, @createdDate, BulkColumn
		   FROM OPENROWSET(BULK N'C:\Users\...\PDFProject\Data\invoice_sample_3.pdf', SINGLE_BLOB) AS VARBINARY;

		INSERT INTO Invoices.dbo.Invoices(CreatedDate, InvoicePeriodStart, InvoicePeriodEnd, DueDate, Description, DocumentExists)
		   VALUES ( current_timestamp, '2023-04-03', '2023-04-10', '2023-04-25', 'No document exists.', 0);

	

   	COMMIT TRANSACTION
   END TRY
   BEGIN CATCH
   	PRINT 
   		'Error ' + CONVERT(varchar(50), ERROR_NUMBER()) +
     		', Severity ' + CONVERT(varchar(5), ERROR_SEVERITY()) +
     		', State ' + CONVERT(varchar(5), ERROR_STATE()) +
     		', Line ' + CONVERT(varchar(5), ERROR_LINE())
     
   	PRINT ERROR_MESSAGE();
    ROLLBACK TRANSACTION
     	
   END CATCH;