#region Imports
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

#endregion
namespace Data_Structures_Wiki_Revision_1
{
    public partial class Data_Structures_Wiki_Revision_1 : Form
    {
        #region Initialise Form
        public Data_Structures_Wiki_Revision_1()
        {
            InitializeComponent();
            PopulateComboBox();
        }
        #endregion
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
        [T]..Display on ListBox.
        [T]..Clear Inputs.
        */
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(BlankCheck() == true)
            {
                MessageBox.Show("One of the input fields are blank, " +
                   "please try again",
                   "Duplication Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AddItemDuplicateCheck();
        }
        private void AddItemDuplicateCheck()
        {
            if (ValidName(textBoxName.Text) == false)
            {
                AddItem();
                UpdateListView();
                toolStripStatusLabel.Text = "Record added.";
            }
            else
            {
                MessageBox.Show("An entry with that 'Name' already exists, " +
                    "please try again",
                    "Duplication Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void AddItem()
        {
            Information addInformation = new Information();
            addInformation.SetName(textBoxName.Text);
            addInformation.SetCategory(comboBoxCategory.Text);
            addInformation.SetStructure(GetStructureRadioButton());
            addInformation.SetDefinition(textBoxDefinition.Text);
            Wiki.Add(addInformation);
            ClearIO();
        }
        private Boolean BlankCheck()
        {
            Boolean inputError = false;
            if (comboBoxCategory.SelectedItem == null)
            {
                return true;
            }
            if (textBoxName.Text == "")
            {
                return true;
            }
            if (textBoxDefinition.Text == "")
            {
                return true;
            }
            if (GetStructureRadioButton() == "")
            {
                return true;
            }
            return inputError;
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
            comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        #endregion
        #region 6.05 'Name' Duplicate Checker, Character Filter
        /*
        [T]..Create a custom ValidName method which will take a parameter 
             string value from the Textbox Name and returns a Boolean after 
             checking for duplicates. 
        [T]..Use the built in List<T> method “Exists” to answer this 
             requirement.
        [T]..filter out numeric or special character input. 
        */
        private Boolean ValidName(string name)
        {
            return Wiki.Exists(x => x.GetName() == name);
        }
        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && (e.KeyChar != ' ') 
                && (e.KeyChar != '-') && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
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
                    rbValue = "";
                }
            }
            return rbValue;
        }
        //private Boolean
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
                RemoveCurrentItem();
                UpdateListView();
                toolStripStatusLabel.Text = "Record deleted.";
            }
            else
            {
                toolStripStatusLabel.Text = "'Delete' operation canceled.";
            }
        }
        private void RemoveCurrentItem()
        {
            int currentItem = listView.SelectedIndices[0];
            listView.Items.RemoveAt(currentItem);
            Wiki.RemoveAt(currentItem);
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
            if (BlankCheck() == true)
            {
                MessageBox.Show("One of the input fields are blank, " +
                   "please try again",
                   "Duplication Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var result = MessageBox.Show("Are you sure you want to change " +
                "this record?", "Edit Record",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if(textBoxName.Text == 
                    Wiki[listView.SelectedIndices[0]].GetName())
                {
                    RemoveCurrentItem();
                    AddItem();
                    UpdateListView();
                    toolStripStatusLabel.Text = "Record updated.";
                }
                else
                {
                    EditItemDuplicateCheck();
                }
            }
            else
            {
                toolStripStatusLabel.Text = "'Edit' operation canceled.";
            }
        }
        private void EditItemDuplicateCheck()
        {
            if (ValidName(textBoxName.Text) == false)
            {
                RemoveCurrentItem();
                AddItem();
                UpdateListView();
                toolStripStatusLabel.Text = "Record updated.";
            }
            else
            {
                MessageBox.Show("An entry with that 'Name' already exists, " +
                    "please try again", "Duplication Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        #region 6.10 'Search' Button
        /*
        [T]..Create a button method that will use the builtin binary search 
             to find a Data Structure name. 
        [T]..If the record is found the associated details will populate the 
             appropriate input controls and highlight the name in the 
             ListView. 
        [T]..At the end of the search process the search input TextBox must 
             be cleared.
        [T]..refocus the I beam icon into search textbox.
        */
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxSearch.Text))
            {
                Information findItem = new Information();
                findItem.SetName(textBoxSearch.Text);
                int found = Wiki.BinarySearch(findItem);
                if (found >= 0)
                {
                    listView.SelectedItems.Clear();
                    listView.Items[found].Selected = true;
                    //listView.Focus();
                    textBoxName.Text = Wiki[found].GetName();
                    comboBoxCategory.Text = Wiki[found].GetCategory();
                    SetStructureRadioButton(found);
                    textBoxDefinition.Text = Wiki[found].GetDefinition();
                    textBoxSearch.Clear();
                    textBoxSearch.Focus();
                    toolStripStatusLabel.Text = "Record found. " +
                        "Attributes displayed above.";
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
                MessageBox.Show("Please enter a Data Structure 'Name' " +
                    "into the search box", 
                    "Input Error", MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                textBoxName.Clear();
                textBoxName.Focus();
            }
        }
        #endregion
        #region 6.11 ListView Select
        /*
        [T]..Create a ListView event so a user can select a Data Structure 
             Name from the list of Names and the associated information will 
             be displayed in the related 
        [T]..text boxes 
        [T]..combo box and 
        [T]..radio button.
        */
        private void listView_Click(object sender, EventArgs e)
        {
            int currentItem = listView.SelectedIndices[0];
            textBoxName.Text = Wiki[currentItem].GetName();
            comboBoxCategory.Text = Wiki[currentItem].GetCategory();
            SetStructureRadioButton(currentItem);
            textBoxDefinition.Text = Wiki[currentItem].GetDefinition();
            toolStripStatusLabel.Text = 
                "Record selected. Attributes displayed above.";
        }
        #endregion
        #region 6.12 Clear
        /*
        [T]..Create a custom method that will clear and reset the 
        [T]..TextBoxes, 
        [T]..ComboBox and 
        [T]..Radio button.
        */
        private void ClearIO()
        {
            textBoxName.Clear();
            comboBoxCategory.SelectedItem = null;   
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
            ClearIO();
            toolStripStatusLabel.Text = "All attributes cleared.";
        }
        private void textBoxName_MouseHover(object sender, EventArgs e)
        {
            toolTipTextBoxName.SetToolTip(textBoxName, 
                "Double click to clear all attributes");
        }
        #endregion
        #region 6.14 'Load' and 'Save' Buttons
        /*
        [ ]..Create two buttons for the manual open and save option; 
        [ ]..this must use a dialog box to select a file or rename a saved 
             file. 
        [ ]..All Wiki data is stored/retrieved using a binary reader/writer 
             file format.
        */
        private void buttonLoad_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "BIN FILES|*.bin";
            openFileDialog.Title = "Open a BIN file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Open(openFileDialog.FileName);
                UpdateListView();
                toolStripStatusLabel.Text = "Array imported from file.";
            }
            else
                toolStripStatusLabel.Text = "Load operation canceled.";
        }
        private void Open(string openFileName)
        {
            try
            {
                using (Stream stream = File.Open(openFileName, 
                    FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, 
                        Encoding.UTF8, false))
                    {
                        while (stream.Position < stream.Length)
                        {
                            Information addInformation = new Information();
                            addInformation.SetName(reader.ReadString());
                            addInformation.SetCategory(reader.ReadString());
                            addInformation.SetStructure(reader.ReadString());
                            addInformation.SetDefinition(reader.ReadString());
                            Wiki.Add(addInformation);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        string defaultFileName = "default.bin";
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bin file|*.bin";
            saveFileDialog.Title = "Save A BIN file";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.DefaultExt = "bin";
            saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            if (saveFileDialog.FileName != "")
            {
                Save(fileName);
                toolStripStatusLabel.Text = "Array saved to: " + fileName;
            }
            else
            {
                Save(defaultFileName);
                toolStripStatusLabel.Text = "Array saved to: " + 
                    Application.StartupPath + "\\" + defaultFileName;
            }
        }
        private void Save(string saveFileName)
        {
            try
            {
                using (Stream stream = File.Open(saveFileName, 
                    FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, 
                        Encoding.UTF8, false))
                    {
                        foreach (var item in Wiki)
                        {
                            writer.Write(item.GetName());
                            writer.Write(item.GetCategory());
                            writer.Write(item.GetStructure());
                            writer.Write(item.GetDefinition());
                            //writer.Write(string.Join(",",
                            //bike.GetAccessories()));
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        #region 6.15 Save on 'Close'
        /*
        [ ]..The Wiki application will save data when the form closes. 
        */
        private void Data_Structures_Wiki_Revision_1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save("AutoSave.bin");
        }
        #endregion
        #region MISC
        /*
        */
        #endregion
    }
}
