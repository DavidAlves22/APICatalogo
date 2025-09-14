using APICatalogo.Controllers;
using APICatalogo.Domain.DTOs.Produtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoxUnitTests.UnitTests.Produtos
{
    public class PostProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public PostProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.unitOfWork, controller.mapper, controller.mapsterMapper);
        }

        [Fact]
        public async Task PostProduto_CreatedResult()
        {
            //Arrange
            var produtoDTO = new ProdutoDTO
            {
                Nome = "Produto Teste",
                Descricao = "Descrição do Produto Teste",
                Preco = 10.50M,
                ImagemUrl = "http://teste.com/produto.jpg",
                CategoriaId = 1
            };

            //Act
            var data = await _controller.PostAsync(produtoDTO);

            //Assert (xunit)
            //var createdResult = Assert.IsType<CreatedAtRouteResult>(data.Result);
            //Assert.Equal(201, createdResult.StatusCode);

            //Asset (FluentAssertions)
            data.Result.Should().BeOfType<CreatedAtRouteResult>()
                .Which.StatusCode.Should().Be((int)System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task PostProduto_BadRequestResult()
        {
            //Arrange
            ProdutoDTO produtoDTO = null;

            //Act
            var data = await _controller.PostAsync(produtoDTO);

            //Assert (xunit)
            //var badRequestResult = Assert.IsType<BadRequestObjectResult>(data.Result);
            //Assert.Equal(400, badRequestResult.StatusCode);

            //Asset (FluentAssertions)
            data.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);
        }
    }
}
