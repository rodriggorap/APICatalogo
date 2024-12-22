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

public class PostProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PostProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }
    
    [Fact]
    public void PostProduto_Return_CreatedStatusCode()
    {
        Random rand = new Random();
        var test = rand.Next(0, 100);
        
        var novoProdutoDto = new ProdutoDTO
        {
            Nome = "Novo Produto" + test.ToString(),
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 3
        }; 

        var data = _controller.Post(novoProdutoDto);

        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public void PostProduto_Return_BadRequest_ProdutoExistente()
    {
        var novoProdutoDto = new ProdutoDTO
        {
            Nome = "Novo Produto21",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 3
        };

        var data = _controller.Post(novoProdutoDto);

        var createdResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
        createdResult.Subject.StatusCode.Should().Be(400);
    }

    [Fact]
    public void PostProduto_Return_BadRequest_Categoria_Nao_Encontrada()
    {
        var novoProdutoDto = new ProdutoDTO
        {
            Nome = "Novo Produto21",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 55
        };


        var data = _controller.Post(novoProdutoDto);

        var createdResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
        createdResult.Subject.StatusCode.Should().Be(400);
    }
}
