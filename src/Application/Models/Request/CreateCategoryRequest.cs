using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class CreateCategoryRequest
    {
        public required string Name { get; set; } //qué campos espera la API cuando alguien crea o actualiza una categoría.lo que el usuario manda
    }
}