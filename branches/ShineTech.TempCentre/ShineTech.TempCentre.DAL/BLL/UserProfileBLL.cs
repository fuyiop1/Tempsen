using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

namespace ShineTech.TempCentre.DAL
{
    public class UserProfileBLL
    {
        private IDataProcessor processor;
        public UserProfileBLL()
        {
            processor = new DeviceProcessor();
        }
        public UserProfile GetProfileByUserName(string username)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("username", username);
            return processor.QueryOne<UserProfile>("select * from userprofile where username=@username COLLATE NOCASE", dic);
        }
        public int GetProfilePKValue()
        {
            object u = processor.QueryScalar("select max(id) from userprofile", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public bool InsertProfile(UserProfile profile,DbTransaction tran)
        {
            return processor.Insert<UserProfile>(profile, tran);
        }
        public bool InsertProfile(UserProfile profile)
        {
            return InsertProfile(profile,null);
        }
        public bool UpdateProfile(UserProfile profile)
        {
            return processor.Update<UserProfile>(profile, null);
        }
        public List<UserProfile> GetGlobalSetting()
        {
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("@IsGlobal", true);
            return processor.Query<UserProfile>("select * from userprofile where IsGlobal=1", null);
        }
        public UserProfile GetProfileByPK(int id)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@id", id);
            return processor.QueryOne<UserProfile>("select * from userprofile where id=@id", dic);
        }

        public IList<UserProfile> GetAllUserProfiles()
        {
            IList<UserProfile> result = new List<UserProfile>();
            result = processor.Query<UserProfile>("SELECT * FROM userprofile", null);
            return result;
        }

    }
}
