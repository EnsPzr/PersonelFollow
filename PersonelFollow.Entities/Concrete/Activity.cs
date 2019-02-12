using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonelFollow.Entities.Concrete
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        public String ActivityName { get; set; }

        public bool isNumeric { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActivityRegisterDate { get; set; }

        public Guid UserId { get; set; }

        public UserInformation UserInformation { get; set; }

        public List<ActivityFollow> ActivityFollows { get; set; }
        public bool isActive { get; set; }
    }
}
