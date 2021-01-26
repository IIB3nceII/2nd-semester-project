using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memoria_2020_04_17
{
      class Memoria_NincsIlyenElemExceptoin : Exception
      {
            public Memoria_NincsIlyenElemExceptoin() : base("Nincs ilyen elem a listában")
            {
            }
      }

      class Memoria_KifutottATartomanybolException : Exception
      {
            public Memoria_KifutottATartomanybolException() : base("A memória kifutott a tartományból!")
            {
            }
      }

      class Memoria_VanMarIilyenValtozoEzenProgramnalException : Exception
      {
            public Memoria_VanMarIilyenValtozoEzenProgramnalException() : base("Van már ilyen azonosítójú változó ennél a programnál!")
            {
            }
      }

      class Memoria_AzElemTombException : Exception
      {
            public Memoria_AzElemTombException() : base("Ez az elem tömb! Kezelje tömbként!")
            {
            }
      }

      class Memoria_AzElemNemTombException : Exception
      {
            public Memoria_AzElemNemTombException() : base("Ez az elem nem tömb! Nem tömbként kell kezelni!")
            {
            }
      }

      class Memoria_TombIndexHataronKivulEsikException : Exception
      {
            public Memoria_TombIndexHataronKivulEsikException() : base("Az index a tömb határain kívül esik!")
            {
            }
      }

      class Memoria_TipusNemEgyezikException : Exception
      {
            public Memoria_TipusNemEgyezikException() : base("Típus nem egyezik!")
            {
            }
      }


      class ListaElem<T>
      {
            private T tartalom;
            public T Tartalom { get => tartalom; }

            private string nev;
            public string Nev { get => nev; }

            public int Kezdo_index { get; set; }

            public int Hossz { get; set; }

            protected bool tomb_e;
            public bool Tomb_e { get => tomb_e; }

            private int tomb_index;
            public int Tomb_index { get => tomb_index; }

            private string program;
            public string Program { get => program; }

            public ListaElem<T> Kovetkezo { get; set; }

            public ListaElem(T Tartalom, string Nev, int Hossz, int Kezdo_index, bool Tomb_e, int Tomb_index, string Program)
            {
                  tartalom = Tartalom;
                  nev = Nev;
                  this.Hossz = Hossz;
                  this.Kezdo_index = Kezdo_index;
                  tomb_e = Tomb_e;
                  tomb_index = Tomb_index;
                  program = Program;
            }

      }

      class LancoltLista<T>
      {
            public ListaElem<T> Fej { get; set; }
            public LancoltLista()
            {
                  Fej = null;
            }
            public void Add(T t, string nev, int h, int index = 0, bool tomb_e = false, int tomb_index = 0, string prg = "")
            {
                  if (Fej == null)
                  {
                        Fej = new ListaElem<T>(t, nev, h, index, tomb_e, tomb_index, prg);
                        Fej.Kovetkezo = null;
                  }
                  else
                  {
                        ListaElem<T> p = Fej;
                        while (p.Kovetkezo != null)
                        {
                              p = p.Kovetkezo;
                        }
                        ListaElem<T> a = new ListaElem<T>(t, nev, h, index, tomb_e, tomb_index, prg);
                        a.Kovetkezo = null;
                        p.Kovetkezo = a;
                  }
            }

            public void Del(string nev, string prg = "")
            {
                  ListaElem<T> p = Fej;
                  ListaElem<T> elozo = null;
                  while (p != null && !(p.Nev == nev && p.Program == prg))
                  {
                        elozo = p;
                        p = p.Kovetkezo;
                  }
                  if (p != null)
                  {
                        if (p == Fej)
                        {
                              Fej = p.Kovetkezo;
                        }
                        else
                        {
                              elozo.Kovetkezo = p.Kovetkezo;
                        }
                        p = null; // p törlése???
                  }
                  else
                  {
                        throw new Memoria_NincsIlyenElemExceptoin();
                  }
            }

            public void DelAll()
            {
                  ListaElem<T> p = Fej;
                  while (p != null)
                  {
                        ListaElem<T> elozo = null;
                        while (p != null)
                        {
                              elozo = p;
                              p = p.Kovetkezo;
                        }
                        if (p == null)
                        {
                              Del(elozo.Nev, elozo.Program);
                              p = Fej;
                        }
                  }
                  Fej = null;
            }
      }

      class Memoria
      {
            private int delete_db;
            private int kapacitas;
            public int Kapacitas { get => kapacitas; }

            public string MelyikProgram { get; set; }

            byte[] mem;

            LancoltLista<Valtozo> szabad;
            LancoltLista<Valtozo> foglalt;

            public Memoria(int Kapacitas)
            {
                  kapacitas = Kapacitas;
                  mem = new byte[Kapacitas];

                  foglalt = new LancoltLista<Valtozo>();
                  MemInit();
            }

            protected void MemInit()
            {
                  for (int i = 0; i < mem.Length; i++)
                  {
                        mem[i] = 0;
                  }

                  delete_db = 0;
                  szabad = new LancoltLista<Valtozo>();
                  string sv = "del" + (delete_db++).ToString();
                  szabad.Add(new Bajt(sv, 0), sv, kapacitas);
            }

            protected void ValtozoBajtjaiaitMemoriabaKiir(Valtozo v, int index)
            {
                  List<byte> x = v.GetAdatokBajtTombben();
                  int j = 0;
                  for (int i = index; i < index + x.Count; i++)
                  {
                        mem[i] = x[j];
                        j++;
                  }
            }

            protected void ValtozoBajtjaiaitMemoriabanNullaz(int index, int darab)
            {
                  for (int i = index; i < index + darab; i++)
                  {
                        mem[i] = 0;
                  }
            }

            protected ListaElem<Valtozo> ElsoSzabadHely(int meret)
            {
                  ListaElem<Valtozo> p = szabad.Fej;
                  bool ok = false;
                  while (p != null && !ok)
                  {
                        if (p.Hossz >= meret)
                        {
                              ok = true;
                        }
                        else
                              p = p.Kovetkezo;
                  }
                  return p;
            }

            public ListaElem<Valtozo> KeresValtozo(string nev, string prg = "")
            {
                  ListaElem<Valtozo> p = foglalt.Fej;
                  bool ok = false;
                  while (p != null && !ok)
                  {
                        if (p.Nev == nev && p.Program == prg)
                        {
                              ok = true;
                        }
                        else
                        {
                              p = p.Kovetkezo;
                        }
                  }
                  if (ok)
                  {
                        return p;
                  }
                  else
                  {
                        return null;
                  }
            }

            public void SetValue(string nev, string uj)
            {
                  ListaElem<Valtozo> elem = KeresValtozo(nev, MelyikProgram);
                  if (elem != null)
                  {
                        try
                        {
                              if (elem.Tomb_e)
                              {
                                    throw new Memoria_AzElemTombException();
                              }
                              switch (elem.Tartalom.GetTipus())
                              {
                                    case 'B':
                                          (elem.Tartalom as Bajt).Ertek = Convert.ToByte(uj);
                                          break;
                                    case 'K':
                                          (elem.Tartalom as Karakter).Ertek = Convert.ToChar(uj);
                                          break;
                                    case 'E':
                                          (elem.Tartalom as Egesz).Ertek = Convert.ToInt32(uj);
                                          break;
                                    case 'T':
                                          (elem.Tartalom as Tort).Ertek = Convert.ToDouble(uj);
                                          break;
                                    case 'S':
                                          if ((elem.Tartalom as Szoveg).Ertek.Length == uj.Length)
                                          {
                                                (elem.Tartalom as Szoveg).Ertek = uj;
                                          }
                                          else
                                          {
                                                Szoveg sv = new Szoveg(nev, uj);
                                                DelVariable(nev);
                                                SetVariable(sv);
                                          }
                                          break;
                                    default:
                                          throw new Exception("Nem azonosított típus!");
                              }
                              if (elem.Tartalom.GetTipus() != 'S')
                              {
                                    ValtozoBajtjaiaitMemoriabaKiir(elem.Tartalom, elem.Kezdo_index);
                              }
                        }
                        catch (Memoria_TipusNemEgyezikException)
                        {
                              throw new Memoria_TipusNemEgyezikException();
                        }
                        catch (Exception)
                        {
                              throw new Exception("Hiba az új érték beállításánál!");
                        }
                  }
                  else
                  {
                        throw new Memoria_NincsIlyenElemExceptoin();
                  }
            }

            public void SetValue(string nev, int index, string uj)
            {
                  ListaElem<Valtozo> elem = KeresValtozo(nev, MelyikProgram);
                  if (elem != null)
                  {
                        try
                        {
                              if (!elem.Tomb_e)
                              {
                                    throw new Memoria_AzElemNemTombException();
                              }
                              if (index >= 0 && index <= elem.Tomb_index)
                              {
                                    for (int i = 0; i < index - 1; i++)
                                    {
                                          elem = elem.Kovetkezo;
                                    }
                                    switch (elem.Tartalom.GetTipus())
                                    {
                                          case 'B':
                                                (elem.Tartalom as Bajt).Ertek = Convert.ToByte(uj);
                                                break;
                                          case 'K':
                                                (elem.Tartalom as Karakter).Ertek = Convert.ToChar(uj);
                                                break;
                                          case 'E':
                                                (elem.Tartalom as Egesz).Ertek = Convert.ToInt32(uj);
                                                break;
                                          case 'T':
                                                (elem.Tartalom as Tort).Ertek = Convert.ToDouble(uj);
                                                break;
                                          case 'S':
                                                if ((elem.Tartalom as Szoveg).Ertek.Length == uj.Length)
                                                {
                                                      (elem.Tartalom as Szoveg).Ertek = uj;
                                                }
                                                else
                                                {
                                                      throw new Exception("Szoveg tömb esetében csak azonos hosszúságú új érték adható meg!");
                                                }
                                                break;
                                          default:
                                                throw new Exception("Nem azonosított típus!");
                                    }
                                    if (elem.Tartalom.GetTipus() != 'S')
                                    {
                                          ValtozoBajtjaiaitMemoriabaKiir(elem.Tartalom, elem.Kezdo_index);
                                    }
                              }
                              else
                              {
                                    throw new Memoria_TombIndexHataronKivulEsikException();
                              }
                        }
                        catch (Memoria_TipusNemEgyezikException)
                        {
                              throw new Memoria_TipusNemEgyezikException();
                        }
                        catch (Exception err)
                        {
                              throw new Exception("Hiba az új érték beállításánál! " + err.Message);
                        }
                  }
                  else
                  {
                        throw new Memoria_NincsIlyenElemExceptoin();
                  }
            }

            public void SetVariable(Valtozo v)
            {
                  ListaElem<Valtozo> elem = KeresValtozo(v.Nev, MelyikProgram);
                  if (elem == null)
                  {
                        ListaElem<Valtozo> hely = ElsoSzabadHely(v.Meret);
                        if (hely != null)
                        {
                              foglalt.Add(v, v.Nev, v.Meret, hely.Kezdo_index, false, 0, MelyikProgram);
                              ValtozoBajtjaiaitMemoriabaKiir(v, hely.Kezdo_index);
                              hely.Kezdo_index = hely.Kezdo_index + v.Meret;
                              hely.Hossz = hely.Hossz - v.Meret;
                              if (hely.Hossz == 0)
                              {
                                    szabad.Del(hely.Nev, "");
                              }
                        }
                        else
                        {
                              throw new Memoria_KifutottATartomanybolException();
                        }
                  }
                  else
                  {
                        throw new Memoria_VanMarIilyenValtozoEzenProgramnalException();
                  }
            }

            public void SetVariable(string nev, Valtozo[] v)
            {
                  ListaElem<Valtozo> elem = KeresValtozo(nev, MelyikProgram);
                  if (elem == null)
                  {
                        int OsszMeret = 0;
                        for (int i = 0; i < v.Length; i++)
                        {
                              OsszMeret = OsszMeret + v[i].Meret;
                        }
                        ListaElem<Valtozo> tmp = ElsoSzabadHely(OsszMeret);
                        if (tmp != null)
                        {
                              v[0].Nev = nev;
                              ListaElem<Valtozo> hely = ElsoSzabadHely(v[0].Meret);
                              foglalt.Add(v[0], nev, v[0].Meret, hely.Kezdo_index, true, v.Length, MelyikProgram);
                              ValtozoBajtjaiaitMemoriabaKiir(v[0], hely.Kezdo_index);
                              hely.Kezdo_index = hely.Kezdo_index + v[0].Meret;
                              hely.Hossz = hely.Hossz - v[0].Meret;
                              if (hely.Hossz == 0)
                              {
                                    szabad.Del(hely.Nev, "");
                              }

                              for (int i = 1; i < v.Length; i++)
                              {
                                    hely = ElsoSzabadHely(v[i].Meret);
                                    if (hely != null)
                                    {
                                          string sv = string.Format("{0}[{1}]", nev, i);
                                          v[i].Nev = sv;
                                          foglalt.Add(v[i], sv, v[i].Meret, hely.Kezdo_index, false, 0, MelyikProgram);
                                          ValtozoBajtjaiaitMemoriabaKiir(v[i], hely.Kezdo_index);
                                          hely.Kezdo_index = hely.Kezdo_index + v[i].Meret;
                                          hely.Hossz = hely.Hossz - v[i].Meret;
                                          if (hely.Hossz == 0)
                                          {
                                                szabad.Del(hely.Nev, "");
                                          }
                                    }
                              }
                        }
                        else
                        {
                              throw new Memoria_KifutottATartomanybolException();
                        }
                  }
                  else
                  {
                        throw new Memoria_VanMarIilyenValtozoEzenProgramnalException();
                  }
            }

            public Valtozo GetVariable(string nev)
            {
                  ListaElem<Valtozo> elem = KeresValtozo(nev, MelyikProgram);
                  if (elem != null)
                  {
                        if (!elem.Tomb_e)
                        {
                              return elem.Tartalom;
                        }
                        else
                        {
                              throw new Memoria_AzElemTombException();
                        }
                  }
                  else
                  {
                        throw new Memoria_NincsIlyenElemExceptoin();
                  }
            }

            public Valtozo GetVariable(string nev, int index)
            {
                  ListaElem<Valtozo> elem = KeresValtozo(nev, MelyikProgram);
                  if (elem != null)
                  {
                        if (elem.Tomb_e)
                        {
                              if (index >= 0 && index <= elem.Tomb_index)
                              {
                                    for (int i = 0; i < index - 1; i++)
                                    {
                                          elem = elem.Kovetkezo;
                                    }
                                    return elem.Tartalom;
                              }
                              else
                              {
                                    throw new Memoria_TombIndexHataronKivulEsikException();
                              }
                        }
                        else
                        {
                              throw new Memoria_AzElemNemTombException();
                        }
                  }
                  else
                  {
                        throw new Memoria_NincsIlyenElemExceptoin();
                  }
            }

            public void DelVariable(string nev)
            {
                  string sv = "", nev_sv = "";
                  ListaElem<Valtozo> elem = KeresValtozo(nev, MelyikProgram);
                  if (elem != null)
                  {
                        if (!elem.Tomb_e)
                        {
                              sv = "del" + (delete_db++).ToString();
                              szabad.Add(new Bajt(sv, 0), sv, elem.Hossz, elem.Kezdo_index);
                              ValtozoBajtjaiaitMemoriabanNullaz(elem.Kezdo_index, elem.Hossz);
                              foglalt.Del(nev, MelyikProgram);
                        }
                        else
                        {
                              int db = elem.Tomb_index;

                              sv = "del" + (delete_db++).ToString();
                              szabad.Add(new Bajt(sv, 0), sv, elem.Hossz, elem.Kezdo_index);
                              ValtozoBajtjaiaitMemoriabanNullaz(elem.Kezdo_index, elem.Hossz);
                              foglalt.Del(nev, MelyikProgram);

                              for (int i = 1; i < db; i++)
                              {
                                    nev_sv = string.Format("{0}[{1}]", nev, i);
                                    elem = KeresValtozo(nev_sv, MelyikProgram);
                                    sv = "del" + (delete_db++).ToString();
                                    szabad.Add(new Bajt(sv, 0), sv, elem.Hossz, elem.Kezdo_index);
                                    ValtozoBajtjaiaitMemoriabanNullaz(elem.Kezdo_index, elem.Hossz);
                                    foglalt.Del(nev_sv, MelyikProgram);
                              }
                        }
                  }
                  else
                  {
                        throw new Memoria_NincsIlyenElemExceptoin();
                  }
            }

            public void Clean()
            {
                  szabad.DelAll();
                  MemInit();

                  int index = 0;
                  ListaElem<Valtozo> tmp = foglalt.Fej;
                  while (tmp != null)
                  {
                        tmp.Kezdo_index = index;
                        ValtozoBajtjaiaitMemoriabaKiir(tmp.Tartalom, index);

                        index = index + tmp.Hossz;
                        tmp = tmp.Kovetkezo;
                  }
                  szabad.Fej.Kezdo_index = index;
                  szabad.Fej.Hossz = kapacitas - index;
            }

            public void MemoriaDump()
            {
                  MemoriaDump(1, kapacitas);
            }

            public void MemoriaDump(int darab)
            {
                  MemoriaDump(1, darab);
            }

            public void MemoriaDump(int kezd, int darab)
            {
                  kezd--;
                  ConsoleColor sv = Console.ForegroundColor;
                  Console.ForegroundColor = ConsoleColor.White;
                  Console.WriteLine("Memória állapota, kezdő cím: {0}, hossza: {1} bájt", kezd, darab);
                  int y = Console.CursorTop + 1;
                  if (kezd >= 0 && kezd + darab <= kapacitas)
                  {
                        for (int i = kezd; i < kezd + darab; i++)
                        {
                              Console.Write("{0,-4}", mem[i]);
                        }

                        ConsoleColor szin = (ConsoleColor)1;
                        ListaElem<Valtozo> p = foglalt.Fej;
                        while (p != null)
                        {
                              if (p.Kezdo_index >= kezd && p.Kezdo_index <= kezd + darab)
                              {
                                    Console.ForegroundColor = szin;
                                    if (p.Tomb_e)
                                    {
                                          char tomb_tipus = p.Tartalom.GetTipus();
                                          string tomb_neve = p.Nev;
                                          string tomb_program = p.Program;
                                          int tomb_db = p.Tomb_index;
                                          int tomb_kezdo_index = p.Kezdo_index;
                                          for (int k = 0; k < tomb_db && p != null; k++)
                                          {
                                                List<byte> adat = p.Tartalom.GetAdatokBajtTombben();
                                                for (int i = 0; i < adat.Count && i <= kezd + darab; i++)
                                                {
                                                      Console.SetCursorPosition(4 * (p.Kezdo_index + i), y);
                                                      Console.Write("{0,-4}", adat[i]);
                                                }
                                                Console.SetCursorPosition(4 * p.Kezdo_index, y + 3);
                                                Console.Write("{0}", p.Tartalom.GetErtek());
                                                p = p.Kovetkezo;
                                          }
                                          Console.SetCursorPosition(4 * tomb_kezdo_index, y + 1);
                                          string s = tomb_neve;
                                          if (s.Length > 4)
                                          {
                                                s = s.Substring(0, 4);
                                          }
                                          Console.Write("{0}", s);
                                          Console.SetCursorPosition(4 * tomb_kezdo_index, y + 2);
                                          Console.Write("A[{0}]", tomb_tipus);
                                          Console.SetCursorPosition(4 * tomb_kezdo_index, y + 4);
                                          Console.Write("{0}", tomb_program);
                                          szin++;
                                          if (szin == (ConsoleColor)15)
                                          {
                                                szin = (ConsoleColor)1;
                                          }
                                    }
                                    else
                                    {
                                          List<byte> adat = p.Tartalom.GetAdatokBajtTombben();
                                          for (int i = 0; i < adat.Count && i <= kezd + darab; i++)
                                          {
                                                Console.SetCursorPosition(4 * (p.Kezdo_index + i), y);
                                                Console.Write("{0,-4}", adat[i]);
                                          }
                                          Console.SetCursorPosition(4 * p.Kezdo_index, y + 1);
                                          string s = p.Nev;
                                          if (s.Length > 4)
                                          {
                                                s = s.Substring(0, 4);
                                          }
                                          Console.Write("{0}", s);
                                          Console.SetCursorPosition(4 * p.Kezdo_index, y + 2);
                                          Console.Write("{0}", p.Tartalom.GetTipus());
                                          Console.SetCursorPosition(4 * p.Kezdo_index, y + 3);
                                          Console.Write("{0}", p.Tartalom.GetErtek());
                                          Console.SetCursorPosition(4 * p.Kezdo_index, y + 4);
                                          Console.Write("{0}", p.Program);
                                          szin++;
                                          if (szin == (ConsoleColor)15)
                                          {
                                                szin = (ConsoleColor)1;
                                          }
                                          p = p.Kovetkezo;
                                    }
                              }
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.ForegroundColor = sv;
                  }
                  else
                  {
                        throw new Exception("Memória lekérdezés a tartományon kívülre mutat!");
                  }
            }
      }
}