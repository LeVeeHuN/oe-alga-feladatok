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
            foreach (T elem in tarolo)
            {
                if (elem != null) elem.Vegrehajtas();
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
            foreach(T elem in tarolo)
            {
                if (elem != null && elem.FuggosegTeljesul) elem.Vegrehajtas();
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
            if (jelenlegiIndex >= elemekSzama) return false;
            jelenlegiIndex++;
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