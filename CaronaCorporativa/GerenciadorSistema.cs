using System;
using System.Collections.Generic;

public class GerenciadorSistema
{
    private Tela tela;
    private List<Motorista> motoristas;
    private List<Passageiro> passageiros;
    private GerenciadorRotasUnificado gerenciadorUnificado; // Sistema unificado para relatórios
    
    // Classes CRUD
    private MotoristaCRUD motoristaCRUD;
    private PassageiroCRUD passageiroCRUD;

    public GerenciadorSistema()
    {
        tela = new Tela();
        motoristas = new List<Motorista>();
        passageiros = new List<Passageiro>();
        
        // Inicializa classes CRUD
        motoristaCRUD = new MotoristaCRUD();
        passageiroCRUD = new PassageiroCRUD();
        
        // Inicializa sistema unificado
        gerenciadorUnificado = new GerenciadorRotasUnificado(null, null, null);
        
        // Sincronizar dados compartilhados
        SincronizarDados();
        
        // Criar dados de teste para demonstrar funcionalidades
        CriarDadosTeste();
    }
    
    private void SincronizarDados()
    {
        // Garantir que ambos os módulos tenham acesso às mesmas listas
        var solicitacoesPassageiro = passageiroCRUD.ObterSolicitacoes();
        var solicitacoesMotorista = motoristaCRUD.ObterSolicitacoes();
        
        // Sincronizar solicitações - PassageiroCRUD é a fonte principal
        foreach (var solicitacao in solicitacoesPassageiro)
        {
            if (!solicitacoesMotorista.Any(s => s.Id == solicitacao.Id))
            {
                solicitacoesMotorista.Add(solicitacao);
            }
        }
    }
    
    private void CriarDadosTeste()
    {
        // Criar alguns usuários de teste para permitir login
        var motoristaTest = new Motorista
        {
            Nome = "João Silva (TESTE)",
            Cpf = "12345678901",
            Cargo = "Supervisor",
            Cnh = "123456789"
        };
        
        var passageiroTest = new Passageiro
        {
            Nome = "Maria Santos (TESTE)",
            Cpf = "10987654321",
            Cargo = "Analista",
            NumeroCartao = "987654321"
        };
        
        // Adicionar aos dados centrais
        motoristas.Add(motoristaTest);
        passageiros.Add(passageiroTest);
        
        // Sincronizar com os módulos CRUD
        SincronizarUsuarios();
    }
    
    private void SincronizarUsuarios()
    {
        // Sincronizar motoristas
        var motoristasModulo = motoristaCRUD.ObterMotoristas();
        foreach (var motorista in motoristas)
        {
            if (!motoristasModulo.Any(m => m.Cpf == motorista.Cpf))
            {
                motoristasModulo.Add(motorista);
            }
        }
        
        // Sincronizar passageiros
        var passageirosModulo = passageiroCRUD.ObterPassageiros();
        foreach (var passageiro in passageiros)
        {
            if (!passageirosModulo.Any(p => p.Cpf == passageiro.Cpf))
            {
                passageirosModulo.Add(passageiro);
            }
        }
    }

    public void IniciarSistema()
    {
        int opcao;
        
        do
        {
            string opcaoStr = tela.ExibirMenuPrincipalComRetorno();
            opcao = int.Parse(opcaoStr ?? "0");

            switch (opcao)
            {
                case 1:
                    MenuGestor();
                    break;
                case 2:
                    LoginMotorista();
                    break;
                case 3:
                    LoginPassageiro();
                    break;
                case 0:
                    tela.ExibirMensagem("Obrigado por usar o Sistema de Carona Corporativa!");
                    break;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    break;
            }
        } while (opcao != 0);
    }

    private void MenuGestor()
    {
        int opcao;
        
        do
        {
            string opcaoStr = tela.ExibirMenuGestorComRetorno();
            opcao = int.Parse(opcaoStr ?? "0");

            switch (opcao)
            {
                case 1:
                    GerenciarMotoristas();
                    break;
                case 2:
                    GerenciarPassageiros();
                    break;
                case 3:
                    GerenciarCaronas();
                    break;
                case 4:
                    GerenciarAlertas();
                    break;
                case 5:
                    GerenciarReembolsos();
                    break;
                case 6:
                    GerarRelatorios();
                    break;
                case 0:
                    break;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    break;
            }
        } while (opcao != 0);
    }

    private void GerenciarMotoristas()
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("GERENCIAR MOTORISTAS", "Administração de Motoristas");

