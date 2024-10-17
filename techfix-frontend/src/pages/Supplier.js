import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Row, Col, Card, Container, Modal } from 'react-bootstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../styles/Supplier.css'; 

const Supplier = () => {
  const [suppliers, setSuppliers] = useState([]);
  const [name, setName] = useState('');
  const [contactInfo, setContactInfo] = useState('');
  const [editSupplier, setEditSupplier] = useState(null);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    fetchSuppliers();
  }, []);

  const fetchSuppliers = async () => {
    try {
      const res = await axios.get('http://localhost:5000/api/supplier');
      setSuppliers(res.data);
      console.log('Suppliers : '+suppliers)
    } catch (error) {
      console.error('Error fetching suppliers:', error);
      toast.error('Error fetching suppliers');
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    const supplierData = {
      name,
      contactInfo
    };
  
    try {
      if (editSupplier) {
        await axios.put(`http://localhost:5000/api/supplier/${editSupplier.supplierId}`, supplierData);
        toast.success('Supplier updated successfully!');
      } else {
        await axios.post('http://localhost:5000/api/supplier', supplierData);
        toast.success('Supplier added successfully!');
      }
      fetchSuppliers();
      resetForm();
      handleModalClose();
    } catch (error) {
      console.error('Error submitting supplier:', error);
      toast.error('Error submitting supplier!');
    }
  };

  const resetForm = () => {
    setName('');
    setContactInfo('');
    setEditSupplier(null);
  };

  const handleDelete = async (supplierId) => {
    try {
      await axios.delete(`http://localhost:5000/api/supplier/${supplierId}`);
      fetchSuppliers();
      toast.success('Supplier deleted successfully');
    } catch (error) {
      console.error('Error deleting supplier:', error);
      toast.error('Error deleting supplier');
    }
  };

  const handleEdit = (supplier) => {
    setEditSupplier(supplier);
    setName(supplier.name || '');
    setContactInfo(supplier.contactInfo || '');
    setShowModal(true);
  };

  const handleModalClose = () => setShowModal(false);

  const handleAddNewSupplier = () => {
    resetForm();
    setShowModal(true); 
  };

  return (
    <>
      <Container className="supplier-container mt-5">
        <ToastContainer autoClose={5000} />
        <h1 className="text-center mb-4">Manage Suppliers</h1>

        <div className="add-supplier-wrapper">
          <Button variant="primary" onClick={handleAddNewSupplier} className="mb-4 add-supplier-button">
            Add New Supplier
          </Button>
        </div>

        <Row className="supplier-list">
            {suppliers.map((item, index) => (
                <Col md={4} key={item.supplierId || index} className="mb-4">
                <Card className="supplier-card shadow-sm">
                    <Card.Body>
                    <Card.Title>{item.name}</Card.Title>
                    <Card.Text><strong>Contact Info: </strong>{item.contactInfo}</Card.Text>
                    <Button variant="warning" onClick={() => handleEdit(item)} className="me-2">
                        Edit
                    </Button>
                    <Button variant="danger" onClick={() => handleDelete(item.supplierId)}>
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
            <Modal.Title>{editSupplier ? 'Update Supplier' : 'Add Supplier'}</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form onSubmit={handleSubmit}>
              <Form.Group controlId="supplierName" className="mb-3">
                <Form.Label>Supplier Name</Form.Label>
                <Form.Control 
                  type="text" 
                  placeholder="Enter supplier name" 
                  value={name} 
                  onChange={(e) => setName(e.target.value)} 
                  required 
                />
              </Form.Group>

              <Form.Group controlId="supplierContactInfo" className="mb-3">
                <Form.Label>Contact Info</Form.Label>
                <Form.Control 
                  type="text" 
                  placeholder="Enter contact info" 
                  value={contactInfo} 
                  onChange={(e) => setContactInfo(e.target.value)} 
                  required 
                />
              </Form.Group>

              <Button variant="primary" type="submit">
                {editSupplier ? 'Update Supplier' : 'Add Supplier'}
              </Button>
            </Form>
          </Modal.Body>
        </Modal>
      </Container>
    </>
  );
};

export default Supplier;
