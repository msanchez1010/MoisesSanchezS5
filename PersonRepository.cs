using Microsoft.Maui.Controls;
using MoisesSanchezS5.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoisesSanchezS5
{
    public class PersonRepository
    {
        string dbPath;
        private SQLiteConnection conn;
        public string StatusMessage { get; set; }

        public void Init() 
        {
            if (conn is not null)
                return;
            conn = new(dbPath);
            conn.CreateTable<Persona>();                   
        }
        public PersonRepository (string dbPath1)
        {
            dbPath = dbPath1;
        }
        //CRUD

        //METODO ADD (AÑADIR PERSONA)
        public void AddNewPerson(string nombre)
        {
            int result = 0;
            //manejo de excepciones
            try
            {
                Init();
                if (string.IsNullOrEmpty(nombre))
                    throw new Exception("Nombre Requerido");
                Persona persona = new Persona() { Name = nombre }; //, apellido..}
               result = conn.Insert(persona);
               StatusMessage = string.Format("Fila añadida correctamente", result, nombre);
            }
            catch (Exception ex)
            {

               StatusMessage = string.Format("error al insertar", nombre, ex.Message);
            }
        }
        //METODO LIST (LISTAR PERSONA)
        public List<Persona> GetAllPeorle()
        {
            try
            {
                Init();
                    return conn.Table<Persona>().ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = string.Format("error al listar", ex.Message);
            }
            return new List<Persona>();
        }
        
        //METODO UPDATE (ACTUALIZAR PERSONA)

        public void UpdatePerson(Persona updatedPerson)
        {
            try
            {
                Init();

                if (updatedPerson != null)
                {
                    int result = conn.Update(updatedPerson);
                    StatusMessage = result > 0 ? "Persona actualizada correctamente" : "Error al actualizar la persona";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al actualizar la persona: {ex.Message}";
            }
        }

        //METODO DELETE (ELIMINAR PERSONA)
        public void DeletePerson(Persona persona)
        {
            try
            {
                Init();
                conn.Delete<Persona>(persona.Id);
                StatusMessage = "Persona eliminada correctamente";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar la persona: {ex.Message}";
            }
        }

    }
}
