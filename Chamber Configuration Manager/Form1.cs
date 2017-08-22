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
            DataRow row;
            Document.Load("C:\\Users\\DrStrange\\Source\\Repos\\Chamber-Config-Manager\\Chamber Configuration Manager\\Chambers.xml");
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
            dataTable.Columns.Add("Chamber Id", typeof(string));
            dataTable.Columns.Add("Tool Id", typeof(string));
            dataTable.Columns.Add("Chamber Location", typeof(string));
            foreach( string[] attribute in chamberList[0].chamberAttributes)
            {
                dataTable.Columns.Add(attribute[0], typeof(string));
            }
            dataGridView1.DataSource = dataTable;
            foreach(ChamberConfig chamber in chamberList)
            {
                row = dataTable.NewRow();
                row["Chamber Id"] = chamber.chamberId;
                row["Tool Id"] = chamber.toolId;
                row["Chamber Location"] = chamber.chamberLoc;
                foreach(string[] attribute in chamber.chamberAttributes)
                {
                    row[attribute[0]] = attribute[1];
                }
                dataTable.Rows.Add(row);
            }
            dataGridView1.Refresh();
        }
    }
}
