using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleCQRS_MediatR.Application.Humans.Commands;
using SampleCQRS_MediatR.ViewModels.Requests;
using SampleCQRS_MediatR.ViewModels.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleCQRS_MediatR.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HumansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HumansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets humans list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetHumansListAsync()
        {
            return Ok();
        }
        
        /// <summary>
        /// Gets human by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHumanByIdAsync(Guid id)
        {
            return Ok();
        }

        /// <summary>
        /// Updates human by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHumanByIdAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity();

            try
            {
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Creates new human
        /// </summary>
        /// <param name="createNewHumanViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewHumanAsync(CreateNewHumanViewModel createNewHumanViewModel)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity();

            try
            {
                var command = new CreateNewHumanCommand()
                {
                    FirstName = createNewHumanViewModel.FirstName,
                    LastName = createNewHumanViewModel.LastName,
                    MiddleName = createNewHumanViewModel.MiddleName,
                    DateOfBirth = createNewHumanViewModel.DateOfBirth,
                };

                SampleCQRS_MediatR.Application.Humans.Responses.CreateNewHumanResponse response = await _mediator.Send(command).ConfigureAwait(false);

                return Ok(new HumanViewModel()
                {
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    MiddleName = response.MiddleName,
                    DateOfBirth = response.DateOfBirth,
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
