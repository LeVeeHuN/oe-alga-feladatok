using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public Predicate<T> BejaroFeltetel { get; set; }

        public FeltetelesFeladatTarolo(uint meret) : base(meret) { }

        public void FeltetelesVegrehajtas(Predicate<T> feltetel)
        {
            //for (int i = 0; i < n; i++)
            //{
            //    if (feltetel(tarolo[i])) tarolo[i].Vegrehajtas();
            //}

            foreach (T elem in this)
            {
                if (feltetel(elem)) elem.Vegrehajtas();
            }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            if (BejaroFeltetel == null) return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, (x) => true);
            return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel);
        }
    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        private int index;
        private int elemekSzama;
        private T[] tarolo;
        private Predicate<T> feltetel;

        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int elemekSzama, Predicate<T> feltetel)
        {
            this.tarolo = tarolo;
            this.elemekSzama = elemekSzama;
            this.feltetel = feltetel;
            Reset();
        }


        public T Current => tarolo[index];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            index++;
            // Elfogytak az elemek
            if (index >= elemekSzama) return false;
            // Átugorjuk az elemet, mert nem teljesíti a feltételt
            if (!feltetel(tarolo[index])) MoveNext();
            return true;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}
