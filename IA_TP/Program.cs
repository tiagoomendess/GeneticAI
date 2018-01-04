using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IA_TP
{
    class Program
    {
        static ISelection selection = new EliteSelection();
        static ICrossover crossover = new UniformCrossover(0.8f);
        static IMutation mutation = new UniformMutation();
        static Fitness fitness = new Fitness();
        static Cromosoma chromosome = new Cromosoma();
        static Population population = new Population(2000, 2000, chromosome);
        public static GeneticAlgorithm ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
        
        public static string fitList;

        static void Main(string[] args)
        {

            Console.WriteLine("A iniciar programa...");
            fitList = "";
            Thread t, j;

            ga.Termination = new OrTermination(new GenerationNumberTermination(5000000), new FitnessThresholdTermination(99.999), new MyTermination());

            Console.WriteLine("Algoritmo a correr...");

            //Event handler para ser executado a cada geração
            ga.GenerationRan += delegate
            {

                    Console.Clear();
                    var bestChromosome = ga.Population.BestChromosome;
                    Console.WriteLine("Melhor Cromosoma (Fitness {0}):", bestChromosome.Fitness);
                    Console.WriteLine(Cromosoma.BuildCalendar(ga.BestChromosome.GetGenes()));
                    Console.WriteLine("Geração: {0}", ga.Population.GenerationsNumber);
                    Console.WriteLine("Tempo: {0}h {1}m {2}s", ga.TimeEvolving.Hours, ga.TimeEvolving.Minutes, ga.TimeEvolving.Seconds);

            };

            //Ouvir o teclado para poder parar a meio se pressionar a tecla p
            t = new Thread(new MyTermination().Run4Ever);
            t.Start();

            //Gravar stats em real time
            j = new Thread(new Helper().AddFitnessToList);
            j.Start();

            //Começar
            ga.Start();
            fitList += ga.Population.BestChromosome.Fitness + "\n";

            Console.Clear();
            Console.WriteLine("Melhor Cromosoma (Fitness {0}):", ga.Population.BestChromosome.Fitness);
            Console.WriteLine(Cromosoma.BuildCalendar(ga.Population.BestChromosome.GetGenes()));
            Console.WriteLine("Total de Gerações: {0}", ga.Population.GenerationsNumber);
            Console.WriteLine("Tempo Total: {0}h {1}m {2}s", ga.TimeEvolving.Hours, ga.TimeEvolving.Minutes, ga.TimeEvolving.Seconds);
            Console.WriteLine("O Algoritmo Parou!");

            Helper.ExportCsv(ga.BestChromosome);
            Helper.ExportStats(fitList);
            Console.ReadLine();
        }

    }
}
