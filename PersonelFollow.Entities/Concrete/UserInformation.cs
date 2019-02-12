using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonelFollow.Entities.Concrete
{
    public class UserInformation
    {
        [Key]
        public Guid UserId { get; set; }

        public String UserName { get; set; }

        public String UserSurname { get; set; }

        public String EMail { get; set; }

        public String Password { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UserRegisterDate { get; set; }

        public bool isAdministrator { get; set; }
        
        public List<Activity> Activities { get; set; }

        //public List<ActivityFollow> ActivityFollows { get; set; }

    }
}
