using System;
using System.Collections.Generic;
using System.Text;

namespace fTabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] Pecas;
        public Tabuleiro(int pLinhas, int pColunas)
        {
            Linhas = pLinhas;
            Colunas = pColunas;
            Pecas = new Peca[pLinhas, pColunas];
        }

        public Peca GetPeca(int pLinha, int pColuna)
        {
            return Pecas[pLinha, pColuna];
        }

        public Peca GetPeca(Posicao pPosicao)
        {
            return Pecas[pPosicao.linha, pPosicao.coluna];
        }

        public Peca RemovePeca(Posicao pPosicao)
        {
            if (GetPeca(pPosicao) == null)
            {
                return null;
            }
            Peca aux = GetPeca(pPosicao);
            aux.Posicao = null;
            Pecas[pPosicao.linha, pPosicao.coluna] = null;
            return aux;

        }

        public void SetPeca(Posicao pPosicao, Peca pPeca)
        {
            if (ExistePeca(pPosicao))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição");
            }

            Pecas[pPosicao.linha, pPosicao.coluna] = pPeca;
            pPeca.Posicao = pPosicao;
        }

        public bool ExistePeca(Posicao pPosicao)
        {
            ValidarPosicao(pPosicao);
            return GetPeca(pPosicao) != null;
        }
        public bool PosicaoValida(Posicao pPosicao)
        {
            if (pPosicao.linha < 0 || pPosicao.linha >= Linhas || pPosicao.coluna < 0 || pPosicao.coluna >= Colunas)
                return false;

            return true;
        }

        public void ValidarPosicao(Posicao pPosicao)
        {
            if (!PosicaoValida(pPosicao))
                throw new TabuleiroException("Posição inválida");

        }
    }
}
