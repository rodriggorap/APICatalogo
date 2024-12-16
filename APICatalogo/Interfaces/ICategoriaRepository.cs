using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task <PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParams);

    Task <PagedList<Categoria>> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams);
}

