using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    //IEnumerable<Produto> GetProduto(ProdutosParameters produtosParams);
    Task <PagedList<Produto>> GetProdutos(ProdutosParameters produtosParams);

    Task <PagedList<Produto>> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
    Task <IEnumerable<Produto>> GetProdutoPorCategoria(int id);
}

