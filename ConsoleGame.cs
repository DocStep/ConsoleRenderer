using System;
using System.Collections.Generic;


#pragma warning disable CA1416
namespace ConsoleRenderer {
    public class ConsoleGame {


        protected void Init_internal () {
            Start();
        }



        /// <summary> First frame. </summary>
        public virtual void Init () {

        }

        public virtual void Keys () {

        }

        /// <summary> First frame. </summary>
        public virtual void Start () {

        }

        /// <summary> Update frame. </summary>
        public virtual void Update () {

        }

        /// <summary> Update frame. </summary>
        public virtual void UpdateSkip () {

        }

        /// <summary> Update frame. </summary>
        public virtual void Exit () {

        }


    }
}
