
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Fields.ValidationRules;


namespace Business.Handlers.Fields.Commands
{


    public class UpdateFieldCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateFieldCommandHandler : IRequestHandler<UpdateFieldCommand, IResult>
        {
            private readonly IFieldRepository _fieldRepository;
            private readonly IMediator _mediator;

            public UpdateFieldCommandHandler(IFieldRepository fieldRepository, IMediator mediator)
            {
                _fieldRepository = fieldRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateFieldValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateFieldCommand request, CancellationToken cancellationToken)
            {
                var isThereFieldRecord = await _fieldRepository.GetAsync(u => u.Id == request.Id);


                isThereFieldRecord.Name = request.Name;


                _fieldRepository.Update(isThereFieldRecord);
                await _fieldRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

