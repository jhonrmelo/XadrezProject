using System;
using System.Collections.Generic;
using System.Text;
using fTabuleiro;

namespace Xadrez
{
    class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(tab, cor)
        { }
        public override string ToString()
        {
            return "T";
        }

        private bool podeMover(Posicao pPosicao)
        {
            Peca P = Tab.GetPeca(pPosicao);
            return P == null || P.Cor != Cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao posicao = new Posicao(0,0);

            posicao.DefinirValores(Posicao.linha - 1, Posicao.coluna);
            while (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
                if (Tab.GetPeca(posicao) != null && Tab.GetPeca(posicao).Cor != Cor)
                {
                    break;
                }
                posicao.linha--;
            }
            posicao.DefinirValores(Posicao.linha + 1, Posicao.coluna);
            while (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
                if (Tab.GetPeca(posicao) != null && Tab.GetPeca(posicao).Cor != Cor)
                {
                    break;
                }
                posicao.linha++;
            }
            posicao.DefinirValores(Posicao.linha , Posicao.coluna + 1);
            while (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
                if (Tab.GetPeca(posicao) != null && Tab.GetPeca(posicao).Cor != Cor)
                {
                    break;
                }
                posicao.coluna++;
            }
            posicao.DefinirValores(Posicao.linha, Posicao.coluna - 1);
            while (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
                if (Tab.GetPeca(posicao) != null && Tab.GetPeca(posicao).Cor != Cor)
                {
                    break;
                }
                posicao.coluna--;
            }
            return mat;
        }
    }
}
