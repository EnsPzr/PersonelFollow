using System;
using System.Collections.Generic;
using System.Text;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Abstract
{
    public interface IActiviyRepository
    {
        List<Activity> GetActivities(String userId);

        Activity GetActivity(int activityId, String userId);

        List<Activity> GetAllActivities();

        bool Add(Activity entity);

        bool Update(Activity entity);

        void Delete(Activity entity);

        bool HasActivity(int activityId, String id);

        bool HasActivity(String activityName,bool isNumeric, String id);

        bool HasActivity(String activityName, bool isNumeric, String id, int activityId);
    }
}
