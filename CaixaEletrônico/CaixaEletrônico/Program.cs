using System;

public class Program
{
    public static void Main()
    {
        // Criando instância do caixa eletrônico
        CaixaEletronico caixaEletronico = new CaixaEletronico(0.08, 0.045, 0.06, 0.08);

        // Exibindo mensagem de boas-vindas
        Console.WriteLine("Bem-vindo(a) ao Caixa Eletrônico!");

        // Cadastro do cliente
        Console.WriteLine("\nDigite os dados do cliente:");
        Console.Write("Nome Completo: ");
        string nomeCompleto = Console.ReadLine();
        Console.Write("CPF: ");
        string cpf = Console.ReadLine();
        Console.Write("Senha (4 números que não podem ser 0000, 1111 ou 9999): ");
        string senha = Console.ReadLine();

        Cliente cliente = new Cliente(nomeCompleto, cpf, senha);

        bool sair = false;
        while (!sair)
        {
            Console.WriteLine("\nOpções disponíveis:");
            Console.WriteLine("1. Consultar Saldo");
            Console.WriteLine("2. Realizar Depósito");
            Console.WriteLine("3. Realizar Saque");
            Console.WriteLine("4. Realizar Câmbio");
            Console.WriteLine("5. Comprar Moeda");
            Console.WriteLine("6. Exibir Relatórios");
            Console.WriteLine("7. Sair");

            Console.Write("Digite o número da opção desejada: ");
            int opcao;
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 1:
                        Console.WriteLine("\nSaldo disponível em cada moeda:");
                        Console.WriteLine($"BRL: R$ {cliente.SaldoBRL:F2}");
                        Console.WriteLine($"USD: $ {cliente.SaldoUSD:F2}");
                        Console.WriteLine($"ARS: $ {cliente.SaldoARS:F2}");
                        Console.WriteLine($"CAD: $ {cliente.SaldoCAD:F2}");
                        break;
                    case 2:
                        Console.Write("\nDigite o valor a ser depositado: ");
                        double valorDeposito;
                        if (double.TryParse(Console.ReadLine(), out valorDeposito))
                        {
                            Console.Write("Digite o tipo de moeda (BRL, USD, ARS ou CAD): ");
                            string tipoMoedaDeposito = Console.ReadLine().ToUpper();

                            if (caixaEletronico.Depositar(cliente, valorDeposito, tipoMoedaDeposito))
                            {
                                Console.WriteLine("Depósito realizado com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("Depósito não realizado. Verifique o valor e o tipo de moeda.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Valor inválido! Tente novamente.");
                        }
                        break;
                    case 3:
                        Console.Write("\nDigite o valor a ser sacado: ");
                        double valorSaque;
                        if (double.TryParse(Console.ReadLine(), out valorSaque))
                        {
                            Console.Write("Digite o tipo de moeda (BRL, USD, ARS ou CAD): ");
                            string tipoMoedaSaque = Console.ReadLine().ToUpper();

                            if (caixaEletronico.Sacar(cliente, valorSaque, tipoMoedaSaque))
                            {
                                Console.WriteLine("Saque realizado com sucesso!");
                            }
                            else
                            {
                                double trocoViaPixETED = valorSaque - cliente.GetSaldo(tipoMoedaSaque);
                                if (trocoViaPixETED > 0)
                                {
                                    if (cliente.ReceberTrocoViaPixETED(trocoViaPixETED))
                                    {
                                        Console.WriteLine("Troco pago via PIX ou TED.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Saque não realizado. Verifique o saldo e o tipo de moeda.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Valor inválido! Tente novamente.");
                        }
                        break;
                    case 4:
                        Console.Write("\nDigite o valor a ser trocado: ");
                        double valorCambio;
                        if (double.TryParse(Console.ReadLine(), out valorCambio))
                        {
                            Console.Write("Digite o tipo de moeda de origem (BRL, USD, ARS ou CAD): ");
                            string tipoMoedaOrigem = Console.ReadLine().ToUpper();
                            Console.Write("Digite o tipo de moeda de destino (BRL, USD, ARS ou CAD): ");
                            string tipoMoedaDestino = Console.ReadLine().ToUpper();

                            caixaEletronico.RealizarCambio(cliente, valorCambio, tipoMoedaOrigem, tipoMoedaDestino);
                            Console.WriteLine("Câmbio realizado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Valor inválido! Tente novamente.");
                        }
                        break;
                    case 5:
                        Console.Write("\nDigite o valor a ser utilizado para comprar moeda: ");
                        double valorCompraMoeda;
                        if (double.TryParse(Console.ReadLine(), out valorCompraMoeda))
                        {
                            Console.Write("Digite o tipo de moeda a ser comprada (USD, ARS ou CAD): ");
                            string tipoMoedaCompra = Console.ReadLine().ToUpper();

                            if (caixaEletronico.ComprarMoeda(cliente, valorCompraMoeda, tipoMoedaCompra))
                            {
                                Console.WriteLine("Compra de moeda realizada com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("Compra de moeda não realizada. Verifique o saldo e o tipo de moeda.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Valor inválido! Tente novamente.");
                        }
                        break;
                    case 6:
                        caixaEletronico.ExibirRelatorios();

                        break;
                    case 7:
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida! Tente novamente.");
            }
        }

        Console.WriteLine("Obrigado por utilizar o Caixa Eletrônico!");
    }
}
