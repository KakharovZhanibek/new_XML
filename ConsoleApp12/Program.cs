using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApp12
{
    public struct HabrNews
    {
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public DateTime pubDate { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\n--> {1}\n-->{2:dd.MM.yyyy}\n", title, link, pubDate);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //foreach (HabrNews item in GetHabrNews())
            //    Console.WriteLine(item);

            Exmpl001();
        }

        static List<HabrNews> GetHabrNews()
        {
            List<HabrNews> habrNewses = new List<HabrNews>();

            foreach (XmlNode item in GetDoc("https://habr.com/ru/rss/interesting/")
                .SelectNodes("//rss/channel/item"))
            {
                HabrNews hn = new HabrNews();
                hn.title = item.SelectSingleNode("title").InnerText;
                hn.link = item.SelectSingleNode("link").InnerText;
                hn.description = item.SelectSingleNode("description").InnerText;
                hn.pubDate = Convert.ToDateTime(item.SelectSingleNode("pubDate").InnerText);
                habrNewses.Add(hn);
            }

            return habrNewses;
        }
        static XmlDocument GetDoc(string link)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(link);
            return doc;
        }



        static void exmpl()
        {
            XmlDocument doc = new XmlDocument();
            //1 ссылку на ресус
            doc.Load("https://news.rambler.ru/rss/world/");
            //2
            //doc.Load("note.xml");
            //3
            //doc.LoadXml(GetXml());

            //DocumentElement - возвращает корневой элемент
            foreach (XmlNode root in doc.DocumentElement.ChildNodes)
            {
                foreach (XmlNode channel in root.ChildNodes)
                {
                    if (channel.Name.Equals("title"))
                    {
                        string title = channel.InnerText;
                        Console.WriteLine(title);
                    }
                }
            }

            XmlNode titleNode = doc.SelectSingleNode("//rss/channel/title");
            Console.WriteLine(titleNode.InnerText);

            foreach (XmlNode item in doc.SelectNodes("//rss/channel/item"))
            {
                XmlNode guidNode = item.SelectSingleNode("guid");
                Console.WriteLine(guidNode.InnerText);

                foreach (XmlAttribute attr in guidNode.Attributes)
                {
                    Console.WriteLine("{0} - {1}", attr.Name, attr.InnerText);
                }
            }
        }
        static void Exmpl001()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration =
              doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement note = doc.CreateElement("note");
            XmlAttribute date = doc.CreateAttribute("date");
            date.InnerText = DateTime.Now.ToShortDateString();
            note.Attributes.Append(date);

            XmlElement to = doc.CreateElement("to");
            to.InnerText = "Tove";

            XmlElement from = doc.CreateElement("from");
            from.InnerText = "Jani";

            XmlElement heading = doc.CreateElement("heading");
            heading.InnerText = "Напоминание";

            XmlElement body = doc.CreateElement("body");
            body.InnerText = "Не забудь обо мне в эти выходные!";

            note.AppendChild(to);
            //<note> <to></to>  </note>
            note.AppendChild(from);
            note.AppendChild(heading);
            note.AppendChild(body);

            doc.AppendChild(note);
            doc.Save("note.xml");



            XElement el = new XElement("note", new XElement("to", "Yevgeniy"),
                                               new XElement("from", "Jani"),
                                               new XElement("heading", "Напоминание"),
                                               new XElement("body", "Не забудь обо мне в эти выходные!"));

            el.Save("el_note.xml");
        }
        static string GetXml()
        {
            return @"<note date='05.03.2019'><to>Tove</to><from>Jani</from><heading>Напоминание</heading><body>Не забудь обо мне в эти выходные!</body></note>";
        }



        static void exmpl02()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("note.xml");

            //Изменение
            EditElement(doc, "to", "Yevgeniy");

            //Добавление
            //XmlElement priority = doc.CreateElement("priority");
            //priority.InnerText = "Lower";

            //var to = doc.SelectSingleNode("//note/to");
            //doc.DocumentElement.InsertAfter(priority, to);
            
            //Удаление
            var priority = doc.SelectSingleNode("//note/priority");
            doc.DocumentElement.RemoveChild(priority);


            doc.Save("note.xml");
        }

        static void EditElement(XmlDocument doc, string name, string value)
        {
          doc.DocumentElement.SelectSingleNode(name).InnerText = value;
        }
    }
}
