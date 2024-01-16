using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrelloBack.Models;

namespace TrelloBack.Controllers
{
    public class CarteController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DbTrelloContext _context;

        public CarteController(ILogger<HomeController> logger, DbTrelloContext context)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("/cartes/{id}")]
        public IActionResult getCartes(int? id)
        {
            //recuperer une carte de users

            Console.WriteLine("-----getCartes-----");
            var cartes = _context.Cartes.Where((carte)=> carte.IdListe == id);

            //renvoyer la carte en json

            return Json(cartes);
        }

        /*
        [HttpGet]
        [Route("/cartes/{id}")]
        public IActionResult getCartes(int id)
        {
            //recuperer un  thread

            var carte = _context.Cartes.Find(id);
            //renvoyer un thread en json

            return Json(carte);
        }*/

        [HttpPost]
        [Route("/cartes")]
        public IActionResult createCarte(Carte newCarte)
        {
            Console.WriteLine($"---------newCarte id : {newCarte.Id}--------");
            Console.WriteLine($"---------newCarte Description : {newCarte.Description}--------");
            Console.WriteLine($"---------newCarte Titre : {newCarte.Titre}--------");
            Console.WriteLine($"---------newCarte DateCreation : {newCarte.DateCreation}--------");
            Console.WriteLine($"---------newCarte IdListe : {newCarte.IdListe}--------");
            Console.WriteLine($"---------newCarte Commentaires : {newCarte.Commentaires.ToString()}--------");

            try
            {
                if (newCarte == null)
                {
                    return BadRequest("Les données de carte sont nulles.");
                }

                _context.Cartes.Add(newCarte);
                _context.SaveChanges();

                // Retourne une r�ponse JSON avec le nouveau fil de discussion
                return Json(newCarte);
            }
            catch (Exception ex)
            {
                // G�rez l'exception de mani�re appropri�e (journalisation, renvoi d'un message d'erreur, etc.)
                return StatusCode(500, "Une erreur interne s'est produite lors de la cr�ation du carte.");
            }
        }

        [HttpPut]
        [Route("/cartes")]
        // On en enlevé le /{id}
        public IActionResult updateCarte(int id, Carte updatedCarte)
        {
            Console.WriteLine($"------updatedCarte {id}--------");
            try
            {
                var existingCarte = _context.Cartes.Find(id);

                existingCarte.Titre = updatedCarte.Titre;
                existingCarte.Description = updatedCarte.Description;
                existingCarte.DateCreation = updatedCarte.DateCreation;
                existingCarte.IdListe = updatedCarte.IdListe;




                _context.Update(existingCarte);
                _context.SaveChanges();

                return Json(existingCarte);

                // Votre code ici
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
                return StatusCode(500, "Une erreur s'est produite lors de la mise à jour de la carte.");
            }

        }

        /*
                [HttpDelete]
                [Route("/cartes/{id}")]
                public IActionResult deleteCarte(int id)
                {
                    Console.WriteLine($"------delete carte {id}--------");

                    var carte = _context.Cartes.Find(id);

                    _context.Cartes.Remove(carte);

                    _context.SaveChanges();

                    return Ok();

                }
        */
        [HttpDelete]
        [Route("cartes/{id}")]
        // On en enlevé le /{id}
        public IActionResult DeleteCarte(int id)
        {
            Console.WriteLine($"------ Delete carte {id} --------");

            // Vérifiez si le carte avec l'ID spécifié existe
            var carte = _context.Cartes.Include((x)=>x.Commentaires).ToList().Find(x => x.Id == id);

            if (carte == null)
            {
                // Retournez NotFound si le carte n'est pas trouvé
                return NotFound($"Carte avec l'ID {id} non trouvé.");
            }

            // Supprimez le carte de la base de données
            _context.Cartes.Remove(carte);

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
