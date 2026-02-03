public class Presupuesto
{
    public int? IdPresupuesto { get; }
    public string? NombreDestinatario { get; private set; }
    public string? FechaCreacion { get; private set; }
    public List<PresupuestoDetalle> Detalle;

    public Presupuesto()
    {
        Detalle = new List<PresupuestoDetalle>();
    }
    public Presupuesto(int? IdPresupuesto, string? NombreDestinatario, string? FechaCreacion)
    {
        this.IdPresupuesto = IdPresupuesto;
        this.NombreDestinatario = NombreDestinatario;
        this.FechaCreacion = FechaCreacion;
        Detalle = new List<PresupuestoDetalle>();
    }
    public double MontoPresupuesto()
    {
        double sum = 0;
        foreach(PresupuestoDetalle p in Detalle)
        {
            if(p.MiProducto!=null)
            {sum += p.MiProducto.Precio*p.Cantidad;}
        }
        return sum;
    }
    double MontoPresupuestoConIva()
    {
        double retorno = MontoPresupuesto() * 1.21;
        return retorno;
    }
    public int CantidadProductos()
    {
        int sum = 0;
        foreach(PresupuestoDetalle p in Detalle)
        {sum += p.Cantidad;}
        return sum;
    }
}