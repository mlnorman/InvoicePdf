using InvoiceClient.Infrastructure;
using InvoiceClient.Models;
using InvoiceClient.Pages;
using InvoiceClient.Services;
using InvoiceDocumentApi.Controllers;
using InvoiceDocumentApi.Infrastructure;
using InvoiceDocumentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFProject.Tests
{
    public class InvoiceClientTests
    {
        [Fact]
        public async Task GetAllInvoices_WhenCalled_ReturnAllInvoices()
        {

            var contextMock = new Mock<InvoiceContext>();
            contextMock.Setup<DbSet<Invoice>>(x => x.Invoices).ReturnsDbSet(GetInvoices());

            var configMock = new Mock<IConfiguration>();

            InvoiceService service = new(contextMock.Object, configMock.Object);

            InvoicesModel model = new(service);
            await model.OnGetAsync();

            Assert.NotNull(model.Invoices);
            Assert.Single(model.Invoices);
        }

        [Fact]
        public async Task OnGetAsync_WhenCalled_ReturnAllInvoices()
        {
            var contextMock = new Mock<InvoiceContext>();
            contextMock.Setup<DbSet<Invoice>>(x => x.Invoices).ReturnsDbSet(GetInvoices());

            var configMock = new Mock<IConfiguration>();

            InvoiceService service = new(contextMock.Object, configMock.Object);

            var invoices = await service.GetAllInvoices();

            Assert.NotNull(invoices);
            Assert.Single(invoices);
        }

        private static List<Invoice> GetInvoices()
        {
            return new List<Invoice>() {
                new Invoice
                {
                    InvoiceId = 1,
                    InvoicePeriodStart= DateTime.Now.AddDays(1),
                    InvoicePeriodEnd = DateTime.Now.AddDays(10),
                    DueDate = DateTime.Now.AddDays(15),
                    CreatedDate = DateTime.Now,
                    Description = "Test",
                }
            };
        }
    }
}
