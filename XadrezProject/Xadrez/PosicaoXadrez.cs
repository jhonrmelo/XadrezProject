using fTabuleiro;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xadrez
{
    class PosicaoXadrez
    {
        public char Coluna { get; set; }
        public int Linha { get; set; }

        public PosicaoXadrez(char pColuna, int pLinha)
        {
            Coluna = pColuna;
            Linha = pLinha;
        }

        public Posicao ConvertPosicao()
        {
            return new Posicao(8 - Linha, Coluna - 'a');
        }
        public override string ToString()   
        {
            return "" + Coluna + Linha;
        }



    }
}
