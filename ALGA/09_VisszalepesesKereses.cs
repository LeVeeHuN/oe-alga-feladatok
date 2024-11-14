namespace OE.ALGA.Optimalizalas;

public class VisszalepesesOptimalizacio<T>
    {
        int n;
        int[] M;
        T[,] R;
        Func<int, T, bool> ft;
        Func<int, T, T[], bool> fk;
        Func<T[], float> josag;

        public int LepesSzam { get; private set; }

        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        public virtual void Backtrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            int i = -1;
            while (i < M[szint] - 1)
            {
                i++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];

                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                Array.Copy(E, O, E.Length);
                            }
                            van = true;
                        }
                        else
                            Backtrack(szint + 1, ref E, ref van, ref O);
                    }
                }
            }
        }

        public T[] OptimalisMegoldas()
        {
            bool van = false;
            T[] E = new T[n];
            T[] optimalismegoldas = new T[n];

            Backtrack(0, ref E, ref van, ref optimalismegoldas);
            if (van)
                return optimalismegoldas;
            else
                throw new Exception("Nincs megoldás");
        }
    }


    public class VisszalepesesHatizsakPakolas
    {
        HatizsakProblema problema;
        public int LepesSzam { get; private set; }

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];

            for (int i = 0; i < problema.n; i++)
            {
                M[i] = 2;

                R[i, 0] = true;
                R[i, 1] = false;
            }

            VisszalepesesOptimalizacio<bool> visszalepesesoptimalizacio = new VisszalepesesOptimalizacio<bool>(
                problema.n, M, R,

                (szint, r) =>
                {
                    return !r || problema.w[szint] <= problema.Wmax;
                },
                (szint, r, E) =>
                {
                    int suly = 0;
                    for (int i = 0; i < szint; i++)
                    {
                        if (E[i])
                            suly += problema.w[i];
                    }

                    return (suly <= problema.Wmax) && (!r || suly + problema.w[szint] <= problema.Wmax);
                },
                problema.OsszErtek
                );

            LepesSzam = visszalepesesoptimalizacio.LepesSzam;
            return visszalepesesoptimalizacio.OptimalisMegoldas();
        }

        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }


    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        Func<int, T[], float> fb;
        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag) : base(n, M, R, ft, fk, josag)
        {
        }

        public override void Backtrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            base.Backtrack(szint, ref E, ref van, ref O);
        }

    }


    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema)
        {
        }
    }