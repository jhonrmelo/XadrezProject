using System;
using fTabuleiro;
using Xadrez;

namespace XadrezProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidadeXadrez partidadeXadrez = new PartidadeXadrez();

                while (!partidadeXadrez.Terminada)
                {
                    try
                    {
                        Console.Clear();
                        Tela.ImprimirPartida(partidadeXadrez);
                        Console.Write("Origem: ");
                        Posicao origem = Tela.lerPosicaoXadrez().ConvertPosicao();
                        partidadeXadrez.validarPosicaoDeOrigem(origem);

                        Console.Clear();
                        bool[,] posicoesValidas = partidadeXadrez.Tab.GetPeca(origem).movimentosPossiveis();
                        Tela.imprimirTabuleiro(partidadeXadrez.Tab, posicoesValidas);
                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao Destino = Tela.lerPosicaoXadrez().ConvertPosicao();
                        partidadeXadrez.validarPosicaoDestino(origem, Destino);

                        partidadeXadrez.RealizaJogada(origem, Destino);
                    }
                    catch (TabuleiroException exTab)
                    {
                        Console.WriteLine(exTab.Message);
                        Console.ReadLine();
                    }

                    Console.Clear();
                    Tela.ImprimirPartida(partidadeXadrez);
                }

                Tela.imprimirTabuleiro(partidadeXadrez.Tab);
            }
            catch (TabuleiroException ExTab)
            {
                Console.WriteLine(ExTab.Message);
            }
        }
    }
}
