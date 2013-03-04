using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Drawing;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade.ViewModel
{
    public class MeaningViewModel
    {
        private Meanings _mean;
        private Font _font;
        private int _displayWidth;

        public MeaningViewModel(Meanings mean, Font font, int displayWidth)
        {
            _mean = mean;
            _font = font;
            _displayWidth = displayWidth;
        }

        public int Id
        {
            get { return _mean == null ? 0 : _mean.Id; }
        }

        public string Desc
        {
            get { return _mean == null ? string.Empty : _mean.Desc; }
        }

        public string DisplayDesc
        {
            get { return _mean == null ? string.Empty : Utils.GetDisplayString(_mean.Desc, _font, _displayWidth); }
        }
    }
}
