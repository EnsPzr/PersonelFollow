using System;
using System.Collections.Generic;
using System.Text;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Abstract
{
    public interface IMyActivityFollowRepository
    {
        List<ActivityFollow> MyActivity(DateTime date, String userId);

        void Save(List<ActivityFollow> entity);

        bool HasActivity(int activityFollowId, String userId);
    }
}
