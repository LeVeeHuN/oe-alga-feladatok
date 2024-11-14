namespace OE.ALGA.Optimalizalas;

public class DinamikusHatizsakPakolas
{
    HatizsakProblema problema;
    public int LepesSzam { get; private set; }

    public DinamikusHatizsakPakolas(HatizsakProblema problema)
    {
        this.problema = problema;
    }

    public int[,] TablazatFeltoltes()
    {
        int[,] F = new int[problema.n + 1, problema.Wmax + 1];

        for (int t = 0; t <= problema.n; t++)
        {
            for (int h = 0; h <= problema.Wmax; h++)
            {
                if (h == 0 || t == 0)
                {
                    F[t, h] = 0;
                }
                else if (h < problema.w[t - 1])
                {
                    F[t, h] = F[t - 1, h];
                }
                else
                {
                    F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - problema.w[t - 1]] + (int)problema.p[t - 1]);
                }
            }
        }

        return F;
    }

    public int OptimalisErtek()
    {
        int[,] F = TablazatFeltoltes();
        return F[problema.n, problema.Wmax];
    }

    public bool[] OptimalisMegoldas()
    {
        int[,] F = TablazatFeltoltes();
        bool[] megoldas = new bool[problema.n];
        int h = problema.Wmax;

        for (int t = problema.n; t > 0; t--)
        {
            if (F[t, h] != F[t - 1, h])
            {
                megoldas[t - 1] = true;
                h -= problema.w[t - 1];
            }
            else
            {
                megoldas[t - 1] = false;
            }
        }

        return megoldas;
    }
}