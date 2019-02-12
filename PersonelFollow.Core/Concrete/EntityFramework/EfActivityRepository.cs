using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PersonelFollow.Core.Abstract;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Concrete.EntityFramework
{
    public class EfActivityRepository : IActiviyRepository
    {
        private readonly DataContext _context;

        public EfActivityRepository(DataContext context)
        {
            this._context = context;
        }

        public List<Activity> GetActivities(String userId)
        {
            return _context.Activities.Where(p => p.UserId.ToString() == userId).ToList();
        }

        public Activity GetActivity(int activityId, String userId)
        {
            if (!HasActivity(activityId, userId))
            {
                return null;
            }
            else
            {
                var activity = _context.Activities.FirstOrDefault(p => p.ActivityId == activityId);
                return activity;
            }
        }

        public List<Activity> GetAllActivities()
        {
            //return new List<Activity>();
            return _context.Activities.Include(p => p.UserInformation).ToList();
        }

        public bool Add(Activity entity)
        {
            if (HasActivity(entity.ActivityName, entity.isNumeric, entity.UserId.ToString()))
            {
                entity.ActivityRegisterDate = DateTime.Today;
                entity.isActive = true;
                var addedActivity = _context.Entry(entity);
                addedActivity.State = EntityState.Added;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool Update(Activity entity)
        {
            if (HasActivity(entity.ActivityId, entity.UserId.ToString()))
            {
                if (HasActivity(entity.ActivityName, entity.isNumeric, entity.UserId.ToString(), entity.ActivityId))
                {
                    var updatedEntity = _context.Activities.FirstOrDefault(p => p.ActivityId == entity.ActivityId);
                    if (updatedEntity != null)
                    {
                        updatedEntity.ActivityName = entity.ActivityName;
                        updatedEntity.isActive = entity.isActive;
                        if (updatedEntity.isNumeric != entity.isNumeric)
                        {
                            var dailyActivities =
                                _context.ActivityFollows.Where(p => p.ActivityId == entity.ActivityId).ToList();
                            for (int i = 0; i < dailyActivities.Count(); i++)
                            {
                                dailyActivities[i].NumberOfActivities = 0;
                            }
                            updatedEntity.isNumeric = entity.isNumeric;
                        }
                    }
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Delete(Activity entity)
        {
            var deletedEntity = _context.Activities.FirstOrDefault(p => p.ActivityId == entity.ActivityId);
            if (deletedEntity != null) deletedEntity.isActive = false;
            _context.SaveChanges();
        }

        public bool HasActivity(int activityId, String id)
        {
            return _context.Activities.FirstOrDefault(p => p.ActivityId == activityId && p.UserId.ToString() == id) != null;
        }

        public bool HasActivity(string activityName, bool isNumeric, string id)
        {
            return _context.Activities.FirstOrDefault(p => p.isNumeric == isNumeric
                                                           && p.ActivityName == activityName
                                                           && p.UserId.ToString() == id) == null;
        }

        public bool HasActivity(string activityName, bool isNumeric, string id, int activityId)
        {
            return _context.Activities.FirstOrDefault(p => p.ActivityName == activityName
                                                           && p.isNumeric == isNumeric
                                                           && p.UserId.ToString() == id
                                                           && p.ActivityId != activityId) == null;
        }
    }
}
