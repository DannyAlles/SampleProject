using MediatR;
using SampleCQRS.Application.Humans.Commands;
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
    public class GetHumansListHandler : IRequestHandler<GetHumansListQuery, IEnumerable<GetHumansListResponse>>
    {
        private readonly IHumanRepository _humanRepository;

        public GetHumansListHandler(IHumanRepository humanRepository)
        {
            _humanRepository = humanRepository;
        }

        public async Task<IEnumerable<GetHumansListResponse>> Handle(GetHumansListQuery request, CancellationToken cancellationToken)
        {
            var humanInfos = await _humanRepository.GetHumansListAsync().ConfigureAwait(false);

            return humanInfos.Select(x => new GetHumansListResponse()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                DateOfBirth = x.DateOfBirth,
            });
        }
    }
}
