using System;
using System.Collections.Generic;

namespace AdministracionSanatorio
{
    // Clase para representar una intervención programada/realizada
    public class IntervencionProgramada
    {
        public int Identificador { get; set; }
        public DateTime Fecha { get; set; }
        public Intervencion Intervencion { get; set; }
        public Doctor Medico { get; set; }
        public Paciente Paciente { get; set; }
        public bool Pagado { get; set; }

        public IntervencionProgramada(int identificador, DateTime fecha, Intervencion intervencion, Doctor medico, Paciente paciente, bool pagado)
        {
            Identificador = identificador;
            Fecha = fecha;
            Intervencion = intervencion;
            Medico = medico;
            Paciente = paciente;
            Pagado = pagado;
        }
    }

    public class Hospital
    {
        // Colecciones para almacenar los datos
        public List<Paciente> Pacientes { get; private set; }
        public List<Doctor> Doctores { get; private set; }
        public List<Intervencion> Intervenciones { get; private set; }
        public List<IntervencionProgramada> IntervencionesProgramadas { get; private set; }
        public double PorcentajeAdicionalAltaComplejidad { get; set; }

        // Constructor
        public Hospital(double porcentajeAdicional)
        {
            Pacientes = new List<Paciente>();
            Doctores = new List<Doctor>();
            Intervenciones = new List<Intervencion>();
            IntervencionesProgramadas = new List<IntervencionProgramada>();
            PorcentajeAdicionalAltaComplejidad = porcentajeAdicional;
        }

        // Métodos para cargar datos de prueba (compatibles con tu ejemplo)
        public void CargarDatosPrueba()
        {
            // Doctores
            Doctores.Add(new Doctor("Juan Pérez", "12345", "Cardiología", true));
            Doctores.Add(new Doctor("Laura Gómez", "23456", "Traumatología", false));
            Doctores.Add(new Doctor("Carlos Ruiz", "34567", "Neurología", true));
            Doctores.Add(new Doctor("María Silva", "45678", "Gastroenterología", true));
            Doctores.Add(new Doctor("Fernando Torres", "56789", "Cardiología", true));
            Doctores.Add(new Doctor("Cecilia López", "67890", "Traumatología", true));

            // Pacientes
            Pacientes.Add(new Paciente("30111222", "Ana Torres", "1111-2222", "ObraMed", 80));
            Pacientes.Add(new Paciente("29222333", "Luis Fernández", "2222-3333", null, 0));
            Pacientes.Add(new Paciente("28444555", "Clara Méndez", "3333-4444", "SaludPlus", 90));
            Pacientes.Add(new Paciente("27555666", "Pedro Gómez", "4444-5555", "VidaTotal", 70));
            Pacientes.Add(new Paciente("26666777", "Lucía Ortega", "5555-6666", null, 0));
            Pacientes.Add(new Paciente("25777888", "Jorge Ramírez", "6666-7777", "SaludPlus", 60));

            // Intervenciones comunes
            Intervenciones.Add(new IntervencionComun("INT001", "Bypass coronario", "Cardiología", 120000));
            Intervenciones.Add(new IntervencionComun("INT003", "Artroscopía de rodilla", "Traumatología", 80000));
            Intervenciones.Add(new IntervencionComun("INT005", "Endoscopía digestiva", "Gastroenterología", 40000));
            Intervenciones.Add(new IntervencionComun("INT007", "Colocación de stent", "Cardiología", 95000));
            Intervenciones.Add(new IntervencionComun("INT008", "Reducción de fractura", "Traumatología", 60000));

            // Intervenciones de alta complejidad - agregando el porcentaje adicional
            decimal porcentajeAdicional = (decimal)(PorcentajeAdicionalAltaComplejidad / 100);
            Intervenciones.Add(new IntervencionAltaComplejidad("INT002", "Neurocirugía", "Neurología", 200000, porcentajeAdicional));
            Intervenciones.Add(new IntervencionAltaComplejidad("INT004", "Revascularización miocárdica", "Cardiología", 250000, porcentajeAdicional));
            Intervenciones.Add(new IntervencionAltaComplejidad("INT006", "Cirugía de columna", "Traumatología", 180000, porcentajeAdicional));
            Intervenciones.Add(new IntervencionAltaComplejidad("INT009", "Cirugía bariátrica", "Gastroenterología", 220000, porcentajeAdicional));
            Intervenciones.Add(new IntervencionAltaComplejidad("INT010", "Craneotomía", "Neurología", 270000, porcentajeAdicional));
        }

