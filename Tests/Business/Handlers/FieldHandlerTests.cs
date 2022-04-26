
using Business.Handlers.Fields.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Fields.Queries.GetFieldQuery;
using Entities.Concrete;
using static Business.Handlers.Fields.Queries.GetFieldsQuery;
using static Business.Handlers.Fields.Commands.CreateFieldCommand;
using Business.Handlers.Fields.Commands;
using Business.Constants;
using static Business.Handlers.Fields.Commands.UpdateFieldCommand;
using static Business.Handlers.Fields.Commands.DeleteFieldCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class FieldHandlerTests
    {
        Mock<IFieldRepository> _fieldRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _fieldRepository = new Mock<IFieldRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Field_GetQuery_Success()
        {
            //Arrange
            var query = new GetFieldQuery();

            _fieldRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Field, bool>>>())).ReturnsAsync(new Field()
//propertyler buraya yazılacak
//{																		
//FieldId = 1,
//FieldName = "Test"
//}
);

            var handler = new GetFieldQueryHandler(_fieldRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.FieldId.Should().Be(1);

        }

        [Test]
        public async Task Field_GetQueries_Success()
        {
            //Arrange
            var query = new GetFieldsQuery();

            _fieldRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Field, bool>>>()))
                        .ReturnsAsync(new List<Field> { new Field() { /*TODO:propertyler buraya yazılacak FieldId = 1, FieldName = "test"*/ } });

            var handler = new GetFieldsQueryHandler(_fieldRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Field>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Field_CreateCommand_Success()
        {
            Field rt = null;
            //Arrange
            var command = new CreateFieldCommand();
            //propertyler buraya yazılacak
            //command.FieldName = "deneme";

            _fieldRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Field, bool>>>()))
                        .ReturnsAsync(rt);

            _fieldRepository.Setup(x => x.Add(It.IsAny<Field>())).Returns(new Field());

            var handler = new CreateFieldCommandHandler(_fieldRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _fieldRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Field_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateFieldCommand();
            //propertyler buraya yazılacak 
            //command.FieldName = "test";

            _fieldRepository.Setup(x => x.Query())
                                           .Returns(new List<Field> { new Field() { /*TODO:propertyler buraya yazılacak FieldId = 1, FieldName = "test"*/ } }.AsQueryable());

            _fieldRepository.Setup(x => x.Add(It.IsAny<Field>())).Returns(new Field());

            var handler = new CreateFieldCommandHandler(_fieldRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Field_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateFieldCommand();
            //command.FieldName = "test";

            _fieldRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Field, bool>>>()))
                        .ReturnsAsync(new Field() { /*TODO:propertyler buraya yazılacak FieldId = 1, FieldName = "deneme"*/ });

            _fieldRepository.Setup(x => x.Update(It.IsAny<Field>())).Returns(new Field());

            var handler = new UpdateFieldCommandHandler(_fieldRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _fieldRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Field_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteFieldCommand();

            _fieldRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Field, bool>>>()))
                        .ReturnsAsync(new Field() { /*TODO:propertyler buraya yazılacak FieldId = 1, FieldName = "deneme"*/});

            _fieldRepository.Setup(x => x.Delete(It.IsAny<Field>()));

            var handler = new DeleteFieldCommandHandler(_fieldRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _fieldRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

