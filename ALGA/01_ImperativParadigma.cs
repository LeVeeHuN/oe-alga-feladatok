using System.Collections;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        public void Vegrehajtas();
    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }




    public class TaroloMegteltKivetel : Exception
    {

    }



    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        protected int n;

        public FeladatTarolo(uint meret)
        {
            tarolo = new T[meret];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            if (n == tarolo.Length) throw new TaroloMegteltKivetel();
            tarolo[n] = elem;
            n++;
        }

        public virtual void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new FeladatTaroloBejaro<T>(tarolo, n);
        }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(uint meret) : base(meret) { }

        public override void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul) tarolo[i].Vegrehajtas();
            }
        }
    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        private int jelenlegiIndex;
        private int elemekSzama;
        T[] tarolo;

        public FeladatTaroloBejaro(T[] tarolo, int elemekSzama)
        {
            this.tarolo = tarolo;
            this.elemekSzama = elemekSzama;
            Reset();
        }

        public bool MoveNext()
        {
            jelenlegiIndex++;
            if (jelenlegiIndex >= elemekSzama) return false;
            return true;
        }

        public void Reset()
        {
            jelenlegiIndex = -1;
        }

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public T Current
        {
            get
            {
                return tarolo[jelenlegiIndex];
            }
        }
    }
}