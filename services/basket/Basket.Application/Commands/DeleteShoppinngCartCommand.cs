using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Basket.Application.Commands
{
    public class DeleteShoppinngCartCommand : IRequest<Unit>
    {
        public string UserName { get; set; }
        public DeleteShoppinngCartCommand(string userName)
        {
            UserName = userName;
        }
    }
}