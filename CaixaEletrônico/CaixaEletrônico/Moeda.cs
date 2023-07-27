public class Moeda
{
    public string TipoMoeda { get; private set; }
    public double Valor { get; private set; }

    public Moeda(string tipoMoeda, double valor)
    {
        TipoMoeda = tipoMoeda;
        Valor = valor;
    }
}
