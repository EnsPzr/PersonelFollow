using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PersonelFollow.Core.Abstract;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Concrete.EntityFramework
{
    public class EfMyActivityRepository : IMyActivityFollowRepository
    {
        private readonly DataContext _context;

        public EfMyActivityRepository(DataContext context)
        {
            _context = context;
        }

        public List<ActivityFollow> MyActivity(DateTime date, String userId)
        {
            //return new List<ActivityFollow>();
            var userActivities = _context.Activities.Where(p => p.isActive == true && p.UserId.ToString() == userId
                                                                && p.ActivityRegisterDate <= date).ToList();
            foreach (var t in userActivities)
            {
                int activityId = t.ActivityId;
                DateTime activityRegisterDate = t.ActivityRegisterDate;
                var hasActivity = _context.ActivityFollows.Include(p => p.Activity).FirstOrDefault(p => p.ActivityId == activityId
                                                                                 && p.ActivityDate == date &&
                                                                                 p.Activity.ActivityRegisterDate >= activityRegisterDate);
                if (hasActivity == null)
                {
                    var newActivity = new ActivityFollow()
                    {
                        ActivityDate = date,
                        ActivityId = activityId,
                        NumberOfActivities = 0
                    };
                    _context.ActivityFollows.Add(newActivity);
                }
            }

            _context.SaveChanges();
            return _context.ActivityFollows.Include(p => p.Activity).Where(p => p.ActivityDate == date
                                                                                && p.Activity.UserId.ToString() == userId).ToList();
        }
        
        public void Save(List<ActivityFollow> entity)
        {
            foreach (var t in entity)
            {
                int activityFollowId = t.ActivityFollowId;
                var activityFollow =
                    _context.ActivityFollows.FirstOrDefault(p => p.ActivityFollowId == activityFollowId);
                if (activityFollow != null)
                {
                    activityFollow.NumberOfActivities = t.NumberOfActivities;
                }
            }

            _context.SaveChanges();
        }

        public bool HasActivity(int activityFollowId, string userId)
        {
            var activity = _context.ActivityFollows.Include(p => p.Activity).FirstOrDefault(p =>
                p.ActivityFollowId == activityFollowId
                && p.Activity.UserId.ToString() == userId);
            if (activity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
