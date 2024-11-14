namespace OE.ALGA.Optimalizalas;

public class HatizsakProblema
    {
        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public HatizsakProblema(int n, int Wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = Wmax;
            this.w = w;
            this.p = p;
        }

        public float OsszErtek(bool[] X)
        {
            float s = 0;
            for (int i = 0; i < X.Length; i++)
            {
                if (X[i])
                {
                    s += p[i];
                }
            }
            return s;
        }

        public int OsszSuly(bool[] X)
        {
            int s = 0;
            for (int i = 0; i < X.Length; i++)
            {
                if (X[i])
                {
                    s += w[i];
                }
            }
            return s;
        }

        public bool Ervenyes(bool[] X)
        {
            return OsszSuly(X) <= Wmax;
        }
    }

    public class NyersEro<T>
    {
        int m;
        Func<int, T> generator;
        Func<T, float> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int,T> generator, Func<T,float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
        }

        public T OptimalisMegoldas()
        {
            T o = generator(1);
            for (int i = 2; i <= m; i++)
            {
                T x = generator(i);
                LepesSzam++;
                if (josag(x) > josag(o))
                {
                    o = x;
                }
            }
            return o;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        public int LepesSzam { get; private set; }

        HatizsakProblema problema;

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool[] Generator(int i)
        {
            int szam = i - 1;
            bool[] K = new bool[problema.n];

            for (int j = 0; j < problema.n; j++)
            {
                K[j] = (szam / (int)Math.Pow(2, j)) % 2 == 1;
            }
            return K;
        }

        public float Josag(bool[] pakolas)
        {
            if (!problema.Ervenyes(pakolas))
            {
                return -1;
            }
            return problema.OsszErtek(pakolas);
        }

        public bool[] OptimalisMegoldas()
        {
            NyersEro<bool[]> tomb = new NyersEro<bool[]>((int)Math.Pow(2, problema.n), Generator, Josag);

            LepesSzam = tomb.LepesSzam;

            return tomb.OptimalisMegoldas();
        }

        public float OptimalisErtek()
        {
            bool[] optimalisMegoldas = OptimalisMegoldas();
            return problema.OsszErtek(optimalisMegoldas);
        }
    }