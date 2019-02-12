using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonelFollow.WebUI.Models
{
    public class NewAndEditActivityViewModel
    {
        public int ActivityId { get; set; }

        [Display(Name = "Aktivite Adı")]
        [MinLength(3,ErrorMessage = "Aktivite adı en az {1} karakter olabilir."),MaxLength(50,ErrorMessage = "Aktivite adı en fazla {1} karakter olabilir.")]
        [Required(ErrorMessage = "Aktivite adı girilmelidir.")]
        public String ActivityName { get; set; }

        [Display(Name = "Sayısal Değer Mi Yoksa Yapıldı Yapılmadı Mı?")]
        [Required(ErrorMessage = "Aktivite türü seçilmelidir.")]
        public bool IsNumeric { get; set; }

        [Display(Name = "Aktif Durumu")]
        [Required(ErrorMessage = "Aktiflik durumu seçilmelidir.")]
        public bool IsActive { get; set; }

        public String UserId { get; set; }
    }
}
