using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class CategoriaDTO
{
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "A imagem é obrigatória")]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
}

