using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProductSystem.BLL.DTOs
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    public class CategoryUpdateDto
    {
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
