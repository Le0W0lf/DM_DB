using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;



namespace DM_DB
{
    class Program
    {
        static void Main(string[] args)
        {

            Read_Bestiary();

        }


        static void Read_Bestiary()
        {
            Bestiary[] Creatures = new Bestiary[1000];
            int I = 0;
            string Url = "http://dungeon.su/bestiary/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument bestiary = web.Load(Url);
            HtmlDocument creature;
            string creature_info, url, creature_name, file;
            for (int i = 2; i < 800; i++)
            {
                try
                {
                    url = bestiary.DocumentNode.SelectSingleNode("/html/body/main/div/div[3]/div/section[2]/div[1]/div/ul[2]/li["
                        + i + "]/a").Attributes["href"].Value;

                    creature = web.Load("http://dungeon.su" + url);

                    creature_name = creature.DocumentNode.SelectSingleNode("/html/body/main/div/div[3]/div/" +
                        "section[2]/div[2]/div/div[1]/h2/a[2]").InnerText;

                    Console.WriteLine(creature_name);
                    file = "DB/" + I + ".txt";
                    Creatures[I] = new Bestiary(creature_name, file);
                    I++;

                    using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
                    {

                        for (int j = 1; j < 100; j++)
                        {

                            try
                            {
                                creature_info = creature.DocumentNode.SelectSingleNode("/html/body/main/div/div[3]/div/" +
                                "section[2]/div[2]/div/div[2]/ul/li[" + j + "]").Attributes["class"].Value;
                                if (creature_info == "stats")
                                {
                                    for (int k = 1; k < 6; k++)
                                    {
                                        creature_info = creature.DocumentNode.SelectSingleNode("/html/body/main/div/div[3]/div/" +
                                            "section[2]/div[2]/div/div[2]/ul/li[" + j + "]/div[" + k + "]").InnerText;
                                        sw.WriteLine(creature_info);
                                    }
                                }
                                else if (creature_info == "subsection")
                                {
                                    try
                                    {
                                        creature_info = creature.DocumentNode.SelectSingleNode("/html/body/main/div/div[3]/div/" +
                                            "section[2]/div[2]/div/div[2]/ul/li[" + j + "]/h3").InnerText.ToUpper();
                                        sw.WriteLine(creature_info);
                                        for (int k = 1; k < 20; k++)
                                        {
                                            creature_info = creature.DocumentNode.SelectSingleNode("/html/body/main/div/div[3]/div/" +
                                            "section[2]/div[2]/div/div[2]/ul/li[" + j + "]/div/p[" +k + "]").InnerText;
                                            sw.WriteLine(creature_info);
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                                else if (creature_info == "translate-by")
                                {
                                    break;
                                }
                            }
                            catch
                            {
                                creature_info = creature.DocumentNode.SelectSingleNode("/html/body/main/div/div[3]/div/" +
                                "section[2]/div[2]/div/div[2]/ul/li[" + j + "]").InnerText;
                                sw.WriteLine(creature_info);
                            }
                            //Console.WriteLine(creature_name);
                            
                        }
                    }


                }
                catch
                {

                }
            }
            using (StreamWriter sw = new StreamWriter("bestiary.txt", false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < I; i++)
                {
                    sw.WriteLine(Creatures[i].GetInfo());
                }
            }


                string temp = Console.ReadLine();
        }

    }

    class Bestiary
    {
        public string name;
        public string path;
        public Bestiary(string s1,string s2)
        {
            name = s1;
            path = s2;
        }
        public string GetInfo()
        {
            return name + "\n" + path;
        }
    }
}
