using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton
{
    class AutoMaton
    {

        public List<State> states;

        public AutoMaton()
        {
            this.states = new List<State>();
        }

        public State FindStateByName(string name)
        {
            return this.states.First(s => s.name==name);
        }

        public bool Transition(string input,int num, State nowstate)
        {
            if ((num >= input.Length) && nowstate.isFinalState)
            {
                Console.WriteLine("CLEAR");
                return true;
            }
            else if((num >= input.Length) && !nowstate.isFinalState)
            {
                return false;
            }
            else
            {
                //現状態のpointersのpointerを探索
                foreach (Pointer p in nowstate.pointers)
                {
                    if (input[num] == p.input)
                    {
                        Console.WriteLine(num+1 + "文字目クリア");
                        Console.WriteLine(nowstate.name + "=>" + p.dest);
                        return Transition(input, num += 1, FindStateByName(p.dest));
                    }
                }
                //......
               return false;
            }
        }

        public AutoMaton TranslateINfaToNfa(AutoMaton infa)
        {
            var newNfa = new AutoMaton();
            //nfa->dfaの変換関数
            //まずεを取り除く　簡単に言うとε遷移を同じグループにして、01の遷移先を一意にする
            foreach(State s in infa.states)
            {
                //まずnfaに変換してそこからは別の関数を使う
                //一つの状態ごとに
                var group = new List<State>();
                group.Add(s);
                //ε遷移を同じグループに
                RecursiveFindDestState(ref group, 'i');
                //TODO::group内のポインターをすべて登録
                //groupを1状態に対応させたものがnfa　状態数は変わらないからすぐできそう上行の作業をすれば終わり
                //nfaでの受理処理はだるいから最終的にdfaに変換することにしよう
                var newState = new State(s.name);
                foreach(State sing in group)
                {
                    foreach(Pointer p in sing.pointers)
                    {
                        newState.pointers.Add(p);
                    }
                }
                newNfa.states.Add(newState);
            }
            return newNfa;
        }
        /*
        public AutoMaton TranslateNfaToDfa(AutoMaton nfa)
        {
            
        }
        */

        private void  RecursiveFindDestState(ref List<State> sg, char c)
        {
            //foreach(State s in s)
            for (int i = 0; i < sg.Count;i++ )
            {
                State s = sg[i];
                    foreach (Pointer p in s.pointers)
                    {
                        if (p.input == c && !sg.Contains(FindStateByName(p.dest)))
                        {
                            Console.WriteLine("ε遷移を追加" + p.dest);
                            //gr.Add(FindStateByName(p.dest));
                            sg.Add(FindStateByName(p.dest));
                            RecursiveFindDestState(ref sg, c);
                        }
                    }
               }

        }

    }
    class State
    {
        public string name { get; set; }
        public List<Pointer> pointers;
        public bool isFinalState{get; set;}
        public State(string name)
        {
            this.name = name;
            this.pointers = new List<Pointer>();
            this.isFinalState = false;
        }

        
        public override string ToString()
        {
            return this.name;
        }
    }

    class Pointer
    {
        public char input { get; set; }
        public string dest { get; set; }

        public Pointer(char inp, string des)
        {
            this.input = inp;
            this.dest = des;
        }

        public override string ToString()
        {
            return this.input + ":" + this.dest;
        }
    }
}
