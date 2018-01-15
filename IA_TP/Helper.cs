using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IA_TP
{
    class Helper
    {

        public void DrawConsole()
        {
            Thread.Sleep(500);
            Console.Clear();
            var bestChromosome = Program.ga.Population.BestChromosome;
            Console.WriteLine("Melhor Cromosoma (Fitness {0}):", bestChromosome.Fitness);
            Console.WriteLine(Cromosoma.BuildCalendar(bestChromosome.GetGenes()));
            Console.WriteLine("Geração: {0}", Program.ga.Population.GenerationsNumber);
            Console.WriteLine("Tempo: {0}h {1}m {2}s", Program.ga.TimeEvolving.Hours, Program.ga.TimeEvolving.Minutes, Program.ga.TimeEvolving.Seconds);
            Thread.Sleep(500);
        }

        /// <summary>
        /// Adiciona uma lista o fitness atual
        /// </summary>
        public void AddFitnessToList()
        {
            int delay = 2000; //Vai adicionar de 2 em dois segundos

            //Esperar que a instancia de ga seja criada no Main do Program
            while (Program.ga.BestChromosome == null)
            {
                Thread.Sleep(1000);
            }

            while (true)
            {
                Program.fitList += Program.ga.Population.BestChromosome.Fitness.ToString() + "\n";
                Thread.Sleep(delay);
            }
        }


        /// <summary>
        /// Exporta para CSV o resultado final
        /// </summary>
        /// <param name="cromosome"></param>
        public static void ExportCsv(IChromosome cromosome)
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            Gene[] genes = cromosome.GetGenes();
            int tamanho = genes.Length;
            int h = 1;
            string content = "";
            string filename = date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + "_cromossoma.csv";
            string filePath = @"D:\Dropbox\IPCA\LESI\3° Ano\1° Semestre\IA\Trabalho\Exports\" + filename;

            for (int i = 0; i < tamanho; i++)
            {

                if (h % 28 == 0)
                {
                    content += genes[i].Value.ToString() + "\n";
                }
                else
                {
                    content += genes[i].Value.ToString() + ";";
                }
                    

                h++;
            }

            File.WriteAllText(filePath, content);
        }


        /// <summary>
        /// Exporta os stats
        /// </summary>
        /// <param name="stats"></param>
        public static void ExportStats(string stats)
        {
            DateTime date = new DateTime();
            date = DateTime.Now;

            string filename = date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + "_stats.csv";
            string filePath = @"D:\Dropbox\IPCA\LESI\3° Ano\1° Semestre\IA\Trabalho\Exports\" + filename;

            File.WriteAllText(filePath, stats);
        }
    }
}
