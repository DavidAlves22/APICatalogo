﻿using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Domain.DTOs.Categorias
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }
    }
}
