import React, { useState, useEffect } from 'react';
import axios from 'axios';
import {  Row, Col, Card, Container } from 'react-bootstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../styles/ProductItem.css'; 

const ProductItem = () => {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      const res = await axios.get('http://localhost:5001/api/product');
      setProducts(res.data);
    } catch (error) {
      console.error('Error fetching products:', error);
      toast.error('Error fetching products');
    }
  };

  return (
    <>
      <Container className="product-item-container mt-4 ">
        <ToastContainer autoClose={5000} />
        <h1 className="text-center mb-4">Manage Products</h1>

        <Row className="product-list">
          {products.map((item) => (
            <Col md={4} key={item.productId} className="mb-5">
              <Card className="product-item-card shadow-sm">
              <Card.Img 
                variant="top" 
                src={item.imageBase64 ? `data:image/jpeg;base64,${item.imageBase64}` : 'https://via.placeholder.com/200'} 
                alt={item.name} 
                />
                <Card.Body>
                  <Card.Title>{item.name}</Card.Title>
                  <Card.Text>{item.description}</Card.Text>
                  <Card.Text><strong>Price: </strong>${item.price}</Card.Text>
                  <Card.Text><strong>Stock Level: </strong>{item.stockLevel}</Card.Text>
                </Card.Body>
              </Card>
            </Col>
          ))}
        </Row>
      </Container>
    </>
  );
};

export default ProductItem;
