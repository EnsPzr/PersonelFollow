using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonelFollow.WebUI.Models
{
    public class ActivitiesViewModel
    {
        public int DailyActivityId { get; set; }

        public int ActivityId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActivityDate { get; set; }

        [Display(Name = "Aktivite Adı")]
        public String ActivityName { get; set; }
        [Display(Name = "Yapıldı, Yapılmadı veya Kaç kere Yapıldığı")]
        public int NumberOfActivity { get; set; }

        public bool isNumeric { get; set; }
    }
}
