using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public SulyozottEgeszGrafEl(int Honnan, int Hova, float suly) : base(Honnan, Hova)
        {
            Suly = suly;
        }

        public float Suly { get; private set; }
    }


    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        int n;
        float[,] M;

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = float.NaN;
                }
            }
        }
        public int CsucsokSzama { get { return n; } }

        public int ElekSzama => M.Cast<float>().Count(x => !float.IsNaN(x));


        public Halmaz<int> Csucsok
        {
            get
            {
                Halmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                Halmaz<SulyozottEgeszGrafEl> elek = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j]))
                        {
                            elek.Beszur(new SulyozottEgeszGrafEl(i, j, Suly(i, j)));
                        }
                    }
                }
                return elek;
            }
        }

        public float Suly(int honnan, int hova)
        {
            if (!float.IsNaN(M[honnan, hova]))
                return M[honnan, hova];
            else
                throw new NincsElKivetel();
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();

            Elek.Bejar(x =>
            {
                if (x.Honnan == csucs)
                {
                    szomszedok.Beszur(x.Hova);
                }
            });

            return szomszedok;
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return !float.IsNaN(M[honnan, hova]);
        }
    }
    public class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> graf, V start)
        {
            Szotar<V, float> L = new HasitoSzotarTulcsordulasiTerulettel<V, float>(graf.CsucsokSzama);
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(graf.CsucsokSzama);
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(graf.CsucsokSzama, (ez, ennel) => L.Kiolvas(ez) < L.Kiolvas(ennel));

            graf.Csucsok.Bejar(x =>
            {
                L.Beir(x, float.MaxValue);
                S.Sorba(x);
            });

            L.Beir(start, 0);
            S.Frissit(start);

            while (!S.Ures)
            {
                V u = S.Sorbol();

                graf.Szomszedai(u).Bejar((x) =>
                {
                    if (L.Kiolvas(u) + graf.Suly(u, x) < L.Kiolvas(x))
                    {
                        L.Beir(x, L.Kiolvas(u) + graf.Suly(u, x));
                        S.Frissit(x);
                        P.Beir(x, u);
                    }
                });
            }

            return (L);
        }
    }


    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> graf, V start) where V : IComparable<V>
        {
            var K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(graf.CsucsokSzama);
            var P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(graf.CsucsokSzama);
            var S = new FaHalmaz<V>();

            var prioritasosSor = new KupacPrioritasosSor<V>(graf.CsucsokSzama,
                (a, b) => K.Kiolvas(a) < K.Kiolvas(b));

            graf.Csucsok.Bejar(x =>
            {
                K.Beir(x, float.PositiveInfinity);
                S.Beszur(x);
                prioritasosSor.Sorba(x);
            });

            K.Beir(start, 0);
            prioritasosSor.Frissit(start);

            while (!prioritasosSor.Ures)
            {
                var u = prioritasosSor.Sorbol();
                S.Torol(u);

                graf.Szomszedai(u).Bejar(x =>
                {
                    if (S.Eleme(x) && graf.Suly(u, x) < K.Kiolvas(x))
                    {
                        K.Beir(x, graf.Suly(u, x));
                        P.Beir(x, u);
                        prioritasosSor.Frissit(x);
                    }
                });
            }

            return P;
        }
        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> graf) where E : SulyozottGrafEl<V>, IComparable<E>
        {
            var A = new FaHalmaz<E>();
            var szulok = new HasitoSzotarTulcsordulasiTerulettel<V, V>(graf.CsucsokSzama);
            var rangok = new HasitoSzotarTulcsordulasiTerulettel<V, int>(graf.CsucsokSzama);

            graf.Csucsok.Bejar(csucs =>
            {
                szulok.Beir(csucs, csucs);
                rangok.Beir(csucs, 0);
            });

            var rendezettElek = new List<E>();
            var feldolgozottElek = new HashSet<(V, V)>();

            graf.Elek.Bejar(el =>
            {
                var u = el.Honnan;
                var v = el.Hova;

                if (!feldolgozottElek.Contains((u, v)) && !feldolgozottElek.Contains((v, u)))
                {
                    rendezettElek.Add(el);
                    feldolgozottElek.Add((u, v));
                }
            });
            rendezettElek.Sort((a, b) => a.Suly.CompareTo(b.Suly));

            V Find(V x)
            {
                if (!szulok.Kiolvas(x).Equals(x))
                {
                    szulok.Beir(x, Find(szulok.Kiolvas(x)));
                }
                return szulok.Kiolvas(x);
            }

            void Union(V x, V y)
            {
                V xRoot = Find(x);
                V yRoot = Find(y);

                if (xRoot.Equals(yRoot)) return;

                if (rangok.Kiolvas(xRoot) < rangok.Kiolvas(yRoot))
                {
                    szulok.Beir(xRoot, yRoot);
                }
                else if (rangok.Kiolvas(xRoot) > rangok.Kiolvas(yRoot))
                {
                    szulok.Beir(yRoot, xRoot);
                }
                else
                {
                    szulok.Beir(yRoot, xRoot);
                    rangok.Beir(xRoot, rangok.Kiolvas(xRoot) + 1);
                }
            }

            foreach (var el in rendezettElek)
            {
                if (!Find(el.Honnan).Equals(Find(el.Hova)))
                {
                    A.Beszur(el);
                    Union(el.Honnan, el.Hova);
                }
            }

            return A;
        }
    }
}
