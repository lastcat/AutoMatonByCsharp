using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Automaton
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        AutoMaton automaton = new AutoMaton();
        string[] lines;
        bool acception;
        StreamWriter writer;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lines = File.ReadAllLines(ofd.FileName);
                //1行につき1状態
                foreach(string line in lines)
                {
                    string[] data = line.Split(',');
                    var state = new State(data[0]);

                    for (int i = 1; i < data.Length - 1; i+=2)
                    {
                        var p = new Pointer(data[i][0], data[i + 1]);
                        state.pointers.Add(p);
                    }

                    if (state.name[state.name.Length-1] == 'f')
                    {
                        //state.name = state.name.Trim('f');
                        state.isFinalState = true;
                    }
                    automaton.states.Add(state);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.Clear();
            writer = new StreamWriter("C:\\Users\\Yoshitake\\Documents\\log.txt",false,System.Text.Encoding.GetEncoding("shift_jis"));
            automaton.TranslateINfaToNfa(automaton,writer);
            
            var dfa = new AutoMaton();
            var firstState = new State(automaton.states[0].name);
            dfa.states.Add(firstState);
            dfa.TranslateNfaToDfa(dfa,automaton,writer);
            dfa.ToString();
            
            string input = textBox1.Text;
            acception = dfa.Transition(input, 0, dfa.states[0], writer);
            result.Text = acception.ToString();
            Console.WriteLine(acception.ToString());
            //dfa.ToString();
        }



        
    }
}
