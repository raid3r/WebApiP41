using Microsoft.EntityFrameworkCore;
using WebApiP41.Models;
using WebApiP41.Models.DTO;

namespace WebApiP41.Services;

public class ProductService(ApiDbContext dbContext)
{
    public async Task<PageResult<Product>> GetPagedProductsAsync(PageRequest<ProductFilter> pageRequest)
    {
        // пагінація 
        // 50

        // 1 1-10
        // 2 11-20
        // 3 21-30
        // 4 31-40
        // 5 41-50

        // порядковий номер сторінки (page number) та кількість елементів на сторінці (page size)

        /*
         *       Product 1
         *       Product 2
         * 
         *      1) cкільки всього продуктів в базі даних? 50
         *      2) номер сторінки (page number) 
         *      3) кількість елементів на сторінці (page size)
         *      4) масив продуктів
         *          
         *      [First page] [1][2][3][4][5] [Last page] [10 <] page size
         *                      ---                      [20 <] page size
         *                                               [50 <] page size
         */


        // + фільтрація, сортування, пошук (filtering, sorting, searching)
        // фільтрація (делегат який застосовує для додавання до запиту параметрів фільтрації)
        Func<IQueryable<Product>, IQueryable<Product>> filter = (q) =>
        {
            if (pageRequest.Filter != null)
            {
                // якщо вказано фільтр по імені, то застосовуємо його
                if (!string.IsNullOrEmpty(pageRequest.Filter.Name))
                {
                    // WHERE Name LIKE '%filter.Name%'
                    q = q.Where(p => p.Name.Contains(pageRequest.Filter.Name));
                }
                // якщо вказано фільтр по ціні (мінімальна ціна), то застосовуємо його
                if (pageRequest.Filter.MinPrice.HasValue)
                {
                    // AND Price >= filter.MinPrice
                    q = q.Where(p => p.Price >= pageRequest.Filter.MinPrice.Value);
                }
                // якщо вказано фільтр по ціні (максимальна ціна), то застосовуємо його
                if (pageRequest.Filter.MaxPrice.HasValue)
                {
                    // AND Price <= filter.MaxPrice
                    q = q.Where(p => p.Price <= pageRequest.Filter.MaxPrice.Value);
                }
                // якщо вказано фільтр по опису, то застосовуємо його
                if (!string.IsNullOrEmpty(pageRequest.Filter.Description))
                {
                    // AND Description LIKE '%filter.Description%'
                    q = q.Where(p => p.Description.Contains(pageRequest.Filter.Description));
                }
            }
            return q;
        };

        // сортування (делегат який застосовує для додавання до запиту параметрів сорутвання)
        Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = (q) =>
        {
            return pageRequest.OrderBy switch
            {
                "Id" => q.OrderBy(p => p.Id), // сортування за зростанням по ідентифікатору
                "-Id" => q.OrderByDescending(p => p.Id), // сортування за спаданням по ідентифікатору
                "Name" => q.OrderBy(p => p.Name), // сортування за зростанням по імені
                "-Name" => q.OrderByDescending(p => p.Name), // сортування за спаданням по імені
                "Price" => q.OrderBy(p => p.Price), // сортування за зростанням по ціні
                "-Price" => q.OrderByDescending(p => p.Price), // сортування за спаданням по ціні
                _ => q.OrderBy(p => p.Id) // сортування за замовчуванням, якщо параметр OrderBy не вказаний або невідомий
            };
        };
        
        // пагінація (делегат який застосовує для додавання до запиту параметрів пагінації)
        Func<IQueryable<Product>, IQueryable<Product>> paginate = (q) =>
        {
            return q
            .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
            .Take(pageRequest.PageSize);
        };

        var query = dbContext.Products.AsQueryable();

        // застосовуємо фільтрацію
        query = filter(query);
        // застосовуємо сортування
        query = orderBy(query);
        

        // SELECT COUNT(*) FROM Products ORDER BY Name DESC
        var totalItems = await query.CountAsync();

        // застосовуємо пагінацію
        query = paginate(query);
        // використовуємо dbContext для отримання всіх продуктів // 4
        // SELECT * FROM Products ORDER BY Name DESC LIMIT 10 OFFSET 30
        var products = await query.ToListAsync();

        // обчислюємо загальну кількість сторінок
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageRequest.PageSize);
        // створюємо об'єкт PageResult<Product> для повернення результату
        var result = new PageResult<Product>
        {
            TotalItems = totalItems,
            TotalPages = totalPages,
            CurrentPage = pageRequest.Page,
            PageSize = pageRequest.PageSize,
            Items = products
        };
        return result;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<Product> AddProductAsync(Product newProduct)
    {
        await dbContext.Products.AddAsync(newProduct);
        await dbContext.SaveChangesAsync();
        return newProduct;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
