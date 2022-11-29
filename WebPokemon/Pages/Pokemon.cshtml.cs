using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokeApiNet;

namespace WebPokemon.Pages
{
    public class PokemonModel : PageModel
    {
        private readonly ILogger<PokemonModel> _logger;
        public List<Pokemon> Pokemon = new List<Pokemon>();

        public PokemonModel(ILogger<PokemonModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            // instantiate client
            PokeApiClient pokeClient = new PokeApiClient();

            // get a resource by name
            Pokemon pokemon = await pokeClient.GetResourceAsync<Pokemon>("ho-oh");

            // ... or by id
            var pageResponse = await pokeClient.GetNamedResourcePageAsync<Pokemon>(12, 0);

            //Create a list of tasks for calling getting the details of each pokemon from the list above
            var tasks = pageResponse.Results
                .Select(p => pokeClient.GetResourceAsync<Pokemon>(p.Name));

            //Await all the tasks and set the data model for the page
            Pokemon = (await Task.WhenAll(tasks)).ToList();
        }
    }
}
