namespace OE.ALGA.Paradigmak
{
    interface IVegrehajto
    {
        public void Vegrehajtas();
    }

    interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }




    class TaroloMegteltKivetel : Exception
    {

    }



    class FeladatTarolo<T> where T : IVegrehajto
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
                elem.Vegrehajtas();
            }
        }
    }

    class FuggoFeladatTarolo<T> : FeladatTarolo<T>
        where T : IVegrehajto, IFuggo
    {
        public FuggoFeladatTarolo(uint meret) : base(meret) { }

        public override void MindentVegrehajt()
        {
            foreach(T elem in tarolo)
            {
                if (elem.FuggosegTeljesul) elem.Vegrehajtas();
            }
        }

    }

    // Utolsó feladat maradt hátra
}