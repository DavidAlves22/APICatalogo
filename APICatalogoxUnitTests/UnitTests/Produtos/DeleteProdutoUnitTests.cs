using APICatalogo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APICatalogoxUnitTests.UnitTests.Produtos
{
    public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.unitOfWork, controller.mapper, controller.mapsterMapper);
        }

        [Fact]
        public async Task DeleteProduto_OkResult()
        {
            // Arrange
            var produtoId = 17; // ID do produto que você deseja deletar (certifique-se de que este ID exista no banco de dados de teste)
            // Act
            var data = await _controller.Delete(produtoId);

            // Assert
            //Assert.IsType<OkObjectResult>(data);

            //Asset (FluentAssertions)
            data.Result.Should().NotBeNull();
            data.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteProduto_NotFoundResult()
        {
            // Arrange
            var produtoId = 13; // ID do produto que você deseja deletar (certifique-se de que este ID NÃO exista no banco de dados de teste)
            // Act
            var data = await _controller.Delete(produtoId);

            // Assert
            //Assert.IsType<NotFoundObjectResult>(data);

            //Asset (FluentAssertions)
            data.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteProduto_BadRequestResult()
        {
            // Arrange
            var produtoId = 0; 
            // Act
            var data = await _controller.Delete(produtoId);

            // Assert
            //Assert.IsType<NotFoundObjectResult>(data);

            //Asset (FluentAssertions)
            data.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
