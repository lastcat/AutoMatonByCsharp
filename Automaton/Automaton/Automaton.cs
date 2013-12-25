// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Automaton.cs" company="lastcat">
//   MIT Licence
// </copyright>
// <summary>
//   オートマトンクラス。状態のリストといくつかの操作関数によって構成される。
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton
{
    /// <summary>
    /// オートマトンクラス。状態のリストといくつかの操作関数によって構成される。
    /// </summary>
    public class AutoMaton
    {
        /// <summary>
        /// 状態のリスト。
        /// </summary>
        private List<State> states;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMaton"/> class.
        /// </summary>
        public AutoMaton()
        {
            this.states = new List<State>();
        }

        /// <summary>
        /// Gets or sets the states.
        /// </summary>
        public List<State> States
        {
            get
            {
                return this.states;
            }

            set
            {
                this.states = value;
            }
        }

        /// <summary>
        /// 状態を名前から探す。矢印は名前しか持ってないのでこれ使ってStateを戻す必要がある。
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="State"/>.
        /// </returns>
        public State FindStateByName(string name)
        {
            return this.states.First(s => s.Name == name);
        }

        /// <summary>
        /// 入力文字列に対しての遷移関数。
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="num">
        /// The num.
        /// </param>
        /// <param name="nowstate">
        /// The nowstate.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Transition(string input, int num, State nowstate)
        {
            if ((num >= input.Length) && nowstate.IsFinalState)
            {
                Console.WriteLine("CLEAR");
                return true;
            }
            else if ((num >= input.Length) && !nowstate.IsFinalState)
            {
                return false;
            }
            else
            {
                // 現状態のpointersのpointerを探索
                foreach (Pointer p in nowstate.pointers)
                {
                    if (input[num] == p.Input)
                    {
                        Console.WriteLine(num + 1 + "文字目クリア");
                        Console.WriteLine(nowstate.Name + "=>" + p.Dest);
                        return this.Transition(input, num += 1, this.FindStateByName(p.Dest));
                    }
                }

                // ......
                return false;
            }
        }

        /// <summary>
        /// ε遷移NfaをNfaに変換する関数。
        /// </summary>
        /// <param name="infa">
        /// The infa.
        /// </param>
        /// <returns>
        /// The <see cref="AutoMaton"/>.
        /// </returns>
        public AutoMaton TranslateINfaToNfa(AutoMaton infa)
        {
            var newNfa = new AutoMaton();

            // nfa->dfaの変換関数
            // まずεを取り除く　簡単に言うとε遷移を同じグループにして、01の遷移先を一意にする
            foreach (State s in infa.states)
            {
                // まずnfaに変換してそこからは別の関数を使う
                // 一つの状態ごとに
                var group = new List<State>();
                group.Add(s);

                // ε遷移を同じグループに
                this.RecursiveFindDestState(ref group, 'i');

                // TODO::group内のポインターをすべて登録
                // groupを1状態に対応させたものがnfa　状態数は変わらないからすぐできそう上行の作業をすれば終わり
                // nfaでの受理処理はだるいから最終的にdfaに変換することにしよう
                var newState = new State(s.Name);
                foreach (State sing in group)
                {
                    foreach (Pointer p in sing.pointers)
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

        /// <summary>
        /// 再帰的に状態を探っていく。今のところε遷移先をグルーピングすることにしか使ってない。
        /// </summary>
        /// <param name="sg">
        /// The sg.
        /// </param>
        /// <param name="c">
        /// The c.
        /// </param>
        private void RecursiveFindDestState(ref List<State> sg, char c)
        {
            // foreach(State s in s)
            for (int i = 0; i < sg.Count; i++)
            {
                State s = sg[i];
                foreach (Pointer p in s.pointers)
                {
                    if (p.Input == c && !sg.Contains(this.FindStateByName(p.Dest)))
                    {
                        Console.WriteLine("ε遷移を追加" + p.Dest);

                        // gr.Add(FindStateByName(p.dest));
                        sg.Add(this.FindStateByName(p.Dest));
                        this.RecursiveFindDestState(ref sg, c);
                    }
                }
            }
        }
    }
}
