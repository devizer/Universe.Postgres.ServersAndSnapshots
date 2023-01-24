using System.ComponentModel.DataAnnotations;

namespace SampleDb.Entities
{

    public class Country
    {
        
        public short Id { get; set; }

        [MaxLength(2048)]
        public string EnglishName { get; set; }

        public byte[] Flag { get; set; }

    }

}