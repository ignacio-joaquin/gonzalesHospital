using System;

namespace AdministracionSanatorio
{
    public class Doctor
    {
        public string NombreCompleto { get; set; }
        public string Matricula { get; set; }
        public string Especialidad { get; set; }
        public bool Disponible { get; set; }

        public Doctor(string nombreCompleto, string matricula, string especialidad, bool disponible)
        {
            NombreCompleto = nombreCompleto;
            Matricula = matricula;
            Especialidad = especialidad;
            Disponible = disponible;
        }

        public override string ToString()
        {
            string estado = Disponible ? "Disponible" : "No disponible";
            return $"Dr. {NombreCompleto} - Matrícula: {Matricula} - Especialidad: {Especialidad} - Estado: {estado}";
        }
    }
}