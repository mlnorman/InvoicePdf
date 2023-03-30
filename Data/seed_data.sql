
   BEGIN TRY
   	BEGIN TRANSACTION
   		
		DECLARE @CreatedDate DATETIME = DATEADD(DAY, -20, CURRENT_TIMESTAMP)

		INSERT INTO InvoiceDocumentsDb.dbo.InvoiceDocuments(CreatedDate, pdfDocument) 
		   SELECT @CreatedDate, BulkColumn
		   FROM OPENROWSET(BULK N'C:\Users\...\PDFProject\Data\invoice_sample_1.pdf', SINGLE_BLOB) AS VARBINARY
   
		INSERT INTO InvoicesDb.dbo.Invoices(CreatedDate, InvoicePeriodStart, InvoicePeriodEnd, DueDate, Description)
		   VALUES ( @CreatedDate, '2023-02-01', '2023-02-19', '2023-03-01', 'Walmart paper sale.')

		SET @CreatedDate  = DATEADD(Month, -10, CURRENT_TIMESTAMP)
		INSERT INTO InvoiceDocumentsDb.dbo.InvoiceDocuments(CreatedDate, pdfDocument) 
		   SELECT @CreatedDate, BulkColumn
		   FROM OPENROWSET(BULK N'C:\Users\...\PDFProject\Data\invoice_sample_2.pdf', SINGLE_BLOB) AS VARBINARY
   
		INSERT INTO InvoicesDb.dbo.Invoices(CreatedDate, InvoicePeriodStart, InvoicePeriodEnd, DueDate, Description)
		   VALUES ( @CreatedDate, '2022-04-19', '2022-04-22', '2022-05-07', 'Invoice for taking scrap removal.')

		SET @CreatedDate  = DATEADD(DAY, 15,DATEADD(year, -2, CURRENT_TIMESTAMP))
		INSERT INTO InvoiceDocumentsDb.dbo.InvoiceDocuments(CreatedDate, pdfDocument) 
		   SELECT @createdDate, BulkColumn
		   FROM OPENROWSET(BULK N'C:\Users\...\PDFProject\Data\invoice_sample_3.pdf', SINGLE_BLOB) AS VARBINARY

		INSERT INTO InvoicesDb.dbo.Invoices(CreatedDate, InvoicePeriodStart, InvoicePeriodEnd, DueDate, Description)
		   VALUES ( @CreatedDate, '2021-04-01', '2021-04-13', '2021-04-25', 'Invoice for taking in paper rolls.')

   	COMMIT TRANSACTION
   END TRY
   BEGIN CATCH
   	PRINT 
   		'Error ' + CONVERT(varchar(50), ERROR_NUMBER()) +
     		', Severity ' + CONVERT(varchar(5), ERROR_SEVERITY()) +
     		', State ' + CONVERT(varchar(5), ERROR_STATE()) +
     		', Line ' + CONVERT(varchar(5), ERROR_LINE())
     
   	PRINT ERROR_MESSAGE();
     
     	IF XACT_STATE() <> 0 BEGIN
   		ROLLBACK TRANSACTION
     	END
   END CATCH;



   --SELECT * FROM InvoicesDb.dbo.Invoices
   --SELECT * FROM InvoiceDocumentsDb.dbo.InvoiceDocuments
   