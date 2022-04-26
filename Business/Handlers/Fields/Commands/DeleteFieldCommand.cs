
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Fields.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteFieldCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteFieldCommandHandler : IRequestHandler<DeleteFieldCommand, IResult>
        {
            private readonly IFieldRepository _fieldRepository;
            private readonly IMediator _mediator;

            public DeleteFieldCommandHandler(IFieldRepository fieldRepository, IMediator mediator)
            {
                _fieldRepository = fieldRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteFieldCommand request, CancellationToken cancellationToken)
            {
                var fieldToDelete = _fieldRepository.Get(p => p.Id == request.Id);

                _fieldRepository.Delete(fieldToDelete);
                await _fieldRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

