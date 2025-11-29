
public class Producto
{
    private static int contadorIds;
    public int IdProducto { get; set; }
    public string? Descripcion { get; set; }
    public double Precio { get; set; }

    public Producto(){}
    public Producto(int IdProducto, string? Descripcion, double Precio)
    {
        this.IdProducto = IdProducto;
        this.Descripcion = Descripcion;
        this.Precio = Precio;
    }
}