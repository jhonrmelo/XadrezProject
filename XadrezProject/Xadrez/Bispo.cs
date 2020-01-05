using fTabuleiro;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xadrez
{
    class Bispo : Peca
    {
        public Bispo(Tabuleiro tab, Cor cor) : base(tab, cor)
        { }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];


            Posicao pos = new Posicao(0, 0);

            pos.DefinirValores(Posicao.linha - 1, Posicao.coluna - 1);
            while (Tab.PosicaoValida(pos) && podeMover(pos))
            {
                var peca = Tab.GetPeca(pos);
                mat[pos.linha, pos.coluna] = true;

                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.linha - 1, pos.coluna - 1);
            }

            pos.DefinirValores(Posicao.linha - 1, Posicao.coluna + 1);
            while (Tab.PosicaoValida(pos) && podeMover(pos))
            {
                var peca = Tab.GetPeca(pos);
                mat[pos.linha, pos.coluna] = true;

                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.linha - 1, pos.coluna + 1);
            }


            pos.DefinirValores(Posicao.linha + 1, Posicao.coluna + 1);
            while (Tab.PosicaoValida(pos) && podeMover(pos))
            {
                var peca = Tab.GetPeca(pos);
                mat[pos.linha, pos.coluna] = true;

                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.linha + 1, pos.coluna + 1);
            }

            pos.DefinirValores(Posicao.linha + 1, Posicao.coluna - 1);
            while (Tab.PosicaoValida(pos) && podeMover(pos))
            {
                var peca = Tab.GetPeca(pos);
                mat[pos.linha, pos.coluna] = true;

                if (peca != null && peca.Cor != Cor)
                    break;

                pos.DefinirValores(pos.linha + 1, pos.coluna - 1);
            }

            return mat;
        }

        public override string ToString()
        {
            return "B";
        }

        private bool podeMover(Posicao pPosicao)
        {
            Peca p = Tab.GetPeca(pPosicao);
            return p == null || p.Cor != Cor;
        }

    }
}
