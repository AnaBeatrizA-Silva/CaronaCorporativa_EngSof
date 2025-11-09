using System;

public class Passageiro
{
    public string Nome = "";
    public string Cpf = "";
    public string Cargo = "";
    public string NumeroCartao = "";

    // Construtor simples
    public Passageiro()
    {
    }

    public Passageiro(string nome, string cpf, string cargo, string numeroCartao)
    {
        this.Nome = nome;
        this.Cpf = cpf;
        this.Cargo = cargo;
        this.NumeroCartao = numeroCartao;
    }

    // Métodos simples sem polimorfismo
    public void SolicitarReserva()
    {
        Console.WriteLine($"Passageiro {Nome} solicitando reserva");
    }

    public void RealizarCheckIn()
    {
        Console.WriteLine($"Passageiro {Nome} realizando check-in");
    }

    public void RealizarCheckOut()
    {
        Console.WriteLine($"Passageiro {Nome} realizando check-out");
    }
}