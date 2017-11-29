using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Data
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }
        [MaxLength(100)]
        public string email { get; set; }
        [MaxLength(100)]
        public string password { get; set; }
        [MaxLength(50)]
        public string firstName { get; set; }
        [MaxLength(50)]
        public string lastName { get; set; }
        [MaxLength(20)]
        public string role { get; set; }
        [DefaultValue(0)]
        public int countyId { get; set; }
        [DefaultValue("getutcdate()")]
        public DateTime dateAdded { get; set; }
        [DefaultValue("getutcdate()")]
        public DateTime dateUpdated { get; set; }
    }

    public class County
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int countyId { get; set; }
        [MaxLength(50)]
        public string name { get; set; }
    }
}
