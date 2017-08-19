using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;

namespace Chamber_Configuration_Manager
{
    class ChamberConfig
    {
        public IEnumerator configAttributes;
        public int chamberId;
        public string toolConfig;
        public string chamberLoc;
        public XmlNode xmlNode;
        public List<string[]> chamberAttributes = new List<string[]>();

        public ChamberConfig(XmlNode node)
        {
            chamberId = Convert.ToInt32(node["chamberId"].InnerText);
            toolConfig = node["toolConfig"].InnerText;
            chamberLoc = node["chamberLoc"].InnerText;
            configAttributes =node["chamberConfig"].GetEnumerator();
            while(configAttributes.MoveNext())
            {
                xmlNode = (XmlNode)configAttributes.Current;
                string[] tempString =  new string[2]{ xmlNode.Name, xmlNode.InnerText};
                chamberAttributes.Add(tempString);
            }
        }

        
    }


}
