using System;

public class Funcionario
{
    // Campos públicos como no padrão SistemaBiblioteca
    public string Nome = "";
    public string Cpf = "";
    public string Cargo = "";

    // Construtor simples (opcional)
    public Funcionario()
    {
    }

    public Funcionario(string nome, string cpf, string cargo)
    {
        this.Nome = nome;
        this.Cpf = cpf;
        this.Cargo = cargo;
    }
}