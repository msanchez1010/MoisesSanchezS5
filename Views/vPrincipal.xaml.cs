using MoisesSanchezS5.Models;
using static Android.Icu.Util.LocaleData;

namespace MoisesSanchezS5.Views;

public partial class vPrincipal : ContentPage
{
    private List<Persona> allPeople;  // Esta sería tu lista completa de personas
    private int pageSize = 5;  // Número de personas por página
    private int currentPage = 0;
    public vPrincipal()
    {
        InitializeComponent();
    }
    private void btnInsertar_Clicked(object sender, EventArgs e)
    {
        statusMessage.Text = "";
        try
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                App.personRepo.AddNewPerson(txtName.Text);
                statusMessage.Text = App.personRepo.StatusMessage;

                // Si la inserción fue exitosa, borra el campo de ingreso de nombre
                txtName.Text = "";
            }
            else
            {
                statusMessage.Text = "Nombre requerido";
            }
        }
        catch (Exception ex)
        {
            statusMessage.Text = $"Error al insertar: {ex.Message}";
        }
    }

    private void btnMostrar_Clicked(object sender, EventArgs e)
    {
        statusMessage.Text = "";
        List<Persona> people = App.personRepo.GetAllPeorle();
        ListaPersona.ItemsSource = people;
    }

    private void btnEditar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Persona selectedPerson)
        {
            // Almacena la persona seleccionada para editar
            selectedPersonForEdit = selectedPerson;

            // Configura el Entry con el nombre actual de la persona
            txtName.Text = selectedPerson.Name;

            // Habilita el botón de actualización
            btnActualizar.IsEnabled = true;
            btnInsertar.IsEnabled = false;
        }
    }

    private Persona selectedPersonForEdit;
    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Persona selectedPerson)
        {
            bool confirmarEliminacion = await Application.Current.MainPage.DisplayAlert("Confirmar", $"¿Estás seguro de eliminar a {selectedPerson.Name}?", "Sí", "No");

            if (confirmarEliminacion)
            {
                App.personRepo.DeletePerson(selectedPerson);

                // Actualizar la lista después de la eliminación
                List<Persona> people = App.personRepo.GetAllPeorle();
                ListaPersona.ItemsSource = people;
            }
        }
    }

    private void btnActualizar_Clicked(object sender, EventArgs e)
    {
        if (selectedPersonForEdit != null)
        {
            // Modifica la persona seleccionada
            selectedPersonForEdit.Name = txtName.Text;
            App.personRepo.UpdatePerson(selectedPersonForEdit);

            // Limpia el campo de nombre y deshabilita el botón de actualización
            txtName.Text = string.Empty;
            btnActualizar.IsEnabled = false;

            // Actualiza la lista después de la edición
            List<Persona> people = App.personRepo.GetAllPeorle();
            ListaPersona.ItemsSource = people;

            // Resetea la persona seleccionada para editar
            selectedPersonForEdit = null;
            btnInsertar.IsEnabled = true;
        }
    }
}
