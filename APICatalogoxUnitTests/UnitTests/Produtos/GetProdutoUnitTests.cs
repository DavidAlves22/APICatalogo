using APICatalogo.Controllers;
using APICatalogo.Domain.DTOs.Produtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APICatalogoxUnitTests.UnitTests.Produtos;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.unitOfWork, controller.mapper, controller.mapsterMapper);
    }

    [Fact]
    public async Task GetProdutoById_OkResult()
    {
        //Arrange
        var produtoId = 2;
        
        //Act
        var data = await _controller.GetAsync(produtoId);

        //Assert (xunit)
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Asset (FluentAssertions)
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProdutoById_NotFoundResult()
    {
        //Arrange
        var produtoId = 999;

        //Act
        var data = await _controller.GetAsync(produtoId);

        //Assert (xunit)
        //var notFoundResult = Assert.IsType<NotFoundObjectResult>(data.Result);
        //Assert.Equal(404, notFoundResult.StatusCode);

        //Asset (FluentAssertions)
        data.Result.Should().BeOfType<NotFoundObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProdutoById_BadRequestResult()
    {
        //Arrange
        var produtoId = 0;

        //Act
        var data = await _controller.GetAsync(produtoId);

        //Assert (xunit)
        //var badRequestResult = Assert.IsType<BadRequestResult>(data.Result);
        //Assert.Equal(400, badRequestResult.StatusCode);

        //Asset (FluentAssertions)
        data.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetProdutos_Return_ListaProdutosDTO()
    {
        //Act
        var data = await _controller.GetAsync();

        //Asset (FluentAssertions)
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
            .And.NotBeNull();
    }

    [Fact]
    public async Task GetProdutos_BadRequestResult()
    {
        //Para esse teste dar sucesso é necessário lançar uma exceção no método GetAsync do controller ProdutosController (Para simular um erro inesperado)

        //Act
        var data = await _controller.GetAsync();

        //Asset (FluentAssertions)
        data.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
