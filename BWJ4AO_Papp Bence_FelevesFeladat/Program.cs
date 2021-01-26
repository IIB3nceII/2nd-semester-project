using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memoria_2020_04_17
{
    class Program
    {
        static int mem_db = 30;
        static int dump_db = 30;

        static void teszt1()
        {
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            Console.WriteLine("VÁLTOZÓK LÉTREHOZÁSA, ÉRTÉK MÓDOSÍTÁSA, TÖRLÉSE, TÖMBÖK LÉTREHOZÁSA, ÉRTÉK MÓDOSÍTÁSA TÖRLÉSE, CLEAN");
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            Console.WriteLine();

            Memoria m = new Memoria(mem_db);
            m.MelyikProgram = "pr1";
            Bajt a = new Bajt("a", 100);
            m.SetVariable(a);
            Karakter b = new Karakter("b", 'A');
            m.SetVariable(b);
            m.MelyikProgram = "pr2";
            Szoveg c = new Szoveg("c", "OK");
            m.SetVariable(c);
            m.MelyikProgram = "pr1";
            Egesz d = new Egesz("d", 2020);
            m.SetVariable(d);
            Tort e = new Tort("e", 123456789.123456789);
            m.SetVariable(e);
            m.MelyikProgram = "pr2";
            Szoveg f = new Szoveg("f", "csak teszt");
            m.SetVariable(f);

            m.MemoriaDump(dump_db);

            m.MelyikProgram = "pr1";
            m.SetValue("a", "200");
            m.SetValue("b", "a");
            m.SetValue("d", "1234");
            m.SetValue("e", "3,14");
            m.MelyikProgram = "pr2";
            m.SetValue("f", "új");
            m.SetValue("c","A");

            m.MemoriaDump(dump_db);

            m.MelyikProgram = "pr1";

            m.DelVariable("b");
            m.DelVariable("d");

            m.MelyikProgram = "pr2";

            m.DelVariable("f");

            m.MemoriaDump(dump_db);

            Szoveg g = new Szoveg("g", "TESZT");
            m.SetVariable(g);
            Egesz h = new Egesz("a", 123456789);
            m.SetVariable(h);

            m.MemoriaDump(dump_db);

            m.DelVariable("g");
            m.DelVariable("a");
            m.MelyikProgram = "pr1";
            m.DelVariable("e");

            m.MemoriaDump(dump_db);

            m.Clean();

            m.MemoriaDump(dump_db);

            Egesz[] t = new Egesz[3];
            for (int i = 0; i < t.Length; i++)
            {
                int ertek = (i + 1) * 10;
                t[i] = new Egesz("", ertek);
            }
            m.SetVariable("tomb", t);

            Szoveg[] s = new Szoveg[4];
            s[0] = new Szoveg("", "A");
            s[1] = new Szoveg("", "AB");
            s[2] = new Szoveg("", "ABC");
            s[3] = new Szoveg("", "Z");
            m.SetVariable("str", s);

            Karakter x = new Karakter("z", 'W');
            m.SetVariable(x);

            m.MemoriaDump(dump_db);

            m.SetValue("tomb", 2, "555");
            m.SetValue("str", 3, "XYZ");

            m.MemoriaDump(dump_db);

            m.DelVariable("tomb");

            m.MemoriaDump(dump_db);

            m.DelVariable("str");

            m.MemoriaDump(dump_db);

            m.Clean();

            m.MemoriaDump(dump_db);
        }

        static void teszt2()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("EGESZ TÖMB RENDEZÉSE");
            Console.WriteLine("--------------------");
            Console.WriteLine();

            Memoria m = new Memoria(mem_db);
            m.MelyikProgram = "pr1";

            m.MemoriaDump(dump_db);

            Random r = new Random();
            Egesz[] t = new Egesz[4];
            for (int a = 0; a < t.Length; a++)
            {
                t[a] = new Egesz("", r.Next(100,200));
            }
            m.SetVariable("tomb", t);

            Egesz i = new Egesz("i", 1);
            m.SetVariable(i);

            Egesz j = new Egesz("j", 0);
            m.SetVariable(j);

            Egesz sv = new Egesz("sv", 0);
            m.SetVariable(sv);

            m.MemoriaDump(dump_db);

            while ((m.GetVariable("i") as Egesz).Ertek <= m.GetVariable("tomb",1).Meret - 1)
            {
                m.SetValue("j", ((m.GetVariable("i") as Egesz).Ertek + 1).ToString());
                while ((m.GetVariable("j") as Egesz).Ertek <= m.GetVariable("tomb", 1).Meret)
                {
                    if ((m.GetVariable("tomb", (m.GetVariable("i") as Egesz).Ertek) as Egesz).Ertek >
                         (m.GetVariable("tomb", (m.GetVariable("j") as Egesz).Ertek) as Egesz).Ertek)
                    {
                        m.SetValue("sv", m.GetVariable("tomb", (m.GetVariable("i") as Egesz).Ertek).GetErtek());
                        m.SetValue("tomb",
                                   (m.GetVariable("i") as Egesz).Ertek,
                                   m.GetVariable("tomb", (m.GetVariable("j") as Egesz).Ertek).GetErtek());
                        m.SetValue("tomb",
                                   (m.GetVariable("j") as Egesz).Ertek,
                                   m.GetVariable("sv").GetErtek());
                    }

                    m.MemoriaDump(dump_db);
                    Console.ReadKey();

                    m.SetValue("j", (Convert.ToInt32(m.GetVariable("j").GetErtek()) + 1).ToString());
                }
                m.SetValue("i", (Convert.ToInt32(m.GetVariable("i").GetErtek()) + 1).ToString());
            }

            m.DelVariable("i");
            m.DelVariable("j");
            m.DelVariable("sv");
            m.Clean();
            m.MemoriaDump(dump_db);
            Console.WriteLine("RENDEZÉS KÉSZ!");
        }

        static void Main(string[] args)
        {
            // változók létrehozása, törlése, tömbök létrehozása, törlése, clean
            teszt1();
            
            // egesz tömb rendezése...
            teszt2();
                       
            Console.ReadKey();
        }
    }
}
