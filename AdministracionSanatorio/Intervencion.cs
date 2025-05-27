using System;

namespace AdministracionSanatorio
{
    // Clase base para Intervencion
    public class Intervencion
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Especialidad { get; set; }
        public decimal Arancel { get; set; }

        public Intervencion(string codigo, string descripcion, string especialidad, decimal arancel)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Especialidad = especialidad;
            Arancel = arancel;
        }

        // Método virtual para calcular costo, puede ser sobrescrito en clases derivadas
        public virtual decimal CalcularCosto()
        {
            return Arancel;
        }

        public override string ToString()
        {
            return $"Código: {Codigo}, Descripción: {Descripcion}, Especialidad: {Especialidad}, Arancel: {Arancel:C}";
        }
    }

    // Clase derivada para intervención común (sin costo adicional)
    public class IntervencionComun : Intervencion
    {
        public IntervencionComun(string codigo, string descripcion, string especialidad, decimal arancel)
            : base(codigo, descripcion, especialidad, arancel)
        {
        }

        // Simplemente devuelve el arancel base
        public override decimal CalcularCosto()
        {
            return Arancel;
        }

        public override string ToString()
        {
            return base.ToString() + ", Tipo: Común";
        }
    }

    // Clase derivada para intervención de alta complejidad
    public class IntervencionAltaComplejidad : Intervencion
    {
        // Porcentaje adicional para costo extra (ejemplo: 0.20 = 20%)
        public decimal PorcentajeAdicional { get; set; }

        public IntervencionAltaComplejidad(string codigo, string descripcion, string especialidad, decimal arancel, decimal porcentajeAdicional)
            : base(codigo, descripcion, especialidad, arancel)
        {
            PorcentajeAdicional = porcentajeAdicional;
        }

        // Se sobrescribe para agregar el porcentaje adicional
        public override decimal CalcularCosto()
        {
            return Arancel + (Arancel * PorcentajeAdicional);
        }

        public override string ToString()
        {
            return base.ToString() + $", Tipo: Alta Complejidad, % Adicional: {PorcentajeAdicional:P}";
        }
    }
}
