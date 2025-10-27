using MediatR;
using PaymentTestCase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Commands;

public record CancelCommand(string bank, string orderReference, decimal amount) : IRequest<Transaction>;