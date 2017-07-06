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
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DisplayName("Total de la Venta")]
        public decimal TotalVenta { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DisplayName("Impuesto IVA 19% CLP")]
        public decimal IVA { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DisplayName("Valor Bruto en CLP")]
        public decimal TotalBruto { get; set; }
        [DisplayName("RUT Dueño")]
        public string RUTDueno { get; set; }
    }
}