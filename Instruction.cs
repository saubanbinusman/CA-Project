using System;
using System.Windows.Forms;

namespace CA_Project
{
    class Instruction
    {
        public InstructionTypes instructionType { get; set; }
        public string op1 { get; set; }
        public string op2 { get; set; }
        public string op3 { get; set; }

        public string[] toListViewStringArray(string serial)
        {
            return new string[] { serial, instructionType.ToString(), op1, op2, op3, "" };
        }

        public static Instruction fromListViewItem(ListViewItem listViewItem)
        {
            return new Instruction
            {
                instructionType = (InstructionTypes)Enum.Parse(typeof(InstructionTypes), listViewItem.SubItems[1].Text),
                op1 = listViewItem.SubItems[2].Text,
                op2 = listViewItem.SubItems[3].Text,
                op3 = listViewItem.SubItems[4].Text
            };
        }
    }
}
