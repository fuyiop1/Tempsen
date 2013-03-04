using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.Versions;

namespace ShineTech.TempCentre.Platform
{


    public static class Messages
    {
        // popped up box title
        public static string Caption
        {
            get
            {
                string result = string.Empty;
                if (SoftwareVersion.Version == Versions.SoftwareVersions.S)
                {
                    result = "TempCentre Lite";
                }
                else
                {
                    result = "TempCentre";
                }
                return result;
            }

        }

        // icon type
        public const string TitleWarning = "Warning";
        public const string TitleNotification = "Notification";
        public const string TitleError = "Error";

        // log off
        public const string LogOut = "Are you sure you want to logout?";

        // log in
        public const string WrongUserNameOrPassword = "Invalid user name or password.";
        public const string WrongPasswordExcceedCertainTimes = "Sorry, you have attempted to login system with incorrect password for too many times, this account has already been locked, please contact administrator to unlock it.";
        public const string UserLocked = "This user has been locked, please contact administrator to unlock.";
        public const string PasswordExpired = "Password expired, please reset a new password.";
        public const string WrongOldPasswordWhenResetPassword = "Incorrect password, please try again.";
        public const string NewSameOfOldWhenResetPassword = "New password must not be equal to the old password.";
        public const string MismatchPassword = "Passwords do not match!";
        public const string ResetPasswordSuccessfully = "Password changed successfully.";
        public const string ResetPasswordFailed = "Failed to change password.";

        // Device Manager
        public const string ConnectWithNoDeviceSelected = "Please select an appropriate data logger to connect.";
        public const string ConnectDeviceFailed = "Failed to connect, please make sure data logger has been properly connected to computer, and verify you have selected the appropriate data logger in device list to connect.";
        public const string AutoConnectDeviceFailed = "Failed to connect, please make sure data logger has been properly connected to computer.";
        public const string SaveDataSuccessfully = "Data record {0}_{1} has been saved to data base successfully!";
        public const string SaveDataFailed = "Failed to save data record to data base.";
        public const string SaveDataToDbFirst = "Please save data record to data base first, save it now?";
        public const string SignSuccessfully = "Data record signed successfully!";
        public const string UnauthorizedTps = "Data has been damaged.";
        public const string B19 = "Please save data record to data base first, save it now?";
        public const string B20 = "Changes to data record has not been saved, save it now?";
        public const string StopSuccessfully = "Stop the device successfully.";
        public const string StopFailed = "Stop the device failed.";
        public const string SaveCfgTimeLess = "Time for Auto Start must not be earlier than current time, considering that new configurtion need some time to take effect, so please make sure time for Auto Start is at least 5 minutes later than current time.";
        public static string B93 = "Data record of same name {0} has been found in data base, and data points in current data record is more than the previously stored data record, do you still want to save?";
        public static string B94 = "Data record of same name {0} has been found in data base, saving current data record will cover previously stored data record, do you still want to save?";
        
        //TODO
        //public const string ExitWithoutSavingDataRecord = "Data record has not been saved, are you sure you want to leave?";
        //public const string ExitWithoutSavingCommentOrSignature = "Changes to data record has not been saved, ara you sure you want to leave?";

        // Export 
        public const string SameNameFileOpened = "Error: File of same name has been opened up,save failure.";
        public const string IncorrectPrintSetting = "Error: Incorrect print settings.";
        public const string OutlookError = "Error: Could not start outlook.";

        // Data Manager
        public const string DeleteSelectedRecord = "Caution: Deleted data cannot be recovered, are you sure you still want to delete the selected data record?";
        public const string DeleteAllRecord = "Caution: Deleted data cannot be recovered, are you sure you still want to delete all data records?";
        public const string OnlyCanDeleteUnsignedRecords = "You do not have permission ro deleted electronically signed record.";
        public const string CheckedRecordsOverLimit = "Only a maximum of 10 data records are allowed for multi-record view,to add a new data record, please at first uncheck any one in 10 data records already checked, and then check the new data record.";
        public static string SameRecordFoundInDatabase = "Data record of same name [{0}] has been found in data base, select \"Yes\" to replace the existing data record with current data record, select \"No\" to save current data record as a new record [{1}].";

