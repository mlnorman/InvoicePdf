using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceDocumentApi.Migrations
{
    public partial class initital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PdfDocument = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDocumentsId", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDocuments_InvoiceId",
                table: "InvoiceDocuments",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.Sql("""
                    CREATE TRIGGER dbo.trg_DocumentInsert
                 	    ON dbo.InvoiceDocuments
                 	    AFTER Insert
                     AS		
                	      UPDATE Invoices.dbo.Invoices SET DocumentExists = 1 WHERE InvoiceId = (SELECT InvoiceId FROM inserted);
                """);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER dbo.trg_DocumentInsert");

            migrationBuilder.DropTable(
                name: "InvoiceDocuments");
        }
    }
}
