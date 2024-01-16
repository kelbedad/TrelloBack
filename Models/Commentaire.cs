using System;
using System.Collections.Generic;

namespace TrelloBack.Models;

public partial class Commentaire
{
    public int Id { get; set; }

    public string? Contenu { get; set; }

    public DateOnly? DateCreation { get; set; }

    public int? IdCarte { get; set; }

    public virtual Carte? IdCarteNavigation { get; set; }
}
