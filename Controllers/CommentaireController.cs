using Microsoft.AspNetCore.Mvc;
using TrelloBack.Models;

namespace TrelloBack.Controllers
{
    public class CommentaireController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DbTrelloContext _context;

        public CommentaireController(ILogger<HomeController> logger, DbTrelloContext context)
        {
            _context = context;
            _logger = logger;
        }
// -- requete pour les commentaires ---------

        [HttpGet]
        [Route("/commentaires/{id}")]
        public IActionResult getCommentaires(int? id)
        {
            //recuperer une liste de users

            Console.WriteLine("-----getCommentaires-----");
            var commentaires = _context.Commentaires.Where((commentaires)=>commentaires.IdCarte == id);

            //renvoyer la liste en json

            return Json(commentaires);
        }

        [HttpPost]
        [Route("/commentaires")]
        public IActionResult createCommentaire(Commentaire newCommentaire)
        {
            Console.WriteLine($"---------newCommentaire id : {newCommentaire.Id}--------");
            Console.WriteLine($"---------newCommentaire nom : {newCommentaire.Contenu}--------");
            try
            {
                if (newCommentaire == null)
                {
                    return BadRequest("Les donn�es du commentaire sont nulles.");
                }

                _context.Commentaires.Add(newCommentaire);
                _context.SaveChanges();

                // Retourne une r�ponse JSON avec le nouveau fil de discussion
                return Json(newCommentaire);
            }
            catch (Exception ex)
            {
                // G�rez l'exception de mani�re appropri�e (journalisation, renvoi d'un message d'erreur, etc.)
                return StatusCode(500, "Une erreur interne s'est produite lors de la cr�ation du commentaire.");
            }
        }

        [HttpPut]
        [Route("/commentaires")]
        // On en enlevé le /{id}
        public IActionResult updateCommentaire(int id, Commentaire updatedCommentaire)
        {
            Console.WriteLine($"------updatedCommentaire {id}--------");
            var existingCommentaire = _context.Commentaires.Find(id);

            existingCommentaire.Contenu = updatedCommentaire.Contenu;
            existingCommentaire.DateCreation = updatedCommentaire.DateCreation;
            existingCommentaire.IdCarte = updatedCommentaire.IdCarte;


            _context.Update(existingCommentaire);
            _context.SaveChanges();

            return Json(existingCommentaire);
        }

        [HttpDelete]
        [Route("commentaires")]
        // On en enlevé le /{id}
        public IActionResult DeleteCommentaire(int id)
        {
            Console.WriteLine($"------ Delete commentaire {id} --------");

            // Vérifiez si le commentaire avec l'ID spécifié existe
            var commentaire = _context.Commentaires.Find(id);

            if (commentaire == null)
            {
                // Retournez NotFound si le commentaire n'est pas trouvé
                return NotFound($"Commentaire avec l'ID {id} non trouvé.");
            }

            // Supprimez le commentaire de la base de données
            _context.Commentaires.Remove(commentaire);

            // Enregistrez les modifications dans la base de données
            _context.SaveChanges();

            // Retournez un résultat Ok pour indiquer que la suppression a réussi
            return Ok();
        }

        public IActionResult Index()
        {
            return View();

        }
    }
}
