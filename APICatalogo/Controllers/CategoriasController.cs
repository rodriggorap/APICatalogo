using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : Controller
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    /*
    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
    {
        return _context.Categorias.AsNoTracking().Include(p=> p.Produtos).ToList();
    }
    */
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        var categorias = await _uof.CategoriaRepository.GetAll();

        var categoriaDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriaDTO);
    }

    [HttpGet("filter/nome/pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas = await _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltro);

        return ObterCategorias(categoriasFiltradas);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")] 
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        var categoria = await _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaDTO);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);
        return ObterCategorias(categorias);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriasDTO);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
    {
        if(categoriaDTO is null)
        {
            return BadRequest("Dados inválidos...");
        }

        var categoria = _mapper.Map<Categoria>(categoriaDTO);

        var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
        await _uof.Commit();

        var categoriaCriadaDTO = _mapper.Map<CategoriaDTO>(categoriaCriada);

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoriaCriadaDTO.CategoriaId }, categoriaCriadaDTO);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
    {
        if (id != categoriaDTO.CategoriaId)
        {
            return BadRequest("Dados inválidos...");
        }

        if (await _uof.CategoriaRepository.Get(c => c.CategoriaId == id) is null)
        {
            return NotFound("Produto não localizado...");
        }

        var categoria = _mapper.Map<Categoria>(categoriaDTO);

        var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
        await _uof.Commit();

        var categoriaAtualizadaDTO = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaAtualizadaDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {
        var categoria = await _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Produto não localizado...");
        }
        
        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        await _uof.Commit();

        var categoriaExcluidaDTO = _mapper.Map<ProdutoDTO>(categoriaExcluida);

        return Ok(categoriaExcluida);
    }
}

