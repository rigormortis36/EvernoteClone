using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} max {1} karakter olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-Posta"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(70, ErrorMessage = "{0} max {1} karakter olmalıdır."),
            EmailAddress(ErrorMessage = "{0} alanı için geçerli bir E-Posta adresi giriniz.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} max {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrarı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{1} max 25 karakter olmalıdır."),
            Compare("Password", ErrorMessage ="{0} ile {1} uyuşmuyor")]
        public string RePassword { get; set; }

    }
}