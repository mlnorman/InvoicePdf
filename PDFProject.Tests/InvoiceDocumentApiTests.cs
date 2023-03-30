using Castle.Core.Logging;
using InvoiceDocumentApi.Controllers;
using InvoiceDocumentApi.Infrastructure;
using InvoiceDocumentApi.Models;
using InvoiceDocumentApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PDFProject.Tests
{
    public class InvoiceDocumentApiTests
    {
        [Fact]
        public async Task GetAsync_WhenCalled_ReturnsDocumentAsync()
        {
            var loggerMock = new Mock<ILogger<InvoiceDocumentController>> ();
            ILogger<InvoiceDocumentController> logger = loggerMock.Object;

            var serviceMock = new Mock<IInvoiceDocumentService>();

            InvoiceDocumentController controller = new(serviceMock.Object, loggerMock.Object);
            var document = (await controller.GetAsync(1));

            Assert.NotNull(document);
            Assert.IsAssignableFrom<ActionResult>(document);
        }

        [Fact]
        public async Task GetAsync_WhenCalledWithZeroId_ReturnsNotFound()
        {
            var loggerMock = new Mock<ILogger<InvoiceDocumentController>>();
            ILogger<InvoiceDocumentController> logger = loggerMock.Object;

            var serviceMock = new Mock<IInvoiceDocumentService>();

            InvoiceDocumentController controller = new(serviceMock.Object, loggerMock.Object);
            var document = (await controller.GetAsync(0));

            Assert.IsType<NotFoundResult>(document);
        }

        [Fact]
        public async Task GetDocumentByInvoiceId_WhenCalled_ReturnsDocument()
        {
            var mock = GetDocuments().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(1)).ReturnsAsync(GetDocuments().Find(x => x.Id == 1));

            var contextMock = new Mock<InvoiceDocumentContext>();
            contextMock.Setup(x => x.InvoiceDocuments).Returns(mock.Object);

            var loggerMock = new Mock<ILogger<InvoiceDocumentService>>();
            ILogger<InvoiceDocumentService> logger = loggerMock.Object;

            InvoiceDocumentService service = new(contextMock.Object, logger);

            var document = (await service.GetDocumentByInvoiceId(1));

            Assert.NotNull(document);
            Assert.Equal(1, document.InvoiceId);
            Assert.Equal(Encoding.ASCII.GetBytes("testing"), document.PdfDocument);
        }


        private static List<InvoiceDocument> GetDocuments()
        {
            return new List<InvoiceDocument>() {new InvoiceDocument()
                {
                    InvoiceId = 1,
                    CreatedDate = DateTime.Now,
                    Id = 1,
                    PdfDocument = Encoding.ASCII.GetBytes("testing")
                }
            };
        }
        
    }
}
