using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Domain.Entities
{
    public class BaseEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
