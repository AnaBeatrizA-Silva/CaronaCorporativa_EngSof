using System;

public class Motorista
{
    public string Nome = "";
    public string Cpf = "";
    public string Cargo = "";
    public string Cnh = "";

    // Construtor simples
    public Motorista()
    {
    }

    public Motorista(string nome, string cpf, string cargo, string cnh)
    {
        this.Nome = nome;
        this.Cpf = cpf;
        this.Cargo = cargo;
        this.Cnh = cnh;
    }

    // Métodos simples sem polimorfismo
    public bool ValidarCNH()
    {
        if (string.IsNullOrWhiteSpace(Cnh) || Cnh.Length != 11)
        {
            Console.WriteLine("CNH invalida");
            return false;
        }
        Console.WriteLine("CNH valida");
        return true;
    }

    public void CadastrarRota()
    {
        Console.WriteLine($"Motorista {Nome} cadastrando nova rota");
    }

    public void EmitirReembolso()
    {
        Console.WriteLine($"Motorista {Nome} emitindo reembolso");
    }
}