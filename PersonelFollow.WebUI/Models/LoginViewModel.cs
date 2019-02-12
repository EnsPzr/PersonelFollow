using System;
using System.ComponentModel.DataAnnotations;

namespace PersonelFollow.WebUI.Models
{
    public class LoginViewModel
    {
        [Display(Name ="E Posta Adresi")]
        [Required(ErrorMessage = "E Posta boş bırakılamaz.")]
        [MinLength(5,ErrorMessage = "En az {1} karakter girebilirsiniz."),MaxLength(50,ErrorMessage = "En fazla {1} karakter girebilirsiniz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e posta adresi giriniz.")]
        public String EMail { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [MinLength(6, ErrorMessage = "En az {1} karakter girebilirsiniz."), MaxLength(50, ErrorMessage = "En fazla {1} karakter girebilirsiniz.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}
