using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace tl2_tp8_2025_GastonValla.Web.ViewModels
{
    public class ProductoViewModel
    {
        // Se incluye Id para la acción de EDICIÓN
        [DefaultValue(0)]
        public int? IdProducto { get; set; }
        
        // Validación: Máximo 250 caracteres. Es opcional por defecto si no tiene [Required]
        [Display(Name = "Descripción del Producto")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        public string? Descripcion { get; set; }

        // Validación: Requerido y debe ser positivo
        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        public ProductoViewModel(){}
    }
}