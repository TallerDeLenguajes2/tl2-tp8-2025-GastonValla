using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_GastonValla.Models;
using MiApi.Data;
using tl2_tp8_2025_GastonValla.Web.ViewModels;

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
    public IActionResult ProductoIndex(string? desc)
    {
        List<Producto> encontrado = productoRepository.BuscarN(desc);
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
    public IActionResult ProductoCreate(ProductoViewModel prod)
    {
        if(!ModelState.IsValid)
        {
            return View(prod);
        }
        Producto p = new Producto{Descripcion = prod.Descripcion, Precio = (double)prod.Precio};

        bool? retorno = productoRepository.Nuevo(p.Descripcion, p.Precio);
        if(retorno == true)
        {return RedirectToAction("ProductoIndex");}
        ModelState.AddModelError("", "No se pudo guardar el producto. Intente nuevamente.");
        return View(prod);
    }

    [HttpGet]
    public IActionResult ProductoEdit()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ProductoEdit(int? idProd)
    {
        if(idProd!=null && idProd>0){
            Producto? encontrado = productoRepository.Buscar(idProd);
            if(encontrado!=null)
            {
                ProductoViewModel retorno = new ProductoViewModel
                {
                    IdProducto = idProd,
                    Descripcion = encontrado.Descripcion,
                    Precio = (decimal)encontrado.Precio
                };
                return View(retorno);
            }
            else
            {return View(null);}
        }
        else
        {
            return View(null);
        }
    }

    [HttpPost]
    public IActionResult ProductoEditConfirm(ProductoViewModel productoModelo)
    {
        if(!ModelState.IsValid)
        {
            return View("ProductoEdit",productoModelo);
        }
        Producto producto = new Producto()
        {
            Descripcion = productoModelo.Descripcion,
            Precio = (double)productoModelo.Precio
        };
        bool retorno = productoRepository.Modificar(productoModelo.IdProducto, producto);
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
    public IActionResult ProductoDeleteConfirm(ProductoViewModel producto)
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
