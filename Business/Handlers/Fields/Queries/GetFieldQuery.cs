
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Fields.Queries
{
    public class GetFieldQuery : IRequest<IDataResult<Field>>
    {
        public int Id { get; set; }

        public class GetFieldQueryHandler : IRequestHandler<GetFieldQuery, IDataResult<Field>>
        {
            private readonly IFieldRepository _fieldRepository;
            private readonly IMediator _mediator;

            public GetFieldQueryHandler(IFieldRepository fieldRepository, IMediator mediator)
            {
                _fieldRepository = fieldRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Field>> Handle(GetFieldQuery request, CancellationToken cancellationToken)
            {
                var field = await _fieldRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Field>(field);
            }
        }
    }
}
