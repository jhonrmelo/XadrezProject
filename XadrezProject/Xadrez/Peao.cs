using fTabuleiro;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xadrez
{
    class Peao : Peca
    {
        public Peao(Tabuleiro Tab, Cor cor, PartidadeXadrez pPartida) : base(Tab, cor)
        {
            partida = pPartida;
        }

        private PartidadeXadrez partida;

        public override string ToString()
        {
            return "P";
        }

        private bool existeInimigo(Posicao pos)
        {
            Peca p = Tab.GetPeca(pos);
            return p != null && p.Cor != Cor;
        }

        private bool livre(Posicao pos)
        {
            return Tab.GetPeca(pos) == null;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            if (Cor == Cor.Branca)
            {
                pos.DefinirValores(Posicao.linha - 1, Posicao.coluna);
                if (Tab.PosicaoValida(pos) && livre(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(Posicao.linha - 2, Posicao.coluna);
                Posicao p2 = new Posicao(Posicao.linha - 1, Posicao.coluna);
                if (Tab.PosicaoValida(p2) && livre(p2) && Tab.PosicaoValida(pos) && livre(pos) && QtdMovimento == 0)
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(Posicao.linha - 1, Posicao.coluna - 1);
                if (Tab.PosicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(Posicao.linha - 1, Posicao.coluna + 1);
                if (Tab.PosicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }

                #region jogada Especial en Passant
                if (Posicao.linha == 3)
                {
                    Posicao esquerda = new Posicao(Posicao.linha, Posicao.coluna - 1);

                    if (Tab.PosicaoValida(esquerda) && existeInimigo(esquerda) && Tab.GetPeca(esquerda) == partida.VulneravelEnPassant)
                    {
                        mat[esquerda.linha - 1, esquerda.coluna] = true;
                    }

                    Posicao direita = new Posicao(Posicao.linha, Posicao.coluna + 1);

                    if (Tab.PosicaoValida(direita) && existeInimigo(direita) && Tab.GetPeca(direita) == partida.VulneravelEnPassant)
                    {
                        mat[direita.linha - 1, direita.coluna] = true;
                    }
                }
                #endregion
            }
            else
            {
                pos.DefinirValores(Posicao.linha + 1, Posicao.coluna);
                if (Tab.PosicaoValida(pos) && livre(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(Posicao.linha + 2, Posicao.coluna);
                Posicao p2 = new Posicao(Posicao.linha + 1, Posicao.coluna);
                if (Tab.PosicaoValida(p2) && livre(p2) && Tab.PosicaoValida(pos) && livre(pos) && QtdMovimento == 0)
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(Posicao.linha + 1, Posicao.coluna - 1);
                if (Tab.PosicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(Posicao.linha + 1, Posicao.coluna + 1);
                if (Tab.PosicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }

                #region jogada Especial en Passant
                if (Posicao.linha == 4)
                {
                    Posicao esquerda = new Posicao(Posicao.linha, Posicao.coluna - 1);

                    if (Tab.PosicaoValida(esquerda) && existeInimigo(esquerda) && Tab.GetPeca(esquerda) == partida.VulneravelEnPassant)
                    {
                        mat[esquerda.linha + 1, esquerda.coluna] = true;
                    }

                    Posicao direita = new Posicao(Posicao.linha, Posicao.coluna + 1);

                    if (Tab.PosicaoValida(direita) && existeInimigo(direita) && Tab.GetPeca(direita) == partida.VulneravelEnPassant)
                    {
                        mat[direita.linha + 1, direita.coluna] = true;
                    }
                }

                #endregion
            }

            return mat;
        }
    }
}
