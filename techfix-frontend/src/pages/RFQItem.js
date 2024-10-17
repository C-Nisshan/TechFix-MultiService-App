import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Table, Form, Button, Row, Col, Card, Container, Modal } from 'react-bootstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../styles/RFQItem.css';

const RFQItem = () => {
  const [rfqs, setRFQs] = useState([]);
  const [createdBy, setCreatedBy] = useState('');
  const [supplierId, setSupplierId] = useState('');
  const [suppliers, setSuppliers] = useState([]);
  const [products, setProducts] = useState([]);
  const [rfqItems, setRFQItems] = useState([{ productId: '', requestedQuantity: 1 }]);
  const [editRFQ, setEditRFQ] = useState(null);
  const [showModal, setShowModal] = useState(false);

  // Move fetchRFQs function here so it's accessible globally in the component
  const fetchRFQs = async () => {
    try {
      const res = await axios.get('http://localhost:5000/api/rfq');
      setRFQs(res.data);
    } catch (error) {
      console.error('Error fetching RFQs:', error);
      toast.error('Error fetching RFQs');
    }
    console.log(rfqs);
  };

  useEffect(() => {
    // Fetch Suppliers
    const fetchSuppliers = async () => {
      try {
        const res = await axios.get('http://localhost:5000/api/supplier');
        setSuppliers(res.data);
      } catch (error) {
        console.error('Error fetching suppliers:', error);
        toast.error('Error fetching suppliers');
      }
    };

    // Fetch Products
    const fetchProducts = async () => {
      try {
        const res = await axios.get('http://localhost:5000/api/productref');
        setProducts(res.data); // Assuming res.data is an array of products
      } catch (error) {
        console.error('Error fetching products:', error);
        toast.error('Error fetching products');
      }
    };

    fetchRFQs();
    fetchSuppliers();
    fetchProducts();
  }, []);

  // Log products when they are updated
  useEffect(() => {
    if (products.length > 0) {
      console.log('Fetched products:', products);
    }
  }, [products]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const rfqData = {
      createdBy,
      supplierId: parseInt(supplierId),
      rfqItems: rfqItems.map((item) => ({
        productId: parseInt(item.productId),
        requestedQuantity: parseInt(item.requestedQuantity),
      })),
    };

    // Log rfqData to the console to inspect its structure
    console.log("RFQ Data being submitted:", rfqData);

    try {
      if (editRFQ) {
        await axios.put(`http://localhost:5000/api/rfq/${editRFQ.rfqId}`, rfqData);
        toast.success('RFQ updated successfully!');
      } else {
        await axios.post('http://localhost:5000/api/rfq', rfqData);
        toast.success('RFQ added successfully!');
      }
      fetchRFQs();
      resetForm();
      handleModalClose();
    } catch (error) {
      console.error('Error submitting RFQ:', error);
      toast.error('Error submitting RFQ!');
    }
  };

  const resetForm = () => {
    setCreatedBy('');
    setSupplierId('');
    setRFQItems([{ productId: '', requestedQuantity: 1 }]);
    setEditRFQ(null);
  };

  const handleAddItem = () => {
    setRFQItems([...rfqItems, { productId: '', requestedQuantity: 1 }]);
  };

  const handleRemoveItem = (index) => {
    const items = [...rfqItems];
    items.splice(index, 1);
    setRFQItems(items);
  };

  const handleItemChange = (index, field, value) => {
    const updatedItems = [...rfqItems];
    updatedItems[index][field] = value;
    setRFQItems(updatedItems);
  };

  const handleDelete = async (rfqId) => {
    try {
      await axios.delete(`http://localhost:5000/api/rfq/${rfqId}`);
      fetchRFQs();
      toast.success('RFQ deleted successfully');
    } catch (error) {
      console.error('Error deleting RFQ:', error);
      toast.error('Error deleting RFQ');
    }
  };

  const handleEdit = (rfq) => {
    setEditRFQ(rfq);
    setCreatedBy(rfq.createdBy || '');
    setSupplierId(rfq.supplierId || '');
    setRFQItems(rfq.rfqItems || [{ productId: '', requestedQuantity: 1 }]);
    setShowModal(true);
  };

  const handleModalClose = () => setShowModal(false);

  const handleAddNewRFQ = () => {
    resetForm();
    setShowModal(true);
  };

  const statusMapping = {
    0: 'PENDING', // Assuming 0 is for PENDING
    1: 'APPROVED', // Update these based on your actual values
    2: 'REJECTED'
  };

  return (
    <Container className="rfq-item-container mt-5">
      <ToastContainer autoClose={5000} />
      <h1 className="text-center mb-4">Manage RFQs</h1>

      <div className="add-rfq-item-wrapper">
        <Button variant="primary" onClick={handleAddNewRFQ} className="mb-4 add-rfq-item-button">
          Add New RFQ
        </Button>
      </div>
      
      <Row className="rfq-list">
        {rfqs.map((rfq) => (
          <Col md={4} key={rfq.rfqId} className="mb-4">
            <Card className="rfq-item-card shadow-sm">
              <Card.Body>
                <Card.Text>
                  <strong>Created By:</strong> {rfq.createdBy}<br />
                  <strong>Supplier:</strong> {rfq.supplierName}<br />
                  <strong>Status:</strong> <span className={`status-badge ${statusMapping[rfq.status].toLowerCase()}`}>{statusMapping[rfq.status] || 'UNKNOWN'}</span><br />
                  <strong>Items:</strong>
                  <ul>
                    {rfq.rfqItems.map((item, index) => (
                      <li key={index}><strong>{item.productName}</strong> - Qty: {item.requestedQuantity}</li>
                    ))}
                  </ul>
                </Card.Text>
                <div className="action-buttons">
                  <Button variant="outline-primary" onClick={() => handleEdit(rfq)} className="me-2 action-btn">
                    <i className="fas fa-edit"></i> Edit
                  </Button>
                  <Button variant="outline-primary" onClick={() => handleDelete(rfq.rfqId)} className="action-btn">
                    <i className="fas fa-trash"></i> Delete
                  </Button>
                </div>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>


      {/* Modal for form */}
      <Modal show={showModal} onHide={handleModalClose}>
        <Modal.Header closeButton>
          <Modal.Title>{editRFQ ? 'Update RFQ' : 'Add RFQ'}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="createdBy" className="mb-3">
              <Form.Label>Created By</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter your name"
                value={createdBy}
                onChange={(e) => setCreatedBy(e.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="supplierId" className="mb-3">
              <Form.Label>Supplier</Form.Label>
              <Form.Control
                as="select"
                value={supplierId}
                onChange={(e) => setSupplierId(e.target.value)}
                required
              >
                <option value="">Select a supplier</option>
                {suppliers.map((supplier) => (
                  <option key={supplier.id} value={supplier.id}>
                    {supplier.name}
                  </option>
                ))}
              </Form.Control>
            </Form.Group>

            {rfqItems.map((item, index) => (
              <div key={index} className="rfq-item-fields mb-3">
                <Form.Group controlId={`productId-${index}`} className="mb-2">
                  <Form.Label>Product</Form.Label>
                  <Form.Control
                    as="select"
                    value={item.productId}
                    onChange={(e) => handleItemChange(index, 'productId', e.target.value)}
                    required
                  >
                    <option value="">Select a product</option>
                    {products.map((product) => (
                      <option key={product.productId} value={product.productId}>
                        {product.productName}
                      </option>
                    ))}
                  </Form.Control>
                </Form.Group>

                <Form.Group controlId={`quantity-${index}`} className="mb-2">
                  <Form.Label>Requested Quantity</Form.Label>
                  <Form.Control
                    type="number"
                    placeholder="Enter Quantity"
                    value={item.requestedQuantity}
                    onChange={(e) => handleItemChange(index, 'requestedQuantity', e.target.value)}
                    required
                  />
                </Form.Group>

                <Button variant="danger" onClick={() => handleRemoveItem(index)}>
                  Remove
                </Button>
              </div>
            ))}

            <Button variant="primary" onClick={handleAddItem} className="mb-3">
              Add Item
            </Button>

            <Button variant="success" type="submit" className="w-100">
              {editRFQ ? 'Update RFQ' : 'Add RFQ'}
            </Button>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
};

export default RFQItem;
