using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTicketUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Dictionary<string, AlarmCount> Data = new Dictionary<string, AlarmCount>(); // actual data
            Data.Add("Kasaragod", new AlarmCount { IsAck = true, Priority = "H" });
            Data.Add("Kannur", new AlarmCount { IsAck = false, Priority = "I" });

            var processedData = new List<GridData>();
            var withoutstate = Data.Select(p => p.Value).ToList();
            foreach (var d in withoutstate.GroupBy(p=>p.IsAck)) // massaging data to display in grid
            {
                var toadd = new GridData
                {
                    Alarms = d.Key ? "Ack" : "Non-Ack",
                    A = d.Where(p => p.Priority=="A").Count(),
                    H = d.Where(p => p.Priority == "H").Count(),
                    I= d.Where(p => p.Priority == "I").Count(),
                    L= d.Where(p => p.Priority == "L").Count(),
                    M= d.Where(p => p.Priority == "M").Count()
                };
                processedData.Add(toadd);

            }
            dataGridView1.AutoGenerateColumns = true; 
            dataGridView1.DataSource = processedData;

        }

        public void addImage(string file)
        {
            try
            {
                System.Drawing.Image myImage = Image.FromFile($"{file}.jpg");
                imageList1.Images.Add(file, myImage);
            }
            catch(Exception e) { }
            
        }
        void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >0 && e.RowIndex == -1) // choosing only the header row to assign images
            {
                var keyval = dataGridView1.Columns[e.ColumnIndex].HeaderText;
                addImage(keyval); // adding image tok array
                var idx = this.imageList1.Images.IndexOfKey(keyval); // finding index in array
                if(idx < 0)
                {
                    e.Handled = true;
                    return;
                }
                e.PaintBackground(e.ClipBounds, false);

                Point pt = e.CellBounds.Location; 

                int offset = (e.CellBounds.Width - this.imageList1.ImageSize.Width) / 2;
                pt.X += offset;
                pt.Y += 1;
                this.imageList1.Draw(e.Graphics, pt, idx); // drawng image
                e.Handled = true;
            }
            
        }
    }
    public class GridData
    {
        public string Alarms { get; set; }
        public int H { get; set; }
        public int M { get; set; }
        public int L { get; set; }
        public int I { get; set; }
        public int A { get; set; }
    }
    public class AlarmCount
    {
        public string Priority { get; set; }
        public bool IsAck { get; set; }
    }
}
