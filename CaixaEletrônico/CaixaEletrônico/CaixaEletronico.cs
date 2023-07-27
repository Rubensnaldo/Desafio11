using System;
using System.Collections.Generic;

public class CaixaEletronico : IDeposito, ISaque
{
    private double taxaCambio;
    private double taxaSaque;
    private double taxaCambioDeposito;
    private double taxaCambioCartao;
    private double taxaCambioCompra;

    private double saldoBanco;
    private double saldoPixETED;
    private Dictionary<string, double> notas;
    private Dictionary<string, double> moedas;

    private int operacoesDeposito;
    private int operacoesSaque;
    private int operacoesCambio;
    private int operacoesCompraMoeda;

    public int OperacoesDeposito => operacoesDeposito;
    public int OperacoesSaque => operacoesSaque;
    public int OperacoesCambio => operacoesCambio;
    public int OperacoesCompraMoeda => operacoesCompraMoeda;

    public CaixaEletronico(double taxaCambio, double taxaSaque, double taxaCambioDeposito, double taxaCambioCartao)
    {
        this.taxaCambio = taxaCambio;
        this.taxaSaque = taxaSaque;
        this.taxaCambioDeposito = taxaCambioDeposito;
        this.taxaCambioCartao = taxaCambioCartao;
        this.taxaCambioCompra = 1.08; // Taxa de câmbio de compra com cartão

        notas = new Dictionary<string, double>();
        moedas = new Dictionary<string, double>();
        saldoPixETED = 0;
    }

    public double TaxaCambioCartao
    {
        get { return taxaCambioCartao; }
        set { taxaCambioCartao = value; }
    }

    public void CadastrarNotasEMoedas(double valor, string tipoMoeda)
    {
        if (tipoMoeda.ToUpper() == "BRL")
        {
            if (notas.ContainsKey(tipoMoeda))
                notas[tipoMoeda] += valor;
            else
                notas[tipoMoeda] = valor;
        }
        else
        {
            if (moedas.ContainsKey(tipoMoeda))
                moedas[tipoMoeda] += valor;
            else
                moedas[tipoMoeda] = valor;
        }
    }

    public bool Depositar(Cliente cliente, double valor, string tipoMoeda)
    {
        double valorConvertido = valor;

        if (tipoMoeda.ToUpper() != "BRL")
            valorConvertido = valor * taxaCambioDeposito;

        if (cliente.RealizarDeposito(valorConvertido, tipoMoeda))
        {
            if (tipoMoeda.ToUpper() != "BRL")
            {
                double taxa = valor * (1 - taxaCambioDeposito);
                saldoBanco += taxa;
                cliente.RealizarDeposito(taxa, "BRL");
            }

            operacoesDeposito++;
            return true;
        }

        return false;
    }

    public bool Sacar(Cliente cliente, double valor, string tipoMoeda)
    {
        double valorComTaxa = 0;

        if (tipoMoeda.ToUpper() == "BRL")
        {
            valorComTaxa = valor * (1 + taxaSaque);
            if (cliente.RealizarSaque(valorComTaxa, tipoMoeda))
            {
                saldoBanco += valor * taxaSaque;
                operacoesSaque++;
                return true;
            }
        }
        else
        {
            double valorConvertido = valor * taxaCambio;
            if (cliente.RealizarSaque(valorConvertido, tipoMoeda))
            {
                saldoBanco += valor * (1 - taxaCambio);
                operacoesSaque++;
                return true;
            }
        }

        return false;
    }

    public void RealizarCambio(Cliente cliente, double valor, string tipoMoedaOrigem, string tipoMoedaDestino)
    {
        double valorConvertido = 0;

        switch (tipoMoedaOrigem.ToUpper())
        {
            case "BRL":
                valorConvertido = valor * taxaCambio;
                saldoBanco -= valor;
                break;
            case "USD":
                valorConvertido = valor / taxaCambio;
                saldoBanco -= valor;
                break;
            case "ARS":
                valorConvertido = valor / taxaCambio;
                saldoBanco -= valor;
                break;
            case "CAD":
                valorConvertido = valor / taxaCambio;
                saldoBanco -= valor;
                break;
            default:
                Console.WriteLine("Tipo de moeda de origem inválido.");
                return;
        }

        switch (tipoMoedaDestino.ToUpper())
        {
            case "BRL":
                saldoBanco += valorConvertido;
                break;
            case "USD":
                saldoBanco += valorConvertido * taxaCambio;
                break;
            case "ARS":
                saldoBanco += valorConvertido * taxaCambio;
                break;
            case "CAD":
                saldoBanco += valorConvertido * taxaCambio;
                break;
            default:
                Console.WriteLine("Tipo de moeda de destino inválido.");
                return;
        }

        operacoesCambio++;
    }

    public bool ComprarMoeda(Cliente cliente, double valor, string tipoMoeda)
    {
        double valorConvertido = valor * taxaCambioCompra;

        if (cliente.RealizarSaque(valorConvertido, "BRL"))
        {
            cliente.RealizarDeposito(valor, tipoMoeda);
            operacoesCompraMoeda++;
            return true;
        }

        return false;
    }

    public bool RealizarTrocoViaPixETED(Cliente cliente, double valor)
    {
        if (saldoPixETED >= valor)
        {
            saldoPixETED -= valor;
            cliente.RealizarDeposito(valor, "BRL");
            return true;
        }

        return false;
    }

    public void ExibirRelatorios()
    {
        Console.WriteLine("\nRelatórios:");
        Console.WriteLine($"Quantidade de operações de Depósito: {OperacoesDeposito}");
        Console.WriteLine($"Quantidade de operações de Saque: {OperacoesSaque}");
        Console.WriteLine($"Quantidade de operações de Câmbio: {OperacoesCambio}");
        Console.WriteLine($"Quantidade de operações de Compra de Moeda: {OperacoesCompraMoeda}");
        Console.WriteLine($"Saldo total em conta bancária: R$ {saldoBanco:F2}");
        Console.WriteLine($"Saldo total de troco via PIX ou TED: R$ {saldoPixETED:F2}");
        Console.WriteLine($"Quantidade de notas disponíveis:");
        foreach (var nota in notas)
        {
            Console.WriteLine($"{nota.Key}: {nota.Value}");
        }
        Console.WriteLine($"Quantidade de moedas disponíveis:");
        foreach (var moeda in moedas)
        {
            Console.WriteLine($"{moeda.Key}: {moeda.Value}");
        }
    }
}

