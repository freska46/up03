using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PakDzal_Games_API.Models;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(50)]
    public string City { get; set; } = "Курск";

    public DateTime RegistrationDate { get; set; } = DateTime.Now.Date;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}