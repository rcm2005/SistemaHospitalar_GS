using System;
using System.Collections.Generic;
using SistemaHospitalarApp.Models;
using SistemaHospitalarApp.Services;

namespace SistemaHospitalarApp.Services
{
    public static class MenuPrincipal
    {
        public static void Mostrar(Usuario usuario)
        {
            bool sair = false;

            while (!sair)
            {
                Console.Clear();
                Console.WriteLine("=== Menu Principal ===");
                Console.WriteLine($"Usuário logado: {usuario.Nome} ({usuario.Cargo})\n");

                switch (usuario.Cargo.ToLower())
                {
                    case "admin":
                        MenuAdmin();
                        break;
                    case "técnico":
                    case "tecnico":
                        MenuTecnico();
                        break;
                    case "médico":
                    case "medico":
                        MenuMedico();
                        break;
                    default:
                        Console.WriteLine("Cargo não reconhecido. Encerrando sessão...");
                        sair = true;
                        break;
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        private static void MenuAdmin()
        {
            Console.WriteLine("1. Cadastrar Paciente");
            Console.WriteLine("2. Visualizar Pacientes");
            Console.WriteLine("3. Modificar Paciente");
            Console.WriteLine("4. Deletar Paciente");
            Console.WriteLine("5. Visualizar Logs de Eventos");
            Console.WriteLine("6. Sincronizar com a Nuvem");
            Console.WriteLine("7. Gerar alerta de teste");
            Console.WriteLine("8. Sair");
            Console.Write("\nEscolha uma opção: ");

            string? opcao = Console.ReadLine();
            ExecutarOpcaoAdmin(opcao);
        }

        private static void ExecutarOpcaoAdmin(string? opcao)
        {
            switch (opcao)
            {
                case "1":
                    CadastrarPaciente();
                    break;
                case "2":
                    VisualizarPacientes();
                    break;
                case "3":
                    ModificarPaciente();
                    break;
                case "4":
                    DeletarPaciente();
                    break;
                case "5":
                    VisualizarLogs();
                    break;
                case "6":
                    new Sincronizador().SincronizarAsync().GetAwaiter().GetResult();
                    break;
                case "7":
                    GerarAlertaDeTeste();
                    break;
                case "8":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }

        private static void MenuTecnico()
        {
            Console.WriteLine("1. Visualizar Logs de Eventos");
            Console.WriteLine("2. Sincronizar com a Nuvem");
            Console.WriteLine("3. Gerar alerta de teste");
            Console.WriteLine("4. Sair");
            Console.Write("\nEscolha uma opção: ");

            string? opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    VisualizarLogs();
                    break;
                case "2":
                    new Sincronizador().SincronizarAsync().GetAwaiter().GetResult();
                    break;
                case "3":
                    GerarAlertaDeTeste();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }

        private static void MenuMedico()
        {
            Console.WriteLine("1. Cadastrar Paciente");
            Console.WriteLine("2. Visualizar Logs de Eventos");
            Console.WriteLine("3. Gerar alerta de teste");
            Console.WriteLine("4. Sair");
            Console.Write("\nEscolha uma opção: ");

            string? opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    CadastrarPaciente();
                    break;
                case "2":
                    VisualizarLogs();
                    break;
                case "3":
                    GerarAlertaDeTeste();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }

        private static void CadastrarPaciente()
        {
            try
            {
                // Nome (não vazio)
                string nome;
                while (true)
                {
                    Console.Write("Nome do paciente: ");
                    nome = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(nome))
                    {
                        nome = nome.Trim();
                        break;
                    }
                    Console.WriteLine("Nome não pode ficar em branco. Tente novamente.");
                }

                // CPF (11 dígitos numéricos + duplicidade)
                string cpf;
                while (true)
                {
                    Console.Write("CPF (11 dígitos, apenas números): ");
                    cpf = (Console.ReadLine() ?? "").Trim();
                    if (cpf.Length != 11 || !cpf.All(char.IsDigit))
                    {
                        Console.WriteLine("CPF inválido. Deve conter 11 dígitos numéricos.");
                        continue;
                    }
                    if (PacienteService.ExisteCpfDuplicado(cpf))
                    {
                        Console.WriteLine("Já existe um paciente cadastrado com esse CPF. Tente outro.");
                        continue;
                    }
                    break;
                }

                // Idade (inteiro >= 0)
                int idade;
                while (true)
                {
                    Console.Write("Idade: ");
                    string idadeTxt = (Console.ReadLine() ?? "").Trim();
                    if (int.TryParse(idadeTxt, out idade) && idade >= 0)
                    {
                        break;
                    }
                    Console.WriteLine("Idade inválida. Digite um número inteiro não-negativo.");
                }

                // Diagnóstico (não vazio)
                string diagnostico;
                while (true)
                {
                    Console.Write("Diagnóstico: ");
                    diagnostico = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(diagnostico))
                    {
                        diagnostico = diagnostico.Trim();
                        break;
                    }
                    Console.WriteLine("Diagnóstico não pode ficar em branco. Tente novamente.");
                }

                var novoPaciente = new Paciente(nome, cpf, idade, diagnostico);
                PacienteService.AdicionarPaciente(novoPaciente);
                Console.WriteLine("\nPaciente cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar paciente: {ex.Message}");
            }
        }

        private static void VisualizarPacientes()
        {
            var pacientes = PacienteService.ObterPacientes();
            if (pacientes == null || pacientes.Count == 0)
            {
                Console.WriteLine("Não há pacientes cadastrados.");
                return;
            }

            Console.WriteLine("=== Lista de Pacientes ===");
            for (int i = 0; i < pacientes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {pacientes[i]}");
            }
        }

        private static void ModificarPaciente()
        {
            try
            {
                var pacientes = PacienteService.ObterPacientes();
                if (pacientes == null || pacientes.Count == 0)
                {
                    Console.WriteLine("Não há pacientes cadastrados.");
                    return;
                }

                Console.WriteLine("=== Lista de Pacientes Cadastrados ===");
                for (int i = 0; i < pacientes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {pacientes[i]}");
                }

                Console.Write("\nDigite o número do paciente que deseja modificar: ");
                string? selecionadoTxt = Console.ReadLine();
                if (!int.TryParse(selecionadoTxt, out int indice) ||
                    indice < 1 || indice > pacientes.Count)
                {
                    Console.WriteLine("Opção inválida. Voltando ao menu.");
                    return;
                }

                int idxZeroBased = indice - 1;
                var pacienteOriginal = pacientes[idxZeroBased];

                Console.WriteLine($"\n--- Dados Atuais do Paciente ({pacienteOriginal.Nome}) ---");
                Console.WriteLine($"1. Nome: {pacienteOriginal.Nome}");
                Console.WriteLine($"2. CPF: {pacienteOriginal.CPF}");
                Console.WriteLine($"3. Idade: {pacienteOriginal.Idade}");
                Console.WriteLine($"4. Diagnóstico: {pacienteOriginal.Diagnostico}");
                Console.WriteLine("-------------------------------\n");

                string novoNome;
                while (true)
                {
                    Console.Write("Novo nome (pressione Enter para manter): ");
                    string? campoNome = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(campoNome))
                    {
                        novoNome = pacienteOriginal.Nome;
                        break;
                    }
                    campoNome = campoNome.Trim();
                    if (campoNome.Length > 0)
                    {
                        novoNome = campoNome;
                        break;
                    }
                    Console.WriteLine("Nome não pode ficar vazio. Tente novamente ou pressione Enter para manter.");
                }

                string novoCpf;
                while (true)
                {
                    Console.Write("Novo CPF (11 dígitos, apenas números; pressione Enter para manter): ");
                    string? campoCpf = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(campoCpf))
                    {
                        novoCpf = pacienteOriginal.CPF;
                        break;
                    }
                    campoCpf = campoCpf.Trim();
                    if (campoCpf.Length != 11 || !campoCpf.All(char.IsDigit))
                    {
                        Console.WriteLine("CPF inválido. Deve conter 11 dígitos numéricos.");
                        continue;
                    }
                    if (PacienteService.ExisteCpfDuplicado(campoCpf, idxZeroBased))
                    {
                        Console.WriteLine("Já existe outro paciente com esse CPF. Tente outro ou pressione Enter para manter.");
                        continue;
                    }
                    novoCpf = campoCpf;
                    break;
                }

                int novaIdade;
                while (true)
                {
                    Console.Write($"Nova idade (pressione Enter para manter {pacienteOriginal.Idade}): ");
                    string? campoIdade = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(campoIdade))
                    {
                        novaIdade = pacienteOriginal.Idade;
                        break;
                    }
                    campoIdade = campoIdade.Trim();
                    if (int.TryParse(campoIdade, out int idadeTemp) && idadeTemp >= 0)
                    {
                        novaIdade = idadeTemp;
                        break;
                    }
                    Console.WriteLine("Idade inválida. Digite um número inteiro não-negativo ou pressione Enter para manter.");
                }

                string novoDiag;
                while (true)
                {
                    Console.Write("Novo diagnóstico (pressione Enter para manter): ");
                    string? campoDiag = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(campoDiag))
                    {
                        novoDiag = pacienteOriginal.Diagnostico;
                        break;
                    }
                    campoDiag = campoDiag.Trim();
                    if (campoDiag.Length > 0)
                    {
                        novoDiag = campoDiag;
                        break;
                    }
                    Console.WriteLine("Diagnóstico não pode ficar vazio. Tente novamente ou pressione Enter para manter.");
                }

