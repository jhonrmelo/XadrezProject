using System;
using System.Collections.Generic;
using System.Text;
using fTabuleiro;

namespace fTabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QtdMovimento { get; set; }
        public Tabuleiro Tab { get; set; }

        public Peca(Tabuleiro pTab, Cor pCor)
        {
            Posicao = null;
            Cor = pCor;
            Tab = pTab;
            QtdMovimento = 0;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();
            for (int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pPos)
        {
            return movimentosPossiveis()[pPos.linha, pPos.coluna];
        }

        public void IncrementarMovimento()
        {
            QtdMovimento++;
        }

        public void DecrementarMovimentos()
        {
            QtdMovimento--;
        }

        public abstract bool[,] movimentosPossiveis();





    }
}
