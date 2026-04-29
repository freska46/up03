using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PakDzal_Games_API.Models;

[Table("Games")]
public class Game
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GameId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerHour { get; set; }

    public bool Available { get; set; } = true;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}