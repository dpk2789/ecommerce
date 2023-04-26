namespace Ecommerce.Infrastructure.Domains
{
    public interface IEntity<T> 
   {
       T Id { get; set; }
   }
}
