using AutoMapper;
using EksiSozlukClone.Common.Events.User;
using EksiSozlukClone.Common.Infrastructre;
using EksiSozlukClone.Common;
using EksiSozlukClone.Common.Infrastructre.Exceptions;
using EksiSozlukClone.Common.Models.RequestModels;
using EksiSozlukClone.Core.Application.Interface.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozlukClone.Core.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand,Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);
         
            if (dbUser is null) {
                throw new DatabaseValidationExceptions("User not found");
            }
            var dbEmailAdress = dbUser.EmailAdress;
            var emailChanged = string.CompareOrdinal(dbEmailAdress, request.EmailAdress) != 0;

            mapper.Map(request, dbUser);

            var rows = await userRepository.UpdateAsync(dbUser);

            if (emailChanged && rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    oldEmailAdress = null,
                    newEmailAdress = dbUser.EmailAdress,

                };

                QueueFactory.SendMessage(exchangeName: SozlukConstants.UserExchangeName, exchangeType: SozlukConstants.DefaultExchangeType, queueName: SozlukConstants.UserEmailChangedQueueName, obj: @event);
                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);
                 
            }
            return dbUser.Id;
        }
    }
}
