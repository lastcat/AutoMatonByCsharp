// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pointer.cs" company="lastcat">
//   MIT Licence
// </copyright>
// <summary>
//   矢印のクラス。入力と行先を持つ。
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Automaton
{
    /// <summary>
    /// 矢印のクラス。入力と行先を持つ。
    /// </summary>
    public class Pointer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pointer"/> class.
        /// </summary>
        /// <param name="inp">
        /// 入力。
        /// </param>
        /// <param name="des">
        /// 入力に対する行先。
        /// </param>
        public Pointer(char inp, string des)
        {
            this.Input = inp;
            this.Dest = des;
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        public char Input { get; set; }

        /// <summary>
        /// Gets or sets the dest.
        /// </summary>
        public string Dest { get; set; }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Input + ":" + this.Dest;
        }
    }
}
