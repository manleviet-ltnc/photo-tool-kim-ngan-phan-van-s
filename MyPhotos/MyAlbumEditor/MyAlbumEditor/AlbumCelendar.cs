using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manning.MyPhotoAlbum;
using Manning.MyPhotoControls;

namespace MyAlbumEditor
{
    public partial class AlbumCelendar : UserControl
    {
        private AlbumManager _manager = null;
        public AlbumManager Manager
        {
            get { return _manager; }
            set
            {
                _manager = value;
                UpdateCelendar();
            }
        }

        public AlbumCelendar()
        {
            InitializeComponent();
        }

        private void UpdateCelendar()
        {
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            calDates.RemoveAllBoldedDates();


            if (Manager == null)
            {
                minDate = DateTime.Now;
                maxDate = DateTime.Now.AddMonths(2);
            }
            else
            {
                foreach (Photograph p in Manager.Album)
                {
                    DateTime date = p.DateTaken;
                    calDates.AddBoldedDate(date);

                    if (date < minDate) minDate = date;
                    if (date > maxDate) maxDate = date;
                }
            }

            calDates.MinDate = minDate;
            calDates.MaxDate = maxDate;
            calDates.SelectionEnd = minDate;
            calDates.UpdateBoldedDates();
        }

        private void calDates_MouseDown(object sender, MouseEventArgs e)
        {
            // Igrore click ì no album selected
            if (Manager == null) return;

            MonthCalendar.HitTestInfo info = calDates.HitTest(e.X, e.Y);

            if (info.HitArea == MonthCalendar.HitArea.Date)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                cms.ShowImageMargin = false;

                // See if any photo at date
                for (int i = 0; i < Manager.Album.Count; i++)
                {
                    Photograph p = Manager.Album[i];
                    if (p.DateTaken.Date == info.Time.Date)
                    {
                        ToolStripItem mnuItem = cms.Items.Add(p.FileName);
                        mnuItem.Tag = i;
                        mnuItem.Click += cmsItem_Click;
                    }
                }

                if (cms.Items.Count > 0)
                    cms.Show(calDates, e.Location);
            }
        }

        private void cmsItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;

            if (item != null && item.Tag is int)
            {
                Manager.Index = (int)item.Tag;
                using (PhotoEditDialog dlg = new PhotoEditDialog(Manager))
                {
                    DateTime oldDate = Manager.Current.DateTaken;
                    if (dlg.ShowDialog() == DialogResult.OK
                        && oldDate != Manager.Current.DateTaken)
                    {
                        // DateTanken was modified
                        UpdateCelendar();
                    }
                }
            }
        }
    }
}