        // Administrator 
        public const string DisableYourself = "You could not disable yourself.";
        public const string DisableUser = "Caution: User disabled cannot be recovered,do you still want to disable user: {0}?";
        public const string NoRightItemSelected = "Please select at least one item.";
        public const string DeleteMeaning = "Deleting meaning may affect other users related with this meaning, are you sure you want to delete this meaning?";
        public static string FirstCreate = "Administrator has been successfully added, please remember your user name and password."+Environment.NewLine+"Do you want to launch the software right now?";
        public const string TooLargeFile = "The image could not be larger than 2MB.";
        public static string B32 = "Note: User account locked will not be able to access to this software, are you sure you want to lock user: {0}?";

        // Update
        public const string MissingUpdateCompenents = "Missing update components.";
        public const string CloseSystemWhenUpdate = "The main program is running, please save the data record first and then close the window.";
        public const string NoAvailableUpdate = "No available update.";
        public const string ErrorUpdating = "Error updating!";
        public const string ConnectServerFailed = "Connecting to server failure.";
        public const string DownLoadFailed = "Failed to download upadtes.";

        // No permission
        public const string NoPermission = "You do not have sufficient permission to handle this operation, please contact administrator.";

        // Apply notification
        public const string ApplySuccessfully = "Your configuration changes have been applied and saved successfully.";
        public const string ApplyFailed = "Failed to apply your configuration changes.";

        // Add user/Edit User
        public const string UserNameOccupied = "User name has already been used.";
        public const string PasswordWithIllegalCharacters = "Password entered contains illegal characters.";
        public const string PasswordShortThanDefined = "Password must have at least {0} characters.";

        // Configurate Device
        public const string TripNumberInvalid = "Trip number must be alphanumeric characters.";
        public const string TemperatureValueInvalid = "Please enter numeric characters.";
        public const string AlarmDelayInvalid = "Alarm delay must be longer than log interval.";
        public const string AlarmHighLimitInvalid = "Alarm limit over maximum extreme of device operating range.";
        public const string AlarmLowLimitInvalid = "Alarm limit under minimum extreme of device operating range.";
        public const string InvalidCharacter = "Invalid Character.";
        //TODO
        public const string HighLimitMoreThanLowLimit = "High limit must be more than low limit.";

        //sign
        public const string NoSignRight = "You have no right to sign records.";
        public const string FileDamage = "The tps file was breakdown.";

        //new added ones
        public const string NoSignMeanSelected = "Please select a meaing for signature.";
        public const string NoSignMeanings = "Please assign a meaning of signature for current user.";
        public const string EmptyContentError = "Required field.";
        public const string RgbInvalid = "The rgb expression is invalid.";
        public const string SessionExpire = "log out";
        public const string ConfirmContinueToConfigureDevice = "Data logger connected has not been configured, do you want configure it right now?";
        public const string NoTempPoint = "No valid data!";
        public static string StopTheLogger = string.Format("Data logger connected is recording data, are you sure you want to stop it and re-configure it?{0}Note: New configuration will delete previous recorded data in data logger.", Environment.NewLine);
        public const string AtLeastChooseOneLimit = "Please at least set one alarm limit.";
        public const string HasIllegalConfiguration = "Some configuration illegal";
        public const string WriteConfigOk = "Data logger was successfully configured.";
        public const string WriteConfigFailed = "Failed to configure data logger.";
        public const string ChangeNewPasswordOk = "Password changed successfully.";
        public const string ChangeNewPasswordFailed = "Failed to change password.";

        //TODO
        public const string AssignMeaningFirst = "Please assign a meaning of signature for current user.";
        //public const string NotificationAfterFirstAdminCreated = string.Format("Administrator has been successfully added, please remember your user name and password.{0}Do you want to launch the software right now?", Environment.NewLine);
        //public const string RemoveSoftware = "Uninstallation of TempCentre software will not remove database covering data records and user information.";
        //public const string DBAreadyExist = "There already exists a data base in current directory, would you like to remove it and install a new data base?";
        //public const string ConfigStoppedDevice = "Note: New configuration will delete previous recorded data in data logger, please make sure you have already saved data into data base.";

        // save
        public const string SavedSuccessfully = "Saved Successfully.";
        public const string SavedFailed = "Saved Failed.";

        public const string NoPrinterInstalled = "No printer installed.";
        //policy
        public const string PasswordLength = "Minimal size of a password must be between 3 to 12 characters.";
        public const string Characters = "Please enter a numeric.";
        public const string ConfigTips = "Note: New configuration will delete previous recorded data in data logger, please make sure you have already saved data into data base.";

        //option
        public const string B47 = "Your configuration changes have been applied and saved successfully.";
        public const string B48 = "Your configuration changes have been saved successfully and will take effect next time when launching the software.";
        public const string B49 = "Failed to apply your configuration changes.";
    }
}
