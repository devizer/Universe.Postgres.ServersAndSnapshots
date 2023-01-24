namespace SampleDb.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Organization")]
        [Required]
        public int OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Surname { get; set; }


        [ForeignKey("Country")]
        public short? CountryId { get; set; }
        public Country Country { get; set; }


    }
}