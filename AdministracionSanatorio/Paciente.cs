using System;
using System.Collections.Generic;

namespace AdministracionSanatorio
{
    public class Paciente
    {
        public string DocumentoIdentidad { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string ObraSocial { get; set; }
        public double MontoCobertura { get; set; }
        private List<IntervencionProgramada> intervenciones;

        // Constructor para paciente con obra social
        public Paciente(string documentoIdentidad, string nombreCompleto, string telefono, string obraSocial, double montoCobertura)
        {
            DocumentoIdentidad = documentoIdentidad;
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            ObraSocial = obraSocial;
            MontoCobertura = montoCobertura;
            intervenciones = new List<IntervencionProgramada>();
        }

        // Constructor para paciente sin obra social
        public Paciente(string documentoIdentidad, string nombreCompleto, string telefono)
            : this(documentoIdentidad, nombreCompleto, telefono, null, 0)
        {
        }

        public void AgregarIntervencion(IntervencionProgramada intervencion)
        {
            intervenciones.Add(intervencion);
        }

        public List<IntervencionProgramada> ObtenerIntervencionesPendientes()
        {
            return intervenciones.FindAll(i => !i.Pagado);
        }

        public List<IntervencionProgramada> ObtenerTodasLasIntervenciones()
        {
            return intervenciones;
        }

        public override string ToString()
        {
            string infoObraSocial = ObraSocial != null 
                ? $"Obra Social: {ObraSocial} ({MontoCobertura}% cobertura)" 
                : "Sin obra social";
            
            return $"DNI: {DocumentoIdentidad}\n" +
                   $"Nombre: {NombreCompleto}\n" +
                   $"Teléfono: {Telefono}\n" +
                   $"{infoObraSocial}\n" +
                   $"Intervenciones registradas: {intervenciones.Count}";
        }
    }
}