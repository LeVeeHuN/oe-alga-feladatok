using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class FaElem<T> where T : IComparable<T>
    {
        public T tart;
        public FaElem<T>? bal;
        public FaElem<T>? jobb;

        public FaElem(T tart, FaElem<T> jobb, FaElem<T> bal)
        {
            this.tart = tart;
            this.jobb = jobb;
            this.bal = bal;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        FaElem<T>? gyoker;


        private FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)
            {
                return new FaElem<T>(ertek, null, null);
            }

            if (p.tart.CompareTo(ertek) > 0)
            {
                p.bal = ReszfabaBeszur(p.bal, ertek);
            }
            else if (p.tart.CompareTo(ertek) < 0)
            {
                p.jobb = ReszfabaBeszur(p.jobb, ertek);
            }
            return p;
        }

        private bool ReszfaEleme(FaElem<T>? p, T ertek)
        {
            if (p == null) return false;

            if (p.tart.CompareTo(ertek) > 0)
            {
                return ReszfaEleme(p.bal, ertek);
            }

            if (p.tart.CompareTo(ertek) < 0)
            {
                return ReszfaEleme(p.jobb, ertek);
            }

            return true;

        }

        private FaElem<T> ReszfabolTorol(FaElem<T>? p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        p.jobb = ReszfabolTorol(p.jobb, ertek);
                    }
                    else
                    {
                        if (p.bal == null)
                        {
                            FaElem<T> q = p;
                            p = p.jobb;

                        }
                        else
                        {
                            if (p.jobb == null)
                            {
                                FaElem<T> q = p;
                                p = p.bal;
                            }
                            else
                            {
                                p.bal = KetGyerek(p, p.bal);
                            }
                        }
                    }
                }
                return p;

            }
            else
            {
                throw new NincsElemKivetel();
            }

            FaElem<T> KetGyerek(FaElem<T> e, FaElem<T> r)
            {
                if (r.jobb != null)
                {
                    r.jobb = KetGyerek(e, r.jobb);
                    return r;
                }
                else
                {
                    e.tart = r.tart;
                    FaElem<T> q = r.bal;
                    return q;
                }


            }
        }

        private void ReszfaBejarasPreOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }

        }

        private void ReszfaBejarasInOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasInOrder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasInOrder(p.jobb, muvelet);
            }
        }

        private void ReszfaBejarasPostOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPostOrder(p.bal, muvelet);
                ReszfaBejarasPostOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }




        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }
    }
}
