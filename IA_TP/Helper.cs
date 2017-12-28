using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP
{
    class Helper
    {
        public static void AddBestFitness(double value)
        {
            DateTime date = new DateTime();
            date = DateTime.Now;

            string filename = date.ToString() + "_log.csv";
            string filepath = @"D:\Dropbox\IPCA\LESI\3° Ano\1° Semestre\IA\Trabalho\" + filename;

            File.AppendAllText(filepath, value.ToString());
        }

        public static void ExportCsv(IChromosome cromosome)
        {
            Gene[] genes = cromosome.GetGenes();
            int tamanho = genes.Length;
            int h = 1;
            string content = "";
            string filePath = @"D:\Dropbox\IPCA\LESI\3° Ano\1° Semestre\IA\Trabalho\export.csv";

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
    }
}
