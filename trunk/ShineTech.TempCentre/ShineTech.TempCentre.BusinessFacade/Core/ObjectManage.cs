using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class ObjectManage
    {
        private static IEntity user;
        public static IEntity GetInstance(int id) 
        {
            switch (id)
            {
                case 1:
                    if (user == null)
                        user = new UserInfo();
                    break;
                case 2:
                    if (user == null)
                        user = new Policy();
                    break;
                case 3:
                    if (user == null)
                        user = new Meanings();
                    break;
            }
            return user;
                        
        }
    }
}
