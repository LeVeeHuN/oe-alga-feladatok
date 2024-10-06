using System.Collections;

namespace OE.ALGA.Adatszerkezetek;

public class TombVerem<T> : Verem<T>
{
    private T[] adatok;
    private int nextIndex = 0;

    public TombVerem(int meret)
    {
        adatok = new T[meret];
    }

    public bool Ures
    {
        get
        {
            return nextIndex == 0;
        }
    }

    public void Verembe(T ertek)
    {
        if (nextIndex >= adatok.Length)
        {
            throw new NincsHelyKivetel();
        }
        adatok[nextIndex] = ertek;
        nextIndex++;
    }

    public T Verembol()
    {
        if (Ures)
        {
            throw new NincsElemKivetel();
        }
        // defaultra is állíthatnám, de indexxel tartom számon
        // adatok[nextIndex - 1] = default(T)
        return adatok[--nextIndex];
    }

    public T Felso()
    {
        if (Ures)
        {
            throw new NincsElemKivetel();
        }
        return adatok[nextIndex - 1];
    }
}

public class TombSor<T> : Sor<T>
{
    private T[] adatok;
    private int nextIndex = 0;

    public TombSor(int meret)
    {
        adatok = new T[meret];
    }
    
    public bool Ures => nextIndex == 0;
    public void Sorba(T ertek)
    {
        if (nextIndex == adatok.Length) throw new NincsHelyKivetel();
        adatok[nextIndex] = ertek;
        nextIndex++;
    }

    public T Sorbol()
    {
        if (Ures) throw new NincsElemKivetel();
        T elem = adatok[0];
        for (int i = 0; i < adatok.Length - 1; i++)
        {
            adatok[i] = adatok[i + 1];
        }
        nextIndex--;
        return elem;
    }

    public T Elso()
    {
        if (Ures) throw new NincsElemKivetel();
        return adatok[0];
    }
    
    
    
}

public class TombLista<T> : Lista<T>, IEnumerable<T>
{
    private T[] tarolo;
    private int n = 0;

    public int Elemszam
    {
        get
        {
            return n;
        }
    }

    public TombLista(int meret)
    {
        tarolo = new T[meret];
    }

    public TombLista()
    {
        tarolo = new T[32];
    }

    private void MeretNovel()
    {
        T[] ujTarolo = new T[tarolo.Length * 2];
        for (int i = 0; i < tarolo.Length; i++)
        {
            ujTarolo[i] = tarolo[i];
        }
        tarolo = ujTarolo;
    }
    
    public T Kiolvas(int index)
    {
        // Index ellenőrzés
        if (index >= n || index < 0) throw new HibasIndexKivetel();
        
        // Érték visszaadása
        return tarolo[index];
    }

    public void Modosit(int index, T ertek)
    {
        // Index ellenőrzés
        if (index >= n || index < 0) throw new HibasIndexKivetel();
        
        // Érték módosítása
        tarolo[index] = ertek;
    }

    public void Hozzafuz(T ertek)
    {
        // Méret ellenőrzése
        if (n >= tarolo.Length) MeretNovel();
        
        // Elem hozzáfűzése és elemszám növelése
        tarolo[n++] = ertek;
    }

    public void Beszur(int index, T ertek)
    {
        // Index ellenőrzés
        if (index > n || index < 0) throw new HibasIndexKivetel();
        
        // Méret ellenőrzése
        if (n >= tarolo.Length) MeretNovel();
        
        // Minden elem arrébköltöztetése és érték beszúrása
        for (int i = n - 1; i >= index; i--)
        {
            tarolo[i + 1] = tarolo[i];
        }
        tarolo[index] = ertek;
        n++;
    }

    public void Torol(T ertek)
    {
        // Hátulról megyek
        for (int i = n - 1; i >= 0; i--)
        {
            // Ha találtam egyező elemet
            if (tarolo[i].Equals(ertek))
            {
                // Az egyező elem az i-ik indexen van
                // Tőle jobbra nincs olyan elem, amely egyenlő vele
                
                // Leellenőrzöm, hogy nem ő-e az utolsó elem (ha ő az utolsó, akkor csak csökkentem n-t.
                if (i != n - 1)
                {
                    // Minden elemet tőle jobbra egyel balráb költöztetek
                    for (int j = i; j < n; j++)
                    {
                        tarolo[j] = tarolo[j + 1];
                    }
                }

                n--;
            }
        }
    }

    public void Bejar(Action<T> muvelet)
    {
        foreach (T elem in this)
        {
            muvelet(elem);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new TombListaBejaro<T>(tarolo, n);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class TombListaBejaro<T> : IEnumerator<T>
{
    private T[] tarolo;
    private int index = -1;
    private int n;

    public TombListaBejaro(T[] tarolo, int n)
    {
        this.tarolo = tarolo;
        this.n = n;
    }
    
    public bool MoveNext()
    {
        index++;
        if (index >= n) return false;
        return true;
    }

    public void Reset()
    {
        index = -1;
    }

    public T Current
    {
        get
        {
            return tarolo[index];
        }
    }

    object? IEnumerator.Current => Current;

    public void Dispose()
    {
        
    }
}