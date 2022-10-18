#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
namespace Data_Structures_Wiki_Revision_1
{
    #region 6.01 IComparable Class
    /*
    [X]..Create a separate class file to hold the four data items of the 
         Data Structure 
    [X]..(use the Data Structure Matrix as a guide). 
    [X]..Use private properties for the fields which must be of type “string”. 
    [P]..The class file must have separate setters and getters, 
    [X]..add an appropriate IComparable for the Name attribute. 
    [X]..Save the class as “Information.cs”.
    */
    internal class Information : IComparable<Information>
    {
        private string Name;
        private string Category;
        private string Structure;
        private string Definition;
        public int CompareTo(Information other)
        {
            return Name.CompareTo(other.Name);
        }
        public void SetName(string newName)
        {
            Name = newName;
        }
        public void SetCategory(string newCategory)
        {
            Category = newCategory;
        }
        public void SetStructure(string newStructure)
        {
            Structure = newStructure;
        }
        public void SetDefinition(string newDefinition)
        {
            Definition = newDefinition;
        }
        public string GetName()
        {
            return Name;
        }
        public string GetCategory()
        {
            return Category;
        }
        public string GetStructure()
        {
            return Structure;
        }
        public string GetDefinition()
        {
            return Definition;
        }
    }
    #endregion
}
