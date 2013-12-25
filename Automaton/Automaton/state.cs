// --------------------------------------------------------------------------------------------------------------------
// <copyright file="state.cs" company="lastcat">
//   MIT Licence
// </copyright>
// <summary>
//   状態クラス。状態名と矢印のリストを持つ。
// </summary>
// --------------------------------------------------------------------------------------------------------------------

    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace Automaton
{
    /// <summary>
    /// 状態クラス。状態名と矢印のリストを持つ。
    /// </summary>
    public class State
    {
        /// <summary>
        /// 矢印のリスト。
        /// </summary>
        private List<Pointer> pointers;

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public State(string name)
        {
            this.Name = name;
            this.pointers = new List<Pointer>();
            this.IsFinalState = false;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 終了状態であるかどうかのフラグ。
        /// </summary>
        public bool IsFinalState { get; set; }
        
        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
