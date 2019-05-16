using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GraphiQl.example.GraphQl;
using GraphiQl.example.GraphQl.Models;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace GraphiQl.example.Controllers
{
    [Route(Startup.GraphQlPath)]
    public class GraphQlController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQlQuery query)
        {
            var schema = new Schema { Query = new StarWarsQuery() };

            var result = await new DocumentExecuter().ExecuteAsync(x =>
            {
                x.Schema = schema;
                x.Query = query.Query;
                x.Inputs = query.Variables;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        public class AuthorizeTokenViewModel
        {
            [Required]
            public string ClientId { get; set; }
            [Required]
            public string ClientSecret { get; set; }
        }


        [HttpPost, Route("auth/token")]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize([FromBody] AuthorizeTokenViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            return Json(new
            {
                access_token = "12345"
            });
            //Unauthorized();
        }
    }
}
