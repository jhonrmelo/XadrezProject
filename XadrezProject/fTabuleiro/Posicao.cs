using System;
using System.Collections.Generic;
using System.Text;

namespace fTabuleiro
{
    public class Posicao
    {
        public Posicao(int plinha, int pcoluna)
        {
            DefinirValores(plinha, pcoluna);
        }
        public int linha { get; set; }
        public int coluna { get; set; }

        public override string ToString()
        {
            return $"{linha}, {coluna}";
        }
        public void DefinirValores(int plinha, int pcoluna)
        {
            linha = plinha;
            coluna = pcoluna;
        }
    }



}
