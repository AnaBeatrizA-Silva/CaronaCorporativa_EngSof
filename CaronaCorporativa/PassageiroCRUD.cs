using System;
using System.Collections.Generic;
using System.Linq;

public class PassageiroCRUD
{
    private List<Passageiro> passageiros;
    private List<SolicitacaoCarona> solicitacoes;
    private Tela tela;

    public PassageiroCRUD()
    {
        this.passageiros = new List<Passageiro>();
        this.solicitacoes = new List<SolicitacaoCarona>();
        this.tela = new Tela();
    }

    public void ExecutarCRUD()
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("MÓDULO PASSAGEIRO", "Sistema de Caronas Corporativas");

            List<string> opcoes = new List<string>
            {
                "1 - Acessar cadastro existente",
                "2 - Criar novo cadastro",
                "0 - Voltar ao menu principal"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    AcessarCadastroExistente();
                    break;
                case "2":
                    CriarNovoCadastro();
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

        // Verificar se CPF já existe
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

        // Posiciona corretamente os dados atuais
        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"---Dados atuais do passageiro:---");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Nome: {passageiro.Nome}");
        Console.WriteLine($"CPF: {passageiro.Cpf}");
        Console.WriteLine($"Cargo: {passageiro.Cargo}");
        Console.WriteLine($"Numero do Cartao: {passageiro.NumeroCartao}");
        Console.WriteLine(); // Linha em branco
        Console.WriteLine(); // Segunda linha em branco para espaçamento

        // Define a próxima linha de input após as informações e espaçamento
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

        Console.WriteLine($"Solicitante: {passageiro.Nome}");
        Console.WriteLine($"CPF: {passageiro.Cpf}");
        Console.WriteLine();

        string origem = tela.LerTexto("Endereco de origem");
        string destino = tela.LerTexto("Endereco de destino");
        
        string distanciaStr = tela.LerTexto("Distancia em quilometros (ex: 10.5)");
        
        if (!double.TryParse(distanciaStr, out double distancia))
        {
            tela.ExibirErro("Distancia invalida! Use formato: 10.5");
            tela.AguardarTecla();
            return;
        }

        // Criar a solicitacao
        SolicitacaoCarona solicitacao = new SolicitacaoCarona(
            solicitacoes.Count + 1,
            passageiro.Cpf,
            origem,
            destino,
            distancia
        );

        // Exibir resumo da solicitacao
        Console.WriteLine("\n=== RESUMO DA SOLICITACAO ===");
        Console.WriteLine($"Origem: {origem}");
        Console.WriteLine($"Destino: {destino}");
        Console.WriteLine($"Distancia: {distancia:F1} km");
        Console.WriteLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm}");

        if (tela.ConfirmarAcao("Confirma a solicitacao de carona?"))
        {
            solicitacoes.Add(solicitacao);
            tela.ExibirSucesso("Solicitacao de carona enviada com sucesso!");
            
            Console.WriteLine("\nSua solicitacao foi registrada e esta aguardando um motorista.");
            Console.WriteLine("Voce sera notificado quando houver uma resposta.");
        }
        
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
}