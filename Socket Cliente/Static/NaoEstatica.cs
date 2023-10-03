using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Cliente.Static
{
    public class NaoEstatica
    {
        public int Idade { get; set; }
        public int altura { get; set; }
        public string nome { get; set; }

        public NaoEstatica()
        {

        }

        public NaoEstatica(int x)
        {
            this.Idade = x; 
        }

        public NaoEstatica(int x, int y)
        {
            this.Idade = x;
            this.altura = y;
        }
        public NaoEstatica(int x, int y, string nome)
        {
            this.Idade = x;
            this.altura = y;
            this.nome = nome;
        }
    }
}
