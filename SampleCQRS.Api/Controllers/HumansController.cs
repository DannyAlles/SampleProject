using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleCQRS.Api.ViewModels.Requests;
using SampleCQRS.Api.ViewModels.Responses;
using SampleCQRS.Application.Exceptions;
using SampleCQRS.Application.Humans.Commands;
using SampleCQRS.Application.Humans.Queries;
using SampleCQRS.Application.Humans.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleCQRS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HumansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HumansController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets humans list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetHumansListAsync()
        {
            try
            {
                var query = new GetHumansListQuery();

                IEnumerable<GetHumansListResponse> response = await _mediator.Send(query).ConfigureAwait(false);

                return Ok(response.Select(x => new HumanViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    DateOfBirth = x.DateOfBirth,
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Gets human by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHumanByIdAsync(Guid id)
        {
            try
            {
                var query = new GetHumanByIdQuery()
                {
                    Id = id
                };

                GetHumanByIdResponse response = await _mediator.Send(query).ConfigureAwait(false);

                return Ok(new HumanViewModel 
                { 
                    Id = response.Id,
                    LastName = response.LastName,
                    DateOfBirth = response.DateOfBirth,
                    FirstName = response.FirstName,
                    MiddleName = response.MiddleName
                });
            }
            catch (HumanNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Updates human by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHumanByIdAsync(Guid id, [FromBody] UpdateHumanViewModel updateHumanViewModel)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity();

            try
            {
                var command = new UpdateHumanCommand()
                {
                    Id = id,
                    FirstName = updateHumanViewModel.FirstName,
                    LastName = updateHumanViewModel.LastName,
                    MiddleName = updateHumanViewModel.MiddleName,
                    DateOfBirth = updateHumanViewModel.DateOfBirth,
                };

                SampleCQRS.Application.Humans.Responses.UpdateHumanResponse response = await _mediator.Send(command).ConfigureAwait(false);

                return Ok(new HumanViewModel()
                {
                    Id = response.Id,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    MiddleName = response.MiddleName,
                    DateOfBirth = response.DateOfBirth,
                });
            }
            catch (HumanNotFoundException)
            {
                return NotFound();
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

                SampleCQRS.Application.Humans.Responses.CreateNewHumanResponse response = await _mediator.Send(command).ConfigureAwait(false);

                return Ok(new HumanViewModel()
                {
                    Id = response.Id,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    MiddleName = response.MiddleName,
                    DateOfBirth = response.DateOfBirth,
                });
            }
            catch (HumanNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
