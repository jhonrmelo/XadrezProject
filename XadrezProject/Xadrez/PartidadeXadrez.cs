using fTabuleiro;
using System.Collections.Generic;

namespace Xadrez
{
    class PartidadeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        private HashSet<Peca> Pecas;

        private HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }

        public Peca VulneravelEnPassant { get; private set; }


        public PartidadeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
            Xeque = false;
            VulneravelEnPassant = null;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.SetPeca(new PosicaoXadrez(coluna, linha).ConvertPosicao(), peca);
            Pecas.Add(peca);
        }

        public void validarPosicaoDeOrigem(Posicao pPos)
        {
            var peca = Tab.GetPeca(pPos);

            if (peca == null)
            {
                throwException("Não existe peça na posição de origem escolhida!");
            }

            if (JogadorAtual != peca.Cor)
            {
                throwException("A peça escolhida não é sua!");
            }

            if (!peca.ExisteMovimentosPossiveis())
            {
                throwException("Não existem movimentos possiveis para essa peça!");
            }
        }

        public bool validaReiEmXeque(Cor pCor)
        {
            Peca R = Rei(pCor);

            if (R == null)
                throw new TabuleiroException($"Não tem rei da cor {pCor} no Tabuleiro");

            foreach (Peca p in PecasEmJogo(Adversaria(pCor)))
            {
                bool[,] mat = p.movimentosPossiveis();

                if (mat[R.Posicao.linha, R.Posicao.coluna])
                    return true;
            }

            return false;
        }
        public void validarPosicaoDestino(Posicao pOrigem, Posicao pDestino)
        {
            var pecaOrigem = Tab.GetPeca(pOrigem);

            if (!pecaOrigem.MovimentoPossivel(pDestino))
            {
                throwException("Posição de destino inválida!");
            }
        }

        public void throwException(string message)
        {
            throw new TabuleiroException(message);
        }
        public Peca ExecutaMovimento(Posicao pOrigem, Posicao pDestino)
        {
            Peca p = Tab.RemovePeca(pOrigem);
            p.IncrementarMovimento();
            Peca pecaCaputurada = Tab.RemovePeca(pDestino);
            Tab.SetPeca(pDestino, p);
            if (pecaCaputurada != null)
            {
                Capturadas.Add(pecaCaputurada);
            }

            #region Jogada Especial Roque Pequeno
            if (p is Rei && pDestino.coluna == pOrigem.coluna + 2)
            {
                Posicao origemT = new Posicao(pOrigem.linha, pOrigem.coluna + 3);
                Posicao destinoT = new Posicao(pOrigem.linha, pOrigem.coluna + 1);

                Peca T = Tab.RemovePeca(origemT);
                T.IncrementarMovimento();
                Tab.SetPeca(destinoT, T);
            }
            #endregion

            #region Jogada Especial Roque Grande
            if (p is Rei && pDestino.coluna == pOrigem.coluna - 2)
            {
                Posicao origemT = new Posicao(pOrigem.linha, pOrigem.coluna - 4);
                Posicao destinoT = new Posicao(pOrigem.linha, pOrigem.coluna - 1);

                Peca T = Tab.RemovePeca(origemT);
                T.IncrementarMovimento();
                Tab.SetPeca(destinoT, T);
            }
            #endregion


            #region Jogada Especial en Passant
            if (p is Peao)
            {
                if (pOrigem.coluna != pDestino.coluna && pecaCaputurada == null)
                {
                    Posicao posP;

                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(pDestino.linha + 1, pDestino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(pDestino.linha - 1, pDestino.coluna);
                    }

                    pecaCaputurada = Tab.RemovePeca(posP);

                    Capturadas.Add(pecaCaputurada);
                }
            }
            #endregion

            return pecaCaputurada;
        }

        private Cor CorAdversaria(Cor pCor)
        {
            if (pCor == Cor.Branca)
                return Cor.Preta;
            else
                return Cor.Branca;
        }
        public HashSet<Peca> PecasCapturadas(Cor pCor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (var peca in Capturadas)
            {
                if (peca.Cor == pCor)
                {
                    aux.Add(peca);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor pCor)
        {
            HashSet<Peca> aux = null;
            aux = new HashSet<Peca>();
            foreach (var peca in Pecas)
            {
                if (peca.Cor == pCor)
                {
                    aux.Add(peca);
                }
            }
            aux.ExceptWith(PecasCapturadas(pCor));

            return aux;
        }
        public void RealizaJogada(Posicao pOrigem, Posicao pDestino)
        {
            Peca pecaCapturada = ExecutaMovimento(pOrigem, pDestino);

            Peca p = Tab.GetPeca(pDestino);

            if (validaReiEmXeque(JogadorAtual))
            {
                DesfazMovimento(pOrigem, pDestino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque");
            }

            if (validaReiEmXeque(Adversaria(JogadorAtual)))
                Xeque = true;
            else
                Xeque = false;

            if (testeXequeMate(Adversaria(JogadorAtual)))
                Terminada = true;

            else
            {
                Turno++;
                mudaJogador();
            }

            #region Jogada Especial en Passant
            if (p is Peao
                && pDestino.linha == pOrigem.linha - 2
                || pDestino.linha == pOrigem.linha + 2)
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }

            #endregion

            #region Jogada Especial Promoção
            if (p is Peao)
            {
                if ((p.Cor == Cor.Branca && pDestino.linha == 0) || (p.Cor == Cor.Preta && pDestino.linha == 7))
                {
                    p = Tab.RemovePeca(pDestino);
                    Pecas.Remove(p);

                    Peca dama = new Dama(Tab, p.Cor);
                    Tab.SetPeca(pDestino, dama);
                }
            }
            #endregion
        }

        private void DesfazMovimento(Posicao pOrigem, Posicao pDestino, Peca pecaCapturada)
        {
            Peca p = Tab.RemovePeca(pDestino);
            p.DecrementarMovimentos();
            if (pecaCapturada != null)
            {
                Tab.SetPeca(pDestino, pecaCapturada);
                Capturadas.Remove(pecaCapturada);
            }

            #region Jogada Especial Roque Pequeno
            if (p is Rei && pDestino.coluna == pOrigem.coluna + 2)
            {
                Posicao origemT = new Posicao(pOrigem.linha, pOrigem.coluna + 3);
                Posicao destinoT = new Posicao(pOrigem.linha, pOrigem.coluna + 1);

                Peca T = Tab.RemovePeca(destinoT);
                T.DecrementarMovimentos();
                Tab.SetPeca(origemT, T);
            }
            #endregion

            #region Jogada Especial Roque Grande
            if (p is Rei && pDestino.coluna == pOrigem.coluna - 2)
            {
                Posicao origemT = new Posicao(pOrigem.linha, pOrigem.coluna - 4);
                Posicao destinoT = new Posicao(pOrigem.linha, pOrigem.coluna - 1);

                Peca T = Tab.RemovePeca(destinoT);
                T.DecrementarMovimentos();
                Tab.SetPeca(origemT, T);
            }
            #endregion

            #region Jogada Especial en Passant
            if (p is Peao)
            {
                if (pOrigem.coluna != pOrigem.coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.RemovePeca(pDestino);
                    Posicao posP;

                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, pDestino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, pDestino.coluna);
                    }

                    Tab.SetPeca(posP, peao);
                }
            }
            #endregion

            Tab.SetPeca(pOrigem, p);
        }
        private Cor Adversaria(Cor pCor)
        {
            if (pCor == Cor.Branca)
            {
                return Cor.Preta;
            }

            return Cor.Branca;
        }

        private Peca Rei(Cor pCor)
        {
            foreach (Peca p in PecasEmJogo(pCor))
            {
                if (p is Rei)
                {
                    return p;
                }
            }
            return null;
        }

        public bool testeXequeMate(Cor pCor)
        {
            if (!validaReiEmXeque(pCor))
            {
                return false;
            }
            foreach (Peca p in PecasEmJogo(pCor))
            {
                bool[,] mat = p.movimentosPossiveis();

                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao posicaoOrigem = p.Posicao;
                            Posicao posicaoDestino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(posicaoOrigem, new Posicao(i, j));
                            bool testeXeque = validaReiEmXeque(pCor);
                            DesfazMovimento(posicaoOrigem, posicaoDestino, pecaCapturada);

                            if (!testeXeque)
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        private void mudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
                JogadorAtual = Cor.Preta;
            else
                JogadorAtual = Cor.Branca;
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Preta, this));

        }
    }
}
