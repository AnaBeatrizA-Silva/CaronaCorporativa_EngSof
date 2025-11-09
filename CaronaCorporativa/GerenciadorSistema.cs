using System;
using System.Collections.Generic;

public class GerenciadorSistema
{
    private Tela tela;
    private List<Motorista> motoristas;
    private List<Passageiro> passageiros;
    
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
                    GerenciarAlertas();
                    break;
                case 4:
                    GerenciarReembolsos();
                    break;
                case 5:
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
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR MOTORISTAS", "Visualizar e administrar motoristas");

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

    private void GerenciarPassageiros()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR PASSAGEIROS", "Visualizar e administrar passageiros");

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
            
            // Mostrar solicitações de carona
            var solicitacoes = passageiroCRUD.ObterSolicitacoes();
            if (solicitacoes.Count > 0)
            {
                Console.WriteLine($"\nSolicitacoes de carona: {solicitacoes.Count}");
                foreach (var solicitacao in solicitacoes)
                {
                    solicitacao.ExibirDetalhes();
                }
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

            string opcao = tela.MostrarMenu(opcoes, 10, Console.CursorTop + 2, "Escolha uma opção:");

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