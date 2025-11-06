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
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR ALERTAS", "Sistema de Alertas");
        
        tela.ExibirMensagem("Funcionalidade de alertas sera implementada futuramente.");
        tela.ExibirMensagem("Aqui o gestor podera:");
        tela.ExibirMensagem("- Visualizar alertas do sistema");
        tela.ExibirMensagem("- Gerenciar notificacoes para usuarios");
        tela.ExibirMensagem("- Configurar alertas automaticos");
        
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
        motoristaCRUD.ExecutarCRUD();
        tela.AguardarTecla();
    }

    private void LoginPassageiro()
    {
        passageiroCRUD.ExecutarCRUD();
        tela.AguardarTecla();
    }
}