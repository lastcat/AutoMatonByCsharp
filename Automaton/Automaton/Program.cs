// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="lastcat">
//   MIT Licence
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace Automaton
{
    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
