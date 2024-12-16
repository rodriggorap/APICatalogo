using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{

    public CategoriaRepository(AppDbContext context): base(context)
    {
    }

    public async Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParams)
    {
        var categorias = await GetAll();
            
        var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();
        var resultado = PagedList<Categoria>.ToPageList(categoriasOrdenadas, categoriasParams.PageNumber, categoriasParams.PageSize);

        return resultado;
    }

    public async Task<PagedList<Categoria>> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAll();


        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPageList(categorias.AsQueryable(), categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasFiltradas;
    }
}

