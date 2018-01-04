using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

namespace IA_TP
{
    class Cromosoma : ChromosomeBase
    {
        public static int totalDias = 28;
        public static int totalFuncionarios = 15;
        public static int totalGenes = totalDias * totalFuncionarios;

        public static int TM_SEMANA = 4; //Minimo de Trabalhadores no Turno da Manha à semana
        public static int TT_SEMANA = 3;
        public static int TN_SEMANA = 2;

        public static int TM_FDS = 3; //Minimo de Trabalhadores no Turno da Manha ao fim de semana
        public static int TT_FDS = 2;
        public static int TN_FDS = 1;

        public Cromosoma() : base(totalGenes)
        {
            CreateGenes();
        }
        public override IChromosome CreateNew()
        {
            return new Cromosoma();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            /*
             * 0 - Significa não trabalha
             * 1 - Trabalha turno manha
             * 2 - Turno da Tarde
             * 3 - Turno da Noite
             * 4 - manha e tarde
             * 5 - tarde e noite
             * 6 - manha e noite
             */

            //int[] valores = { 0, 1, 2, 3, 4, 5, 6 }; //Valores repetidos para ter mais probabilidade de trabalhar apenas 1 turno
            //Random r = new Random();
            BasicRandomization random = new BasicRandomization();

            return new Gene(random.GetInt(0,7));
        }

        public static string BuildCalendar(Gene[] genes)
        {
            int h = 1;

            string calendario = "";

            for (int i = 0; i < 28; i++)
            {
                calendario += "- ";
            }

            calendario += "\n";

            for (int i = 0; i < Cromosoma.totalGenes; i++)
            {

                calendario += genes[i] + " ";

                if (h % 28 == 0)
                    calendario += "\n";

                h++;
            }

            return calendario;
        }
    }
}
