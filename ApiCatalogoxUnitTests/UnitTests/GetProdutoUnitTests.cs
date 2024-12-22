using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public void GetprodutoById_OkResult()
    {
        //Arrange
        var prodId = 3;

        //Act
        var data = _controller.Get(prodId);

        //Assert (xunit)
        //var oKResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, oKResult.StatusCode);

        //Assert (fluentassertions)
        data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
                
    }

    [Fact]
    public void GetprodutoById_Return_NotFound()
    {
    
        var prodId = 325;
        var data = _controller.Get(prodId);

        data.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);

    }

    [Fact]
    public void GetprodutoById_Return_BadRequest()
    {

        var prodId = -1;
        var data = _controller.Get(prodId);

        data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);

    }

    [Fact]
    public void GetprodutoById_Return_ListOfProdutoDTO()
    {
        var data = _controller.Get();

        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>().And.NotBeNull();

    }
}
