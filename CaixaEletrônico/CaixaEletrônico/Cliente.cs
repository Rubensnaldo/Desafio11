using System;
using System.Collections.Generic;

public class Cliente
{
    private string nomeCompleto;
    private string cpf;
    private string senha;
    private Dictionary<string, double> saldos;

    public Cliente(string nomeCompleto, string cpf, string senha)
    {
        this.nomeCompleto = nomeCompleto;
        this.cpf = cpf;
        if (senha == "0000" || senha == "1111" || senha == "9999")
        {
            throw new ArgumentException("A senha deve ser diferente de 0000, 1111 ou 9999.");
        }
        this.senha = senha;

        saldos = new Dictionary<string, double>
        {
            { "BRL", 0 },
            { "USD", 0 },
            { "ARS", 0 },
            { "CAD", 0 }
        };
    }

    public string NomeCompleto
    {
        get { return nomeCompleto; }
    }

    public string CPF
    {
        get { return cpf; }
    }

    public double GetSaldo(string tipoMoeda)
    {
        if (saldos.ContainsKey(tipoMoeda))
        {
            return saldos[tipoMoeda];
        }
        return 0;
    }

    public double SaldoBRL
    {
        get { return saldos["BRL"]; }
    }

    public double SaldoUSD
    {
        get { return saldos["USD"]; }
    }

    public double SaldoARS
    {
        get { return saldos["ARS"]; }
    }

    public double SaldoCAD
    {
        get { return saldos["CAD"]; }
    }




    public bool RealizarDeposito(double valor, string tipoMoeda)
    {
        if (saldos.ContainsKey(tipoMoeda))
        {
            saldos[tipoMoeda] += valor;
            return true;
        }
        return false;
    }

    public bool RealizarSaque(double valor, string tipoMoeda)
    {
        if (saldos.ContainsKey(tipoMoeda) && valor <= saldos[tipoMoeda])
        {
            saldos[tipoMoeda] -= valor;
            return true;
        }
        return false;
    }

    public bool ReceberTrocoViaPixETED(double valor)
    {
        if (valor <= saldos["BRL"])
        {
            saldos["BRL"] -= valor;
            return true;
        }
        return false;
    }

}
