using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memoria_2020_04_17
{
      class Valtozo
      {
            protected string nev;
            public string Nev { get => nev; set => nev = value; }

            protected int meret; // 1MB memória esetén ha 1MB-os Szoveg, akkor a Meret milliós érték
            public int Meret { get => meret; }

            public Valtozo(string Nev)
            {
                  nev = Nev;
            }

            public virtual List<byte> GetAdatokBajtTombben()
            {
                  throw new NotImplementedException();
            }

            public virtual string GetErtek()
            {
                  throw new NotImplementedException();
            }

            public virtual char GetTipus()
            {
                  throw new NotImplementedException();
            }

            public override string ToString()
            {
                  string sv = "";
                  sv += "Név: " + Nev + "\n";
                  sv += "Érték: " + GetErtek() + "\n";
                  sv += "Mérete: " + Meret + " bájt\n";
                  sv += "Érték bájtokban: ";
                  List<byte> adat = GetAdatokBajtTombben();
                  for (int i = 0; i < adat.Count; i++)
                  {
                        sv += adat[i] + " ";
                  }
                  return sv;
            }
      }

      class Egesz : Valtozo
      {
            private int ertek;
            public int Ertek { get => ertek; set => ertek = value; }

            public Egesz(string Nev, int Ertek) : base(Nev)
            {
                  this.Ertek = Ertek;
                  this.meret = sizeof(int);
            }

            public override string GetErtek()
            {
                  return Ertek.ToString();
            }

            public override char GetTipus()
            {
                  return 'E';
            }

            public override List<byte> GetAdatokBajtTombben()
            {
                  List<byte> adat = new List<byte>();
                  unsafe
                  {
                        int sv = Ertek;
                        byte* p = (byte*)&sv;
                        for (int i = 0; i < Meret; i++)
                        {
                              adat.Add(*p);
                              p++;
                        }
                  }
                  return adat;
            }
      }

      class Bajt : Valtozo
      {
            private byte ertek;
            public byte Ertek { get => ertek; set => ertek = value; }

            public Bajt(string Nev, byte Ertek) : base(Nev)
            {
                  this.Ertek = Ertek;
                  this.meret = sizeof(byte);
            }

            public override string GetErtek()
            {
                  return Ertek.ToString();
            }

            public override char GetTipus()
            {
                  return 'B';
            }

            public override List<byte> GetAdatokBajtTombben()
            {
                  List<byte> adat = new List<byte>();
                  adat.Add(Ertek);
                  return adat;
            }
      }

      class Karakter : Valtozo
      {
            private char ertek;
            public char Ertek { get => ertek; set => ertek = value; }

            public Karakter(string Nev, char Ertek) : base(Nev)
            {
                  this.Ertek = Ertek;
                  this.meret = sizeof(char);
            }

            public override string GetErtek()
            {
                  return Ertek.ToString();
            }

            public override char GetTipus()
            {
                  return 'K';
            }

            public override List<byte> GetAdatokBajtTombben()
            {
                  List<byte> adat = new List<byte>();
                  unsafe
                  {
                        char sv = Ertek;
                        byte* p = (byte*)&sv;
                        for (int i = 0; i < Meret; i++)
                        {
                              adat.Add(*p);
                              p++;
                        }
                  }
                  return adat;
            }
      }

      class Tort : Valtozo
      {
            private double ertek;
            public double Ertek { get => ertek; set => ertek = value; }

            public Tort(string Nev, double Ertek) : base(Nev)
            {
                  this.Ertek = Ertek;
                  this.meret = sizeof(double);
            }

            public override string GetErtek()
            {
                  return Ertek.ToString();
            }

            public override char GetTipus()
            {
                  return 'T';
            }

            public override List<byte> GetAdatokBajtTombben()
            {
                  List<byte> adat = new List<byte>();
                  unsafe
                  {
                        double sv = Ertek;
                        byte* p = (byte*)&sv;
                        for (int i = 0; i < Meret; i++)
                        {
                              adat.Add(*p);
                              p++;
                        }
                  }
                  return adat;
            }
      }

      class Szoveg : Valtozo
      {
            private string ertek;
            public string Ertek { get => ertek; set => ertek = value; }

            public Szoveg(string Nev, string Ertek) : base(Nev)
            {
                  this.Ertek = Ertek;
                  this.meret = Ertek.Length + 1;
            }

            public override string GetErtek()
            {
                  return Ertek.ToString();
            }

            public override char GetTipus()
            {
                  return 'S';
            }

            public override List<byte> GetAdatokBajtTombben()
            {
                  List<byte> adat = new List<byte>();
                  for (int i = 0; i < Ertek.Length; i++)
                  {
                        adat.Add(Convert.ToByte(Ertek[i]));
                  }
                  adat.Add(Convert.ToByte('\0'));
                  return adat;
            }
      }
}
