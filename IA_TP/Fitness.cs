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
            int totalTurnosDuplos = 0, totalTurnosSingle = 0; //Total de turnos por dia
            int[] totaisTurnos = new int[Cromosoma.totalFuncionarios];
            double desvio;

            #region CICLO_HORIZONTAL
            //Variaveis necessarias para o ciclo horizontal
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

                    //Contar quantos dias um funcionario tem de fazer dois turnos ou um só turno
                    if (valorAtual >= 1 && valorAtual <= 3)
                        totalTurnosSingle++;
                    if (valorAtual >= 4 && valorAtual <= 6)
                        totalTurnosDuplos++;

                    //Garantir que a proxima posição ainda é do funcionario da linha atual
                    if (pos + 1 < finalFuncPos)
                    {
                        proximoValor = (int)genes[pos + 1].Value;

                        //Não	se	podem	fazer	3	turnos	seguidos
                        if (valorAtual == 3 && proximoValor == 4)
                            acumulado += 3;

                        if (valorAtual == 5 && (proximoValor == 1 || proximoValor == 4 || proximoValor == 6))
                            acumulado += 3;

                        if (valorAtual == 6 && proximoValor == 1)
                            acumulado += 3;
                        //-----------------


                        //Se tiver folga ao sabado e domingo adiciona
                        if ((diaAtual != 1) && ((diaAtual - 1) % 6 == 0 && valorAtual == 0))
                        {
                            if (proximoValor == 0)
                                totalFolgaDuplaFDS++;
                        }

                        //Se for uma folga dupla adiciona
                        if (valorAtual == 0 && proximoValor == 0)
                            totalFolgaDupla++;

                        //Quando um	dia	de	folga é	no fim de semana, a	folga deve ser de dois dias	consecutivos ------
                        if ((diaAtual != 1) && ((diaAtual - 1) % 6 == 0)) //Se for sabado
                        {
                            if (valorAtual == 0) //Se for folga
                            {
                                if (proximoValor == 0)
                                {
                                    acumulado -= 0;
                                }
                                else
                                {
                                    acumulado += 1;
                                }
                            }
                                
                        }

                        if ((diaAtual != 1) && ((diaAtual) % 7 == 0)) // Se for domingo
                        {
                            if (valorAtual == 0) //Se for folga
                            {
                                if (proximoValor == 0)
                                {
                                    acumulado -= 0;
                                }
                                else
                                {
                                    acumulado += 1;
                                }
                            }
                        }
                        //----------------------------------

                    }


                    //Não	se	pode	trabalhar	mais	do	que	7	dias	consecutivos
                    if (valorAtual != 0)
                        diasConsecutivos++;
                    else
                        diasConsecutivos = 0;

                    if (diasConsecutivos > 7)
                    {
                        acumulado += 3;
                        diasConsecutivos = 0;
                    }
                    //-------------------------------


                    //Não	se	pode	trabalhar	no	turno	da	noite	mais	do	que	3	noites	consecutivas
                    if (valorAtual == 3 || valorAtual == 5 || valorAtual == 6)
                        noitesConsecutivas++;
                    else
                        noitesConsecutivas = 0;

                    if (noitesConsecutivas > 3)
                    {
                        acumulado += 3;
                        noitesConsecutivas = 0;
                    }
                    //----------------------


                    //Não se pode fazer mais do que 20 turnos em 4 semanas
                    totalTurnos += QuantosTurnos(valorAtual);

                    //Acrescentar turno
                    totaisTurnos[i] += QuantosTurnos(valorAtual);

                    //A cada 4 semanas (28 dias)
                    if ((diaAtual != 1) && diaAtual % (7 * 4) == 0)
                    {

                        //Check da regra dos 20 turnos
                        if (totalTurnos > 20)
                            acumulado += 3;

                        //Check da regra da folga ao fim de semana
                        if (totalFolgaDuplaFDS < 1)
                            acumulado++;

                        //Check da regra de pelo menos 2 folgas duplas
                        if (totalFolgaDupla < 2)
                            acumulado++;
                            
                        //reiniciar variaveis, ja passou 4 semanas
                        totalTurnos = 0;
                        totalFolgaDuplaFDS = 0;
                        totalFolgaDupla = 0;
                    }
                    //--------------------

                    //Incrementar dia Atual
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

                    //Turno da manha
                    if (valorAtual == 1 || valorAtual == 4 || valorAtual == 6) 
                        totalFuncionariosTM++;

                    //Turno da Tarde
                    if (valorAtual == 2 || valorAtual == 4 || valorAtual == 5) 
                        totalFuncionariosTT++;

                    //Turno da noite
                    if (valorAtual == 3 || valorAtual == 5 || valorAtual == 6) 
                        totalFuncionariosTN++;

                }

                //Se o dia e fim de semana
                if ((diaAtual != 1) && (diaAtual % 7 == 0 || (diaAtual - 1) % 6 == 0))
                {
                    //Turno da manha
                    if (totalFuncionariosTM < Cromosoma.TM_FDS)
                        acumulado += 2;
                    else if (totalFuncionariosTM > Cromosoma.TM_FDS)
                        acumulado++;

                    //Turno da tarde
                    if (totalFuncionariosTT < Cromosoma.TT_FDS)
                        acumulado += 2;
                    else if (totalFuncionariosTT > Cromosoma.TT_FDS)
                        acumulado++;

                    //Turno da noite
                    if (totalFuncionariosTN < Cromosoma.TN_FDS)
                        acumulado += 2;
                    else if (totalFuncionariosTN > Cromosoma.TN_FDS)
                        acumulado++;

                }
                else //Dias Uteis
                {
                    //Turno da manha
                    if (totalFuncionariosTM < Cromosoma.TM_SEMANA)
                        acumulado += 2;
                    else if (totalFuncionariosTM > Cromosoma.TM_SEMANA)
                        acumulado++;

                    //Turno da tarde
                    if (totalFuncionariosTT < Cromosoma.TT_SEMANA)
                        acumulado += 2;
                    else if (totalFuncionariosTT < Cromosoma.TT_SEMANA)
                        acumulado++;

                    //Turno da noite
                    if (totalFuncionariosTN < Cromosoma.TN_SEMANA)
                        acumulado += 2;
                    else if (totalFuncionariosTN > Cromosoma.TN_SEMANA)
                        acumulado++;

                }

                diaAtual++;
            }

            //Calculos a fazer sobre o calendario inteiro

            //Benificiar aqueles que cansam menos os funcionarios
            if (totalTurnosSingle > totalTurnosDuplos)
                acumulado -= 2;
            else
                acumulado += 1;

            //Desvio padrão do total de turnos de todos os trbalhadores
            desvio = DesvioPadrao(totaisTurnos);

            //Se o desvio padrão for menor que 3 benificiar, se for maior que 4 penalizar
            if (desvio == 0.0)
                acumulado -= 3;
            else if (desvio < 3)
                acumulado -= 1;
            else if (desvio > 4)
                acumulado += (int)desvio - 3;

            #endregion

            //Se acumulado for menor ou igual a 1 nem vale a pena fazer conta, o fitness é 100
            if (acumulado > 1)
                return 100.00 / acumulado;
            else
                return 100.00;
        }

        //Calcula o desvio padrão de um conjunto de dados
        private double DesvioPadrao(int[] array)
        {
            int tamanho = array.Length;
            int soma = 0;
            int media;
            double desvioPadrao = 0;

            //Somar valores
            for (int i = 0; i < tamanho; i++)
            {
                soma += array[i];
            }

            media = soma / tamanho;

            for (int i = 0; i < tamanho; i++)
            {
                desvioPadrao += Math.Pow(array[i] - media, 2);
            }

            desvioPadrao = desvioPadrao / (tamanho - 1);
            desvioPadrao = Math.Sqrt(desvioPadrao);

            return desvioPadrao;
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
