using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.Platform;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class TrialValidationUI
    {
        /// <summary>
        /// 验证验证码是否过期
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            if (Common.Versions == Versions.SoftwareVersions.Pro)
            {
                return TrialValidation.IsValidated();
            }
            else
                return true;
        }
        public bool VerifyMode7(string number)
        {
            try
            {
                string key = number;
                int raw = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    raw += Convert.ToInt32(key[i].ToString());
                }
                if (raw % 7 == 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveTrialVersion()
        {
            return TrialValidation.RemoveTrialKey();
        }
    }
}
