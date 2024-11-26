namespace OE.ALGA.Adatszerkezetek;

public class EgeszGrafEl : GrafEl<int>, IComparable<EgeszGrafEl>
{
    public int Honnan { get; }
    public int Hova { get; }

    public EgeszGrafEl(int honnan, int hova)
    {
        Honnan = honnan;
        Hova = hova;
    }

    public int CompareTo(EgeszGrafEl? other)
    {
        // null argument kezelése
        if (other is null) throw new ArgumentNullException(nameof(other));

        if (Honnan == other.Honnan)
        {
            return Hova.CompareTo(other.Hova);
        }
        
        return Honnan.CompareTo(other.Honnan);
    }
}

public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
{
    private int n;
    private bool[,] M;
    
    public int CsucsokSzama
    {
        get => n;
    }
    public int ElekSzama
    {
        get
        {
            int c = 0;
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    if (M[i, j]) c++;
                }
            }

            return c;
        }
    }

    public Halmaz<int> Csucsok
    {
        get
        {
            FaHalmaz<int> csucsok = new FaHalmaz<int>();
            for (int i = 0; i < CsucsokSzama; i++) csucsok.Beszur((int)i);
            return csucsok;
        }
    }

    public Halmaz<EgeszGrafEl> Elek
    {
        get
        {
            FaHalmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    if (M[i, j]) elek.Beszur(new EgeszGrafEl((int)i, (int)j));
                }
            }

            return elek;
        }
    }

    public CsucsmatrixSulyozatlanEgeszGraf(int csucsokSzama)
    {
        n = csucsokSzama;
        M = new bool[n, n];
    }
    
    public bool VezetEl(int honnan, int hova)
    {
        return M[honnan, hova];
    }

    public Halmaz<int> Szomszedai(int csucs)
    {
        FaHalmaz<int> szomszedai = new FaHalmaz<int>();
        for (int i = 0; i < M.GetLength(1); i++)
        {
            if (M[csucs, i]) szomszedai.Beszur(i);
        }
        return szomszedai;
    }

    public void UjEl(int honnan, int hova)
    {
        M[honnan, hova] = true;
    }
}

public class GrafBejarasok
{
    public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
    {
        Sor<V> S = new LancoltSor<V>();
        S.Sorba(start);
        Halmaz<V> F = new FaHalmaz<V>();
        F.Beszur(start);

        while (!S.Ures)
        {
            V k = S.Sorbol();
            muvelet(k);
            g.Szomszedai(k).Bejar((e) =>
            {
                if (!F.Eleme(e))
                {
                    S.Sorba(e);
                    F.Beszur(e);
                }
            });
        }

        return F;
    }

    public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
    {
        FaHalmaz<V> F = new FaHalmaz<V>();
        MelysegiBejarasRekurzio(g, start, muvelet, F);
        return F;
    }

    private static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Action<V> muvelet, Halmaz<V> F) where V : IComparable<V>
    {
        F.Beszur(k);
        muvelet(k);
        g.Szomszedai(k).Bejar((k) =>
        {
            if (!F.Eleme(k))
            {
                MelysegiBejarasRekurzio<V, E>(g, k, muvelet, F);
            }
        });
    }
}