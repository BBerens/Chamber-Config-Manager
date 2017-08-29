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
using System.IO;
using System.Collections;

namespace Chamber_Configuration_Manager
{
    public partial class Form1 : Form
    {
        List<ChamberConfig> chamberList = new List<ChamberConfig>();
        List<List<string>> parameterList = new List<List<string>>();
        List<string> columnAttributes;

        public Form1()
        {
            
            InitializeComponent();
            
            
        }

        public Form1(Stream instream)
        {

            InitializeComponent();
            importParse(instream, chamberList, parameterList);
            columnAttributes = drawTable(chamberList);

        }

        private void importParse(Stream instream , List<ChamberConfig> chamberList, List<List<string>> parameterList)
        {
            XmlDocument importFile = new XmlDocument();
            XmlNode root, node, child, chamberNode;
            IEnumerator chamberEnum, paramEnum, valueEnum;
            ChamberConfig newChamber;
            List<string> paramValueList;
            List<string> attributeStr;

            importFile.Load(instream);
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
            attributeStr = new List<string> { "chamberId" };
            parameterList.Insert(0, attributeStr);
            attributeStr = new List<string>{"toolId", ""};
            parameterList.Insert(1, attributeStr);
            attributeStr = new List<string> { "chamberLoc", "" };
            parameterList.Insert(2, attributeStr);
        }

        private void writeXML(Stream instream, List<ChamberConfig> chamberList)
        {
            XmlWriter writer = XmlWriter.Create(instream);
            writer.WriteStartDocument();
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Chambers");
            writer.WriteWhitespace("\n");
            foreach(ChamberConfig chamber in chamberList)
            {
                writer.WriteWhitespace("\t");
                writer.WriteStartElement("Chamber");
                writer.WriteWhitespace("\n");
                foreach(string[] attribute in chamber.chamberAttributes)
                {
                    writer.WriteWhitespace("\t\t");
                    writer.WriteElementString(attribute[0], attribute[1]);
                    writer.WriteWhitespace("\n");
                }
                writer.WriteWhitespace("\t");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
            }
            writer.WriteWhitespace("\t");
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private void openFile()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            importParse(myStream, chamberList, parameterList);
                            columnAttributes = drawTable(chamberList);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            List<int[]> searchParameters = new List<int[]>();
            List<ChamberConfig> searchResults = new List<ChamberConfig>(chamberList);
            int[] tempIndex;
            if(checkBox1.Checked)
            {
                tempIndex = new int[2] { comboBox1.SelectedIndex,comboBox6.SelectedIndex + 1};
                searchParameters.Add(tempIndex);
            }
            if (checkBox2.Checked)
            {
                tempIndex = new int[2] {comboBox2.SelectedIndex, comboBox7.SelectedIndex + 1 };
                searchParameters.Add(tempIndex);
            }
            if (checkBox3.Checked)
            {
                tempIndex = new int[2] { comboBox3.SelectedIndex, comboBox8.SelectedIndex + 1 };
                searchParameters.Add(tempIndex);
            }
            if (checkBox4.Checked)
            {
                tempIndex = new int[2] { comboBox4.SelectedIndex, comboBox9.SelectedIndex + 1 };
                searchParameters.Add(tempIndex);
            }
            if (checkBox5.Checked)
            {
                tempIndex = new int[2] { comboBox5.SelectedIndex, comboBox10.SelectedIndex + 1 };
                searchParameters.Add(tempIndex);
            }
            foreach(int[] searchParameter in searchParameters)
            {
                searchResults = searchResults.Where(chamber => chamber.chamberAttributes[searchParameter[0]][1] == parameterList[searchParameter[0]][searchParameter[1]]).ToList();
            }
            drawTable(searchResults);
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            List<string> sublist1 = new List<string>(parameterList[comboBox1.SelectedIndex].GetRange(1, parameterList[comboBox1.SelectedIndex].Count - 1));
            comboBox6.DataSource = sublist1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            List<string> sublist2 = new List<string>(parameterList[comboBox2.SelectedIndex].GetRange(1, parameterList[comboBox2.SelectedIndex].Count - 1));
            comboBox7.DataSource = sublist2;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<string> sublist3 = new List<string>(parameterList[comboBox3.SelectedIndex].GetRange(1, parameterList[comboBox3.SelectedIndex].Count - 1));
            comboBox8.DataSource = sublist3;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<string> sublist4 = new List<string>(parameterList[comboBox4.SelectedIndex].GetRange(1, parameterList[comboBox4.SelectedIndex].Count - 1));
            comboBox9.DataSource = sublist4;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<string> sublist5 = new List<string>(parameterList[comboBox5.SelectedIndex].GetRange(1, parameterList[comboBox5.SelectedIndex].Count - 1));
            comboBox10.DataSource = sublist5;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                comboBox1.Enabled = true;
                comboBox6.Enabled = true;
                List<string> columnAttributes1 = new List<string>(columnAttributes);
                comboBox1.DataSource = columnAttributes1;
            }
            else
            {
                comboBox1.Enabled = false;
                comboBox6.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                comboBox2.Enabled = true;
                comboBox7.Enabled = true;
                List<string> columnAttributes2 = new List<string>(columnAttributes);
                comboBox2.DataSource = columnAttributes2;
            }
            else
            {
                comboBox2.Enabled = false;
                comboBox7.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                comboBox3.Enabled = true;
                comboBox8.Enabled = true;
                List<string> columnAttributes3 = new List<string>(columnAttributes);
                comboBox3.DataSource = columnAttributes3;
            }
            else
            {
                comboBox3.Enabled = false;
                comboBox8.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                comboBox4.Enabled = true;
                comboBox9.Enabled = true;
                List<string> columnAttributes4 = new List<string>(columnAttributes);
                comboBox4.DataSource = columnAttributes4;
            }
            else
            {
                comboBox4.Enabled = false;
                comboBox9.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                comboBox5.Enabled = true;
                comboBox10.Enabled = true;
                List<string> columnAttributes5 = new List<string>(columnAttributes);
                comboBox5.DataSource = columnAttributes5;
            }
            else
            {
                comboBox5.Enabled = false;
                comboBox10.Enabled = false;
            }
        }

        private List<string> drawTable(List<ChamberConfig> chamberList)
        {
            DataRow row;
            List<string> columnAttributes = new List<string>();

            foreach (List<string> attribute in parameterList)
                columnAttributes.Add(attribute[0]);

            DataTable dataTable = new DataTable("All Chambers");
            foreach (string attribute in columnAttributes)
            {
                dataTable.Columns.Add(attribute, typeof(string));
            }


            dataGridView1.DataSource = dataTable;
            foreach (ChamberConfig chamber in chamberList)
            {
                row = dataTable.NewRow();
                foreach (string[] attribute in chamber.chamberAttributes)
                {
                    row[attribute[0]] = attribute[1];
                }
                dataTable.Rows.Add(row);
            }
            dataGridView1.Refresh();
            return columnAttributes;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    writeXML(myStream, chamberList);
                    myStream.Close();
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFile();
        }


    }
}
