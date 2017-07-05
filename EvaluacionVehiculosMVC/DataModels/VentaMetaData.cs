using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvaluacionVehiculosMVC.DataModels
{

    [MetadataType(typeof(VentaMetaData))]
    public partial class Ventas
    {

    }
    public class VentaMetaData
    {
        public int IdVenta { get; set; }
        [DisplayName("Fecha de Venta")]
        [DataType(DataType.Date)]
        public System.DateTime FechaVenta { get; set; }
        public string Patente { get; set; }
        [DisplayName("Total de la Venta")]
        public decimal TotalVenta { get; set; }
        [DisplayName("Impuesto IVA CLP")]
        public decimal IVA { get; set; }
        [DisplayName("Valor Bruto en CLP")]
        public decimal TotalBruto { get; set; }
        [DisplayName("RUT Dueño")]
        public string RUTDueno { get; set; }
    }
}