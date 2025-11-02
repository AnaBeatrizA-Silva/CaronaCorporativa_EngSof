using System;
using System.Collections.Generic;

public class MotoristaCRUD
{
    private List<Motorista> motoristas;
    private Tela tela;

    public MotoristaCRUD()
    {
        this.motoristas = new List<Motorista>();
        this.tela = new Tela();
    }

    public void ExecutarCRUD()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR MOTORISTAS", "Operacoes CRUD");

        List<string> opcoes = new List<string>
        {
            "1 - Cadastrar Motorista",
            "2 - Listar Motoristas", 
            "3 - Buscar Motorista",
            "4 - Atualizar Motorista",
            "5 - Excluir Motorista",
            "0 - Voltar"
        };

        string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

        switch (opcao)
        {
            case "1":
                CadastrarMotorista();
                break;
            case "2":
                ListarMotoristas();
                break;
            case "3":
                BuscarMotorista();
                break;
            case "4":
                AtualizarMotorista();
                break;
            case "5":
                ExcluirMotorista();
                break;
            case "0":
                return;
            default:
                tela.ExibirErro("Opcao invalida!");
                tela.AguardarTecla();
                break;
        }
    }

    private void CadastrarMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR MOTORISTA", "Novo Motorista");

        Motorista motorista = new Motorista();
        
        motorista.Nome = tela.LerTexto("Nome");
        motorista.Cpf = tela.LerTexto("CPF");
        motorista.Cargo = tela.LerTexto("Cargo");
        motorista.Cnh = tela.LerTexto("CNH");

        if (tela.ConfirmarAcao("Confirma o cadastro?"))
        {
            motoristas.Add(motorista);
            tela.ExibirSucesso("Motorista cadastrado com sucesso!");
        }
        
        tela.AguardarTecla();
    }

    public void ListarMotoristas()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("LISTA DE MOTORISTAS", "Motoristas Cadastrados");

        if (motoristas.Count == 0)
        {
            tela.ExibirMensagem("Nenhum motorista cadastrado.");
            tela.AguardarTecla();
            return;
        }

        Console.WriteLine();
        for (int i = 0; i < motoristas.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Nome: {motoristas[i].Nome} | CPF: {motoristas[i].Cpf} | CNH: {motoristas[i].Cnh}");
        }

        tela.AguardarTecla();
    }

    private void BuscarMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("BUSCAR MOTORISTA", "Consulta por CPF");

        string cpf = tela.LerTexto("Digite o CPF para buscar");
        
        Motorista? encontrado = motoristas.Find(m => m.Cpf == cpf);
        
        if (encontrado != null)
        {
            Console.WriteLine("\n=== MOTORISTA ENCONTRADO ===");
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

    private void AtualizarMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ATUALIZAR MOTORISTA", "Atualizacao de Dados");

        string cpf = tela.LerTexto("Digite o CPF do motorista");
        
        Motorista? motorista = motoristas.Find(m => m.Cpf == cpf);
        
        if (motorista != null)
        {
            Console.WriteLine($"\nMotorista encontrado: {motorista.Nome}");
            
            string novoNome = tela.LerTexto($"Novo nome (atual: {motorista.Nome}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novoNome)) motorista.Nome = novoNome;
            
            string novoCargo = tela.LerTexto($"Novo cargo (atual: {motorista.Cargo}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novoCargo)) motorista.Cargo = novoCargo;
            
            string novaCnh = tela.LerTexto($"Nova CNH (atual: {motorista.Cnh}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novaCnh)) motorista.Cnh = novaCnh;
            
            tela.ExibirSucesso("Motorista atualizado com sucesso!");
        }
        else
        {
            tela.ExibirErro("Motorista nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void ExcluirMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("EXCLUIR MOTORISTA", "Remocao de Cadastro");

        string cpf = tela.LerTexto("Digite o CPF do motorista");
        
        Motorista? motorista = motoristas.Find(m => m.Cpf == cpf);
        
        if (motorista != null)
        {
            Console.WriteLine($"\nMotorista encontrado: {motorista.Nome}");
            
            if (tela.ConfirmarAcao("Confirma a exclusao?"))
            {
                motoristas.Remove(motorista);
                tela.ExibirSucesso("Motorista excluido com sucesso!");
            }
        }
        else
        {
            tela.ExibirErro("Motorista nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    public List<Motorista> ObterMotoristas()
    {
        return motoristas;
    }
}