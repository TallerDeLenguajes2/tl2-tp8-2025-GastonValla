using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_GastonValla.Models;
using MiApi.Data;

namespace tl2_tp8_2025_GastonValla.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;
    private ProductoRepository productoRepository;

    public ProductoController(ILogger<ProductoController> logger)
    {
        productoRepository= new ProductoRepository();
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult ProductoIndex()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ProductoIndex(int idProd)
    {
        Producto? encontrado = productoRepository.Buscar(idProd);
        return View(encontrado);
    }

    [HttpGet]
    public IActionResult ProductoDetail()
    {
        List<Producto> productos = productoRepository.GetAll();
        return View(productos);
    }

    [HttpGet]
    public IActionResult ProductoCreate()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ProductoCreate(string? Descripcion, double Precio)
    {
        bool? retorno = productoRepository.Nuevo(Descripcion, Precio);
        return View(retorno);
    }

    [HttpGet]
    public IActionResult ProductoEdit()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ProductoEdit(int? idProd)
    {
        if(idProd!=null){
            Producto? encontrado = productoRepository.Buscar(idProd);
            return View(encontrado);
        }
        else
        {
            return View(null);
        }
    }

    [HttpPost]
    public IActionResult ProductoEditConfirm(Producto producto)
    {

        bool retorno = productoRepository.Modificar(producto.IdProducto, producto);
        return View(retorno);
    }

    [HttpGet]
    public IActionResult ProductoDelete()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ProductoDelete(int? idProd)
    {
        if(idProd!=null){
            Producto? encontrado = productoRepository.Buscar(idProd);
            return View(encontrado);
        }
        else
        {
            return View(null);
        }
    }

    [HttpPost]
    public IActionResult ProductoDeleteConfirm(Producto producto)
    {

        bool retorno = productoRepository.Borrar(producto.IdProducto);
        return View(retorno);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
