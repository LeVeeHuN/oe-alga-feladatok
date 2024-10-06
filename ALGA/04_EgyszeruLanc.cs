using System.Collections;

namespace OE.ALGA.Adatszerkezetek;

public class LancElem<T>
{
    public T tart;
    public LancElem<T>? kov;

    public LancElem(T tart, LancElem<T>? kov)
    {
        this.tart = tart;
        this.kov = kov;
    }
}

public class LancoltVerem<T> : Verem<T>
{
    private LancElem<T>? fej;

    public LancoltVerem()
    {
        fej = null;
    }

    public bool Ures => fej == null;

    public void Verembe(T ertek)
    {
        fej = new LancElem<T>(ertek, fej);
    }

    public T Verembol()
    {
        if (Ures) throw new NincsElemKivetel();
        
        T ertek = fej.tart;
        fej = fej.kov;
        return ertek;
    }

    public T Felso()
    {
        if (Ures) throw new NincsElemKivetel();
        
        return fej.tart;
    }
}

public class LancoltSor<T> : Sor<T>
{
    private LancElem<T>? fej;
    private LancElem<T>? vege;

    public LancoltSor()
    {
        fej = null;
        vege = null;
    }
    
    public bool Ures => fej == null;
    public void Sorba(T ertek)
    {
        if (Ures)
        {
            fej = new LancElem<T>(ertek, null);
            vege = fej;
        }
        else
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            vege.kov = uj;
            vege = uj;
        }
    }

    public T Sorbol()
    {
        if (Ures) throw new NincsElemKivetel();
        
        T ertek = fej.tart;
        fej = fej.kov;
        return ertek;
    }

    public T Elso()
    {
        if (Ures) throw new NincsElemKivetel();
        
        return fej.tart;
    }
}

public class LancoltLista<T> : Lista<T>, IEnumerable<T>
{
    private LancElem<T>? fej = null;
    private int n = 0;

    public int Elemszam => n;
    public T Kiolvas(int index)
    {
        // Index ellenőrzés
        if (index < 0 || index >= n) throw new HibasIndexKivetel();

        // Eljárás a keresett elemig
        LancElem<T> keresett = fej;
        for (int i = 0; i < index; i++)
        {
            keresett = keresett.kov;
        }
        return keresett.tart;
    }

    public void Modosit(int index, T ertek)
    {
        // Index ellenőrzés
        if (index < 0 || index >= n) throw new HibasIndexKivetel();
        
        // Eljárás a keresett elemig
        LancElem<T> keresett = fej;
        for (int i = 0; i < index; i++)
        {
            keresett = keresett.kov;
        }
        keresett.tart = ertek;
    }

    public void Hozzafuz(T ertek)
    {
        if (n == 0) fej = new LancElem<T>(ertek, null);
        else
        {
            LancElem<T> keresett = fej;
            for (int i = 1; i < n; i++)
            {
                keresett = keresett.kov;
            }
            keresett.kov = new LancElem<T>(ertek, null);
        }
        n++;
    }

    public void Beszur(int index, T ertek)
    {
        // Index ellenőrzés
        if (index < 0 || index > n) throw new HibasIndexKivetel();

        if (index == 0)
        {
            LancElem<T> uj = new LancElem<T>(ertek, fej);
            fej = uj;
        }
        else if (index == n)
        {
            LancElem<T> keresett = fej;
            for (int i = 0; i < n-1; i++)
            {
                keresett = keresett.kov;
            }
            keresett.kov = new LancElem<T>(ertek, null);
        }
        else
        {
            LancElem<T> keresett = fej;
            for (int i = 0; i < index - 1; i++)
            {
                keresett = keresett.kov;
            }

            LancElem<T> tmp = keresett.kov;
            keresett.kov = new LancElem<T>(ertek, tmp);
        }

        n++;


    }

    public void Torol(T ertek)
    {
        LancElem<T>? utolsoJo = null;
        LancElem<T> keresett = fej;
        for (int i = 0; i < n; i++)
        {
            if (keresett.tart.Equals(ertek))
            {
                if (utolsoJo == null)
                {
                    fej = keresett.kov;
                }
                else
                {
                    utolsoJo.kov = keresett.kov;
                }
            }
            else
            {
                utolsoJo = keresett;
            }
            keresett = keresett.kov;
        }
    }

    private T[] Tombbe()
    {
        T[] lista = new T[n];
        LancElem<T> keresett = fej;
        for (int i = 0; i < n; i++)
        {
            lista[i] = keresett.tart;
            keresett = keresett.kov;
        }
        return lista;
    }

    public void Bejar(Action<T> muvelet)
    {
        foreach (T item in this)
        {
            muvelet(item);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new LancoltListaBejaro<T>(Tombbe());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class LancoltListaBejaro<T> : IEnumerator<T>
{
    private T[] tarolo;
    private int index = -1;

    public LancoltListaBejaro(T[] tarolo)
    {
        this.tarolo = tarolo;
    }
    
    public bool MoveNext()
    {
        index++;
        if (index >= tarolo.Length) return false;
        return true;
    }

    public void Reset()
    {
        index = -1;
    }

    public T Current => tarolo[index];

    object? IEnumerator.Current => Current;

    public void Dispose()
    {
        
    }
}