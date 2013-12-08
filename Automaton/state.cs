using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Automaton
{
    class AutoMaton
    {

        public List<State> states;

        //public string id = null;

        public AutoMaton()
        {
            this.states = new List<State>();
        }

        public override string ToString()
        {
            foreach(State s in states)
            {
                s.ToString();
            }
            return "hoge";
        }

        public State FindStateByName(string name)
        {
            return this.states.FirstOrDefault(s => s.name==name);
        }

        public bool Transition(string input,int num, State nowstate,StreamWriter writer)
        {
            //StreamWriter writer = new StreamWriter("C:\\Users\\Yoshitake\\Documents\\log.txt");
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
                        return Transition(input, num += 1, FindStateByName(p.dest), writer);
                    }
                }
                //......
               return false;
            }
        }

        public AutoMaton TranslateINfaToNfa(AutoMaton infa,StreamWriter writer)
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
                RecursiveFindDestState(ref group, 'i',writer);
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

        public AutoMaton TranslateNfaToDfa(AutoMaton dfa, AutoMaton nfa, StreamWriter writer)
        {
            //StreamWriter writer = new StreamWriter("C:\\Users\\Yoshitake\\Documents\\log.txt");
            int i = 0;
            //初期状態を読み込んで、
            //var dfa = new AutoMaton();
            //for (int i = 0; i < dfa.states.Count; i++)
            while (true)
            {
                //if()
                //if (dfa.states[i].processed==false)
                //{
                //if (dfa.states[i].processed) continue;
                writer.WriteLine(dfa.states[i].name + "を処理します" + dfa.states[i].processed.ToString());
                

                var objState = new State(dfa.states[i].name);
                //if (dfa.states[i].processed) continue;
                //↑が,区切りの名前の状態
                //これもリストにしないといけない
                //var nfaObjectState = nfa.FindStateByName(firstState.name);
                var nfaObjectStateList = new List<State>();
                foreach (string obstName in objState.name.Split(','))
                {
                    writer.WriteLine("分割され" + obstName + "がリストに入れられました");
                    //writer.WriteLine(nfa.FindStateByName(obstName).isFinalState);
                    /*if (nfa.FindStateByName(obstName).isFinalState)
                        objState.isFinalState = true;*/
                    nfaObjectStateList.Add(nfa.FindStateByName(obstName));
                }
                //状態遷移先の名前の集合
                var destNameList = new List<string>();
                //pointersのinputが0or1で分けないといけない
                char[] alphabet = { '0', '1' };
                for (int j = 0; j < alphabet.Length; j++)
                {
                    writer.WriteLine("状態" + dfa.states[i].name + ":input" + alphabet[j] + "について");

                    foreach (State s in nfaObjectStateList)
                    {
                        try
                        {
                            writer.WriteLine(s.name + "を処理中です");
                            if (s.pointers.Count != 0)
                            {
                                foreach (Pointer p in s.pointers)
                                {
                                    if (p.input == alphabet[j] && !destNameList.Contains(p.dest))
                                    {
                                        writer.WriteLine(p.dest + "を一次登録……");
                                        destNameList.Add(p.dest);
                                    }
                                }
                            }
                        }

                        catch
                        {
                            writer.WriteLine(s);
                            //writer.Close();
                        }
                    }

                    if (dfa.FindStateByName(string.Join(",", destNameList)) == null)
                    //if (string.Join(",", destNameList) == "")
                    {
                        writer.WriteLine("新たな状態" + string.Join(",", destNameList));
                        //destNameList.Reverse();
                        objState.pointers.Add(new Pointer(alphabet[j], string.Join(",", destNameList)));
                        var newState = new State(string.Join(",", destNameList));
                        if (newState.name.Contains('f'))
                            newState.isFinalState = true;
                        dfa.states.Add(newState);
                        //TranslateNfaToDfa(dfa, nfa, writer);
                        //ここで再帰呼び出しって感じがする
                    }
                    else
                    {
                        writer.WriteLine("既存の状態" + string.Join(",", destNameList) + "を遷移先に登録しました");
                        objState.pointers.Add(new Pointer(alphabet[j], string.Join(",", destNameList)));
                    }
                    
                }
                writer.WriteLine("dfaの次の状態に移ります");
                i++;
                if (i == dfa.states.Count) break;
            }
            writer.Close();
            return dfa;
        }


        private void RecursiveFindDestState(ref List<State> sg, char c, StreamWriter writer)
        {
            //StreamWriter writer = new StreamWriter("C:\\Users\\Yoshitake\\Documents\\log.txt");
            //foreach(State s in s)
            for (int i = 0; i < sg.Count; i++)
            {
                State s = sg[i];
                foreach (Pointer p in s.pointers)
                {
                    if (p.input == c && !sg.Contains(FindStateByName(p.dest)))
                    {
                        writer.WriteLine("ε遷移を追加" + p.dest);
                        //gr.Add(FindStateByName(p.dest));
                        sg.Add(FindStateByName(p.dest));
                        RecursiveFindDestState(ref sg, c,writer);
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
        public bool processed = false;
        public State(string name)
        {
            this.name = name;
            this.pointers = new List<Pointer>();
            this.isFinalState = false;
        }

        public State()
        {
            this.name = null;
        }

        
        public override string ToString()
        {
            Console.WriteLine(this.name);
            //Console.WriteLine(this.isFinalState);
            foreach(Pointer p in this.pointers)
            {
                Console.WriteLine("uooo");
                p.ToString();
            }
            return "hoge";
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
            Console.WriteLine(this.input + ":" + this.dest);
            return "hoge";
        }
    }
}
