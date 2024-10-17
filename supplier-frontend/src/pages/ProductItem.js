import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Row, Col, Card, Container, Modal } from 'react-bootstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../styles/ProductItem.css'; 

const ProductItem = () => {
  const [products, setProducts] = useState([]);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [price, setPrice] = useState('');
  const [stockLevel, setStockLevel] = useState('');
  const [image, setImage] = useState(null);
  const [category, setCategory] = useState('');
  const [editProduct, setEditProduct] = useState(null);
  const [showModal, setShowModal] = useState(false);

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

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    const formData = new FormData(); 
    formData.append('name', name);
    formData.append('description', description);
    formData.append('price', price);
    formData.append('stockLevel', stockLevel);
    formData.append('category', category);
  
    if (image) {
      formData.append('image', image); 
    }
  
    try {
      if (editProduct) {
        console.log([...formData.entries()]);
        await axios.put(`http://localhost:5001/api/product/${editProduct.productId}`, formData, {
          headers: { 'Content-Type': 'multipart/form-data' }, 
        });
        toast.success('Product updated successfully!');
      } else {
        console.log([...formData.entries()]);
        await axios.post('http://localhost:5001/api/product', formData, {
          headers: { 'Content-Type': 'multipart/form-data' }, 
        });
        toast.success('Product added successfully!');
      }
      fetchProducts();
      resetForm();
      handleModalClose();
    } catch (error) {
      console.error('Error submitting product:', error);
      toast.error('Error submitting product!');
    }
  };
  

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(file); 
    }
  };  

  const resetForm = () => {
    setName('');
    setDescription('');
    setPrice('');
    setStockLevel('');
    setImage(null);
    setCategory('');
    setEditProduct(null);
  };

  const handleDelete = async (productId) => {
    console.log('ProductId to test delete API :'+productId);
    try {
      await axios.delete(`http://localhost:5001/api/product/${productId}`);
      fetchProducts();
      toast.success('Product deleted successfully');
    } catch (error) {
      console.error('Error deleting product:', error);
      toast.error('Error deleting product');
    }
  };

  const handleEdit = (product) => {
    setEditProduct(product);
    setName(product.name || '');
    setDescription(product.description || '');
    setPrice(product.price || '');
    setStockLevel(product.stockLevel || '');
    setCategory(product.category || '');
    setImage(product.img || null);
    setShowModal(true);
  };

  const handleModalClose = () => setShowModal(false);

  const handleAddNewProduct = () => {
    resetForm();
    setShowModal(true); 
  };

  return (
      <Container className="product-item-container">
        <ToastContainer autoClose={5000} />
        <h1 className="text-center mb-4">Manage Products</h1>

        <div className="add-product-item-wrapper">
            <Button variant="primary" onClick={handleAddNewProduct} className="mb-4 add-product-item-button">
                Add New Product
            </Button>
        </div>

        <Row className="product-list">
          {products.map((item) => (
            <Col md={4} key={item.productId} className="mb-4">
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
                  <Button variant="warning" onClick={() => handleEdit(item)} className="me-2">
                    Edit
                  </Button>
                  <Button variant="danger" onClick={() => handleDelete(item.productId)}>
                    Delete
                  </Button>
                </Card.Body>
              </Card>
            </Col>
          ))}
        </Row>
  
        {/* Modal for form */}
        <Modal show={showModal} onHide={handleModalClose}>
          <Modal.Header closeButton>
            <Modal.Title>{editProduct ? 'Update Product' : 'Add Product'}</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form onSubmit={handleSubmit}>
              <Row>
                <Col md={6}>
                  <Form.Group controlId="productName" className="mb-3">
                    <Form.Label>Product Name</Form.Label>
                    <Form.Control 
                      type="text" 
                      placeholder="Enter product name" 
                      value={name} 
                      onChange={(e) => setName(e.target.value)} 
                      required 
                    />
                  </Form.Group>
                </Col>
                <Col md={6}>
                  <Form.Group controlId="productPrice" className="mb-3">
                    <Form.Label>Price</Form.Label>
                    <Form.Control 
                      type="number" 
                      placeholder="Enter price" 
                      value={price} 
                      onChange={(e) => setPrice(e.target.value)} 
                      required 
                    />
                  </Form.Group>
                </Col>
              </Row>

              <Form.Group controlId="productDescription" className="mb-3">
                <Form.Label>Description</Form.Label>
                <Form.Control 
                  as="textarea" 
                  rows={3} 
                  placeholder="Enter description" 
                  value={description} 
                  onChange={(e) => setDescription(e.target.value)} 
                  required 
                />
              </Form.Group>

              <Row>
                <Col md={6}>
                  <Form.Group controlId="productStockLevel" className="mb-3">
                    <Form.Label>Stock Level</Form.Label>
                    <Form.Control 
                      type="number" 
                      placeholder="Enter stock level" 
                      value={stockLevel} 
                      onChange={(e) => setStockLevel(e.target.value)} 
                      required 
                    />
                  </Form.Group>
                </Col>
                <Col md={6}>
                  <Form.Group controlId="productCategory" className="mb-3">
                    <Form.Label>Category</Form.Label>
                    <Form.Control 
                      type="text" 
                      placeholder="Enter category" 
                      value={category} 
                      onChange={(e) => setCategory(e.target.value)} 
                      required 
                    />
                  </Form.Group>
                </Col>
              </Row>

              <Form.Group controlId="productImage" className="mb-3">
                <Form.Label>Image</Form.Label>
                <Form.Control 
                  type="file" 
                  accept="image/*" 
                  onChange={handleImageUpload} 
                />
              </Form.Group>

              <Button variant="primary" type="submit">
                {editProduct ? 'Update Product' : 'Add Product'}
              </Button>
            </Form>
          </Modal.Body>
        </Modal>
      </Container>
    
  );
};

export default ProductItem;
