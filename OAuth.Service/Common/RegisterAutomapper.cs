using AutoMapper;
using OAuth.Domain.Model;
using OAuth.Service.ModelDto;

namespace OAuth.Service.Common
{
    public class RegisterAutomapper
    {
        public static void Initialize()
        {
            Mapper.CreateMap<User, UserDto>();

            Mapper.CreateMap<UserDto, User>();

            Mapper.CreateMap<SupplierDto, Supplier>();

            Mapper.CreateMap<Supplier, SupplierDto>();

            Mapper.CreateMap<ItemQuote, ItemQuoteDto>();

            Mapper.CreateMap<ItemQuoteDto, ItemQuote>();

            Mapper.CreateMap<ItemMaterialDto, ItemMaterial>();

        }
    }
}
