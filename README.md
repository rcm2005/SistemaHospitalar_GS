# Sistema Hospitalar – Gerenciamento de Pacientes e Alertas

Este projeto em C# (.NET 8) implementa um sistema de gerenciamento de pacientes e monitoramento de alertas para cenários de falha de energia em ambientes hospitalares. Inclui:

- Autenticação de Usuários (Admin, Técnico, Médico)  
- CRUD de Pacientes (Cadastrar, Listar, Modificar, Deletar)  
- Geração de Alerta de Teste Manual  
- Listener HTTP para Alertas Externos  
- Registro e Visualização de Logs de Eventos  
- Sincronização de Dados (Pacientes + Logs) com Servidor Externo (opcional)

---

## Integrantes
- Henrique Pontes Olliveira – RM 98036  
- Rafael Carvalho Mattos – RM 99874  
- Rafael Autieri Dos Anjos – RM 550885  

---

## Estrutura de Pastas

```
csharp_sistema_hospitalar/
├─ SistemaHospitalarApp.sln
├─ SistemaHospitalarApp/
│   ├─ Program.cs
│   ├─ Services/
│   │   ├─ AlertaListener.cs
│   │   ├─ LogService.cs
│   │   ├─ PacienteService.cs
│   │   ├─ Sincronizador.cs
│   │   ├─ UsuarioService.cs
│   │   └─ MenuPrincipal.cs
│   ├─ Models/
│   │   ├─ Alerta.cs
│   │   ├─ LogEvento.cs
│   │   ├─ Paciente.cs
│   │   └─ Usuario.cs
│   ├─ Utils/
│   │   └─ JsonCryptoStorage.cs
│   └─ data/                   # arquivos JSON criptografados (usuários, pacientes)
├─ .gitignore
└─ README.md                  # este arquivo
```

---

## Tecnologias

- **.NET 8 SDK**  
- Linguagem C#  
- Console Application (apenas terminal)  
- HttpListener (escuta HTTP)  
- System.Text.Json (JSON)  
- Task.Run (listener em background)  

---

## Pré-requisitos

- **.NET 8 SDK** instalado localmente  
- Editor/IDE compatível com C# (.NET 8), ex.: Visual Studio 2022 ou VS Code  

---

## Instalação e Execução

1. Clone o repositório:

   git clone https://github.com/rcm2005/csharp_sistema_hospitalar.git
   cd csharp_sistema_hospitalar/SistemaHospitalarApp


2. Restaure pacotes e compile:

   dotnet restore
   dotnet build


3. Execute a aplicação:

   dotnet run

   - Se não houver usuários cadastrados, será solicitado criar o primeiro (Admin).  
   - Após login, o `AlertaListener` inicia automaticamente e escuta `http://localhost:6000/alerta/` em background.

4. (A ser implementado) Servidor de Teste para Sincronização:
   - Clone e rode o servidor de teste em outra máquina/terminal:

     git clone https://github.com/rcm2005/SistemaHospitalar_TestServer.git
     cd ServidorTesteSync
     dotnet restore
     dotnet run

   - O servidor ficará escutando `http://localhost:6000/sync` e imprimirá todo JSON recebido.


---

## Funcionalidades

1. **Login de Usuário**  
   - Usuários armazenados em `data/usuarios.json` (criptografado).  
   - Papéis: **Admin**, **Técnico**, **Médico**.

2. **CRUD de Pacientes**
   - **Cadastrar Paciente**: validações de Nome, CPF (11 dígitos numéricos), Idade (>= 0), Diagnóstico.  
   - **Visualizar Pacientes**: lista todos os registros.  
   - **Modificar Paciente**: atualiza dados existentes, validações idênticas.  
   - **Deletar Paciente**: confirmação antes de remover.

3. **Geração de Alerta de Teste Manual**
   - Usuário digita Origem, Mensagem e Gravidade; o alerta é exibido e registrado no log.

4. **Listener HTTP para Alertas Externos**
   - `AlertaListener` roda em `Task.Run` e usa `HttpListener` em `http://*:6000/alerta/`.  
   - Recebe POST JSON (`Alerta`), desserializa e chama `Exibir()` + `SalvarLog`.

5. **Registro de Logs de Eventos**
   - Classe `LogService` armazena objetos `LogEvento` em lista e imprime no console.  
   - Logs exibem timestamp, tipo e mensagem.

6. **Visualização de Relatórios de Logs**
   - Menu “Visualizar Logs de Eventos” lista todos os eventos em ordem cronológica.

7. **Sincronização com Servidor Externo (opcional)**
   - Classe `Sincronizador` monta objeto `{{ Timestamp, Pacientes, Logs }}` e faz POST para `http://localhost:6000/sync`.  
   - Registra sucesso/falha no log.

---

## Testando Funcionalidades

### 1. Cadastro e Gerenciamento de Pacientes
- Faça login como **Admin**, escolha “Cadastrar Paciente” e preencha dados válidos.  
- Verifique em “Visualizar Pacientes” que o registro foi adicionado.  
- Use “Modificar Paciente” e “Deletar Paciente” para conferir comportamento.

### 2. Geração de Alerta de Teste
- No menu correspondente (Admin, Técnico ou Médico), selecione “Gerar alerta de teste”.  
- Insira Origem, Mensagem e Gravidade.  
- Verifique no console que o alerta foi exibido e no log que ele foi registrado.

### 3. Recebendo Alerta Externo
- Em outra janela/terminal, execute:

  curl -X POST http://localhost:6000/alerta/        -H "Content-Type: application/json"        -d '{{"origem":"TesteCURL","mensagem":"Falha simulada","gravidade":"Alta"}}'

- Observe no console do sistema C# que o alerta externo foi recebido e registrado.
- Também é possível utilizar um módulo ESP32 para uma simulação mais imersiva

### 4. Sincronização com Servidor Externo (Opcional)
- Em outro terminal, rode o **Servidor de Teste**:

  cd ServidorTesteSync
  dotnet run

- No menu do sistema C#, escolha “Sincronizar com a nuvem”.  
- Verifique no console do servidor de teste que o JSON de pacientes e logs foi impresso.  
- No console do C#, observe “[✔] Dados sincronizados com o servidor na nuvem.” ou “[✘] ...Modo offline.”

---

## Link do Vídeo Demonstrativo (YouTube)



---

## Observações Finais

- Para alterar a porta do `AlertaListener`, edite `AlertaListener.cs` (prefixo `http://*:6000/alerta/`).  
- O arquivo `data/usuarios.json` é criptografado via `JsonCryptoStorage`.  
- Validações de CPF, idade e campos obrigatórios são feitas em `MenuPrincipal`.  
- Exceções de I/O e HTTP são tratadas em `try-catch` para garantir que o sistema não trave em uso normal.
