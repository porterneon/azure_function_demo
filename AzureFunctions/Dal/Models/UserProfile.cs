using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureFunctionDemo.Dal.Models
{
    public class UserProfile
    {
        [Required]
        [StringLength(50)]
        public string Domain { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ManagerEmail { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [StringLength(50)]
        public string GlobalId { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; }

        [Required]
        public DateTime? LastUpdateDate { get; set; }
    }
}