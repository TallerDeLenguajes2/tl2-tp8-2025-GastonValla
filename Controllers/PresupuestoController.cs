using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_GastonValla.Models;
using MiApi.Data;
using tl2_tp8_2025_GastonValla.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp8_2025_GastonValla.Controllers;

public class PresupuestoController : Controller
{
    private readonly ILogger<PresupuestoController> _logger;
    private PresupuestoRepository presupuestoRepository;


    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        presupuestoRepository = new PresupuestoRepository();
        _logger = logger;
    }

    private readonly ProductoRepository _productoRepo = new ProductoRepository();

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        // 1. Obtener los productos para el SelectList
        List<Producto> productos = _productoRepo.GetAll();

        // 2. Crear el ViewModel
        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
        IdPresupuesto = id, // Pasamos el ID del presupuesto actual
        // 3. Crear el SelectList
        ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        {
            if (!ModelState.IsValid)
            {
            var productos = _productoRepo.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            // Devolvemos el modelo con los errores y el dropdown recargado
            return View(model);
            }
            // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
            bool retorno = presupuestoRepository.AgregarProducto(model.IdPresupuesto, model.IdProducto, model.Cantidad);

            // 3. Redirigimos al detalle del presupuesto
            if(retorno==true){return RedirectToAction(nameof(PresupuestoEdit), new { IdPres = model.IdPresupuesto });}
            else 
            {
            var productos = _productoRepo.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            // Devolvemos el modelo con los errores y el dropdown recargado
            return View(model);
            }
        }
    }


    [HttpGet]
    public IActionResult PresupuestoIndex()
    {
        return View();
    }
    [HttpPost]
    public IActionResult PresupuestoIndex(int IdPres)
    {
        Presupuesto? retorno = presupuestoRepository.Buscar(IdPres);
        return View(retorno);
    }

    [HttpGet]
    public IActionResult PresupuestoDetail(int? IdPres)
    {
        List<Presupuesto> retorno = new List<Presupuesto>();
        if(IdPres == null)
        {
            retorno = presupuestoRepository.GetAll();
            return View(retorno);
        }
        else
        {
            retorno.Add(presupuestoRepository.Buscar(IdPres));
            return View(retorno);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult PresupuestoCreate()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult PresupuestoCreate(string? nombre)
    {
        bool? retorno = presupuestoRepository.Nuevo(nombre);
        return View(retorno);
    }

    [HttpGet]
    public IActionResult PresupuestoEdit(int? idPres)
    {
        if(idPres!=null){
            Presupuesto? encontrado = presupuestoRepository.Buscar(idPres);
            return View(encontrado);
        }
        else
        {
            return View(null);
        }
    }

    [HttpPost]
    public IActionResult PresupuestoEditConfirm(PresupuestoViewModel pres)
    {
        Presupuesto presupuesto = new Presupuesto(pres.IdPresupuesto, pres.NombreDestinatario, pres.FechaCreacion);
        bool retorno = presupuestoRepository.Modificar(pres.IdPresupuesto, presupuesto);
        return View(retorno);
    }


    [HttpGet]
    public IActionResult PresupuestoDelete()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult PresupuestoDelete(int? idPres)
    {
        if(idPres!=null){
            Presupuesto? encontrado = presupuestoRepository.Buscar(idPres);
            PresupuestoViewModel retorno = new PresupuestoViewModel
            {
                IdPresupuesto = encontrado.IdPresupuesto,
                NombreDestinatario = encontrado.NombreDestinatario,
                FechaCreacion = encontrado.FechaCreacion
            };
            return View(retorno);
        }
        else
        {
            return View(null);
        }
    }

    [HttpPost]
    public IActionResult PresupuestoDeleteConfirm(PresupuestoViewModel pres)
    {
        bool retorno = presupuestoRepository.Borrar(pres.IdPresupuesto);
        return View(retorno);
    }
}
