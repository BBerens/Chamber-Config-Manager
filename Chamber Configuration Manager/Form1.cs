using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace Chamber_Configuration_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            XmlDocument Document = new XmlDocument();
            XmlNode root, chamberNode;
            List<ChamberConfig> chamberList = new List<ChamberConfig>();
            ChamberConfig newChamber;
            IEnumerator chamberEnum;
            Document.Load("C:\\Users\\bberens153719\\Documents\\Visual Studio 2013\\Projects\\Unified Test Management\\Chamber Configuration Manager\\Chamber Configuration Manager\\Chambers.xml");
            root = Document.DocumentElement;
            chamberEnum = root.GetEnumerator();
            InitializeComponent();
            while(chamberEnum.MoveNext())
            {
                chamberNode = (XmlNode)chamberEnum.Current;
                newChamber = new ChamberConfig(chamberNode);
                chamberList.Add(newChamber);
            }

            DataTable dataTable = new DataTable("Search Results");
        }
    }
}
