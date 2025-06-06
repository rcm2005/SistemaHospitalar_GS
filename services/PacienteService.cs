using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SistemaHospitalarApp.Models;
using SistemaHospitalarApp.Utils;

namespace SistemaHospitalarApp.Services
{
    public static class PacienteService
    {
        private static readonly string CaminhoArquivo = "data/pacientes.json";

        public static List<Paciente> ObterPacientes()
        {
            try
            {
                Directory.CreateDirectory("data");
                var pacientes = JsonCryptoStorage.Ler<List<Paciente>>(CaminhoArquivo);
                return pacientes ?? new List<Paciente>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar pacientes: {ex.Message}");
                return new List<Paciente>();
            }
        }

        public static void SalvarPacientes(List<Paciente> pacientes)
        {
            try
            {
                JsonCryptoStorage.Salvar(CaminhoArquivo, pacientes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar pacientes: {ex.Message}");
            }
        }

        public static void AdicionarPaciente(Paciente novoPaciente)
        {
            if (novoPaciente == null)
            {
                Console.WriteLine("Paciente inválido.");
                return;
            }

            try
            {
                var pacientes = ObterPacientes();

                if (pacientes.Any(p => p.CPF == novoPaciente.CPF))
                {
                    Console.WriteLine("Já existe um paciente com esse CPF.");
                    return;
                }

                pacientes.Add(novoPaciente);
                SalvarPacientes(pacientes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar paciente: {ex.Message}");
            }
        }

        public static Paciente? ObterPorCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return null;

            var pacientes = ObterPacientes();
            return pacientes.FirstOrDefault(p => p.CPF == cpf);
        }

        public static void AdicionarLaudo(string cpf, Laudo laudo)
        {
            if (string.IsNullOrWhiteSpace(cpf) || laudo == null)
            {
                Console.WriteLine("Dados inválidos para adicionar laudo.");
                return;
            }

            try
            {
                var pacientes = ObterPacientes();
                var paciente = pacientes.FirstOrDefault(p => p.CPF == cpf);

                if (paciente != null)
                {
                    // adiciona na lista existente
                    paciente.Laudos.Add(laudo);
                    SalvarPacientes(pacientes);
                }
                else
                {
                    Console.WriteLine("Paciente não encontrado para adicionar laudo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar laudo: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se já existe paciente com este CPF, ignorando o índice fornecido (zero-based).
        /// </summary>
        public static bool ExisteCpfDuplicado(string cpf, int indiceIgnorar = -1)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            var pacientes = ObterPacientes();

            for (int i = 0; i < pacientes.Count; i++)
            {
                if (i == indiceIgnorar) 
                    continue;

                if (pacientes[i].CPF == cpf)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Atualiza o paciente na posição (índice zero-based). 
        /// Lança IndexOutOfRangeException se índice inválido.
        /// Checa duplicidade de CPF (ignorando o índice do próprio paciente).
        /// </summary>
        public static void AtualizarPacientePorIndice(int indiceZeroBased, Paciente pacienteAtualizado)
        {
            var pacientes = ObterPacientes();

            if (indiceZeroBased < 0 || indiceZeroBased >= pacientes.Count)
                throw new IndexOutOfRangeException("Índice de paciente inválido.");

            // Se o CPF foi alterado, verifica duplicidade (ignorando este índice)
            string cpfOriginal = pacientes[indiceZeroBased].CPF;
            if (pacienteAtualizado.CPF != cpfOriginal)
            {
                if (ExisteCpfDuplicado(pacienteAtualizado.CPF, indiceZeroBased))
                {
                    Console.WriteLine("Já existe outro paciente com esse CPF. Operação cancelada.");
                    return;
                }
            }

            // Substitui campos
            pacientes[indiceZeroBased].Nome = pacienteAtualizado.Nome;
            pacientes[indiceZeroBased].CPF = pacienteAtualizado.CPF;
            pacientes[indiceZeroBased].Idade = pacienteAtualizado.Idade;
            pacientes[indiceZeroBased].Diagnostico = pacienteAtualizado.Diagnostico;

            // Mantém Laudos existentes (não faz setter, apenas preserva a lista atual)
            // pacientes[indiceZeroBased].Laudos permanece intocado a menos que você queira sobrescrever.

            SalvarPacientes(pacientes);
        }

        /// <summary>
        /// Remove o paciente no índice (zero-based). Lança IndexOutOfRangeException se inválido.
        /// </summary>
        public static void RemoverPacientePorIndice(int indiceZeroBased)
        {
            var pacientes = ObterPacientes();

            if (indiceZeroBased < 0 || indiceZeroBased >= pacientes.Count)
                throw new IndexOutOfRangeException("Índice de paciente inválido.");

            pacientes.RemoveAt(indiceZeroBased);
            SalvarPacientes(pacientes);
        }

        /// <summary>
        /// Retorna a quantidade de pacientes cadastrados.
        /// </summary>
        public static int ContarPacientes()
        {
            return ObterPacientes().Count;
        }
    }
}
