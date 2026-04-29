using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PakDzal_Games_API.Models;

[Table("Sessions")]
public class Session
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SessionId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [Required]
    public int GameId { get; set; }

    [ForeignKey(nameof(GameId))]
    public virtual Game? Game { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? TotalPrice { get; set; }
}