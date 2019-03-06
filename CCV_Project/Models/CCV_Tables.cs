using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCV_Project.Models
{

        public class UserAcount
        {
            [Key]
            public int UserId { get; set; }

            [Required(ErrorMessage = "First name required")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name required")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email required")]
            [EmailAddress(ErrorMessage = "please insert valid email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Username required")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Confirm your password")]
            [Compare("Password", ErrorMessage = "passwords are not same")]
            public string ConfirmPassword { get; set; }
        }

        public class StoreHouse
        {
            [Key]
            public int StoreHouseId { get; set; }

            [Required(ErrorMessage = "You have to enter storehouse name!")]
            public string StoreHouseName { get; set; }

            [Required(ErrorMessage = "Select type please.")]
            public StoreHouseType storeHouseType { get; set; }

            public bool StoreHouseActiv { get; set; }

            public ICollection<Rack> Racks { get; set; }
        }

        public enum StoreHouseType
        {
           Dry,
           Frish,
           OG
        }

        public class Rack
        {
            [Key]
            public int RackId { get; set; }

            [Required(ErrorMessage = "please insert EAN")]
            public int EAN { get; set; }

            [Required(ErrorMessage = "please enter X-position")]
            public int Xposition { get; set; }

            [Required(ErrorMessage = "please insert Y-position")]
            public int Yposition { get; set; }

            public bool Activ { get; set; }

            [ForeignKey("StoreHouse")]
            public int StoreHouseRefId { get; set; }
            public  StoreHouse StoreHouse { get; set; }

            public ICollection<Shelf> Shelfs { get; set; }
        }

        public class Shelf
        {
            [Key]
            public int ShelfId { get; set; }

            [Required(ErrorMessage = "please insert EAN")]
            public int EAN { get; set; }

            [Required(ErrorMessage = "please enter X-position")]
            public int Xposition { get; set; }

            [Required(ErrorMessage = "please insert Y-position")]
            public int Yposition { get; set; }

            public bool Activ { get; set; }

            [ForeignKey("Rack")]
            public int RackId { get; set; }
            public Rack Rack { get; set; }
        }
}