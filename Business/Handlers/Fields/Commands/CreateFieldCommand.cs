
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Fields.ValidationRules;

namespace Business.Handlers.Fields.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateFieldCommand : IRequest<IResult>
    {

        public string Name { get; set; }


        public class CreateFieldCommandHandler : IRequestHandler<CreateFieldCommand, IResult>
        {
            private readonly IFieldRepository _fieldRepository;
            private readonly IMediator _mediator;
            public CreateFieldCommandHandler(IFieldRepository fieldRepository, IMediator mediator)
            {
                _fieldRepository = fieldRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateFieldValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateFieldCommand request, CancellationToken cancellationToken)
            {
                var isThereFieldRecord = _fieldRepository.Query().Any(u => u.Name == request.Name);

                if (isThereFieldRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedField = new Field
                {
                    Name = request.Name,

                };

                _fieldRepository.Add(addedField);
                await _fieldRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}