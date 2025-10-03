
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            var allItems = _groceryListItemsRepository.GetAll();

            if (productId.HasValue)
                allItems = allItems.Where(i => i.ProductId == productId.Value).ToList();

            var result = allItems.Select(item =>
            {
                var groceryList = _groceryListRepository.Get(item.GroceryListId);

                var client = _clientRepository.Get(groceryList.ClientId);

                var product = _productRepository.Get(item.ProductId);

                return new BoughtProducts(client, groceryList, product);
            }).ToList();

            return result;
        }
    }
}
