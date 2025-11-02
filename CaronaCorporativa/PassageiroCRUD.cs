using System;
using System.Collections.Generic;

public class PassageiroCRUD
{
    private List<Passageiro> passageiros;
    private Tela tela;

    public PassageiroCRUD()
    {
        this.passageiros = new List<Passageiro>();
        this.tela = new Tela();
    }

    public void ExecutarCRUD()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR PASSAGEIROS", "Operacoes CRUD");

        List<string> opcoes = new List<string>
        {
            "1 - Cadastrar Passageiro",
            "2 - Listar Passageiros", 
            "3 - Buscar Passageiro",
            "4 - Atualizar Passageiro",
            "5 - Excluir Passageiro",
            "0 - Voltar"
        };

        string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

        switch (opcao)
        {
            case "1":
                CadastrarPassageiro();
                break;
            case "2":
                ListarPassageiros();
                break;
            case "3":
                BuscarPassageiro();
                break;
            case "4":
                AtualizarPassageiro();
                break;
            case "5":
                ExcluirPassageiro();
                break;
            case "0":
                return;
            default:
                tela.ExibirErro("Opcao invalida!");
                tela.AguardarTecla();
                break;
        }
    }

    private void CadastrarPassageiro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR PASSAGEIRO", "Novo Passageiro");

        Passageiro passageiro = new Passageiro();
        
        passageiro.Nome = tela.LerTexto("Nome");
        passageiro.Cpf = tela.LerTexto("CPF");
        passageiro.Cargo = tela.LerTexto("Cargo");
        passageiro.NumeroCartao = tela.LerTexto("Numero do Cartao");

        if (tela.ConfirmarAcao("Confirma o cadastro?"))
        {
            passageiros.Add(passageiro);
            tela.ExibirSucesso("Passageiro cadastrado com sucesso!");
        }
        
        tela.AguardarTecla();
    }

    public void ListarPassageiros()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("LISTA DE PASSAGEIROS", "Passageiros Cadastrados");

        if (passageiros.Count == 0)
        {
            tela.ExibirMensagem("Nenhum passageiro cadastrado.");
            tela.AguardarTecla();
            return;
        }

        Console.WriteLine();
        for (int i = 0; i < passageiros.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Nome: {passageiros[i].Nome} | CPF: {passageiros[i].Cpf} | Cartao: {passageiros[i].NumeroCartao}");
        }

        tela.AguardarTecla();
    }

    private void BuscarPassageiro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("BUSCAR PASSAGEIRO", "Consulta por CPF");

        string cpf = tela.LerTexto("Digite o CPF para buscar");
        
        Passageiro? encontrado = passageiros.Find(p => p.Cpf == cpf);
        
        if (encontrado != null)
        {
            Console.WriteLine("\n=== PASSAGEIRO ENCONTRADO ===");
            Console.WriteLine($"Nome: {encontrado.Nome}");
            Console.WriteLine($"CPF: {encontrado.Cpf}");
            Console.WriteLine($"Cargo: {encontrado.Cargo}");
            Console.WriteLine($"Numero do Cartao: {encontrado.NumeroCartao}");
        }
        else
        {
            tela.ExibirErro("Passageiro nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void AtualizarPassageiro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ATUALIZAR PASSAGEIRO", "Atualizacao de Dados");

        string cpf = tela.LerTexto("Digite o CPF do passageiro");
        
        Passageiro? passageiro = passageiros.Find(p => p.Cpf == cpf);
        
        if (passageiro != null)
        {
            Console.WriteLine($"\nPassageiro encontrado: {passageiro.Nome}");
            
            string novoNome = tela.LerTexto($"Novo nome (atual: {passageiro.Nome}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novoNome)) passageiro.Nome = novoNome;
            
            string novoCargo = tela.LerTexto($"Novo cargo (atual: {passageiro.Cargo}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novoCargo)) passageiro.Cargo = novoCargo;
            
            string novoCartao = tela.LerTexto($"Novo numero do cartao (atual: {passageiro.NumeroCartao}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novoCartao)) passageiro.NumeroCartao = novoCartao;
            
            tela.ExibirSucesso("Passageiro atualizado com sucesso!");
        }
        else
        {
            tela.ExibirErro("Passageiro nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void ExcluirPassageiro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("EXCLUIR PASSAGEIRO", "Remocao de Cadastro");

        string cpf = tela.LerTexto("Digite o CPF do passageiro");
        
        Passageiro? passageiro = passageiros.Find(p => p.Cpf == cpf);
        
        if (passageiro != null)
        {
            Console.WriteLine($"\nPassageiro encontrado: {passageiro.Nome}");
            
            if (tela.ConfirmarAcao("Confirma a exclusao?"))
            {
                passageiros.Remove(passageiro);
                tela.ExibirSucesso("Passageiro excluido com sucesso!");
            }
        }
        else
        {
            tela.ExibirErro("Passageiro nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    public List<Passageiro> ObterPassageiros()
    {
        return passageiros;
    }
}