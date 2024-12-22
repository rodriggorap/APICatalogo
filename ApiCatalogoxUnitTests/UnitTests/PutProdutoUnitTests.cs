using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiCatalogoxUnitTests.UnitTests;

public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PutProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public void PutProduto_Return_OkResult()
    {
        var prodId = 6;
        var updateProdutoDto = new ProdutoDTO
        {
            ProdutoId = prodId,
            Nome = "Novo Produto212",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 3
        };

        var result = _controller.Put(prodId, updateProdutoDto);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void PutProduto_Return_BadRequest_Dados_Invalidos()
    {
        var prodId = 100;
        var updateProdutoDto = new ProdutoDTO
        {
            ProdutoId = 6,
            Nome = "Novo Produto212",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 3
        };
        var result = _controller.Put(prodId, updateProdutoDto);
        var updateResult = result.Result.Should().BeOfType<BadRequestObjectResult>();
        updateResult.Subject.StatusCode.Should().Be(400);
        updateResult.Subject.Value.Should().Be("Dados inválidos...");
    }

    [Fact]
    public void PutProduto_Return_BadRequest_Produto_Nao_Localizado()
    {
        var prodId = 100;
        var updateProdutoDto = new ProdutoDTO
        {
            ProdutoId = prodId,
            Nome = "Novo Produto212",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 3
        };
        var result = _controller.Put(prodId, updateProdutoDto);
        var updateResult = result.Result.Should().BeOfType<NotFoundObjectResult>();
        updateResult.Subject.StatusCode.Should().Be(404);
        updateResult.Subject.Value.Should().Be("Produto não localizado...");
    }
}
