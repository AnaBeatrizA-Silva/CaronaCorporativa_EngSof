using System;
using System.Collections.Generic;
using System.Linq;

public class PassageiroCRUD
{
    private List<Passageiro> passageiros;
    private List<SolicitacaoCarona> solicitacoes;
    private Tela tela;
    private GerenciadorRotasUnificado gerenciadorUnificado; // Novo gerenciador

    public PassageiroCRUD()
    {
        this.passageiros = new List<Passageiro>();
        this.solicitacoes = new List<SolicitacaoCarona>();
        this.tela = new Tela();
        this.gerenciadorUnificado = new GerenciadorRotasUnificado(null, solicitacoes, null);
    }

    public void ExecutarCRUD()
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("MÓDULO PASSAGEIRO", "Sistema de Caronas Corporativas");

            List<string> opcoes = new List<string>
            {
                "1 - Acessar cadastro",
                "0 - Voltar ao menu principal"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    AcessarCadastroExistente();
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void CriarNovoCadastro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR PASSAGEIRO", "Novo Passageiro");

        Passageiro passageiro = new Passageiro();
        
        passageiro.Nome = tela.LerTexto("Nome completo");
        passageiro.Cpf = tela.LerTexto("CPF (somente numeros)");
        passageiro.Cargo = tela.LerTexto("Cargo na empresa");
        passageiro.NumeroCartao = tela.LerTexto("Numero do cartao corporativo");

        // Valida se CPF já não consta cadastrado
        if (passageiros.Any(p => p.Cpf == passageiro.Cpf))
        {
            tela.ExibirErro("Ja existe um passageiro cadastrado com este CPF!");
            tela.AguardarTecla();
            return;
        }

        if (tela.ConfirmarAcao("Confirma o cadastro?"))
        {
            passageiros.Add(passageiro);
            tela.ExibirSucesso("Passageiro cadastrado com sucesso!");
            tela.AguardarTecla();
        }
    }

    private void AcessarCadastroExistente()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ACESSAR CADASTRO", "Login do Passageiro");

        string cpf = tela.LerTexto("Digite seu CPF");
        
        Passageiro? passageiro = passageiros.FirstOrDefault(p => p.Cpf == cpf);
        
        if (passageiro == null)
        {
            tela.ExibirErro("CPF nao encontrado! Verifique o numero ou crie um novo cadastro.");
            tela.AguardarTecla();
            return;
        }

