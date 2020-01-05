using fTabuleiro;

namespace Xadrez
{
    class Rei : Peca
    {

        private PartidadeXadrez Partida { get; set; }

        public Rei(Tabuleiro tab, Cor cor, PartidadeXadrez pPartida) : base(tab, cor)
        {
            Partida = pPartida;
        }

        private bool podeMover(Posicao pPosicao)
        {
            Peca P = Tab.GetPeca(pPosicao);
            return P == null || P.Cor != Cor;
        }
        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao posicao = new Posicao(0, 0);
            posicao.DefinirValores(Posicao.linha - 1, Posicao.coluna);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            posicao.DefinirValores(Posicao.linha - 1, Posicao.coluna + 1);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            posicao.DefinirValores(Posicao.linha, Posicao.coluna + 1);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            posicao.DefinirValores(Posicao.linha + 1, Posicao.coluna + 1);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            posicao.DefinirValores(Posicao.linha + 1, Posicao.coluna);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            posicao.DefinirValores(Posicao.linha + 1, Posicao.coluna - 1);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            posicao.DefinirValores(Posicao.linha, Posicao.coluna - 1);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            posicao.DefinirValores(Posicao.linha - 1, Posicao.coluna - 1);
            if (Tab.PosicaoValida(posicao) && podeMover(posicao))
            {
                mat[posicao.linha, posicao.coluna] = true;
            }

            #region Jogada especial Roque pequeno
            if (QtdMovimento == 0 && !Partida.Xeque)
            {
                Posicao posT1 = new Posicao(Posicao.linha, Posicao.coluna + 3);
                if (TestaTorreParaRoque(posT1))
                {
                    Posicao p1 = new Posicao(Posicao.linha, Posicao.coluna + 1);
                    Posicao p2 = new Posicao(Posicao.linha, Posicao.coluna + 2);


                    if (Tab.GetPeca(p1) == null && Tab.GetPeca(p2) == null)
                    {
                        mat[Posicao.linha, Posicao.coluna + 2] = true;
                    }
                }
                #endregion

                #region Jogada especial Roque Grande

                Posicao posT2 = new Posicao(Posicao.linha, Posicao.coluna - 4);

                if (TestaTorreParaRoque(posT2))
                {
                    Posicao p1 = new Posicao(Posicao.linha, Posicao.coluna - 1);
                    Posicao p2 = new Posicao(Posicao.linha, Posicao.coluna - 2);
                    Posicao p3 = new Posicao(Posicao.linha, Posicao.coluna - 3);

                    if (Tab.GetPeca(p1) == null && Tab.GetPeca(p2) == null && Tab.GetPeca(p3) == null)
                    {
                        mat[Posicao.linha, Posicao.coluna - 2] = true;
                    }
                }
            }
            #endregion

            return mat;
        }
        private bool TestaTorreParaRoque(Posicao pPosicao)
        {
            Peca p = Tab.GetPeca(pPosicao);
            return p != null && p is Torre && p.Cor == Cor && p.QtdMovimento == 0;
        }
        public override string ToString()
        {
            return "R";
        }

    }
}
