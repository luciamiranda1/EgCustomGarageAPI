using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }
}
