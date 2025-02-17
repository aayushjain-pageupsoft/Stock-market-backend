using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }

        //Navigation properties
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }
    }
}
