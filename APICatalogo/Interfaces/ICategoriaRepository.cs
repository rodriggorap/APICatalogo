using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task <IPagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParams);

    Task <IPagedList<Categoria>> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams);
}

