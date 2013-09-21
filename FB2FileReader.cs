using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace Thumbnailer
{
    class FB2FileReader
    {
        private String FData;        

        public FB2FileReader()
        {
            FData = null;
        }

        public int Read(byte[] AInput)
        {
            FData = GetImageContent(AInput);
            if (FData != null)
                return FData.Length;
            else
                return 0;
        }

        private String GetImageContent(byte[] AInput)
        {
            String vResult = null;
            MemoryStream vStream = new MemoryStream(AInput);
            XmlDocument vFB2 = new XmlDocument();
            vFB2.Load(vStream);
            XmlNodeList vCovers = vFB2.GetElementsByTagName("coverpage");
            XmlNodeList vContent = vFB2.GetElementsByTagName("binary");           
            if (vCovers.Count > 0)
            {
                XmlNode vImage = vCovers.Item(0).FirstChild;
                if (vImage != null)
                {
                    Console.Out.WriteLine("Image name: " + vImage.Name);
                    if (vImage.Name.Equals("image"))
                    {
                        XmlAttributeCollection vAttributes = vImage.Attributes;
                        XmlNode vName = vAttributes.GetNamedItem("l:href");
                        if (vName != null)
                        {
                            String vHref = vName.Value;
                            vHref = vHref.Substring(1);
                            vResult = GetContent(vHref, vContent);                            
                        }
                    }
                }
            }
            return vResult;
        }

        private String GetContent(String AName, XmlNodeList AData)
        {
            String vResult = null;
            for (int i = 0; i < AData.Count; i++)
            {
                XmlAttributeCollection vAttributes = AData[i].Attributes;
                XmlNode vName = vAttributes.GetNamedItem("id");                
                if (vName.Value.Equals(AName))
                {
                    vResult = AData[i].InnerText;                    
                    break;
                }
            }
            return vResult;
        }

        public byte[] GetBuffer()
        {
            byte[] vData;
            try
            {
                vData = Convert.FromBase64String(FData);
            }
            catch
            {
                vData = null;
            }
            finally
            {
                FData = null;                
            }
            return vData;
        }
    }
}
