using System.Collections.Generic;
using System.Linq;

namespace Dominio.Entidades
{
    public class ProductoComprado
    {
        public Producto producto { get; set; }
        public Usuario empresa { get; set; }
        public int Cantidad { get; set; }
    }

    public class Carro
    {
        private List<ProductoComprado> contenidoCarro = new List<ProductoComprado>();

        public IEnumerable<ProductoComprado> ProductosComprados {
            get { return contenidoCarro; }
        }

        public void AñadirProducto(Producto producto, Usuario empresa, int cantidad)
        {
            ProductoComprado productoComprado = contenidoCarro
                                                    .FirstOrDefault(p => p.producto.Nombre == producto.Nombre && p.empresa.Id == empresa.Id);                                                    

            if(productoComprado == null)
            {
                contenidoCarro.Add(new ProductoComprado {
                    producto = producto,
                    Cantidad = cantidad,
                    empresa = empresa
                });
            }
            else
            {
                productoComprado.Cantidad += 1;
            }            
        }

        public void BorrarProducto(Producto producto)
        {
            contenidoCarro.RemoveAll(p => p.producto.Empresa == producto.Empresa && p.producto.Id == producto.Id);
        }

        public decimal CosteTotalCarro()
        {
            return contenidoCarro.Sum(p => p.producto.Precio * p.Cantidad);
        }

        public void VaciarCarro()
        {
            contenidoCarro.Clear();
        }

        
    }
}
