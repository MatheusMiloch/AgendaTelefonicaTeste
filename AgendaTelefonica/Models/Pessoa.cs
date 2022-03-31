using System.ComponentModel.DataAnnotations;

namespace AgendaTelefonica.Models
{
    public class Pessoa
    {
        [Key]
        public int PessoaiId { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(40, ErrorMessage = "Use menos caracteres")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(40, ErrorMessage = "Use menos caracteres")]
        public string? Sobrenome { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [Range(18, int.MaxValue, ErrorMessage = "Idade inválida")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(40, ErrorMessage = "Use menos caracteres")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public TipoTelefone TipoTelefone { get; set; }

        public string Foto { get; set; }
    }

    public enum TipoTelefone
    {
        Pessoal, Comercial
    }
}