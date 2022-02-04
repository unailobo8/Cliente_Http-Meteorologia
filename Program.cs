using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NW = Newtonsoft.Json;
using MS = System.Text.Json;
using Baliza.Models;


    class Program
    {
         static async Task Main(string[] args)
        {
            TimeSpan Actualizacion = new TimeSpan(0,0,600);

            while(true)
            {
               await TareaAsincrona();
               Thread.Sleep(Actualizacion);
            }
        }

        
    

     static async Task TareaAsincrona()
    {
        var cliente = new HttpClient();
        var key = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJtZXQwMS5hcGlrZXkiLCJpc3MiOiJJRVMgUExBSUFVTkRJIEJISSBJUlVOIiwiZXhwIjoyMjM4MTMxMDAyLCJ2ZXJzaW9uIjoiMS4wLjAiLCJpYXQiOjE2NDE5NzU2MjIsImVtYWlsIjoiaWtiZWhAcGxhaWF1bmRpLm5ldCJ9.B2dLkumVqyybpNz9jvD_1gGwAHsfsPweiXEXS7Ib-ti0KWAcMLA7Ad8uEUtnSYLmM_OQBLhsPHrUOnm9fg7VDzFAk22o59VJTpeXkYlMMal31v2gqJfV9KZtu5JFPafCGCnmk-DLzQRF4-7L4VEcJWKpxOX_AQEH_F0twxPeWwfSyFGuyaYU7hzvtk3WhS8m4JFO4uff9tCvnPusiizbTIoixQPyUAXs4tSaY7clIR_9I_AeqfYzoTEpV7vUirr7WJuiTx-Ji1GtF-GH9n-UXmY8nFogiTBwh5hFPBhdqKeWg7pnMDUpBvCahX13D1vm6eJCYGks7-Etl3wY4Fw7lQ";
        cliente.DefaultRequestHeaders.Add("User-Agent", "mi consola");
        cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        cliente.DefaultRequestHeaders.Add("Authorization", "Bearer "+ key);

        var urlRegiones = $"https://api.euskadi.eus/euskalmet/geo/regions/basque_country/zones";
        HttpResponseMessage RespuestaHTTPApiRegiones = await cliente.GetAsync(urlRegiones);
        var ContenidoRespApiReg = await RespuestaHTTPApiRegiones.Content.ReadAsStringAsync();
        dynamic objetoJsonDeserializadoReg = NW.JsonConvert.DeserializeObject(ContenidoRespApiReg);
        var vObjetoJsonDeserializadoReg = objetoJsonDeserializadoReg;
        foreach (var zonas in vObjetoJsonDeserializadoReg)
        {
            var urlLocalidades = $"https://api.euskadi.eus/euskalmet/geo/regions/basque_country/zones/{zonas.regionZoneId}/locations";
            HttpResponseMessage RespuestaHTTPApiLocalidades = await cliente.GetAsync(urlLocalidades);
            var ContenidoRespApiLoc = await RespuestaHTTPApiLocalidades.Content.ReadAsStringAsync();
            dynamic objetoJsonDeserializadoLoc = NW.JsonConvert.DeserializeObject(ContenidoRespApiLoc);
            var vObjetoJsonDeserializadoLoc = objetoJsonDeserializadoLoc;
            foreach (var item in vObjetoJsonDeserializadoLoc)
            {
            
            var Dia = DateTime.Today.Day;
                var diaHoy = "";
                var AñoHoy = DateTime.Today.Year;
                var mes = DateTime.Today.Month;
                var mesHoy = "";
                var hor = DateTime.Now.Hour;
                var horaHoy = "";
                var minuto = DateTime.Now.Minute;
                var minutoHoy = "";
                
                if (Convert.ToInt32(Dia) / 10 == 0)
                {
                    diaHoy = "0" + Convert.ToInt32(Dia);
                }
                else
                {
                    diaHoy = "" + Convert.ToInt32(Dia);
                }
                if (Convert.ToInt32(mes) / 10 == 0)
                {
                    mesHoy = "0" + Convert.ToInt32(mes);
                }
                else
                {
                    mesHoy = "" + Convert.ToInt32(mes);
                }
                if (Convert.ToInt32(hor) / 10 == 0)
                {
                    horaHoy = "0" + Convert.ToInt32(hor);
                }
                else
                {
                    horaHoy = "" + Convert.ToInt32(hor);
                }
                if (Convert.ToInt32(minuto) / 10 == 0)
                {
                    minutoHoy = "0" + Convert.ToInt32(minuto);
                }
                else
                {
                    minutoHoy = "" + Convert.ToInt32(minuto);
                }
                
                var urlLocalizacionForecast = $"https://api.euskadi.eus/euskalmet/weather/regions/basque_country/zones/{zonas.regionZoneId}/locations/{item.regionZoneLocationId}/forecast/trends/measures/at/{AñoHoy}/{mesHoy}/{diaHoy}/for/{AñoHoy}{mesHoy}{diaHoy}";
                var urlLocalizacionHumidity = $"https://api.euskadi.eus/euskalmet/weather/regions/basque_country/zones/{zonas.regionZoneId}/locations/{item.regionZoneLocationId}/reports/for/{AñoHoy}/{mesHoy}/{diaHoy}/last";

                try
                {
                    HttpResponseMessage RespuestaHTTPApiFore = await cliente.GetAsync(urlLocalizacionForecast);
                    var ContenidoRespApiFore = await RespuestaHTTPApiFore.Content.ReadAsStringAsync();
                    dynamic ObjetoJsonDeserializadoFore = NW.JsonConvert.DeserializeObject(ContenidoRespApiFore);
                    var tiempoPretry = ObjetoJsonDeserializadoFore.trends.set;

                    HttpResponseMessage RespuestaHTTPHumidity = await cliente.GetAsync(urlLocalizacionHumidity);
                    var ContenidoHumidity = await RespuestaHTTPHumidity.Content.ReadAsStringAsync();
                    dynamic ObjetoJsonDeserializadoHumidity = NW.JsonConvert.DeserializeObject(ContenidoHumidity);

                    var hora = Convert.ToInt32(DateTime.Now.Hour) - 1;
                    var horaAhora = "";
                    if (hora / 10 == 0)
                    {
                        horaAhora = "0" + hora;
                    }
                    else
                    {
                        horaAhora = "" + hora;
                    }
                    var valor = 0;
                    var i = 0;
                    var stringComp = $"LocalTime:[{horaAhora}:00:00:000..{horaAhora}:59:59:999]";
                    foreach (var x in tiempoPretry)
                    {
                        if (Convert.ToString(x.range) == stringComp) valor = i;
                        i++;
                    }


                    dynamic pp = ObjetoJsonDeserializadoFore.trends.set[valor].temperature;
                    dynamic pp1 = ObjetoJsonDeserializadoFore.trends.set[valor].precipitation;
                    dynamic pp2 = ObjetoJsonDeserializadoFore.trends.set[valor].windspeed;
                    dynamic pp3 = ObjetoJsonDeserializadoFore.trends.set[valor].symbolSet.weather.nameByLang.SPANISH;
                    dynamic pp4 = ObjetoJsonDeserializadoHumidity.report.humidity;


                    Console.WriteLine($"====");
                    Console.WriteLine($"------------------------------------------------------------------");
                    Console.WriteLine($"Localidad : {item.regionZoneLocationId}");
                    Console.WriteLine($"Hora : {ObjetoJsonDeserializadoFore.trends.set[valor].range}");
                    Console.WriteLine($"Temperatura : {pp.value} ºC ");
                    Console.WriteLine($"Precitipacion acumulada : {pp1.value} ml ");
                    Console.WriteLine($"Velocidad del Viento : {pp2.value} Km/h");
                    Console.WriteLine($"Descripcion del tiempo : {pp3} ");
                    Console.WriteLine($"Humedad : {pp4.value} ");
                    Console.WriteLine($"------------------------------------------------------------------");
                    Console.WriteLine("====");



                    using (var db = new BalizaContext())
                    {
                        try
                        {
                            string localizacion = item.regionZoneLocationId;
                            var row = db.Balizas2.Where(a => a.Municipio == localizacion).SingleOrDefault(); 
                             if (row == null)
                             {
                                 var a1 = new Balizas2 { Municipio = item.regionZoneLocationId, Region = zonas.regionZoneId, Temperatura = pp.value, Velocidad_Viento = pp2.value, Descripcion = pp3, Humedad = pp4.value , Precipitacion_acumulada = pp1.value, ultimaHora = ObjetoJsonDeserializadoFore.trends.set[valor].range, Imagen = ObjetoJsonDeserializadoFore.trends.set[valor].symbolSet.weather.path,}; db.Balizas2.Add(a1);
                             }
                             else
                             {
                                 row.ultimaHora = ObjetoJsonDeserializadoFore.trends.set[valor].range;
                                 row.Velocidad_Viento = pp2.value;
                                 row.Temperatura = pp.value;
                                 row.Descripcion = pp3;
                                 row.Humedad = pp4.value;
                                 row.Precipitacion_acumulada = pp1.value;
                                 row.Imagen = ObjetoJsonDeserializadoFore.trends.set[valor].symbolSet.weather.path;
                             }
                            db.SaveChanges();
                        }
                        catch (Exception p)
                        {
                            Console.WriteLine("Error al guardar: " + p);
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Lo siento no hay datos en Euskalmet de "+ item.regionZoneLocationId);
                    
                }
                
            }
        }
    }
}
