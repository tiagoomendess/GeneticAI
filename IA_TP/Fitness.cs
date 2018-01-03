using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP
{
    class Fitness : IFitness
    {

        Gene[] genes;
        int tamanho;

        public double Evaluate(IChromosome chromosome)
        {
            int acumulado = 0; //Mais é pior

            genes = chromosome.GetGenes();
            tamanho = genes.Length;

            #region CICLO_HORIZONTAL
            //Variaveis necessarias para o ciclo principal
            int finalFuncPos;
            int valorAtual, proximoValor;
            int diasConsecutivos, totalTurnos, noitesConsecutivas;
            int pos;
            int diaAtual;
            int totalFolgaDuplaFDS;
            int totalFolgaDupla;

            //Este ciclo calcula a Regra dos 3 turnos e dos 7 dias consecutivos
            //1º ciclo percorre na vertical
            for (int i = 0; i < Cromosoma.totalFuncionarios; i++)
            {

                //Reiniciar variaveis, passou a outro funcionario
                finalFuncPos = (i * Cromosoma.totalDias) + Cromosoma.totalDias;
                diasConsecutivos = 0;
                totalTurnos = 0;
                noitesConsecutivas = 0;
                diaAtual = 1;
                totalFolgaDuplaFDS = 0;
                totalFolgaDupla = 0;

                //Segundo ciclo percorre na horizontal
                for (int j = 0; j < Cromosoma.totalDias; j++)
                {

                    pos = (i * Cromosoma.totalDias) + j;
                    valorAtual = (int)genes[pos].Value;

                    //Não	se	podem	fazer	3	turnos	seguidos
                    if (pos + 1 < finalFuncPos)
                    {
                        proximoValor = (int)genes[pos + 1].Value;

                        if (valorAtual == 3 && proximoValor == 4)
                            acumulado += 20;

                        if (valorAtual == 5 && (proximoValor == 1 || proximoValor == 4 || proximoValor == 6))
                            acumulado += 20;

                        if (valorAtual == 6 && proximoValor == 1)
                            acumulado += 20;

                        //Se tiver folga ao sabado e domingo adiciona
                        if ((diaAtual != 1) && ((diaAtual - 1) % 6 == 0 && valorAtual == 0))
                        {
                            if (proximoValor == 0)
                                totalFolgaDuplaFDS++;
                        }

                        //No	período	de	um	mês	deve	existir	pelo	menos	duas	folgas	com	dois	dias	consecutivos
                        if (valorAtual == 0 && proximoValor == 0)
                            totalFolgaDupla++;

                        //Quando um	dia	de	folga	é	no	fim	de	semana,	a	folga	deve	ser	de	dois	dias	consecutivos
                        if ((diaAtual != 1) && ((diaAtual - 1) % 6 == 0)) //Para o sabado
                        {
                            if (valorAtual == 0 && proximoValor == 0)
                                acumulado -= 0;
                        }

                        if ((diaAtual != 1) && ((diaAtual) % 7 == 0)) // Para o domingo
                        {
                            if (valorAtual == 0 && proximoValor == 0)
                                acumulado -= 0;
                        }
                    }

                    //Não	se	pode	trabalhar	mais	do	que	7	dias	consecutivos
                    if (valorAtual != 0)
                        diasConsecutivos++;
                    else
                        diasConsecutivos = 0;

                    if (diasConsecutivos > 7)
                    {
                        acumulado += 20;
                        diasConsecutivos = 0;
                    }

                    //Não	se	pode	trabalhar	no	turno	da	noite	mais	do	que	3	noites	consecutivas
                    if (valorAtual == 3 || valorAtual == 5 || valorAtual == 6)
                        noitesConsecutivas++;
                    else
                        noitesConsecutivas = 0;

                    if (noitesConsecutivas > 3)
                    {
                        acumulado += 20;
                        noitesConsecutivas = 0;
                    }
                    //----------------------

                    //Não se pode fazer mais do que 20 turnos em 4 semanas
                    totalTurnos += QuantosTurnos(valorAtual);

                    if ((diaAtual != 1) && diaAtual % (7 * 4) == 0) //A cada 4 semanas
                    {

                        //Check da regra dos 20 turnos
                        if (totalTurnos > 20)
                            acumulado += 40 * (totalTurnos - 20); //Quanto mais turnos tiver a mais, mais é penalizado

                        //Check da regra da folga ao fim de semana
                        if (totalFolgaDuplaFDS != 1)
                            acumulado += 40 * Math.Abs(totalFolgaDuplaFDS);

                        //Check da regra de pelo menos 2 folgas duplas
                        if (totalFolgaDupla != 2)
                        {
                            acumulado += 40 * Math.Abs(totalFolgaDuplaFDS);
                        }

                        //reiniciar variaveis, ja passou 4 semanas
                        totalTurnos = 0;
                        totalFolgaDuplaFDS = 0;
                        totalFolgaDupla = 0;
                    }

                    diaAtual++;
                }


            }
            #endregion

            #region CICLO_VERTICAL
            int totalFuncionariosTM, totalFuncionariosTT, totalFuncionariosTN;

            diaAtual = 1;

            for (int i = 0; i < Cromosoma.totalDias; i++)
            {

                //reiniciar Variaveis, passou para outro dia
                totalFuncionariosTM = totalFuncionariosTT = totalFuncionariosTN = 0;

                for (int j = 0; j < Cromosoma.totalFuncionarios; j++)
                {

                    pos = (j * Cromosoma.totalDias) + i;
                    valorAtual = (int)genes[pos].Value;

                    if (valorAtual == 1 || valorAtual == 4 || valorAtual == 6) //Turno da manha
                        totalFuncionariosTM++;

                    if (valorAtual == 2 || valorAtual == 4 || valorAtual == 5) //Turno da Tarde
                        totalFuncionariosTT++;

                    if (valorAtual == 3 || valorAtual == 5 || valorAtual == 6) //Turno da noite
                        totalFuncionariosTN++;

                }

                //Se o dia e fim de semana
                if ((diaAtual != 1) && (diaAtual % 7 == 0 || (diaAtual - 1) % 6 == 0))
                {
                    //Turno da manha
                    if (totalFuncionariosTM == Cromosoma.TM_FDS)
                        acumulado -= 0;
                    else
                        acumulado += 50 * Math.Abs(totalFuncionariosTM - Cromosoma.TM_FDS);

                    //Turno da tarde
                    if (totalFuncionariosTT == Cromosoma.TT_FDS)
                        acumulado -= 0;
                    else
                        acumulado += 50 * Math.Abs(totalFuncionariosTM - Cromosoma.TT_FDS);

                    //Turno da noite
                    if (totalFuncionariosTN == Cromosoma.TN_FDS)
                        acumulado -= 0;
                    else
                        acumulado += 50 * Math.Abs(totalFuncionariosTM - Cromosoma.TN_FDS);

                }
                else //Dias Uteis
                {
                    //Turno da manha
                    if (totalFuncionariosTM == Cromosoma.TM_SEMANA)
                        acumulado -= 0;
                    else
                        acumulado += 50 * Math.Abs(totalFuncionariosTM - Cromosoma.TM_SEMANA);

                    //Turno da tarde
                    if (totalFuncionariosTT == Cromosoma.TT_SEMANA)
                        acumulado -= 0;
                    else
                        acumulado += 50 * Math.Abs(totalFuncionariosTM - Cromosoma.TT_SEMANA);

                    //Turno da noite
                    if (totalFuncionariosTN == Cromosoma.TN_SEMANA)
                        acumulado -= 0;
                    else
                        acumulado += 50 * Math.Abs(totalFuncionariosTM - Cromosoma.TN_SEMANA);
                }

                diaAtual++;
            }

            #endregion

            if (acumulado > 0)
                return 100.00 / acumulado;
            else
                return 100.00;
        }

        /// <summary>
        /// Retorna o numero de turnos que fez para o codigo dado
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int QuantosTurnos(int i)
        {
            if (i > 0 && i <= 3)
                return 1;
            else if (i >= 4 && i <= 6)
                return 2;
            else
                return 0;
        }

    }
}
