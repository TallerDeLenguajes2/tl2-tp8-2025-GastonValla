using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_GastonValla.Models;
using MiApi.Data;

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
    public IActionResult PresupuestoDetail()
    {
        List<Presupuesto> retorno = presupuestoRepository.GetAll();
        return View(retorno);
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
    public IActionResult PresupuestoEdit()
    {
        return View();
    }
    
    [HttpPost]
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
    public IActionResult PresupuestoEditConfirm(int IdPresupuesto, string? NombreDestinatario, string? FechaCreacion)
    {
        Presupuesto presupuesto = new Presupuesto(IdPresupuesto, NombreDestinatario, FechaCreacion);
        bool retorno = presupuestoRepository.Modificar(IdPresupuesto, presupuesto);
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
            return View(encontrado);
        }
        else
        {
            return View(null);
        }
    }

    [HttpPost]
    public IActionResult PresupuestoDeleteConfirm(int IdPresupuesto, string? NombreDestinatario, string? FechaCreacion)
    {
        bool retorno = presupuestoRepository.Borrar(IdPresupuesto);
        return View(retorno);
    }
}
