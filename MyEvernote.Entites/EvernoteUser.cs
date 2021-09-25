using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("Ad"),
            StringLength(25,ErrorMessage ="{0} en fazla {1} karakter uzunluğuunda olmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Soyad"),
            StringLength(25, ErrorMessage = "{0} en fazla {1} karakter uzunluğuunda olmalıdır.")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage ="{0} doldurulması zorunludur."),
            StringLength(25, ErrorMessage = "{0} en fazla {1} karakter uzunluğuunda olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-Mail"),
            Required(ErrorMessage = "{0} doldurulması zorunludur."),
            StringLength(70, ErrorMessage = "{0} en fazla {1} karakter uzunluğuunda olmalıdır.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} doldurulması zorunludur."),
            StringLength(25, ErrorMessage = "{0} en fazla {1} karakter uzunluğuunda olmalıdır.")]
        public string Password { get; set; }

        [StringLength(30)] 
        public string ProfileImageFilename { get; set; }  //images/user_1.jpg




        public bool IsActive { get; set; }

        
        public bool IsAdmin { get; set; }

        [Required]
        public Guid ActivateGuid { get; set; }


        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}