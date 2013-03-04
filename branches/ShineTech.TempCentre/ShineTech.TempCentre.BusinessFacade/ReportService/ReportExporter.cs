using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ShineTech.TempCentre.Platform;
using System.Drawing;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Versions;

namespace ShineTech.TempCentre.BusinessFacade
{
    public abstract class ReportExporter : IReportExportService
    {
        protected SuperDevice device;
        protected ReportDataGenerator reportdataGenerator;
        protected string fileNameWithFullPath;
        protected string unit;
        protected bool IsTempFile;
        protected IList<DigitalSignature> signatureList;

        protected bool isHeaderShown;
        protected bool isAlarmShown;
        protected bool isCommentShown;
        protected bool isDescriptionShown = true;
        protected bool isSignatureShown;
        protected ReportTitleIconStatus reportTitleIconStatus = ReportTitleIconStatus.None;
        protected DeviceDataFrom deviceDataFrom = DeviceDataFrom.ViewManager;


        public ReportExporter(DeviceDataFrom deviceDataFrom, SuperDevice device, IList<DigitalSignature> signatureList) : this(deviceDataFrom, device, signatureList, "memory") { }

        public ReportExporter(DeviceDataFrom deviceDataFrom, SuperDevice device, IList<DigitalSignature> signatureList, string fileNameWithFullPath)
            : this(deviceDataFrom, device, signatureList, fileNameWithFullPath, false)
        {
        }

        public ReportExporter(DeviceDataFrom deviceDataFrom, SuperDevice device, IList<DigitalSignature> signatureList, string fileNameWithFullPath, bool isTempfile)
        {
            this.reportdataGenerator = new ReportDataGenerator();
            this.device = device;
            this.deviceDataFrom = deviceDataFrom;
            this.fileNameWithFullPath = fileNameWithFullPath;
            this.IsTempFile = isTempfile;
            this.signatureList = signatureList;
            unit = "°C";
            if ("F".Equals(this.device.TempUnit, StringComparison.Ordinal))
            {
                unit = "°F";
            }
            this.checkWhetherSectionShouldBeShown();
        }

        protected void checkWhetherSectionShouldBeShown()
        {
            if (this.device != null)
            {
                if (SoftwareVersions.Pro == Common.Versions && Common.GlobalProfile != null && Common.GlobalProfile.IsShowHeader)
                {
                    byte[] logoByte = Common.GlobalProfile.Logo;
                    string contactInfo = Common.GlobalProfile.ContactInfo;
                    if (logoByte != null || !string.IsNullOrWhiteSpace(contactInfo))
                    {
                        this.isHeaderShown = true;
                    }
                }
                if (this.device.AlarmMode != 0)
                {
                    this.isAlarmShown = true;
                }
                if (Common.IsAuthorized(RightsText.CommentRecords))
                {
                    if (!string.IsNullOrWhiteSpace(this.CurrentComment) && this.CurrentComment != ReportConstString.CommentDefaultString)
                    {
                        this.isCommentShown = true;
                    }
                }
                if (this.device.DeviceID < 200)
                {
                    this.isDescriptionShown = false;
                }
                if (this.device.AlarmMode > 0 && this.device.tempList.Count > 0)
                {
                    if (IsDeviceAlarming(this.device))
                    {
                        this.reportTitleIconStatus = ReportTitleIconStatus.Alarm;
                    }
                    else
                    {
                        this.reportTitleIconStatus = ReportTitleIconStatus.OK;
                    }
                }
                if (this.signatureList != null && this.signatureList.Count > 0)
                {
                    this.isSignatureShown = true;
                }
            }
        }

        private bool IsDeviceAlarming(SuperDevice arg)
        {
            bool result = false;
            if (arg != null)
            {
                string[] alarmStrings = new string[]
                {
                    arg.AlarmHighStatus,
                    arg.AlarmLowStatus,
                    arg.AlarmA1Status,
                    arg.AlarmA2Status,
                    arg.AlarmA3Status,
                    arg.AlarmA4Status,
                    arg.AlarmA5Status,
                };
                foreach (var item in alarmStrings)
                {
                    if (ConvertAlarmStringToBool(item))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private bool ConvertAlarmStringToBool(string arg)
        {
            bool result = false;
            string alarmString = "Alarm";
            if (alarmString.Equals(arg, StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
            }
            return result;
        }

        protected virtual void calculateSectionMargin()
        {
        }

        protected abstract void GenerateReportHeader();
        protected abstract void GenerateReportTitle();
        protected abstract void GenerateDeviceConfigurationAndTripInfomation();
        protected abstract void GenerateLoggingSummary();
        protected abstract void GenerateAlarms();
        protected abstract void GenerateComments();
        protected abstract void GenerateDataGraph();
        protected abstract void GenerateSignatures();
        protected abstract void GenerateDataList();
        protected abstract void GenerateReportFooter();

        public virtual bool GenerateReport()
        {
            return true;
        }

        public string CurrentComment
        {
            get;
            set;
        }


        public IList<DAL.DigitalSignature> SignatureList
        {
            get;
            set;
        }


        public string Title
        {
            get;
            set;
        }

        protected Image reportCrossSmall = global::ShineTech.TempCentre.BusinessFacade.Properties.Resources.Report_cross;
        protected Image reportOkSmall = global::ShineTech.TempCentre.BusinessFacade.Properties.Resources.Report_ok;
    }

    public enum ReportTitleIconStatus
    {
        OK, Alarm, None
    }

    public enum DeviceDataFrom
    {
        ViewManager,
        DataManager
    }
}
