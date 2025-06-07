using SistemaHospitalarApp.Models;
using SistemaHospitalarApp.Services;

class Program
{
    static void Main()
    {
        while (true)
        {
            var todosUsuarios = UsuarioService.ObterUsuarios();
            if (todosUsuarios.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nenhum usuário cadastrado. Vamos criar o primeiro usuário.\n");
                bool cadastrado = CadastrarUsuario();

                if (!cadastrado)
                {
                    Console.WriteLine("Não foi possível cadastrar. Tente novamente.");
                    Console.WriteLine("Pressione Enter para continuar...");
                    Console.ReadLine();
                    continue;
                }
            }
            break;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Sistema Hospitalar ===");
            Console.WriteLine("[1] Login");
            Console.WriteLine("[2] Cadastrar novo usuário");
            Console.WriteLine("[0] Sair");
            Console.Write("Escolha uma opção: ");

            string? opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    FazerLogin();
                    break;
                case "2":
                    bool cadastrado = CadastrarUsuario();
                    if (cadastrado)
                    {
                        Console.WriteLine("Pressione Enter para continuar...");
                        Console.ReadLine();
                    }
                    break;
                case "0":
                    Console.WriteLine("Saindo...");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void FazerLogin()
    {
        Console.Clear();
        Console.WriteLine("=== Login ===");

        try
        {
            Console.Write("Usuário: ");
            string? nomeEntrada = Console.ReadLine();
            string nome = nomeEntrada?.Trim() ?? "";

            Console.Write("Senha: ");
            string? senhaEntrada = Console.ReadLine();
            string senha = senhaEntrada ?? "";

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(senha))
            {
                Console.WriteLine("Campos obrigatórios. Pressione Enter para continuar.");
                Console.ReadLine();
                return;
            }

            Usuario? usuario = UsuarioService.Autenticar(nome, senha);
            if (usuario == null)
            {
                Console.WriteLine("Usuário ou senha inválidos. Pressione Enter para continuar.");
                Console.ReadLine();
                return;
            }

            AlertaListener.Iniciar();
            Console.WriteLine("\n[INFO] AlertaListener iniciado. Aguardando requisições POST em /alerta/ …");

            LogService.CarregarLogs();

            Console.WriteLine("Pressione ENTER para abrir o Menu Principal.");
            Console.ReadLine();

            MenuPrincipal.Mostrar(usuario);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro durante login: {ex.Message}");
            Console.WriteLine("Pressione Enter para continuar.");
            Console.ReadLine();
        }
    }

    static bool CadastrarUsuario()
    {
        Console.Clear();
        Console.WriteLine("=== Cadastro de Usuário ===");

        try
        {
            Console.Write("Nome: ");
            string? nomeEntrada = Console.ReadLine();
            string nome = nomeEntrada?.Trim() ?? "";

            Console.Write("Senha: ");
            string? senhaEntrada = Console.ReadLine();
            string senha = senhaEntrada ?? "";

            Console.Write("Cargo (Admin, Técnico, Médico): ");
            string? cargoEntrada = Console.ReadLine();
            string cargo = cargoEntrada?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(senha) ||
                string.IsNullOrWhiteSpace(cargo))
            {
                Console.WriteLine("Todos os campos são obrigatórios. Pressione Enter para continuar.");
                Console.ReadLine();
                return false;
            }

            var usuario = new Usuario(nome, senha, cargo);
            bool sucesso = UsuarioService.AdicionarUsuario(usuario);
            if (sucesso)
                Console.WriteLine("Usuário cadastrado com sucesso!");
            return sucesso;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar usuário: {ex.Message}");
            Console.WriteLine("Pressione Enter para continuar.");
            Console.ReadLine();
            return false;
        }
    }
}
