namespace SampleDb.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public class Organization
    {
        public int Id { get; set; }

        [ForeignKey("Director")]
        public int? DirectorId { get; set; }
        public Employee Director { get; set; }


        [InverseProperty("Organization")]
        public virtual ICollection<Employee> Employees { get; set; }


        [Required]
        [MaxLength(2048)]
        public string Title { get; set; }

        [ForeignKey("Country")]
        public short? CountryId { get; set; }
        public Country Country { get; set; }


    }

}