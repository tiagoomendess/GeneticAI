using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP
{
    class MyCrossover : CrossoverBase
    {

        public MyCrossover() : base(2,2)
        {

        }
        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            throw new NotImplementedException();
        }
    }
}
