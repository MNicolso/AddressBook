using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Display(Name = "Contact")]
        [StringLength(50)]
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        [StringLength(50)]
        public string Email { get; set; }
        [Display(Name = "Avatar")]
        public string ImagePath { get; set; }
        public byte[] ImageData { get; set; }
        [Display(Name="Home Address")]
        [StringLength(50)]
        public string Address1 { get; set; }
        [Display(Name = "Work Address")]
        [StringLength(50)]
        public string Address2 { get; set; }
        [StringLength(25)]
        public string City { get; set; }
        [StringLength(20)]
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        [StringLength(20)]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone Number")]
        [DisplayFormat(DataFormatString = "{0:###-###-####}")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Contact Updated")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset? ContactUpdated { get; set; }


        public virtual ABUser ABUser { get; set; }
    }
}
