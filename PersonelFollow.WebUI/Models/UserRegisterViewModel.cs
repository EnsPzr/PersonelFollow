using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonelFollow.WebUI.Models
{
    public class UserRegisterViewModel
    {
        [Display(Name = "Adınız")]
        [Required(ErrorMessage = "Ad alanı boş geçilemez.")]
        [MinLength(3, ErrorMessage = "Ad alanı en az {1} karakter olabilir."), MaxLength(30, ErrorMessage = "Ad alanı en fazla {1} karakter olabilir.")]
        public String UserName { get; set; }

        [Display(Name = "Soyadınız")]
        [Required(ErrorMessage = "Soyad alanı boş geçilemez.")]
        [MinLength(3, ErrorMessage = "Soyad alanı en az {1} karakter olabilir."), MaxLength(30, ErrorMessage = "Soyad alanı en fazla {1} karakter olabilir.")]
        public String UserSurname { get; set; }

        [Display(Name = "E-Posta Adresiniz")]
        [Required(ErrorMessage = "E-Posta alanı boş geçilemez.")]
        [MinLength(6, ErrorMessage = "Posta alanı en az {1} karakter olabilir."), MaxLength(30, ErrorMessage = "E-Posta alanı en fazla {1} karakter olabilir.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir E-Posta adresi giriniz.")]
        public String EMail { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        [MinLength(8, ErrorMessage = "Şifre alanı en az {1} karakter olabilir."), MaxLength(30, ErrorMessage = "Şifre alanı en fazla {1} karakter olabilir.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [Required(ErrorMessage = "Şifre Tekrar alanı boş geçilemez.")]
        [MinLength(8, ErrorMessage = "Şifre Tekrar alanı en az {1} karakter olabilir."), MaxLength(30, ErrorMessage = "Şifre Tekrar alanı en fazla {1} karakter olabilir.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public String PasswordConfirmed { get; set; }


    }
}
