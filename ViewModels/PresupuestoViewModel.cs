using System.ComponentModel.DataAnnotations;
using System;
namespace tl2_tp8_2025_GastonValla.Web.ViewModels
{
    public class PresupuestoViewModel
    {
        public int? IdPresupuesto { get; set; }
        // Validación: Requerido
            [Display(Name = "Nombre o Email del Destinatario")]
            [Required(ErrorMessage = "El nombre o email es obligatorio.")]
        // Opcional: Se puede añadir la validación de formato de email si se opta por guardar el mail.
        // [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string? NombreDestinatario { get; set; }

        // Validación: Requerido y tipo de dato
            [Display(Name = "Fecha de Creación")]
            [Required(ErrorMessage = "La fecha es obligatoria.")]
            //[DataType(DataType.Date)]
        public string? FechaCreacion { get; set; }

        // La validación de que la fecha no es futura se hará en el Controlador (ver Etapa 3).
    }
}
