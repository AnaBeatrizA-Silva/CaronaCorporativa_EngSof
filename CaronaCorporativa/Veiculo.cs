using System;
using System.Collections.Generic;

public class Veiculo
{
    private int idVeiculo;
    private string placa;
    private string marca;
    private string modelo;
    private int ano;
    private string seguro;
    private int capacidade;
    private Motorista? motorista;
    private List<Reserva> reservas;

    // Construtor
    public Veiculo(int idVeiculo, string placa, string marca, string modelo, int ano, string seguro, int capacidade)
    {
        this.idVeiculo = idVeiculo;
        this.placa = placa;
        this.marca = marca;
        this.modelo = modelo;
        this.ano = ano;
        this.seguro = seguro;
        this.capacidade = capacidade;
        this.reservas = new List<Reserva>();
    }

    // Metodos
    public bool ValidarCapacidade()
    {
        if (capacidade <= 0)
        {
            Console.WriteLine("Capacidade invalida");
            return false;
        }
        Console.WriteLine($"Capacidade valida: {capacidade} passageiros");
        return true;
    }

    public void CadastrarVeiculo()
    {
        Console.WriteLine($"Cadastrando veiculo: {marca} {modelo} - Placa: {placa}");
        // Implementacao do cadastro do veiculo
    }
    
    // Metodos para obter dados (necessarios para o CRUD)
    public int ObterIdVeiculo() { return idVeiculo; }
    public string ObterPlaca() { return placa; }
    public string ObterMarca() { return marca; }
    public string ObterModelo() { return modelo; }
    public int ObterAno() { return ano; }
    public string ObterSeguro() { return seguro; }
    public int ObterCapacidade() { return capacidade; }
    
    public void Consultar()
    {
        Console.WriteLine("=== DADOS DO VEICULO ===");
        Console.WriteLine($"ID: {idVeiculo}");
        Console.WriteLine($"Placa: {placa}");
        Console.WriteLine($"Marca: {marca}");
        Console.WriteLine($"Modelo: {modelo}");
        Console.WriteLine($"Ano: {ano}");
        Console.WriteLine($"Seguro: {seguro}");
        Console.WriteLine($"Capacidade: {capacidade} passageiros");
        Console.WriteLine("========================");
    }
}