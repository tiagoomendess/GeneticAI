using GeneticSharp.Domain;
using GeneticSharp.Domain.Terminations;
using System;
using System.Threading;

namespace IA_TP
{
    class MyTermination : TerminationBase
    {
        
        private static bool stop = false;

        protected override bool PerformHasReached(IGeneticAlgorithm geneticAlgorithm)
        {
            return stop;
        }

        public void Run4Ever()
        {
            char c = 'a';

            Thread.Sleep(100);

            while (!stop)
            {
                c = Console.ReadKey().KeyChar;

                if (c == 'p')
                {
                    stop = true;
                }

                if (c == 's')
                {
                    Helper.ExportCsv(Program.ga.BestChromosome);
                    Helper.ExportStats(Program.fitList);
                }

                Thread.Sleep(100);
            }

        }

    }

    
}
