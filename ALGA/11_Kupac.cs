namespace OE.ALGA.Adatszerkezetek;

public class Kupac<T>
{
    protected T[] E;
    protected int n;
    protected Func<T, T, bool> nagyobbPrioritas;

    public Kupac(T[] alapTomb, int aktualisElemekSzama, Func<T, T, bool> osszehasonlitoFuggveny)
    {
        E = alapTomb;
        n = aktualisElemekSzama;
        nagyobbPrioritas = osszehasonlitoFuggveny;
        KupacotEpit();
    }

    public static int Bal(int i)
    {
        return 2 * i;
    }

    public static int Jobb(int i)
    {
        return 2 * i + 1;
    }

    public static int Szulo(int i)
    {
        return i / 2;
    }

    protected void Kupacol(int i)
    {
        int b = Bal(i);
        int j = Jobb(i);
        int max = int.MinValue;

        if (b < n && nagyobbPrioritas(E[b], E[i]))
        {
            max = b;
        }
        else
        {
            max = i;
        }

        if (j < n && nagyobbPrioritas(E[j], E[max]))
        {
            max = j;
        }

        if (max != i)
        {
            (E[i], E[max]) = (E[max], E[i]);
            Kupacol(max);
        }
    }

    protected void KupacotEpit()
    {
        for (int i = Szulo(n) - 1; i >= 0; i--)
        {
            Kupacol(i);
        }
    }
}

public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
{
    public KupacRendezes(T[] alapTomb) : base(alapTomb, alapTomb.Length, (x, y) => x.CompareTo(y) > 0)
    {
    }

    public void Rendezes()
    {
        KupacotEpit();
        for (int i = n - 1; i >= 0; i--)
        {
            (E[0], E[i]) = (E[i], E[0]);
            n = n - 1;
            Kupacol(0);
        }
    }
}

public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
{
    public KupacPrioritasosSor(int elemekSzama, Func<T, T, bool> osszehasonlitoFuggveny) : base(new T[elemekSzama], 0, osszehasonlitoFuggveny)
    {
    }

    private void KulcsotFelvisz(int i)
    {
        int sz = Szulo(i);
        if (sz >= 0 && nagyobbPrioritas(E[i], E[sz]))
        {
            (E[sz], E[i]) = (E[i], E[sz]);
            KulcsotFelvisz(sz);
        }
    }

    public bool Ures
    {
        get => n == 0;
    }
    
    public void Sorba(T ertek)
    {
        if (n == E.Length) throw new NincsHelyKivetel();
        E[n] = ertek;
        n++;
        KulcsotFelvisz(n - 1);
    }

    public T Sorbol()
    {
        if (Ures) throw new NincsElemKivetel();
        T elem = E[0];
        E[0] = E[n - 1];
        n--;
        Kupacol(0);
        return elem;
    }

    public T Elso()
    {
        if (Ures) throw new NincsElemKivetel();
        return E[0];
    }

    public void Frissit(T elem)
    {
        int i = 0;
        while (i < n && !E[i].Equals(elem)) i++;
        if (i >= n) throw new NincsElemKivetel();
        KulcsotFelvisz(i);
        Kupacol(i);
    }
}