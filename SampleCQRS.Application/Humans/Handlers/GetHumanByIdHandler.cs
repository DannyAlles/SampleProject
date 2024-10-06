using MediatR;
using SampleCQRS.Application.Exceptions;
using SampleCQRS.Application.Humans.Queries;
using SampleCQRS.Application.Humans.Responses;
using SampleCQRS.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Application.Humans.Handlers
{
    public class GetHumanByIdHandler : IRequestHandler<GetHumanByIdQuery, GetHumanByIdResponse>
    {
        private readonly IHumanRepository _humanRepository;

        public GetHumanByIdHandler(IHumanRepository humanRepository)
        {
            _humanRepository = humanRepository;
        }

        public async Task<GetHumanByIdResponse> Handle(GetHumanByIdQuery request, CancellationToken cancellationToken)
        {
            var humanInfo = await _humanRepository.GetHumanByIdAsync(request.Id).ConfigureAwait(false);
            if (humanInfo == null) throw new HumanNotFoundException();

            return new() {
                Id = humanInfo.Id,
                LastName = humanInfo.LastName,
                FirstName = humanInfo.FirstName,
                MiddleName = humanInfo.MiddleName,
                DateOfBirth = humanInfo.DateOfBirth,
            };
        }
    }
}