            List<string> opcoes = new List<string>
            {
                "1 - Listar motoristas cadastrados",
                "2 - Cadastrar novo motorista",
                "3 - Buscar motorista por CPF",
                "4 - Editar dados de motorista",
                "5 - Remover motorista",
                "0 - Voltar"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    ListarMotoristas();
                    break;
                case "2":
                    CadastrarNovoMotorista();
                    break;
                case "3":
                    BuscarMotoristaPorCPF();
                    break;
                case "4":
                    EditarMotorista();
                    break;
                case "5":
                    RemoverMotorista();
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opção inválida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void ListarMotoristas()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("MOTORISTAS CADASTRADOS", "Lista Completa");

        // Listar motoristas cadastrados
        var motoristas = motoristaCRUD.ObterMotoristas();
        
        if (motoristas.Count == 0)
        {
            tela.ExibirMensagem("Nenhum motorista cadastrado no sistema.");
        }
        else
        {
            Console.SetCursorPosition(2, 8);
            Console.WriteLine($"Total de motoristas cadastrados: {motoristas.Count}");
            Console.WriteLine();
            
            for (int i = 0; i < motoristas.Count; i++)
            {
                var motorista = motoristas[i];
                Console.WriteLine($"{i + 1}. {motorista.Nome}");
                Console.WriteLine($"   CPF: {motorista.Cpf}");
                Console.WriteLine($"   Cargo: {motorista.Cargo}");
                Console.WriteLine($"   CNH: {motorista.Cnh}");
                Console.WriteLine();
            }
        }
        
        tela.AguardarTecla();
    }

    private void CadastrarNovoMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR MOTORISTA", "Novo Motorista - Gestor");

        Motorista motorista = new Motorista();
        
        motorista.Nome = tela.LerTexto("Nome completo");
        motorista.Cpf = tela.LerTexto("CPF (somente numeros)");
        motorista.Cargo = tela.LerTexto("Cargo na empresa");
        motorista.Cnh = tela.LerTexto("Numero da CNH");

        // Verificar se CPF já existe
        var motoristas = motoristaCRUD.ObterMotoristas();
        if (motoristas.Any(m => m.Cpf == motorista.Cpf))
        {
            tela.ExibirErro("Ja existe um motorista cadastrado com este CPF!");
            tela.AguardarTecla();
            return;
        }

