﻿using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : Controller
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("categoria/{id}")]
    public async Task<ActionResult <IEnumerable<ProdutoDTO>>> GetProdutosCategoria(int id)
    {
        var produtos = await _uof.ProdutoRepository.GetProdutoPorCategoria(id);

        if (produtos is null)
        {
            return NotFound();
        }

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDTO);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get() 
    {
        //throw new Exception("Exceção ao retornar Produtos");
        var produtos = await _uof.ProdutoRepository.GetAll();

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);
        return ObterProdutos(produtos);
    }
        
    [HttpGet("filtro/preco/pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFiltroPrecoParameters)
    {
        var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPreco(produtosFiltroPrecoParameters);
        return ObterProdutos(produtos);        
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {
        var produto = await _uof.ProdutoRepository.Get(p=> p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDTO);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDTO)
    {
        if (produtoDTO is null)
        {
            return BadRequest("Dados inválidos...");
        }

        var produtos = await _uof.ProdutoRepository.GetProdutoPorCategoria(produtoDTO.CategoriaId);

        if (produtos.Count() == 0)
        {
            return NotFound("CategoriaId não encontrado...");
        }

        var produto = _mapper.Map<Produto>(produtoDTO);

        var novoProduto = _uof.ProdutoRepository.Create(produto);
        await _uof.Commit();

        var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDTO)
    {
        if(id != produtoDTO.ProdutoId)
        {
            return BadRequest("Dados inválidos...");
        }

        if (await _uof.ProdutoRepository.Get(p => p.ProdutoId == id) is null)
        {
            return NotFound("Produto não localizado...");
        }

        var produtos = await _uof.ProdutoRepository.GetProdutoPorCategoria(produtoDTO.CategoriaId);

        if (produtos.Count() == 0)
        {
            return NotFound("CategoriaId não encontrado...");
        }

        var produto = _mapper.Map<Produto>(produtoDTO);

        var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        await _uof.Commit();

        var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if(produto is null)
        {
            return NotFound("Produto não localizado...");
        }
        var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
        await _uof.Commit();

        var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);

        return Ok(produtoDeletadoDTO);
    }
}
