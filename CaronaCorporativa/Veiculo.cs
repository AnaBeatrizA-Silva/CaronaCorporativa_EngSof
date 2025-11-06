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
            // Capacidade inválida - retorna false sem imprimir
            return false;
        }
        // Capacidade válida - retorna true sem imprimir
        return true;
    }

    public void CadastrarVeiculo()
    {
        // Implementacao do cadastro do veiculo - sem output direto
        // Log seria feito pela interface se necessário
    }
    
    // Metodos para obter dados (necessarios para o CRUD)
    public int ObterIdVeiculo() { return idVeiculo; }
    public string ObterPlaca() { return placa; }
    public string ObterMarca() { return marca; }
    public string ObterModelo() { return modelo; }
    public int ObterAno() { return ano; }
    public string ObterSeguro() { return seguro; }
    public int ObterCapacidade() { return capacidade; }
    
    // Retorna string formatada em vez de imprimir diretamente
    public string ObterDadosFormatados()
    {
        return $"=== DADOS DO VEICULO ===\n" +
               $"ID: {idVeiculo}\n" +
               $"Placa: {placa}\n" +
               $"Marca: {marca}\n" +
               $"Modelo: {modelo}\n" +
               $"Ano: {ano}\n" +
               $"Seguro: {seguro}\n" +
               $"Capacidade: {capacidade} passageiros\n" +
               $"========================";
    }
    
    // Método legado mantido para compatibilidade
    public void Consultar()
    {
        Console.WriteLine(ObterDadosFormatados());
    }
}