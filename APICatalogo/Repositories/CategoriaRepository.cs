using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;


namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{

    public CategoriaRepository(AppDbContext context): base(context)
    {
    }

    public async Task<IPagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParams)
    {
        var categorias = await GetAll();
            
        var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();
        //var resultado = PagedList<Categoria>.ToPageList(categoriasOrdenadas, categoriasParams.PageNumber, categoriasParams.PageSize);

        var resultado = categoriasOrdenadas.ToPagedList(categoriasParams.PageNumber, categoriasParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAll();


        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
        }

        //var categoriasFiltradas = PagedList<Categoria>.ToPageList(categorias.AsQueryable(), categoriasParams.PageNumber, categoriasParams.PageSize);

        var categoriasFiltradas = categorias.ToPagedList(categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasFiltradas;
    }
}

