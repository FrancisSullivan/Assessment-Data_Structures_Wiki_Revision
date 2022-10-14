using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace Data_Structures_Wiki_Revision_1
{
    public partial class Data_Structures_Wiki_Revision_1 : Form
    {
        public Data_Structures_Wiki_Revision_1()
        {
            InitializeComponent();
            PopulateComboBox();
        }
        #region 6.2
        /*
        [X]..Create a global List<T> of type Information called Wiki.
        */
        List<Information> Wiki = new List<Information>();
        #endregion
        #region 6.3 
        /*
        [X]..Create a button method to ADD a new item to the list. 
        [X]..Use a TextBox for the Name input, 
        [X]..ComboBox for the Category,
        [X]..Radio group for the Structure and Multiline, 
        [X]..TextBox for the Definition.
        [P]..Display on ListBox.
        [ ]..Clear Inputs.
        */
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Information addInformation = new Information();
            addInformation.SetName(textBoxName.Text);
            addInformation.SetName(comboBoxCategory.Text);
            addInformation.SetStructure(GetStructureRadioButton());
            addInformation.SetDefinition(textBoxDefinition.Text);
            Wiki.Add(addInformation);
            DisplayListView();
        }
        private string GetStructureRadioButton()
        {
            string rbValue = "";
            foreach (RadioButton rb in groupBoxStructure.Controls.OfType<RadioButton>())
            {
                if (rb.Checked)
                {
                    rbValue = rb.Text;
                    break;
                }
                else
                {
                    rbValue = "Other";
                }
            }
            return rbValue;
        }
        #endregion
        #region 6.4
        /*
        [X]..Create a custom method to populate the ComboBox 
             when the Form Load method is called. 
        [X]..The six categories must be read from a simple text file.
        */
        private void PopulateComboBox()
        {
            string[] lineOfContents = File.ReadAllLines("simple.txt");
            foreach (var line in lineOfContents)
            {
                comboBoxCategory.Items.Add(line);
            }
        }
        #endregion
        #region 6.9
        /*
        [ ]..Create a single custom method that will sort and then display 
             the Name and Category from the wiki information in the list.
        */
        private void DisplayListView()
        { 
            listView.Items.Clear();
            Wiki.Sort();
            foreach (var item in Wiki)
            {
                ListViewItem lvi = new ListViewItem(item.GetName());
                lvi.SubItems.Add(item.GetCategory());
                listView.Items.Add(lvi);
            }
        }
        #endregion
    }
}
