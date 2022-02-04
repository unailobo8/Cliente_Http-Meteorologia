using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baliza.Models
{
    [Table("Balizas")]
    public class Balizas2
    {
         [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Municipio{get; set;}
        public string Region{get; set;}
        public string Temperatura{get; set;}
        public string Descripcion{get; set;}
        public string Humedad{get; set;}
        public string Velocidad_Viento{get; set;}
        public string Precipitacion_acumulada{get; set;}
        public string ultimaHora{get; set;}
        public string Imagen{get;set;}
    }
}