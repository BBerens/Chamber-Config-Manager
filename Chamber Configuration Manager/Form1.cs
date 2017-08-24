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
        List<ChamberConfig> chamberList = new List<ChamberConfig>();
        List<List<string>> parameterList = new List<List<string>>();

        public Form1()
        {
                        
            DataRow row;
            List<string> columnAttributes = new List<string>();
            importParse("C:\\Users\\DrStrange\\Source\\Repos\\Chamber-Config-Manager\\Chamber Configuration Manager\\Chamber Configurations.xml", chamberList, parameterList); 
            
 
            InitializeComponent();

            foreach (string[] attribute in chamberList[0].chamberAttributes)
                columnAttributes.Add(attribute[0]);

            DataTable dataTable = new DataTable("All Chambers");
            DataTable searchResults = new DataTable("Search Results");
            foreach( string attribute in columnAttributes)
            {
                dataTable.Columns.Add(attribute, typeof(string));
                searchResults.Columns.Add(attribute, typeof(string));
            }


            dataGridView1.DataSource = dataTable;
            foreach(ChamberConfig chamber in chamberList)
            {
                row = dataTable.NewRow();
                foreach(string[] attribute in chamber.chamberAttributes)
                {
                    row[attribute[0]] = attribute[1];
                }
                dataTable.Rows.Add(row);
            }
            dataGridView1.Refresh();
            comboBox1.DataSource = columnAttributes;
        }

        private void importParse(string filepath, List<ChamberConfig> chamberList, List<List<string>> parameterList)
        {
            XmlDocument importFile = new XmlDocument();
            XmlNode root, node, child, chamberNode;
            IEnumerator chamberEnum, paramEnum, valueEnum;
            ChamberConfig newChamber;
            List<string> paramValueList;

            importFile.Load(filepath);
            root = importFile.DocumentElement;
            paramEnum = root["Parameters"].GetEnumerator();
            while(paramEnum.MoveNext())
            {
                paramValueList = new List<string>();
                node = (XmlNode)paramEnum.Current;
                paramValueList.Add(node.Attributes["name"].Value);
                valueEnum = node.FirstChild.GetEnumerator();
                while(valueEnum.MoveNext())
                {
                    child = (XmlNode)valueEnum.Current;
                    paramValueList.Add(child.InnerText);
                }
                parameterList.Add(paramValueList);
            }

            chamberEnum = root["Testset"].GetEnumerator();
            while (chamberEnum.MoveNext())
            {
                chamberNode = (XmlNode)chamberEnum.Current;
                newChamber = new ChamberConfig(chamberNode, parameterList);
                chamberList.Add(newChamber);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox6.Enabled = true;
            List<string> sublist = parameterList[comboBox1.SelectedIndex].GetRange(1, parameterList[comboBox1.SelectedIndex].Count - 1);
            comboBox6.DataSource = sublist;
        }
    }
}