        if (tela.ConfirmarAcao("Confirma o cadastro?"))
        {
            // Adicionar à lista principal do GerenciadorSistema
            motoristas.Add(motorista);
            
            // Sincronizar com o módulo CRUD
            var motoristasModulo = motoristaCRUD.ObterMotoristas();
            motoristasModulo.Add(motorista);
            
            tela.ExibirSucesso("Motorista cadastrado com sucesso!");
            tela.ExibirMensagem("IMPORTANTE: O motorista pode agora acessar o sistema.");
            tela.AguardarTecla();
        }
    }

    private void BuscarMotoristaPorCPF()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("BUSCAR MOTORISTA", "Consulta por CPF");

        string cpf = tela.LerTexto("Digite o CPF para buscar");
        var motoristas = motoristaCRUD.ObterMotoristas();
        var encontrado = motoristas.FirstOrDefault(m => m.Cpf == cpf);
        
        if (encontrado != null)
        {
            Console.SetCursorPosition(2, 10);
            Console.WriteLine("=== MOTORISTA ENCONTRADO ===");
            Console.WriteLine($"Nome: {encontrado.Nome}");
            Console.WriteLine($"CPF: {encontrado.Cpf}");
            Console.WriteLine($"Cargo: {encontrado.Cargo}");
            Console.WriteLine($"CNH: {encontrado.Cnh}");
        }
        else
        {
            tela.ExibirErro("Motorista nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void EditarMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("EDITAR MOTORISTA", "Alteracao de Dados");
        
        tela.ExibirMensagem("Funcionalidade de edicao sera implementada.");
        tela.ExibirMensagem("Por enquanto, o motorista deve editar seus proprios dados pelo sistema.");
        tela.AguardarTecla();
    }

    private void RemoverMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("REMOVER MOTORISTA", "Exclusao de Cadastro");
        
        tela.ExibirMensagem("Funcionalidade de remocao sera implementada.");
        tela.ExibirMensagem("ATENCAO: A remocao deve cancelar todas as caronas ativas.");
        tela.AguardarTecla();
    }

    private void GerenciarPassageiros()
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("GERENCIAR PASSAGEIROS", "Administração de Passageiros");

            List<string> opcoes = new List<string>
            {
                "1 - Listar passageiros cadastrados",
                "2 - Cadastrar novo passageiro",
                "3 - Buscar passageiro por CPF", 
                "4 - Editar dados de passageiro",
                "5 - Remover passageiro",
                "6 - Ver solicitações de carona",
                "0 - Voltar"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    ListarPassageiros();
                    break;
                case "2":
                    CadastrarNovoPassageiro();
                    break;
                case "3":
                    BuscarPassageiroPorCPF();
                    break;
                case "4":
                    EditarPassageiro();
                    break;
                case "5":
                    RemoverPassageiro();
                    break;
                case "6":
                    VisualizarSolicitacoesCarona();
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opção inválida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void ListarPassageiros()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("PASSAGEIROS CADASTRADOS", "Lista Completa");

        // Listar passageiros cadastrados
        var passageiros = passageiroCRUD.ObterPassageiros();
        
        if (passageiros.Count == 0)
        {
            tela.ExibirMensagem("Nenhum passageiro cadastrado no sistema.");
        }
        else
        {
            Console.SetCursorPosition(2, 8);
            Console.WriteLine($"Total de passageiros cadastrados: {passageiros.Count}");
            Console.WriteLine();
            
            for (int i = 0; i < passageiros.Count; i++)
            {
                var passageiro = passageiros[i];
                Console.WriteLine($"{i + 1}. {passageiro.Nome}");
                Console.WriteLine($"   CPF: {passageiro.Cpf}");
                Console.WriteLine($"   Cargo: {passageiro.Cargo}");
                Console.WriteLine($"   Cartao: {passageiro.NumeroCartao}");
                Console.WriteLine();
            }
        }
        
        tela.AguardarTecla();
    }

    private void CadastrarNovoPassageiro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR PASSAGEIRO", "Novo Passageiro - Gestor");

        Passageiro passageiro = new Passageiro();
        
        passageiro.Nome = tela.LerTexto("Nome completo");
        passageiro.Cpf = tela.LerTexto("CPF (somente numeros)");
        passageiro.Cargo = tela.LerTexto("Cargo na empresa");
        passageiro.NumeroCartao = tela.LerTexto("Numero do cartao corporativo");

        // Verificar se CPF já existe
        var passageiros = passageiroCRUD.ObterPassageiros();
        if (passageiros.Any(p => p.Cpf == passageiro.Cpf))
        {
            tela.ExibirErro("Ja existe um passageiro cadastrado com este CPF!");
            tela.AguardarTecla();
            return;
        }

        if (tela.ConfirmarAcao("Confirma o cadastro?"))
        {
            // Adicionar à lista principal do GerenciadorSistema
            passageiros.Add(passageiro);
            
            // Sincronizar com o módulo CRUD
            var passageirosModulo = passageiroCRUD.ObterPassageiros();
            passageirosModulo.Add(passageiro);
            
            tela.ExibirSucesso("Passageiro cadastrado com sucesso!");
            tela.ExibirMensagem("IMPORTANTE: O passageiro pode agora acessar o sistema.");
            tela.AguardarTecla();
        }
    }

    private void BuscarPassageiroPorCPF()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("BUSCAR PASSAGEIRO", "Consulta por CPF");

        string cpf = tela.LerTexto("Digite o CPF para buscar");
        var passageiros = passageiroCRUD.ObterPassageiros();
        var encontrado = passageiros.FirstOrDefault(p => p.Cpf == cpf);
        
        if (encontrado != null)
        {
            Console.SetCursorPosition(2, 10);
            Console.WriteLine("=== PASSAGEIRO ENCONTRADO ===");
            Console.WriteLine($"Nome: {encontrado.Nome}");
            Console.WriteLine($"CPF: {encontrado.Cpf}");
            Console.WriteLine($"Cargo: {encontrado.Cargo}");
            Console.WriteLine($"Cartao: {encontrado.NumeroCartao}");
        }
        else
        {
            tela.ExibirErro("Passageiro nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void EditarPassageiro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("EDITAR PASSAGEIRO", "Alteracao de Dados");
        
        tela.ExibirMensagem("Funcionalidade de edicao sera implementada.");
        tela.ExibirMensagem("Por enquanto, o passageiro deve editar seus proprios dados pelo sistema.");
        tela.AguardarTecla();
    }

    private void RemoverPassageiro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("REMOVER PASSAGEIRO", "Exclusao de Cadastro");
        
        tela.ExibirMensagem("Funcionalidade de remocao sera implementada.");
        tela.ExibirMensagem("ATENCAO: A remocao deve cancelar todas as solicitacoes ativas.");
        tela.AguardarTecla();
    }

    private void VisualizarSolicitacoesCarona()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("SOLICITAÇÕES DE CARONA", "Monitoramento do Gestor");
        
        var solicitacoes = passageiroCRUD.ObterSolicitacoes();
        if (solicitacoes.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma solicitacao de carona no sistema.");
        }
        else
        {
            Console.SetCursorPosition(2, 8);
            Console.WriteLine($"Total de solicitacoes: {solicitacoes.Count}");
            Console.WriteLine();
            
            var agrupadas = solicitacoes.GroupBy(s => s.Status);
            foreach (var grupo in agrupadas)
            {
                Console.WriteLine($"Status '{grupo.Key}': {grupo.Count()} solicitacoes");
                
                foreach (var sol in grupo.Take(3)) // Mostrar apenas 3 por status
                {
                    Console.WriteLine($"  - {sol.CpfPassageiro}: {sol.EnderecoOrigem} -> {sol.EnderecoDestino}");
                }
                
                if (grupo.Count() > 3)
                {
                    Console.WriteLine($"  ... e mais {grupo.Count() - 3} solicitacoes");
                }
                Console.WriteLine();
            }
        }
        
        tela.AguardarTecla();
    }

    private void GerenciarCaronas()
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("GERENCIAR CARONAS", "Sistema Check-In/Check-Out");

            List<string> opcoes = new List<string>
            {
                "1 - Ver caronas ativas",
                "2 - Ver caronas pendentes check-in",
                "3 - Ver caronas em andamento", 
                "4 - Ver caronas finalizadas",
                "5 - Relatório de caronas",
                "0 - Voltar"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    VisualizarCaronasAtivas();
                    break;
                case "2":
                    VisualizarCaronasPendentesCheckIn();
                    break;
                case "3":
                    VisualizarCaronasEmAndamento();
                    break;
                case "4":
                    VisualizarCaronasFinalizadas();
                    break;
                case "5":
                    RelatorioCaronas();
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opção inválida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void VisualizarCaronasAtivas()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CARONAS ATIVAS", "Status: Aceitas");
        
        var caronasAceitas = passageiroCRUD.ObterSolicitacoes()
                                          .Where(s => s.Status == "Aceita")
                                          .OrderBy(s => s.DataSolicitacao)
                                          .ToList();

        if (caronasAceitas.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma carona com status 'Aceita' encontrada.");
            tela.AguardarTecla();
            return;
        }

        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"=== CARONAS ACEITAS ({caronasAceitas.Count}) ===");
        Console.WriteLine();

        foreach (var carona in caronasAceitas)
        {
            var passageiro = passageiroCRUD.ObterPassageiros()
                                          .FirstOrDefault(p => p.Cpf == carona.CpfPassageiro);
            
            Console.WriteLine($"ID: {carona.Id} | Passageiro: {passageiro?.Nome ?? carona.CpfPassageiro}");
            Console.WriteLine($"Rota: {carona.EnderecoOrigem} → {carona.EnderecoDestino}");
            Console.WriteLine($"Distância: {carona.DistanciaKm:F1}km | Aceita em: {carona.DataSolicitacao:dd/MM HH:mm}");
            
            if (!string.IsNullOrEmpty(carona.CpfMotorista))
            {
                var motorista = motoristaCRUD.ObterMotoristas()
                                           .FirstOrDefault(m => m.Cpf == carona.CpfMotorista);
                Console.WriteLine($"Motorista: {motorista?.Nome ?? carona.CpfMotorista}");
            }
            
            Console.WriteLine("Status: Aguardando check-in do passageiro");
            Console.WriteLine();
        }

        tela.AguardarTecla();
    }

    private void VisualizarCaronasPendentesCheckIn()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("PENDENTES CHECK-IN", "Status: Aguardando Check-In");
        
        var caronasPendentes = passageiroCRUD.ObterSolicitacoes()
                                           .Where(s => s.Status == "Aceita")
                                           .OrderBy(s => s.DataSolicitacao)
                                           .ToList();

        if (caronasPendentes.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma carona aguardando check-in.");
            tela.AguardarTecla();
            return;
        }

        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"=== CARONAS PENDENTES CHECK-IN ({caronasPendentes.Count}) ===");
        Console.WriteLine();

        foreach (var carona in caronasPendentes)
        {
            var passageiro = passageiroCRUD.ObterPassageiros()
                                          .FirstOrDefault(p => p.Cpf == carona.CpfPassageiro);
            
            var motorista = motoristaCRUD.ObterMotoristas()
                                       .FirstOrDefault(m => m.Cpf == carona.CpfMotorista);

            Console.WriteLine($"ID: {carona.Id}");
            Console.WriteLine($"Passageiro: {passageiro?.Nome ?? carona.CpfPassageiro}");
            Console.WriteLine($"Motorista: {motorista?.Nome ?? carona.CpfMotorista ?? "Não atribuído"}");
            Console.WriteLine($"Rota: {carona.EnderecoOrigem} → {carona.EnderecoDestino}");
            Console.WriteLine($"Aceita em: {carona.DataSolicitacao:dd/MM/yyyy HH:mm}");

            // Calcular tempo desde aceitação
            var tempoEspera = DateTime.Now - carona.DataSolicitacao;
            if (tempoEspera.TotalHours > 24)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"⚠️  ATENÇÃO: {tempoEspera.TotalDays:F0} dias sem check-in!");
                Console.ResetColor();
            }
            else if (tempoEspera.TotalHours > 2)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⏰ {tempoEspera.TotalHours:F0}h aguardando check-in");
                Console.ResetColor();
            }
            
            Console.WriteLine();
        }

        tela.AguardarTecla();
    }

    private void VisualizarCaronasEmAndamento()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CARONAS EM ANDAMENTO", "Status: Check-In Feito");
        
        var caronasEmAndamento = passageiroCRUD.ObterSolicitacoes()
                                              .Where(s => s.Status == "Check-in Feito")
                                              .OrderBy(s => s.DataCheckIn)
                                              .ToList();

        if (caronasEmAndamento.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma carona em andamento no momento.");
            tela.AguardarTecla();
            return;
        }

        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"=== CARONAS EM ANDAMENTO ({caronasEmAndamento.Count}) ===");
        Console.WriteLine();

        foreach (var carona in caronasEmAndamento)
        {
            var passageiro = passageiroCRUD.ObterPassageiros()
                                          .FirstOrDefault(p => p.Cpf == carona.CpfPassageiro);
            
            var motorista = motoristaCRUD.ObterMotoristas()
                                       .FirstOrDefault(m => m.Cpf == carona.CpfMotorista);

            Console.WriteLine($"ID: {carona.Id}");
            Console.WriteLine($"Passageiro: {passageiro?.Nome ?? carona.CpfPassageiro}");
            Console.WriteLine($"Motorista: {motorista?.Nome ?? carona.CpfMotorista ?? "Não atribuído"}");
            Console.WriteLine($"Rota: {carona.EnderecoOrigem} → {carona.EnderecoDestino}");
            Console.WriteLine($"Check-in: {carona.DataCheckIn?.ToString("dd/MM/yyyy HH:mm")}");

            // Calcular tempo em andamento
            if (carona.DataCheckIn.HasValue)
            {
                var tempoAndamento = DateTime.Now - carona.DataCheckIn.Value;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"⏱️  Em andamento há: {tempoAndamento.TotalMinutes:F0} minutos");
                Console.ResetColor();
            }
            
            Console.WriteLine("Status: Aguardando check-out do motorista");
            Console.WriteLine();
        }

        tela.AguardarTecla();
    }

    private void VisualizarCaronasFinalizadas()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CARONAS FINALIZADAS", "Status: Check-Out Feito");
        
        var caronasFinalizadas = passageiroCRUD.ObterSolicitacoes()
                                              .Where(s => s.Status == "Finalizada")
                                              .OrderByDescending(s => s.DataCheckOut)
                                              .Take(20) // Limitar aos últimos 20 para evitar sobrecarga
                                              .ToList();

        if (caronasFinalizadas.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma carona finalizada encontrada.");
            tela.AguardarTecla();
            return;
        }

        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"=== CARONAS FINALIZADAS (Últimas {caronasFinalizadas.Count}) ===");
        Console.WriteLine();

        foreach (var carona in caronasFinalizadas)
        {
            var passageiro = passageiroCRUD.ObterPassageiros()
                                          .FirstOrDefault(p => p.Cpf == carona.CpfPassageiro);
            
            var motorista = motoristaCRUD.ObterMotoristas()
                                       .FirstOrDefault(m => m.Cpf == carona.CpfMotorista);

            Console.WriteLine($"ID: {carona.Id}");
            Console.WriteLine($"Passageiro: {passageiro?.Nome ?? carona.CpfPassageiro}");
            Console.WriteLine($"Motorista: {motorista?.Nome ?? carona.CpfMotorista ?? "Não informado"}");
            Console.WriteLine($"Rota: {carona.EnderecoOrigem} → {carona.EnderecoDestino}");
            Console.WriteLine($"Finalizada: {carona.DataCheckOut?.ToString("dd/MM/yyyy HH:mm")}");

            // Calcular tempo total da carona
            if (carona.DataCheckIn.HasValue && carona.DataCheckOut.HasValue)
            {
                var duracaoCarona = carona.DataCheckOut.Value - carona.DataCheckIn.Value;
                Console.WriteLine($"Duração: {duracaoCarona.TotalMinutes:F0} minutos");
            }

            // Verificar elegibilidade para reembolso
            if (gerenciadorUnificado.EhElegivelParaReembolso(carona.DistanciaKm))
            {
                double valorReembolso = carona.DistanciaKm * 2.50;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"💰 Reembolso: R$ {valorReembolso:F2}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("💰 Sem reembolso corporativo");
                Console.ResetColor();
            }
            
            Console.WriteLine();
        }

        tela.AguardarTecla();
    }

    private void RelatorioCaronas()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("RELATÓRIO DE CARONAS", "Estatísticas Gerais");
        
        var todasCaronas = passageiroCRUD.ObterSolicitacoes();
        
        // Contadores por status
        int pendentes = todasCaronas.Count(s => s.Status == "Pendente");
        int aceitas = todasCaronas.Count(s => s.Status == "Aceita");
        int comCheckIn = todasCaronas.Count(s => s.Status == "Check-in Feito");
        int finalizadas = todasCaronas.Count(s => s.Status == "Finalizada");
        int canceladas = todasCaronas.Count(s => s.Status == "Cancelada");
        int rejeitadas = todasCaronas.Count(s => s.Status == "Rejeitada");
        
        int total = todasCaronas.Count();
        
        Console.SetCursorPosition(2, 8);
        Console.WriteLine("=== RELATÓRIO GERAL DE CARONAS ===");
        Console.WriteLine();
        Console.WriteLine($"Total de Solicitações: {total}");
        Console.WriteLine();
        Console.WriteLine("=== POR STATUS ===");
        Console.WriteLine($"Pendentes: {pendentes}");
        Console.WriteLine($"Aceitas: {aceitas}");
        Console.WriteLine($"Em Andamento (Check-in): {comCheckIn}");
        Console.WriteLine($"Finalizadas: {finalizadas}");
        Console.WriteLine($"Canceladas: {canceladas}");
        Console.WriteLine($"Rejeitadas: {rejeitadas}");
        Console.WriteLine();
        
        // Calcular taxas
        if (total > 0)
        {
            double taxaAceitacao = (double)(aceitas + comCheckIn + finalizadas) / total * 100;
            double taxaFinalizacao = finalizadas > 0 ? (double)finalizadas / (aceitas + comCheckIn + finalizadas) * 100 : 0;
            double taxaCheckIn = comCheckIn + finalizadas > 0 ? (double)(comCheckIn + finalizadas) / (aceitas + comCheckIn + finalizadas) * 100 : 0;
            
            Console.WriteLine("=== TAXAS DE SUCESSO ===");
            Console.WriteLine($"Taxa de Aceitação: {taxaAceitacao:F1}%");
            Console.WriteLine($"Taxa de Check-In: {taxaCheckIn:F1}%");
            Console.WriteLine($"Taxa de Finalização: {taxaFinalizacao:F1}%");
            Console.WriteLine();
        }
        
        // Estatísticas de reembolso
        var caronasComReembolso = todasCaronas.Where(s => gerenciadorUnificado.EhElegivelParaReembolso(s.DistanciaKm)).ToList();
        double valorTotalReembolsos = caronasComReembolso.Sum(c => c.DistanciaKm * 2.50);
        
        Console.WriteLine("=== REEMBOLSOS ===");
        Console.WriteLine($"Caronas Elegíveis: {caronasComReembolso.Count}");
        Console.WriteLine($"Valor Total: R$ {valorTotalReembolsos:F2}");
        Console.WriteLine();
        
        // Usuários mais ativos
        var passageirosAtivos = todasCaronas.GroupBy(s => s.CpfPassageiro)
                                          .Select(g => new { CPF = g.Key, Count = g.Count() })
                                          .OrderByDescending(x => x.Count)
                                          .Take(3)
                                          .ToList();
        
        if (passageirosAtivos.Any())
        {
            Console.WriteLine("=== TOP 3 PASSAGEIROS ===");
            foreach (var pa in passageirosAtivos)
            {
                var passageiro = passageiroCRUD.ObterPassageiros().FirstOrDefault(p => p.Cpf == pa.CPF);
                Console.WriteLine($"{passageiro?.Nome ?? pa.CPF}: {pa.Count} caronas");
            }
        }
        
        tela.AguardarTecla();
    }

    private void GerenciarAlertas()
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("GERENCIAR ALERTAS", "Centro de Notificações do Sistema");
            
            var alertasPendentes = GerenciadorAlertas.ObterAlertasPendentes();
            var todosAlertas = GerenciadorAlertas.ObterTodosAlertas();
            
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("=== RESUMO DE ALERTAS ===");
            Console.WriteLine($"🔴 Alertas pendentes: {alertasPendentes.Count}");
            Console.WriteLine($"📊 Total de alertas: {todosAlertas.Count}");
            Console.WriteLine($"🚫 Cancelamentos: {GerenciadorAlertas.ContarAlertasPorTipo("CANCELAMENTO")}");
            Console.WriteLine($"💰 Reembolsos: {GerenciadorAlertas.ContarAlertasPorTipo("REEMBOLSO")}");
            Console.WriteLine($"⚙️ Sistema: {GerenciadorAlertas.ContarAlertasPorTipo("SISTEMA")}");
            Console.WriteLine();

            if (alertasPendentes.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️  ATENÇÃO: Há alertas pendentes que requerem sua atenção!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Todos os alertas foram processados.");
                Console.ResetColor();
            }

            List<string> opcoes = new List<string>
            {
                "1 - Ver alertas pendentes",
                "2 - Ver todos os alertas", 
                "3 - Filtrar por tipo",
                "4 - Marcar alerta como resolvido",
                "5 - Limpar alertas resolvidos",
                "0 - Voltar"
            };

            int posicaoMenu = Math.Min(Console.CursorTop + 2, Console.WindowHeight - 10);
            string opcao = tela.MostrarMenu(opcoes, 10, posicaoMenu, "Escolha uma opção:");

            switch (opcao)
            {
                case "1":
                    VisualizarAlertasPendentes();
                    break;
                case "2":
                    VisualizarTodosAlertas();
                    break;
                case "3":
                    FiltrarAlertasPorTipo();
                    break;
                case "4":
                    MarcarAlertaComoResolvido();
                    break;
                case "5":
                    LimparAlertasResolvidos();
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opção inválida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void VisualizarAlertasPendentes()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ALERTAS PENDENTES", "Alertas que Requerem Atenção");

        var alertasPendentes = GerenciadorAlertas.ObterAlertasPendentes();

        if (alertasPendentes.Count == 0)
        {
            tela.ExibirMensagem("Não há alertas pendentes.");
        }
        else
        {
            Console.SetCursorPosition(2, 8);
            Console.WriteLine($"📋 Exibindo {alertasPendentes.Count} alertas pendentes:");
            Console.WriteLine();

            foreach (var alerta in alertasPendentes)
            {
                // Marcar como visualizado quando o gestor vê
                GerenciadorAlertas.MarcarComoVisualizado(alerta.IdAlerta);

                string tipoIcon = alerta.TipoAlerta switch
                {
                    "CANCELAMENTO" => "🚫",
                    "REEMBOLSO" => "💰",
                    "SISTEMA" => "⚙️",
                    _ => "📢"
                };

                Console.WriteLine($"{tipoIcon} Alerta #{alerta.IdAlerta} - {alerta.TipoAlerta}");
                Console.WriteLine($"📅 {alerta.HorarioEnvio:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"📝 {alerta.Mensagem}");
                
                if (!string.IsNullOrWhiteSpace(alerta.DetalhesAdicionais))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"ℹ️  {alerta.DetalhesAdicionais}");
                    Console.ResetColor();
                }
                
                Console.WriteLine();
            }
        }

        tela.AguardarTecla();
    }

    private void VisualizarTodosAlertas()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("TODOS OS ALERTAS", "Histórico Completo");

        var todosAlertas = GerenciadorAlertas.ObterTodosAlertas();

        if (todosAlertas.Count == 0)
        {
            tela.ExibirMensagem("Nenhum alerta foi gerado ainda.");
        }
        else
        {
            Console.SetCursorPosition(2, 8);
            Console.WriteLine($"📋 Histórico completo ({todosAlertas.Count} alertas):");
            Console.WriteLine();

            foreach (var alerta in todosAlertas.Take(10)) // Mostrar apenas últimos 10
            {
                string statusIcon = alerta.Status switch
                {
                    "PENDENTE" => "🔴",
                    "VISUALIZADO" => "🟡", 
                    "RESOLVIDO" => "🟢",
                    _ => "⚪"
                };

                Console.WriteLine($"{statusIcon} #{alerta.IdAlerta} | {alerta.TipoAlerta} | {alerta.HorarioEnvio:dd/MM HH:mm}");
                Console.WriteLine($"   {alerta.Mensagem}");
                Console.WriteLine();
            }

            if (todosAlertas.Count > 10)
            {
                Console.WriteLine($"... e mais {todosAlertas.Count - 10} alertas");
            }
        }

        tela.AguardarTecla();
    }

    private void FiltrarAlertasPorTipo()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("FILTRAR ALERTAS", "Filtro por Tipo");

        List<string> tipos = new List<string>
        {
            "1 - CANCELAMENTO",
            "2 - REEMBOLSO",
            "3 - SISTEMA",
            "0 - Voltar"
        };

        string opcao = tela.MostrarMenu(tipos, 10, 8, "Selecione o tipo:");

        string? tipoFiltro = opcao switch
        {
            "1" => "CANCELAMENTO",
            "2" => "REEMBOLSO", 
            "3" => "SISTEMA",
            _ => null
        };

        if (tipoFiltro != null)
        {
            var alertasFiltrados = GerenciadorAlertas.ObterAlertasPorTipo(tipoFiltro);
            
            tela.LimparTela();
            tela.DesenharCabecalho($"ALERTAS - {tipoFiltro}", $"Filtrados por {tipoFiltro}");

            if (alertasFiltrados.Count == 0)
            {
                tela.ExibirMensagem($"Nenhum alerta do tipo {tipoFiltro} encontrado.");
            }
            else
            {
                Console.SetCursorPosition(2, 8);
                Console.WriteLine($"📋 {alertasFiltrados.Count} alertas do tipo {tipoFiltro}:");
                Console.WriteLine();

                foreach (var alerta in alertasFiltrados)
                {
                    Console.WriteLine(alerta.ObterDetalhesFormatados());
                    Console.WriteLine();
                }
            }

            tela.AguardarTecla();
        }
    }

    private void MarcarAlertaComoResolvido()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("RESOLVER ALERTA", "Marcar como Resolvido");

        var alertasPendentes = GerenciadorAlertas.ObterAlertasPendentes();

        if (alertasPendentes.Count == 0)
        {
            tela.ExibirMensagem("Não há alertas pendentes para resolver.");
            tela.AguardarTecla();
            return;
        }

        Console.SetCursorPosition(2, 8);
        Console.WriteLine("Alertas pendentes:");
        Console.WriteLine();

        for (int i = 0; i < alertasPendentes.Count; i++)
        {
            var alerta = alertasPendentes[i];
            Console.WriteLine($"[{i + 1}] #{alerta.IdAlerta} - {alerta.TipoAlerta}");
            Console.WriteLine($"    {alerta.Mensagem}");
            Console.WriteLine();
        }

        string numeroStr = tela.LerTexto("Digite o número do alerta para resolver (0 para cancelar)");
        if (int.TryParse(numeroStr, out int numero) && numero > 0 && numero <= alertasPendentes.Count)
        {
            var alerta = alertasPendentes[numero - 1];
            
            if (tela.ConfirmarAcao($"Marcar alerta #{alerta.IdAlerta} como resolvido?"))
            {
                GerenciadorAlertas.MarcarComoResolvido(alerta.IdAlerta);
                tela.ExibirSucesso("Alerta marcado como resolvido!");
            }
        }
        else if (numero != 0)
        {
            tela.ExibirErro("Número inválido!");
        }

        tela.AguardarTecla();
    }

    private void LimparAlertasResolvidos()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("LIMPAR ALERTAS", "Remover Alertas Resolvidos");

        int totalAlertas = GerenciadorAlertas.ObterTodosAlertas().Count;
        int alertasResolvidos = GerenciadorAlertas.ObterTodosAlertas().Count(a => a.Status == "RESOLVIDO");

        tela.ExibirInformacoes("=== LIMPEZA DE ALERTAS ===",
            $"Total de alertas: {totalAlertas}",
            $"Alertas resolvidos: {alertasResolvidos}",
            $"Serão removidos: {alertasResolvidos} alertas");

        if (alertasResolvidos == 0)
        {
            tela.ExibirMensagem("Não há alertas resolvidos para limpar.");
        }
        else if (tela.ConfirmarAcao("Confirma a limpeza dos alertas resolvidos?"))
        {
            GerenciadorAlertas.LimparAlertasResolvidos();
            tela.ExibirSucesso($"{alertasResolvidos} alertas resolvidos foram removidos!");
        }

        tela.AguardarTecla();
    }

    private void GerenciarReembolsos()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR REEMBOLSOS", "Sistema de Reembolsos");
        
        tela.ExibirMensagem("Funcionalidade de reembolsos sera implementada futuramente.");
        tela.ExibirMensagem("Aqui o gestor podera:");
        tela.ExibirMensagem("- Visualizar solicitacoes de reembolso");
        tela.ExibirMensagem("- Aprovar/reprovar reembolsos");
        tela.ExibirMensagem("- Gerar relatorios de reembolsos pagos");
        
        tela.AguardarTecla();
    }

    private void GerarRelatorios()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("RELATORIOS GERENCIAIS", "Dashboard Executivo");
        
        var motoristas = motoristaCRUD.ObterMotoristas();
        var passageiros = passageiroCRUD.ObterPassageiros();
        var solicitacoes = passageiroCRUD.ObterSolicitacoes();
        
        Console.SetCursorPosition(2, 8);
        Console.WriteLine("=== RESUMO GERAL DO SISTEMA ===");
        Console.WriteLine($"Total de motoristas: {motoristas.Count}");
        Console.WriteLine($"Total de passageiros: {passageiros.Count}");
        Console.WriteLine($"Total de solicitacoes de carona: {solicitacoes.Count}");
        
        if (solicitacoes.Count > 0)
        {
            double distanciaTotal = 0;
            foreach (var solicitacao in solicitacoes)
            {
                distanciaTotal += solicitacao.DistanciaKm;
            }
            
            Console.WriteLine($"Distancia total solicitada: {distanciaTotal:F1} km");
            Console.WriteLine($"Media por solicitacao: {(distanciaTotal / solicitacoes.Count):F1} km");
        }
        
        Console.WriteLine("===============================");
        
        tela.AguardarTecla();
    }

    private void LoginMotorista()
    {
        // Sincronizar solicitações antes do motorista acessar
        var solicitacoes = passageiroCRUD.ObterSolicitacoes();
        motoristaCRUD.AtualizarSolicitacoes(solicitacoes);
        
        motoristaCRUD.ExecutarCRUD();
        tela.AguardarTecla();
    }

    private void LoginPassageiro()
    {
        passageiroCRUD.ExecutarCRUD();
        tela.AguardarTecla();
    }
}