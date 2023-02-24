import { ReactElement, FC, useState, useEffect } from "react";
import { PageTitle } from "pages/index.styled";
import { productService } from "services/products";
import { Product } from "interfaces";

const Products: FC = (): ReactElement => {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    const fetchProducts = async () => {
      const res = await productService.getProductsPaginated(0, 10);
      setProducts(res);
      console.log(res);
    };
    fetchProducts();
  }, []);

  return (
    <>
      <PageTitle>Products</PageTitle>
      <ul>
        {products.map((product) => (
          <li key={product.id}>
            <h2>{product.name}</h2>
            <p>{product.description}</p>
            <p>{product.price}</p>
          </li>
        ))}
      </ul>
    </>
  );
};

export default Products;
