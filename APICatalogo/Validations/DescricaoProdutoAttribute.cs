using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations
{
    public class DescricaoProdutoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            //var descricao = value.ToString().Trim();
            if (value.ToString().Trim().Length < 5)
            {
                return new ValidationResult("A descrição deve ter mais de 4 caracteres");
            }

            return ValidationResult.Success;
        }
    }
}
