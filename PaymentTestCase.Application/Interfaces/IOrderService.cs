using PaymentTestCase.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Interfaces;

public interface IOrderService
{
    Task AddAsync(IList<OrderItemDto> items, CancellationToken cancellationToken);
    Task<IEnumerable<OrderDto>> GetAsync(CancellationToken cancellationToken);
}