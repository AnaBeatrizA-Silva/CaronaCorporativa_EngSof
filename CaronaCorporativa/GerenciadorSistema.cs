using System;
using System.Collections.Generic;

public class GerenciadorSistema
{
    private Tela tela;
    private List<Motorista> motoristas;
    private List<Passageiro> passageiros;
    private List<Veiculo> veiculos;
    
    // Classes CRUD
    private MotoristaCRUD motoristaCRUD;
    private PassageiroCRUD passageiroCRUD;
    private VeiculoCRUD veiculoCRUD;

    public GerenciadorSistema()
    {
        tela = new Tela();
        motoristas = new List<Motorista>();
        passageiros = new List<Passageiro>();
        veiculos = new List<Veiculo>();
        
        // Inicializa classes CRUD
        motoristaCRUD = new MotoristaCRUD();
        passageiroCRUD = new PassageiroCRUD();
        veiculoCRUD = new VeiculoCRUD();
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
                    motoristaCRUD.ExecutarCRUD();
                    break;
                case 2:
                    passageiroCRUD.ExecutarCRUD();
                    break;
                case 3:
                    veiculoCRUD.ExecutarCRUD();
                    break;
                case 4:
                    tela.ExibirMensagem("Aínda falta implementar essa funcionalidade");
                    break;
                case 5:
                    tela.ExibirMensagem("Aínda falta implementar essa funcionalidade");
                    break;
                case 6:
                    tela.ExibirMensagem("Aínda falta implementar essa funcionalidade");
                    break;
                case 7:
                    tela.ExibirMensagem("Aínda falta implementar essa funcionalidade");
                    break;
                case 8:
                    tela.ExibirMensagem("Aínda falta implementar essa funcionalidade");
                    break;
                case 0:
                    break;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    break;
            }
        } while (opcao != 0);
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