﻿using MediatR;
using SampleCQRS.Application.Humans.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Application.Humans.Queries
{
    public class GetHumanByIdQuery : IRequest<GetHumanByIdResponse>
    {
        public Guid Id { get; set; }
    }
}
