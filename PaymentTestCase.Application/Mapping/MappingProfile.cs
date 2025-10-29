using AutoMapper;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Order, OrderDto>();
        CreateMap<Product, ProductDto>();
    }
}