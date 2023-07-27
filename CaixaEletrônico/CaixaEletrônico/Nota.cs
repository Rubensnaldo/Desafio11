public class Nota
{
    public string TipoMoeda { get; private set; }
    public double Valor { get; private set; }

    public Nota(string tipoMoeda, double valor)
    {
        TipoMoeda = tipoMoeda;
        Valor = valor;
    }
}