        // Métodos para gestionar pacientes
        public void AgregarPaciente(Paciente paciente)
        {
            Pacientes.Add(paciente);
        }

        public Paciente BuscarPacientePorDNI(string dni)
        {
            return Pacientes.Find(p => p.DocumentoIdentidad == dni);
        }

        public List<Paciente> ListarPacientes()
        {
            return Pacientes;
        }

        // Métodos para gestionar doctores
        public void AgregarDoctor(Doctor doctor)
        {
            Doctores.Add(doctor);
        }

        public List<Doctor> ListarDoctoresDisponiblesPorEspecialidad(string especialidad)
        {
            return Doctores.FindAll(d => d.Especialidad == especialidad && d.Disponible);
        }

        // Métodos para gestionar intervenciones
        public void AgregarIntervencion(Intervencion intervencion)
        {
            Intervenciones.Add(intervencion);
        }

        public Intervencion BuscarIntervencionPorCodigo(string codigo)
        {
            return Intervenciones.Find(i => i.Codigo == codigo);
        }

        // Método para programar una nueva intervención
        public bool ProgramarIntervencion(string dniPaciente, string codigoIntervencion, string matriculaDoctor, DateTime fecha)
        {
            var paciente = BuscarPacientePorDNI(dniPaciente);
            if (paciente == null) return false;

            var intervencion = BuscarIntervencionPorCodigo(codigoIntervencion);
            if (intervencion == null) return false;

            var doctor = Doctores.Find(d => d.Matricula == matriculaDoctor);
            if (doctor == null || !doctor.Disponible || doctor.Especialidad != intervencion.Especialidad)
                return false;

            var nuevaIntervencion = new IntervencionProgramada(
                IntervencionesProgramadas.Count + 1,
                fecha,
                intervencion,
                doctor,
                paciente,
                false
            );
            
            IntervencionesProgramadas.Add(nuevaIntervencion);
            paciente.AgregarIntervencion(nuevaIntervencion);
            return true;
        }

        // Método para generar reporte de liquidaciones pendientes
        public List<string> GenerarReporteLiquidacionesPendientes()
        {
            var reporte = new List<string>();

            foreach (var intervencion in IntervencionesProgramadas)
            {
                if (!intervencion.Pagado)
                {
                    string obraSocial = intervencion.Paciente.ObraSocial ?? "-";
                    double costoTotal = CalcularCostoIntervencion(intervencion);

                    reporte.Add(
                        $"ID: {intervencion.Identificador} | " +
                        $"Fecha: {intervencion.Fecha.ToShortDateString()} | " +
                        $"Descripción: {intervencion.Intervencion.Descripcion} | " +
                        $"Paciente: {intervencion.Paciente.NombreCompleto} | " +
                        $"Médico: {intervencion.Medico.NombreCompleto} ({intervencion.Medico.Matricula}) | " +
                        $"Obra Social: {obraSocial} | " +
                        $"Importe: {costoTotal:C2}"
                    );
                }
            }

            return reporte;
        }

        // Método para calcular el costo de las intervenciones de un paciente
        public double CalcularCostoPaciente(string dni)
        {
            double total = 0;
            var paciente = BuscarPacientePorDNI(dni);

            if (paciente != null)
            {
                foreach (var intervencion in IntervencionesProgramadas)
                {
                    if (intervencion.Paciente.DocumentoIdentidad == dni && !intervencion.Pagado)
                    {
                        total += CalcularCostoIntervencion(intervencion);
                    }
                }
            }

            return total;
        }

        // Método público para calcular el costo de una intervención
        public double CalcularCostoIntervencion(IntervencionProgramada intervencion)
        {
            double costo = (double)intervencion.Intervencion.CalcularCosto();

            if (intervencion.Paciente.ObraSocial != null)
            {
                costo *= (1 - intervencion.Paciente.MontoCobertura / 100);
            }

            return costo;
        }
    }
}