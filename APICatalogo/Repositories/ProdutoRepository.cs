using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{

    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    /*public IEnumerable<Produto> GetProduto(ProdutosParameters produtosParams)
    {
        return GetAll()
            .OrderBy(p => p.Nome)
            .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSIze)
            .Take(produtosParams.PageSIze).ToList();
    }*/

    public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = await GetAll();
        var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();
        var resultado = PagedList<Produto>.ToPageList(produtosOrdenados, produtosParams.PageNumber, produtosParams.PageSize);

        return resultado;
    }

    public async Task<IEnumerable<Produto>> GetProdutoPorCategoria(int id)
    {
        var produtoPorCategoria = await GetAll();
        return produtoPorCategoria.Where(c => c.CategoriaId == id);
    }

    public async Task<PagedList<Produto>> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams)
    {
        var produtos = await GetAll();

        if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
        {
            if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("==", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
        }
        var produtosFiltrados = PagedList<Produto>.ToPageList(produtos.AsQueryable(), produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);

        return produtosFiltrados;
    }
}

