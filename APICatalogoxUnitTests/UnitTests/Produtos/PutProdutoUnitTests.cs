using APICatalogo.Controllers;
using APICatalogo.Domain.DTOs.Produtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APICatalogoxUnitTests.UnitTests.Produtos
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.unitOfWork, controller.mapper, controller.mapsterMapper);
        }

        [Fact]
        public async Task PutProduto_OkResult()
        {
            // Arrange
            var produtoDTO = new APICatalogo.Domain.DTOs.Produtos.ProdutoDTO
            {
                Id = 1,
                Nome = "Produto Atualizado",
                Descricao = "Descrição Atualizada",
                Preco = 99.99m,
                ImagemUrl = "http://example.com/imagem-atualizada.jpg",
                CategoriaId = 1 // Certifique-se de que essa categoria existe no banco de dados
            };
            // Act
            var data = await _controller.PutAsync(produtoDTO);

            //Asset (FluentAssertions)
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task PutProduto_BadRequestResult()
        {
            //Arrange

            var produto = new ProdutoDTO
            {
                Id = 0,
                Nome = "Produto Atualizado",
                Descricao = "Descrição Atualizada",
                Preco = 99.99m,
                ImagemUrl = "http://example.com/imagem-atualizada.jpg",
                CategoriaId = 9999
            };

            //Act

            var data = await _controller.PutAsync(produto);

            //Asset (FluentAssertions)

            data.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);
        }
    }
}
