using System.ComponentModel.DataAnnotations;

namespace Portfolio.Model
{
	public class ContactForm
	{
		[Required(ErrorMessage = "Ce champ est obligatoire.")]
		[MinLength(3, ErrorMessage = "Le nom doit contenir au moins 3 caractères.")]
		public string Name { get; set; } = String.Empty;

		[Required(ErrorMessage = "Ce champ est obligatoire.")]
		[EmailAddress(ErrorMessage = "L'adresse email n'est pas valide.")]
		public string Email { get; set; } = String.Empty;

		[Required(ErrorMessage = "Ce champ est obligatoire.")]
		[MinLength(10, ErrorMessage = "Le message doit contenir au moins 10 caractères.")]
		public string Message { get; set; } = String.Empty;
	}
}