        // Menu do passageiro logado
        MenuPassageiroLogado(passageiro);
    }

    private void MenuPassageiroLogado(Passageiro passageiro)
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho($"BEM-VINDO(A), {passageiro.Nome.ToUpper()}", "Painel do Passageiro");

            List<string> opcoes = new List<string>
            {
                "1 - Alterar cadastro",
                "2 - Solicitar carona",
                "3 - Gerenciar solicitações",
                "4 - Check-in carona",
                "0 - Sair"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    AlterarCadastroPassageiro(passageiro);
                    break;
                case "2":
                    SolicitarCarona(passageiro);
                    break;
                case "3":
                    GerenciarSolicitacoes(passageiro);
                    break;
                case "4":
                    FazerCheckInCarona(passageiro);
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void AlterarCadastroPassageiro(Passageiro passageiro)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ALTERAR CADASTRO", "Atualizacao de Dados");

        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"---Dados atuais do passageiro:---");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Nome: {passageiro.Nome}");
        Console.WriteLine($"CPF: {passageiro.Cpf}");
        Console.WriteLine($"Cargo: {passageiro.Cargo}");
        Console.WriteLine($"Numero do Cartao: {passageiro.NumeroCartao}");
        Console.WriteLine();
        Console.WriteLine();

        tela.DefinirProximaLinhaInput(17);

        string novoNome = tela.LerTexto($"Novo nome (Enter para manter: {passageiro.Nome})");
        if (!string.IsNullOrWhiteSpace(novoNome)) passageiro.Nome = novoNome;
        
        string novoCargo = tela.LerTexto($"Novo cargo (Enter para manter: {passageiro.Cargo})");
        if (!string.IsNullOrWhiteSpace(novoCargo)) passageiro.Cargo = novoCargo;
        
        string novoCartao = tela.LerTexto($"Novo cartao (Enter para manter: {passageiro.NumeroCartao})");
        if (!string.IsNullOrWhiteSpace(novoCartao)) passageiro.NumeroCartao = novoCartao;
        
        tela.ExibirSucesso("Cadastro atualizado com sucesso!");
        tela.AguardarTecla();
    }

    private void SolicitarCarona(Passageiro passageiro)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("SOLICITAR CARONA", "Nova Solicitacao");

        tela.ExibirInformacoes("--- DADOS DO SOLICITANTE ---",
            $"Nome: {passageiro.Nome}",
            $"CPF: {passageiro.Cpf}");

        // Mostra critérios para solicitação de carona
        Console.SetCursorPosition(2, 14);
        Console.WriteLine("CRITÉRIOS PARA SOLICITAÇÃO DE CARONA:");
        Console.WriteLine("• Um dos endereços deve ser a sede da empresa Perini");
        Console.WriteLine("• Se origem não for Perini, destino deve ser Perini");
        Console.WriteLine("• Caronas serão pagas pela corporação somente para distâncias maiores que 10km");
        Console.WriteLine();

        tela.DefinirProximaLinhaInput(20);
        string origem = tela.LerTexto("Endereco de origem");
        
        tela.DefinirProximaLinhaInput(22);
        string destino = tela.LerTexto("Endereco de destino");

        // Faz validação da rota usando o gerenciador unificado
        if (!gerenciadorUnificado.ValidarRota(origem, destino, out double distancia, out string mensagem))
        {
            tela.ExibirErro(mensagem);
            tela.AguardarTecla();
            return;
        }

        // Valida se a distancia é elegível para reembolso, vaverificando se distancia é maior que 10km
        bool elegivelReembolso = gerenciadorUnificado.EhElegivelParaReembolso(distancia);
        double valorReembolso = elegivelReembolso ? distancia * 2.50 : 0;

        // Se enquadrado, cria a solicitação
        SolicitacaoCarona solicitacao = new SolicitacaoCarona(
            solicitacoes.Count + 1,
            passageiro.Cpf,
            origem,
            destino,
            distancia
        );

        // Mostra resumo da solicitação
        tela.LimparTela();
        tela.DesenharCabecalho("RESUMO DA SOLICITACAO", "Análise de Elegibilidade");
        
        tela.ExibirInformacoes("=== DADOS DA SOLICITACAO ===",
            $"Solicitante: {passageiro.Nome}",
            $"CPF: {passageiro.Cpf}",
            $"Origem: {origem}",
            $"Destino: {destino}",
            $"Distancia: {distancia:F1} km",
            $"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm}");
        
        // Posicionamento para status de reembolso
        Console.SetCursorPosition(2, Math.Min(18, Console.WindowHeight - 5));
        if (elegivelReembolso)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ROTA ELEGÍVEL PARA REEMBOLSO!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ℹ    SOLICITAÇÃO INVÁLIDA PARA REEMBOLSO");
            Console.WriteLine("Distancia percorrida deve ser maior que 10km");
            Console.WriteLine("Valor não será reembolsado, carona deve ser paga para motorista.");
        }
        Console.WriteLine("=============================");

        if (tela.ConfirmarAcao("Confirma a solicitacao de carona?"))
        {
            solicitacoes.Add(solicitacao);
            tela.ExibirSucesso("Solicitacao de carona enviada com sucesso!");
            
            Console.WriteLine("\nSua solicitacao foi registrada e esta aguardando um motorista.");

        }
        
        tela.AguardarTecla();
    }

    private void GerenciarSolicitacoes(Passageiro passageiro)
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("GERENCIAR SOLICITAÇÕES", "Solicitações de Carona");

            // Busca solicitações do passageiro
            var solicitacoesPassageiro = solicitacoes.Where(s => s.CpfPassageiro == passageiro.Cpf).ToList();

            if (solicitacoesPassageiro.Count == 0)
            {
                tela.ExibirMensagem("Você não possui solicitações de carona.");
                tela.AguardarTecla();
                return;
            }

            // Mostra lista das solicitações de carona
            Console.SetCursorPosition(2, 8);
            Console.WriteLine($"Total de solicitações: {solicitacoesPassageiro.Count}");
            Console.WriteLine();

            // Limita a exibição para evitar ultrapassar o limite da tela
            int maxItens = Math.Min(solicitacoesPassageiro.Count, 5); // Máximo 5 itens por vez
            
            for (int i = 0; i < maxItens; i++)
            {
                var sol = solicitacoesPassageiro[i];
                bool elegivel = gerenciadorUnificado.EhElegivelParaReembolso(sol.DistanciaKm);

                Console.WriteLine($"[{i + 1}] ID: {sol.Id} | {sol.EnderecoOrigem} → {sol.EnderecoDestino}");
                Console.WriteLine($"    Distância: {sol.DistanciaKm:F1}km | Status: {sol.Status}");
                if (elegivel)
                {
                    Console.WriteLine($"Elegível para reembolso | Data: {sol.DataSolicitacao:dd/MM/yyyy}");
                }
                else
                {
                    Console.WriteLine($"Sem reembolso | Data: {sol.DataSolicitacao:dd/MM/yyyy}");
                }
                Console.WriteLine();
            }

            List<string> opcoes = new List<string>
            {
                "1 - Alterar solicitação",
                "2 - Excluir solicitação", 
                "3 - Consultar status de reembolso",
                "0 - Voltar"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, Math.Min(Math.Max(Console.CursorTop + 2, 15), Console.WindowHeight - 8), "Escolha uma opção:");

            switch (opcao)
            {
                case "1":
                    AlterarSolicitacao(passageiro, solicitacoesPassageiro);
                    break;
                case "2":
                    ExcluirSolicitacao(passageiro, solicitacoesPassageiro);
                    break;
                case "3":
                    ConsultarStatusReembolso(passageiro, solicitacoesPassageiro);
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

    private void AlterarSolicitacao(Passageiro passageiro, List<SolicitacaoCarona> solicitacoesPassageiro)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ALTERAR SOLICITAÇÃO", "Modificar Solicitação de Carona");

        if (solicitacoesPassageiro.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma solicitação para alterar.");
            tela.AguardarTecla();
            return;
        }

        string numeroStr = tela.LerTexto("Digite o número da solicitação para alterar");
        if (!int.TryParse(numeroStr, out int numero) || numero < 1 || numero > solicitacoesPassageiro.Count)
        {
            tela.ExibirErro("Número de solicitação inválido!");
            tela.AguardarTecla();
            return;
        }

        var solicitacao = solicitacoesPassageiro[numero - 1];

        if (solicitacao.Status != "Pendente")
        {
            tela.ExibirErro("Apenas solicitações pendentes podem ser alteradas!");
            tela.AguardarTecla();
            return;
        }

        // Exibir dados atuais da carona
        tela.ExibirInformacoes("=== DADOS ATUAIS ===",
            $"Origem: {solicitacao.EnderecoOrigem}",
            $"Destino: {solicitacao.EnderecoDestino}",
            $"Distância: {solicitacao.DistanciaKm:F1}km",
            $"Status: {solicitacao.Status}");

        string novaOrigem = tela.LerTexto($"Nova origem (Enter para manter: {solicitacao.EnderecoOrigem})");
        if (string.IsNullOrWhiteSpace(novaOrigem)) novaOrigem = solicitacao.EnderecoOrigem;

        string novoDestino = tela.LerTexto($"Novo destino (Enter para manter: {solicitacao.EnderecoDestino})");
        if (string.IsNullOrWhiteSpace(novoDestino)) novoDestino = solicitacao.EnderecoDestino;

        // Validar nova rota
        if (!gerenciadorUnificado.ValidarRota(novaOrigem, novoDestino, out double novaDistancia, out string mensagem))
        {
            tela.ExibirErro(mensagem);
            tela.AguardarTecla();
            return;
        }

        if (tela.ConfirmarAcao("Confirma a alteração?"))
        {
            solicitacao.EnderecoOrigem = novaOrigem;
            solicitacao.EnderecoDestino = novoDestino;
            solicitacao.DistanciaKm = novaDistancia;
            solicitacao.DataSolicitacao = DateTime.Now;

            tela.ExibirSucesso("Solicitação alterada com sucesso!");
        }

        tela.AguardarTecla();
    }

    private void ExcluirSolicitacao(Passageiro passageiro, List<SolicitacaoCarona> solicitacoesPassageiro)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("EXCLUIR SOLICITAÇÃO", "Remover Solicitação de Carona");

        if (solicitacoesPassageiro.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma solicitação para excluir.");
            tela.AguardarTecla();
            return;
        }

        string numeroStr = tela.LerTexto("Digite o número da solicitação para excluir");
        if (!int.TryParse(numeroStr, out int numero) || numero < 1 || numero > solicitacoesPassageiro.Count)
        {
            tela.ExibirErro("Número de solicitação inválido!");
            tela.AguardarTecla();
            return;
        }

        var solicitacao = solicitacoesPassageiro[numero - 1];

        tela.ExibirInformacoes("=== SOLICITAÇÃO A SER EXCLUÍDA ===",
            $"ID: {solicitacao.Id}",
            $"Origem: {solicitacao.EnderecoOrigem}",
            $"Destino: {solicitacao.EnderecoDestino}",
            $"Distância: {solicitacao.DistanciaKm:F1}km",
            $"Status: {solicitacao.Status}",
            $"Data: {solicitacao.DataSolicitacao:dd/MM/yyyy HH:mm}");

        if (tela.ConfirmarAcao("Tem certeza que deseja excluir esta solicitação?"))
        {
            solicitacoes.Remove(solicitacao);
            tela.ExibirSucesso("Solicitação excluída com sucesso!");
        }

        tela.AguardarTecla();
    }

    private void ConsultarStatusReembolso(Passageiro passageiro, List<SolicitacaoCarona> solicitacoesPassageiro)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("STATUS DE REEMBOLSOS", "Consulta de Reembolsos");

        if (solicitacoesPassageiro.Count == 0)
        {
            tela.ExibirMensagem("Nenhuma solicitação encontrada.");
            tela.AguardarTecla();
            return;
        }

        int totalElegiveis = 0;

        Console.SetCursorPosition(2, 8);
        Console.WriteLine("=== RELATÓRIO DE REEMBOLSOS ===");
        Console.WriteLine();

        foreach (var sol in solicitacoesPassageiro)
        {
            bool elegivel = gerenciadorUnificado.EhElegivelParaReembolso(sol.DistanciaKm);

            Console.WriteLine($"Solicitação #{sol.Id}:");
            Console.WriteLine($"  Rota: {sol.EnderecoOrigem} → {sol.EnderecoDestino}");
            Console.WriteLine($"  Distância: {sol.DistanciaKm:F1}km | Data: {sol.DataSolicitacao:dd/MM/yyyy}");
            
            if (elegivel)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" ELEGÍVEL PARA REEMBOLSO");
                totalElegiveis++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("NÃO ELEGÍVEL - Distância menor que 10km");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        Console.WriteLine("==============================");
        Console.WriteLine($"Total de solicitações: {solicitacoesPassageiro.Count}");
        Console.WriteLine($"Solicitações elegíveis: {totalElegiveis}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.ResetColor();

        tela.AguardarTecla();
    }

    public List<Passageiro> ObterPassageiros()
    {
        return passageiros;
    }

    public List<SolicitacaoCarona> ObterSolicitacoes()
    {
        return solicitacoes;
    }

    private void FazerCheckInCarona(Passageiro passageiro)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CHECK-IN CARONA", "Confirmar Presença");

        // Busca caronas aceitas para este passageiro
        var caronasAceitas = solicitacoes.Where(s => s.CpfPassageiro == passageiro.Cpf && 
                                                     s.Status == "Aceita").ToList();

        if (caronasAceitas.Count == 0)
        {
            tela.ExibirMensagem("Você não possui caronas aceitas para fazer check-in.");
            tela.ExibirMensagem("Check-in só é possível quando um motorista aceita sua solicitação.");
            tela.AguardarTecla();
            return;
        }

        Console.SetCursorPosition(2, 8);
        Console.WriteLine("=== CARONAS DISPONÍVEIS PARA CHECK-IN ===");
        Console.WriteLine();

        for (int i = 0; i < caronasAceitas.Count; i++)
        {
            var carona = caronasAceitas[i];
            Console.WriteLine($"[{i + 1}] ID: {carona.Id}");
            Console.WriteLine($"    Rota: {carona.EnderecoOrigem} → {carona.EnderecoDestino}");
            Console.WriteLine($"    Distância: {carona.DistanciaKm:F1}km");
            Console.WriteLine($"    Data aceita: {carona.DataSolicitacao:dd/MM/yyyy HH:mm}");
            Console.WriteLine();
        }

        int posicaoInput = Math.Min(Console.CursorTop + 2, Console.WindowHeight - 5);
        tela.DefinirProximaLinhaInput(posicaoInput);
        string numeroStr = tela.LerTexto("Digite o número da carona para fazer check-in (0 para cancelar)");
        
        if (numeroStr == "0") return;
        
        if (!int.TryParse(numeroStr, out int numero) || numero < 1 || numero > caronasAceitas.Count)
        {
            tela.ExibirErro("Número de carona inválido!");
            tela.AguardarTecla();
            return;
        }

        var caronaSelecionada = caronasAceitas[numero - 1];

        tela.LimparTela();
        tela.DesenharCabecalho("CONFIRMAÇÃO CHECK-IN", "Confirmar Presença na Carona");

        tela.ExibirInformacoes("=== DADOS DA CARONA ===",
            $"ID da carona: {caronaSelecionada.Id}",
            $"Passageiro: {passageiro.Nome}",
            $"Origem: {caronaSelecionada.EnderecoOrigem}",
            $"Destino: {caronaSelecionada.EnderecoDestino}",
            $"Distância: {caronaSelecionada.DistanciaKm:F1}km");

        Console.SetCursorPosition(2, Math.Min(16, Console.WindowHeight - 8));
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("⚠️  IMPORTANTE:");
        Console.WriteLine("• Ao fazer check-in, você confirma que está presente no ponto de encontro");
        Console.WriteLine("• O motorista será notificado de sua presença");
        Console.WriteLine("• A carona mudará para status 'Em Andamento'");
        Console.WriteLine("• Não cancele após fazer check-in sem avisar o motorista");
        Console.ResetColor();
        Console.WriteLine();

        if (tela.ConfirmarAcao("Confirma o check-in nesta carona?"))
        {
            // Atualiza status para "Check-in Feito"
            caronaSelecionada.Status = "Check-in Feito";
            caronaSelecionada.DataCheckIn = DateTime.Now;

            tela.ExibirSucesso("Check-in realizado com sucesso!");
            Console.WriteLine();
            Console.WriteLine("✅ Sua presença foi confirmada!");
            Console.WriteLine("✅ O motorista foi notificado");
            Console.WriteLine("✅ Status da carona: Em Andamento");
            Console.WriteLine();
            Console.WriteLine("Aguarde o motorista para iniciar a viagem.");
        }

        tela.AguardarTecla();
    }
}