                var pacienteAtualizado = new Paciente(novoNome, novoCpf, novaIdade, novoDiag);
                PacienteService.AtualizarPacientePorIndice(idxZeroBased, pacienteAtualizado);
                Console.WriteLine("\nPaciente atualizado com sucesso!");
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao modificar paciente: {ex.Message}");
            }
        }

        private static void DeletarPaciente()
        {
            try
            {
                var pacientes = PacienteService.ObterPacientes();
                if (pacientes == null || pacientes.Count == 0)
                {
                    Console.WriteLine("Não há pacientes cadastrados.");
                    return;
                }

                Console.WriteLine("=== Lista de Pacientes Cadastrados ===");
                for (int i = 0; i < pacientes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {pacientes[i]}");
                }

                Console.Write("\nDigite o número do paciente que deseja DELETAR: ");
                string? selecionadoTxt = Console.ReadLine();
                if (!int.TryParse(selecionadoTxt, out int indice) ||
                    indice < 1 || indice > pacientes.Count)
                {
                    Console.WriteLine("Opção inválida. Voltando ao menu.");
                    return;
                }

                int idxZeroBased = indice - 1;
                var paciente = pacientes[idxZeroBased];
                Console.Write($"\nTem certeza que deseja deletar o paciente \"{paciente.Nome}\"? (S/N): ");
                string? confirm = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(confirm) &&
                    (confirm.Trim().Equals("S", StringComparison.OrdinalIgnoreCase) ||
                     confirm.Trim().Equals("SIM", StringComparison.OrdinalIgnoreCase)))
                {
                    PacienteService.RemoverPacientePorIndice(idxZeroBased);
                    Console.WriteLine($"\nPaciente \"{paciente.Nome}\" deletado com sucesso!");
                }
                else
                {
                    Console.WriteLine("\nOperação cancelada. Nenhum paciente foi deletado.");
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar paciente: {ex.Message}");
            }
        }

        private static void VisualizarLogs()
        {
            var logs = LogService.ObterLogs();
            if (logs == null || logs.Count == 0)
            {
                Console.WriteLine("Não há logs disponíveis.");
                return;
            }

            Console.WriteLine("=== Logs de Eventos ===");
            foreach (var log in logs)
            {
                Console.WriteLine(log);
            }
        }

        private static void GerarAlertaDeTeste()
        {
            try
            {
                Console.WriteLine("\n=== Gerar Alerta de Teste ===");

                string origem;
                while (true)
                {
                    Console.Write("Origem do alerta: ");
                    origem = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(origem))
                    {
                        origem = origem.Trim();
                        break;
                    }
                    Console.WriteLine("Origem não pode ficar em branco. Tente novamente.");
                }

                string mensagem;
                while (true)
                {
                    Console.Write("Mensagem do alerta: ");
                    mensagem = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(mensagem))
                    {
                        mensagem = mensagem.Trim();
                        break;
                    }
                    Console.WriteLine("Mensagem não pode ficar em branco. Tente novamente.");
                }

                string gravidade;
                while (true)
                {
                    Console.Write("Gravidade (por exemplo: Baixa, Média, Alta, Crítica): ");
                    gravidade = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(gravidade))
                    {
                        gravidade = gravidade.Trim();
                        break;
                    }
                    Console.WriteLine("Gravidade não pode ficar em branco. Tente novamente.");
                }

                var alerta = new Alerta(origem, mensagem, gravidade);
                alerta.Exibir();
                LogService.SalvarLog(new LogEvento(alerta.ToString(), DateTime.Now, "ALERTA"));

                Console.WriteLine("\nAlerta de teste gerado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n[ERRO] {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERRO] Falha ao gerar alerta de teste: {ex.Message}");
            }
        }
    }
}
