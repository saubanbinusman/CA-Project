using System;
using System.Drawing;
using System.Windows.Forms;

namespace CA_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void populateComboBoxesWithRegisters()
        {
            cbOp1.Items.Add("-");

            for (int i = 0; i < 32; ++i)
            {
                cbOp1.Items.Add("R" + i.ToString());
                cbOp2.Items.Add("R" + i.ToString());
                cbOp3.Items.Add("R" + i.ToString());
            }
        }

        private void populateInstructionTypesComboBox()
        {
            cbInst.Items.AddRange(Enum.GetNames(typeof(InstructionTypes)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            populateInstructionTypesComboBox();
            populateComboBoxesWithRegisters();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string serial;
            Instruction instruction = new Instruction();

            try
            {
                serial = (listView1.Items.Count + 1).ToString();
                instruction.instructionType = (InstructionTypes)Enum.Parse(typeof(InstructionTypes), cbInst.SelectedItem.ToString());
                instruction.op1 = cbOp1.SelectedItem.ToString();
                instruction.op2 = cbOp2.SelectedItem.ToString();
                instruction.op3 = cbOp3.SelectedItem.ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            listView1.Items.Add(new ListViewItem(instruction.toListViewStringArray(serial)));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private ListViewItem.ListViewSubItem getHazardSubItem(string hazardDetails, bool hazard)
        {
            ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem();

            listViewSubItem.BackColor = hazard ? Color.PaleVioletRed : Color.LightGreen;
            listViewSubItem.Text = hazardDetails;

            return listViewSubItem;
        }

        private bool processRAWHazard(Instruction current, Instruction previous)
        {
            return (previous != null) && (current.op2 == previous.op1 || current.op3 == previous.op1);
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0) return;

            listView1.Items[0].UseItemStyleForSubItems = false;
            listView1.Items[0].SubItems[5] = getHazardSubItem("No Hazard", false);

            for (int i = 1; i < listView1.Items.Count; ++i)
            {
                Instruction current   = Instruction.fromListViewItem(listView1.Items[i]);
                Instruction previous1 = Instruction.fromListViewItem(listView1.Items[i - 1]);
                Instruction previous2 = (i == 1) ? null : Instruction.fromListViewItem(listView1.Items[i - 2]);

                bool hazardRAW = processRAWHazard(current, previous1) || processRAWHazard(current, previous2);

                string hazardDetails = (hazardRAW ? "RAW" : "No Hazard");

                listView1.Items[i].UseItemStyleForSubItems = false;
                listView1.Items[i].SubItems[5] = getHazardSubItem(hazardDetails, hazardRAW);
            }
        }
    }
}
