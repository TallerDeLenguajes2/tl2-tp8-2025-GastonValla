public class PresupuestoDetalle
{
    public Producto? MiProducto { get; private set; }
    public int Cantidad { get; private set; }

    public PresupuestoDetalle(Producto? MiProducto, int Cantidad)
    {
        this.MiProducto = MiProducto;
        this.Cantidad = Cantidad;
    }
    
}