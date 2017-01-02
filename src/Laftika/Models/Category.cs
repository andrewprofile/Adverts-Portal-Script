using System.ComponentModel.DataAnnotations.Schema;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Laftika.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
    }
}
