using AutoMapper;
using EksiSozlukClone.Common;
using EksiSozlukClone.Common.Events.User;
using EksiSozlukClone.Common.Infrastructre;
using EksiSozlukClone.Common.Infrastructre.Exceptions;
using EksiSozlukClone.Core.Application.Interface.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozlukClone.Core.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
           var existUser= await userRepository.GetSingleAsync(i=>i.EmailAdress ==request.EmailAdress);
           
            if (existUser is not null) {
                throw new DatabaseValidationExceptions("User AldreadyExist");
            }

            var dbUser = mapper.Map<Domain.Models.User>(request);
            var rows = await userRepository.AddAsync(dbUser);

            if (rows>0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    oldEmailAdress = null,
                    newEmailAdress = dbUser.EmailAdress,

                };

                QueueFactory.SendMessage(exchangeName:SozlukConstants.UserExchangeName, exchangeType: SozlukConstants.DefaultExchangeType, queueName: SozlukConstants.UserEmailChangedQueueName, obj: @event);
            }
            return dbUser.Id; 

        }

        
    }
}
