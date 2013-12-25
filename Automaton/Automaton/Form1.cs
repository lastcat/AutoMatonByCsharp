// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="lastcat">
//   MIT Licence
// </copyright>
// <summary>
//   フォームです。
// </summary>
// --------------------------------------------------------------------------------------------------------------------

    using System;
    using System.IO;
    using System.Windows.Forms;

namespace Automaton
{
    /// <summary>
    /// フォームです。
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The automaton.
        /// </summary>
        private AutoMaton automaton = new AutoMaton();

        /// <summary>
        /// 入力ファイルを行ごとに収納するstringn配列。
        /// </summary>
       private string[] lines;

        /// <summary>
        /// 入力が受理されたかどうかのbool値。
        /// </summary>
        private bool acception;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 入力ファイルの読み取り。
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.lines = File.ReadAllLines(ofd.FileName);

                // 1行につき1状態
                foreach (string line in this.lines)
                {
                    string[] data = line.Split(',');
                    var state = new State(data[0]);

                    for (int i = 1; i < data.Length - 1; i += 2)
                    {
                        var p = new Pointer(data[i][0], data[i + 1]);
                        state.pointers.Add(p);
                    }

                    if (state.Name[state.Name.Length - 1] == 'f')
                    {
                        state.Name = state.Name.Trim('f');
                        state.IsFinalState = true;
                    }

                    this.automaton.states.Add(state);
                }
            }
        }

        /// <summary>
        /// 今のところ、受理診断のボタン。
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button2_Click(object sender, EventArgs e)
        {
            Console.Clear();

            string input = this.textBox1.Text;
            this.acception = this.automaton.Transition(input, 0, this.automaton.states[0]);
            result.Text = this.acception.ToString();
            Console.WriteLine(this.acception.ToString());
        }
    }
}
