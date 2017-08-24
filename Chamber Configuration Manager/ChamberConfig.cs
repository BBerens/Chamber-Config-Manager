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

        public List<string[]> chamberAttributes = new List<string[]>();

        public ChamberConfig(XmlNode node)
        {
            IEnumerator attrEnum;
            XmlNode xmlNode;

            attrEnum = node["chamberConfig"].GetEnumerator();
            while (attrEnum.MoveNext())
            {
                xmlNode = (XmlNode)attrEnum.Current;
                string[] attribute =  new string[2]{ xmlNode.Name, xmlNode.InnerText};
                chamberAttributes.Add(attribute);
            }
        }

        public ChamberConfig(XmlNode node, List<List<string>> attributes)
        {
            IEnumerator attrEnum;
            XmlNode xmlNode;
            string []attribute;
            int i = 0;

            attrEnum = node.GetEnumerator();
            attrEnum.MoveNext();
            xmlNode = (XmlNode)attrEnum.Current;
            attribute = new string[2] { "chamberId", xmlNode.InnerText};
            chamberAttributes.Add(attribute);
            attribute = new string[2] { "toolId", "" };
            chamberAttributes.Add(attribute);
            attribute = new string[2] { "chamberLoc", "" };
            chamberAttributes.Add(attribute);

            while (attrEnum.MoveNext())
            {
                xmlNode = (XmlNode)attrEnum.Current;
                attribute =  new string[2]{ attributes[i++][0], xmlNode.InnerText};
                chamberAttributes.Add(attribute);
            }
        }


        
    }


}
