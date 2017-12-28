using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP
{
    class Program
    {

        public static int DIAS = 28;
        static void Main(string[] args)
        {
            Console.WriteLine("A iniciar programa...");

            var selection = new EliteSelection();
            var crossover = new UniformCrossover();
            var mutation = new ReverseSequenceMutation();
            var fitness = new Fitness();
            var chromosome = new Cromosoma();
            var population = new Population(500, 500, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new OrTermination(new GenerationNumberTermination(100000), new FitnessThresholdTermination(99));

            Console.WriteLine("Algoritmo a correr...");

            ga.GenerationRan += delegate
            {
                Console.Clear();
                var bestChromosome = ga.Population.BestChromosome;
                Console.WriteLine("Gerações: {0}", ga.Population.GenerationsNumber);
                Console.WriteLine("Fitness: {0,10}", bestChromosome.Fitness);
                Console.WriteLine("Tempo: {0}h {1}m {2}s", ga.TimeEvolving.Hours, ga.TimeEvolving.Minutes, ga.TimeEvolving.Seconds);
                Cromosoma.BuildCalendar(bestChromosome.GetGenes());
            };

            ga.Start();

            Console.WriteLine("A melhor solução encontrada teve {0} de fitness.", ga.BestChromosome.Fitness);
            Console.WriteLine(Cromosoma.BuildCalendar(ga.BestChromosome.GetGenes()));
            Helper.ExportCsv(ga.BestChromosome);
            Console.ReadLine();
        }
    }
}
