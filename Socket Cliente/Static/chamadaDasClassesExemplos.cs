using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Cliente.Static
{
    public  class chamadaDasClassesExemplos
    {
        public void MetodoExemploStatic()
        {
            ESTATICA.Idade = 18;
        }

        public void MetodoExemploNaoStatic()
        {
            NaoEstatica exemplo1 = new NaoEstatica();
            exemplo1.Idade = 17;

            NaoEstatica exemplo2 = new NaoEstatica(17);
            NaoEstatica exemplo3 = new NaoEstatica(17,160,"Alvaro");
        }
    }
}
