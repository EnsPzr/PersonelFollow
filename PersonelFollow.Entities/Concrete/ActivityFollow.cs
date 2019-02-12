using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PersonelFollow.Entities.Concrete
{
    public class ActivityFollow
    {
        [Key]
        public int ActivityFollowId { get; set; }

        public int ActivityId { get; set; }

        public Activity Activity { get; set; }
        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActivityDate { get; set; }

        public int NumberOfActivities { get; set; }
    }
}
