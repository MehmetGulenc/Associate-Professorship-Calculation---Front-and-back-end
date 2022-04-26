
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Fields.Queries
{

    public class GetFieldsQuery : IRequest<IDataResult<IEnumerable<Field>>>
    {
        public class GetFieldsQueryHandler : IRequestHandler<GetFieldsQuery, IDataResult<IEnumerable<Field>>>
        {
            private readonly IFieldRepository _fieldRepository;
            private readonly IMediator _mediator;

            public GetFieldsQueryHandler(IFieldRepository fieldRepository, IMediator mediator)
            {
                _fieldRepository = fieldRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Field>>> Handle(GetFieldsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Field>>(await _fieldRepository.GetListAsync());
            }
        }
    }
}