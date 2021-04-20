using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VFL_Party_Player
{
    public partial class ConfigWindow : Form
    {
        public MainWindow parentwnd;
        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedDances = listBox1.SelectedItems;
            foreach (var dance in selectedDances)
                for(int i=0; i<parentwnd.ListOfDances.Count(); i++)
                {
                    if (parentwnd.ListOfDances[i].name + "   (" + parentwnd.ListOfDances[i].count + ")" == dance.ToString())
                    {
                        if (parentwnd.ListOfDances[i].frequency > 1)
                            if (parentwnd.ListOfDances[i].frequency < 100)
                                parentwnd.ListOfDances[i].frequency--;
                            else
                                parentwnd.ListOfDances[i].frequency = 5;
                    }
                }

            FillList();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            FillList();
        }

        private void FillList()
        {
            while (listBox1.Items.Count > 0)
            {
                listBox1.Items.RemoveAt(0);
                listBox2.Items.RemoveAt(0);
            }
            for (int i = 0; i < parentwnd.ListOfDances.Count(); i++)
            {
                for (int j = 0; j < parentwnd.ListOfDances.Count(); j++)
                {
                    if (parentwnd.ListOfDances[j].order == i)
                    {
                        listBox1.Items.Add(parentwnd.ListOfDances[j].name + "   (" + parentwnd.ListOfDances[j].count+")");
                        listBox2.Items.Add(parentwnd.ListOfDances[j].frequency);
                    }
                }
            }
        }
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.listBox1.SelectedItem == null) return;
            this.listBox1.DoDragDrop(this.listBox1.SelectedItem, DragDropEffects.Move);
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0) index = this.listBox1.Items.Count - 1;
            object data = listBox1.SelectedItem;
            this.listBox1.Items.Remove(data);
            this.listBox1.Items.Insert(index, data);

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                for (int j = 0; j < listBox1.Items.Count; j++)
                {
                    if (listBox1.Items[i].ToString() == (parentwnd.ListOfDances[j].name + "   (" + parentwnd.ListOfDances[j].count + ")"))
                    {
                        parentwnd.ListOfDances[j].order = i;
                    }
                }
            }
            FillList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                parentwnd.fadeLength = System.Convert.ToInt32(textBox1.Text) * 1000;
                if (parentwnd.fadeLength < 5000)
                {
                    parentwnd.fadeLength = 5000;
                }
            }

            this.Visible = false;
            parentwnd.Visible = true;

            MessageBoxManager.Yes = "Ja";
            MessageBoxManager.No = "Nein";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> standardOrder = new List<string>();
            List<int> standardFrequency = new List<int>();
            standardOrder.Add("Langsamer Walzer"); standardFrequency.Add(1);
            standardOrder.Add("ChaChaCha"); standardFrequency.Add(1);
            standardOrder.Add("Tango"); standardFrequency.Add(1);
            standardOrder.Add("Paso Doble"); standardFrequency.Add(4);
            standardOrder.Add("Wiener Walzer"); standardFrequency.Add(3);
            standardOrder.Add("Rumba"); standardFrequency.Add(1);
            standardOrder.Add("Slowfox"); standardFrequency.Add(2);
            standardOrder.Add("Samba"); standardFrequency.Add(2);
            standardOrder.Add("Quickstep"); standardFrequency.Add(1);
            standardOrder.Add("Jive"); standardFrequency.Add(1);
            standardOrder.Add("Discofox"); standardFrequency.Add(1);
            standardOrder.Add("Salsa"); standardFrequency.Add(5);
            standardOrder.Add("Bachata"); standardFrequency.Add(99);

            int Ordering = 0;
            for (int j = 0; j < standardOrder.Count(); j++)
            {
                for (int i = 0; i < parentwnd.ListOfDances.Count(); i++)
                {
                    if (standardOrder[j] == parentwnd.ListOfDances[i].name)
                    {
                        parentwnd.ListOfDances[i].frequency = standardFrequency[j];
                        parentwnd.ListOfDances[i].order = Ordering;
                        Ordering++;
                    }
                }
            }

            for (int i = 0; i < parentwnd.ListOfDances.Count(); i++)
            {
                bool found = false;
                for (int j = 0; j < standardOrder.Count(); j++)
                    if (standardOrder[j] == parentwnd.ListOfDances[i].name)
                    {
                        found = true;
                    }
                if (!found)
                {
                    parentwnd.ListOfDances[i].order = Ordering;
                    Ordering++;
                }
            }
        FillList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var selectedDances = listBox1.SelectedItems;
            foreach (var dance in selectedDances)
                for (int i = 0; i < parentwnd.ListOfDances.Count(); i++)
                {
                    if (parentwnd.ListOfDances[i].name + "   (" + parentwnd.ListOfDances[i].count + ")" == dance.ToString())
                    {
                        if (parentwnd.ListOfDances[i].frequency < 5)
                            parentwnd.ListOfDances[i].frequency++;
                        else
                            parentwnd.ListOfDances[i].frequency = 100;
                    }
                }
            FillList();
        }

        public void setTextboxText(int newtext) {
            textBox1.Text = System.Convert.ToString(newtext);
            textBox1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> standardOrder = new List<string>();
            List<int> standardFrequency = new List<int>();
            standardOrder.Add("Langsamer Walzer"); standardFrequency.Add(1);
            standardOrder.Add("Tango"); standardFrequency.Add(1);
            standardOrder.Add("Wiener Walzer"); standardFrequency.Add(3);
            standardOrder.Add("Slowfox"); standardFrequency.Add(2);
            standardOrder.Add("Quickstep"); standardFrequency.Add(1);
            standardOrder.Add("Discofox"); standardFrequency.Add(1);
            standardOrder.Add("Samba"); standardFrequency.Add(2);
            standardOrder.Add("ChaChaCha"); standardFrequency.Add(1);
            standardOrder.Add("Rumba"); standardFrequency.Add(1);
            standardOrder.Add("Paso Doble"); standardFrequency.Add(4);
            standardOrder.Add("Jive"); standardFrequency.Add(1);
            standardOrder.Add("Salsa"); standardFrequency.Add(5);
            standardOrder.Add("Bachata"); standardFrequency.Add(99);

            int Ordering = 0;
            for (int j = 0; j < standardOrder.Count(); j++)
            {
                for (int i = 0; i < parentwnd.ListOfDances.Count(); i++)
                {
                    if (standardOrder[j] == parentwnd.ListOfDances[i].name)
                    {
                        parentwnd.ListOfDances[i].frequency = standardFrequency[j];
                        parentwnd.ListOfDances[i].order = Ordering;
                        Ordering++;
                    }
                }
            }

            for (int i = 0; i < parentwnd.ListOfDances.Count(); i++)
            {
                bool found = false;
                for (int j = 0; j < standardOrder.Count(); j++)
                    if (standardOrder[j] == parentwnd.ListOfDances[i].name)
                    {
                        found = true;
                    }
                if (!found)
                {
                    parentwnd.ListOfDances[i].order = Ordering;
                    Ordering++;
                }
            }
            FillList();
        }
    }
}
