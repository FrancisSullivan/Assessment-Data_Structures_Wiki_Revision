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
using System.Reflection;

namespace Data_Structures_Wiki_Revision_1
{
    public partial class Data_Structures_Wiki_Revision_1 : Form
    {
        public Data_Structures_Wiki_Revision_1()
        {
            InitializeComponent();
            PopulateComboBox();
        }
        #region 6.02 Object List
        /*
        [X]..Create a global List<T> of type Information called Wiki.
        */
        List<Information> Wiki = new List<Information>();
        #endregion
        #region 6.03 'Add' Button
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
            AddItem();
            UpdateListView();
        }
        private void AddItem()
        {
            Information addInformation = new Information();
            addInformation.SetName(textBoxName.Text);
            addInformation.SetCategory(comboBoxCategory.Text);
            addInformation.SetStructure(GetStructureRadioButton());
            addInformation.SetDefinition(textBoxDefinition.Text);
            Wiki.Add(addInformation);
        }
        #endregion
        #region 6.04 'Category' ComboBox
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
        #region 6.05 'Name' Duplicate Checker
        /*
        [P]..Create a custom ValidName method which will take a parameter 
             string value from the Textbox Name and returns a Boolean after 
             checking for duplicates. 
        [P]..Use the built in List<T> method “Exists” to answer this 
             requirement.
        */
        private Boolean ValidName(string name)
        {
            return Wiki.Exists(x => x.GetName() == name);
        }
        #endregion
        #region 6.06 'Structure' Radio Buttons
        /*
        [ ]..Create two methods to highlight and return the values from the 
             Radio button GroupBox. 
        [P]..The first method must return a string value from the selected 
             radio button (Linear or Non-Linear). 
        [ ]..The second method must send an integer index which will 
             highlight an appropriate radio button.
        */
        private string GetStructureRadioButton()
        {
            string rbValue = "";
            foreach (RadioButton rb in 
                groupBoxStructure.Controls.OfType<RadioButton>())
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
        private void SetStructureRadioButton(int item)
        {
            foreach (RadioButton rb in 
                groupBoxStructure.Controls.OfType<RadioButton>())
            {
                if (rb.Text == Wiki[item].GetStructure())
                {
                    rb.Checked = true;
                }
                else
                {
                    rb.Checked = false;
                }
            }
        }
        #endregion
        #region 6.07 'Delete' Button
        /*
        [P]..Create a button method that will delete the currently selected 
             record in the ListView. 
        [P]..Ensure the user has the option to backout of this action by 
             using a dialog box. 
        [P]..Display an updated version of the sorted list at the end of 
             this process.
        */
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0)
            {
                toolStripStatusLabel.Text = "No record has been selected. " +
                    "To 'delete' first select a record from the 'Name' " +
                    "column.";
                return;
            }
            var result = MessageBox.Show("Are you sure you want to delete " +
                "this record?", "Delete Record",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int currentItem = listView.SelectedIndices[0];
                string name = Wiki[currentItem].GetName();
                listView.Items.RemoveAt(currentItem);
                Wiki.RemoveAt(currentItem);
                UpdateListView();
                toolStripStatusLabel.Text = "Record deleted.";
            }
            else
            {
                toolStripStatusLabel.Text = "'Delete' operation canceled.";
            }
        }
        #endregion
        #region 6.08 'Edit' Button
        /*
        [X]..Create a button method that will save the edited record of the 
             currently selected item in the ListView. 
        [X]..All the changes in the input controls will be written back to 
             the list. 
        [X]..Display an updated version of the sorted list at the end of 
             this process.
        */
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0)
            {
                toolStripStatusLabel.Text = "No record has been selected. " +
                    "To 'edit' first select a record from the 'Name' column.";
                return;
            }
            var result = MessageBox.Show("Are you sure you want to change " +
                "this record?", "Edit Record",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int currentItem = listView.SelectedIndices[0];
                string name = Wiki[currentItem].GetName();
                listView.Items.RemoveAt(currentItem);
                Wiki.RemoveAt(currentItem);
                AddItem();
                UpdateListView();
                toolStripStatusLabel.Text = "Record updated.";
            }
            else
            {
                toolStripStatusLabel.Text = "'Edit' operation canceled.";
            }
        }
        #endregion
        #region 6.09 Update ListView
        /*
        [X]..Create a single custom method that will sort and then display 
             the Name and Category from the wiki information in the list.
        */
        private void UpdateListView()
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
        #region 6.10 'Search' Button - FIX
        /*
        [P]..Create a button method that will use the builtin binary search 
             to find a Data Structure name. 
        [ ]..If the record is found the associated details will populate the 
             appropriate input controls and highlight the name in the 
             ListView. 
        [ ]..At the end of the search process the search input TextBox must 
             be cleared.
        */
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text))
            {
                Information findItem = new Information();
                findItem.SetName(textBoxName.Text);
                int found = Wiki.BinarySearch(findItem);
                if (found >= 0)
                {
                    listView.SelectedItems.Clear();
                    listView.Items[found].Selected = true;
                    listView.Focus();
                    textBoxName.Text = Wiki[found].GetName();
                    comboBoxCategory.Text = Wiki[found].GetName();
                    SetStructureRadioButton(found);
                }
                else
                {
                    MessageBox.Show("Cannot find 'Name'", "Not Found Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxName.Clear();
                    textBoxName.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please enter a Data Structure 'Name' into the search box", 
                    "Input Error", MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                textBoxName.Clear();
                textBoxName.Focus();
            }
        }
        #endregion
        #region 6.11 ListView Select - FILL
        /*
        [ ]..Create a ListView event so a user can select a Data Structure 
             Name from the list of Names and the associated information will 
             be displayed in the related text boxes combo box and radio 
             button.
        */

        #endregion
        #region 6.12 Clear
        /*
        [T]..Create a custom method that will clear and reset the 
        [T]..TextBoxes, 
        [T]..ComboBox and 
        [T]..Radio button.
        */
        private void ClearTextBoxes()
        {
            textBoxName.Clear();
            comboBoxCategory.Text = string.Empty;
            foreach (RadioButton rb in 
                groupBoxStructure.Controls.OfType<RadioButton>())
            {
                rb.Checked = false;
            }
            textBoxDefinition.Clear();
        }
        #endregion
        #region 6.13 'Clear' DoubleClick Event
        /*
        [T]..Create a double click event on the Name TextBox to clear the 
             TextBboxes, ComboBox and Radio button.
        [T]..this must have an associated tool tip.
        */
        private void textBoxName_DoubleClick(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }
        private void textBoxName_MouseHover(object sender, EventArgs e)
        {
            toolTipTextBoxName.SetToolTip(textBoxName, "Double click to clear all attributes");
        }
        #endregion
        #region 6.14 'Load' and 'Save' Buttons - FILL
        /*
        [ ]..Create two buttons for the manual open and save option; 
        [ ]..this must use a dialog box to select a file or rename a saved 
             file. 
        [ ]..All Wiki data is stored/retrieved using a binary reader/writer 
             file format.
        */
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region 6.15 Save on 'Close' - FILL
        /*
        [ ]..The Wiki application will save data when the form closes. 
        */

        #endregion
    }
}
