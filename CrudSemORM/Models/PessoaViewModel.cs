using System;
using System.ComponentModel.DataAnnotations;

namespace CrudSemORM.Models
{
    public class PessoaViewModel
    {
        [Key]
        public int PessoaID { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public string Rg { get; set; }
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string NomeMae { get; set; }
        public string Profissao { get; set; }

        
    }
}
