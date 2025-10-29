using MediatR;
using PaymentTestCase.Application.DTOs;

namespace PaymentTestCase.Application.Queries.Order;

public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>;