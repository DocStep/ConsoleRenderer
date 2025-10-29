using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderer {
    public interface IConsoleGame {

        public void Create (int width, int height, Dictionary<int, ConsoleColor> colors, ref Engine engine);

        void GameStart ();
        void GameUpdate ();

        public abstract void Start ();
        public abstract void Update ();

    }
}
